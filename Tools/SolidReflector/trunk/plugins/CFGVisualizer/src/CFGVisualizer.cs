using Gtk;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoDevelop.Components.Docking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Runtime.Remoting;

using SolidOpt.Services;
using SolidReflector.Plugins.AssemblyBrowser;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;
using SolidOpt.Services.Transformations.Multimodel.CFGtoIL;

namespace SolidReflector.Plugins.CFGVisualizer
{
  public class CFGVisualizer : IPlugin
  {
    private MainWindow mainWindow = null;
    private DrawingArea drawingArea = new DrawingArea();
    private TextView cfgTextView = new TextView();
    private TextView simulationTextView = new TextView();
    private DockItem cfgVisualizingDock = null;
    private DockItem simulationDock = null;
    private ControlFlowGraph<Instruction> currentCfg = null;
    private CFGPrettyDrawer drawer = null;
    private TextView outputTextView = new TextView();

    public CFGVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      ISolidReflector reflector = context as ISolidReflector;
      mainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      ScrolledWindow cfgDrawingAreaWindow = new ScrolledWindow();
      Viewport cfgDrawingAreaViewport = new Viewport();

      cfgDrawingAreaWindow.Add(cfgDrawingAreaViewport);
      cfgDrawingAreaViewport.Add(drawingArea);

      ScrolledWindow cfgTextAreaWindow = new ScrolledWindow();
      Viewport cfgTextAreaViewport = new Viewport();
      
      cfgTextAreaWindow.Add(cfgTextAreaViewport);
      cfgTextAreaViewport.Add(cfgTextView);
      
      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage(cfgTextAreaWindow, new Gtk.Label("CFG Text"));
      nb.AppendPage(cfgDrawingAreaWindow, new Gtk.Label("CFG Visualizer"));
      nb.ShowAll();

      cfgVisualizingDock = mainWindow.DockFrame.AddItem("CFG Visualizer");
      cfgVisualizingDock.Expand = true;
      cfgVisualizingDock.DrawFrame = true;
      cfgVisualizingDock.Label = "CFG Visualizer";
      cfgVisualizingDock.Content = nb;
      cfgVisualizingDock.DefaultVisible = true;
      cfgVisualizingDock.Visible = true;

      ScrolledWindow simulationTextViewWindow = new ScrolledWindow();
      VBox simulationVBox = new VBox(false, 0);
      simulationTextView = new TextView();
      simulationTextViewWindow.Add(simulationTextView);
      Button simulateButton = new Button("Simulate");
      simulateButton.Clicked += HandleClicked;

      simulationVBox.PackStart(simulateButton, false, false, 0);
      simulationVBox.PackStart(new Label("New CFG: "), false, false, 0);
      simulationVBox.PackStart(simulationTextViewWindow, true, true, 0);
      simulationVBox.PackStart(new Label("Method output: "), false, false, 0);
      simulationVBox.PackEnd(outputTextView, true, true, 0);
      
      simulationVBox.ShowAll();

      simulationDock = mainWindow.DockFrame.AddItem("Simulation Visualizer");
      simulationDock.Expand = true;
      simulationDock.DrawFrame = true;
      simulationDock.Label = "Simulation Visualizer";
      simulationDock.Content = simulationVBox;
      simulationDock.DefaultVisible = true;
      simulationDock.Visible = true;
    }

    void IPlugin.UnInit(object context)
    {
      cfgVisualizingDock.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(cfgVisualizingDock);
    }
    #endregion

    private void OnSelectionChanged(object sender, SelectionEventArgs args) {
      if (args.definition != null) {
        // Dump the definition
        CFGPrettyPrinter.PrintPretty(args.definition, cfgTextView);
        drawer = new CFGPrettyDrawer(drawingArea);

        if (args.definition is MethodDefinition) {
          currentCfg = new CilToControlFlowGraph().Decompile(args.definition as MethodDefinition);

          drawer.DrawTextBlocks(currentCfg);
          if (args.module != null) {
            // Dump the module
            if (args.assembly != null) {
              // Dump assembly modules.
            }
          }
        }
      }
    }

    void HandleClicked(object sender, EventArgs e)
    {
      if (currentCfg != null) {
        outputTextView.Buffer.Text = createAssemblyFromCfg(currentCfg);
        CFGPrettyDrawer drawer = new CFGPrettyDrawer(drawingArea);
        drawer.DrawTextBlocks(currentCfg);

        simulationTextView.Buffer.Clear();
        simulationTextView.Buffer.Text = currentCfg.ToString();
      }
    }

    /// <summary>
    /// Swaps the successors of a given block.
    /// </summary>
    /// <param name="currentBlock">Current block.</param>
    /// <param name="oldSuccessor">Old successor.</param>
    /// <param name="newSuccessor">New successor.</param>
    /// 
    void swapSuccessors(BasicBlock<Instruction> currentBlock, BasicBlock<Instruction> oldSuccessor,
                        BasicBlock<Instruction> newSuccessor) {
      if ((newSuccessor.Last.OpCode.FlowControl != FlowControl.Branch) || 
          (newSuccessor.Last.OpCode.FlowControl != FlowControl.Cond_Branch)) {
        Instruction br = Instruction.Create(OpCodes.Br, newSuccessor.Last.Next);
        br.Next = newSuccessor.Last.Next;
        newSuccessor.Last.Next = br;
        newSuccessor.Add(br);
      }

      BasicBlock<Instruction> block = currentBlock.Successors.Find(x => x.Name == oldSuccessor.Name);
      currentBlock.Successors.Remove(block);
      block.Predecessors.Remove(currentBlock);
      
      currentBlock.Successors.Add(newSuccessor);
      newSuccessor.Predecessors.Add(currentBlock);
    }

    /// <summary>
    /// Fixes the branch instructions Operand, Next and Previous properties.
    /// Modifying CFG may make some branch instructions invalid.
    /// Has to be executed after CFG modification in order to get an assembly with valid branches.
    /// </summary>
    /// 
    private void fixBranches() {
      foreach (BasicBlock<Instruction> block in currentCfg.RawBlocks) {
        if (block.Successors.Count == 2) {
          if ((block.Last.OpCode.FlowControl == FlowControl.Branch) || 
              (block.Last.OpCode.FlowControl == FlowControl.Cond_Branch)) {
            block.Last.Operand = block.Successors[1].First;
            
            block.Last.Next = block.Successors[0].First;
            //block.Successors[0].First.Previous = block.Last.Next;
          }
        }
      }
    }

    /// <summary>
    /// Dynamically creates an assembly from a control flow graph.
    /// </summary>
    /// <param name="cfg">Control flow graph</param>
    /// 
    private string createAssemblyFromCfg(ControlFlowGraph<Instruction> cfg) {
      ControlFlowGraphToCil cfgTransformer = new ControlFlowGraphToCil();
      MethodDefinition methodDef = cfgTransformer.Transform(cfg);
      
      AssemblyNameDefinition assemblyName = new AssemblyNameDefinition("Program",
                                                                       new Version("1.0.0.0"));
      
      AssemblyDefinition assemblyDef = AssemblyDefinition.CreateAssembly(assemblyName, "<Module>",
                                                                         ModuleKind.Console);
      
      TypeReference objTypeRef = assemblyDef.MainModule.Import(typeof(System.Object));
      TypeDefinition typeDef = new TypeDefinition("Simulation", "MainClass", TypeAttributes.Public,
                                                  objTypeRef);
      assemblyDef.Modules[0].Types.Add(typeDef);
      
      // Create the main method: public static void Main ()
      TypeReference voidTypeRef = assemblyDef.MainModule.Import(typeof(void));
      MethodDefinition mainMethodDef = new MethodDefinition("Main", MethodAttributes.Public |
                                                            MethodAttributes.HideBySig |
                                                            MethodAttributes.Static,
                                                            voidTypeRef);
      
      // Cannot use methodDef due to it is already in use
      // so we have to create identical MethodDefinition that would be able to be called
      // and we give the method name a "simulated_" prefix
      MethodDefinition trampolineMethod = new MethodDefinition("simulated_" + methodDef.Name,
                                                               methodDef.Attributes,
                                                               methodDef.ReturnType);
      
      // Add the parameters to the callable method
      foreach(ParameterDefinition pDef in methodDef.Parameters)
        trampolineMethod.Parameters.Add(new ParameterDefinition(pDef.Name, pDef.Attributes, 
                                                                pDef.ParameterType));
      
      // Add the local variables to the callable method
      foreach(VariableDefinition varInfo in methodDef.Body.Variables) {
        trampolineMethod.Body.Variables.Add(varInfo);
      }
      
      // Add the instructions to the callable method
      ILProcessor trampolineCIL = trampolineMethod.Body.GetILProcessor();
      foreach (Instruction instr in methodDef.Body.Instructions) {
        if (instr.OpCode == OpCodes.Call)
          instr.Operand = assemblyDef.MainModule.Import(instr.Operand as MethodReference);
        
        trampolineCIL.Append(instr);
      }
      
      typeDef.Methods.Add(trampolineMethod);
      
      MethodReference importedMRef = assemblyDef.MainModule.Import(methodDef);
      
      // Add call to the simulated method and ret instructions to the Main method
      ILProcessor cil = mainMethodDef.Body.GetILProcessor();
      cil.Append(cil.Create(OpCodes.Call, trampolineMethod));
      cil.Append(cil.Create(OpCodes.Ret));
      
      typeDef.Methods.Add(mainMethodDef);
      assemblyDef.EntryPoint = mainMethodDef;
      
      string assemblyPath = Path.Combine(Path.GetTempPath(), "temp.exe");
      assemblyDef.Write(assemblyPath);
      
      Sandbox sandbox = new Sandbox(assemblyPath, trampolineMethod.Name);
      return sandbox.SimulationMethodOutput;
    }
  }
  
  /// <summary>
  /// Loads assembly in separate application domain with restricted permissions.
  /// Currently mono does not support CAS.
  /// </summary>
  /// 
  public class Sandbox : MarshalByRefObject
  {
    public string SimulationMethodOutput = null;
    public Sandbox() { }
    public Sandbox(string assemblyPath, string method) {
      
      // Gives the loaded assembly the ability to be executed
      PermissionSet permSet = new PermissionSet(PermissionState.None);
      permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
      
      AppDomainSetup adSetup = new AppDomainSetup();
      adSetup.ApplicationName = "AppDomainSetup";
      AppDomain domain = AppDomain.CreateDomain("SandboxDomain");
      ObjectHandle handle = Activator.CreateInstanceFrom(domain, 
                                                         typeof(Sandbox).Assembly.ManifestModule.FullyQualifiedName,
                                                         typeof(Sandbox).FullName);
      
      Sandbox sandboxInstance = (Sandbox) handle.Unwrap();
      SimulationMethodOutput = sandboxInstance.ExecuteAssembly(assemblyPath, "Simulation.MainClass",
                                                               method);
      AppDomain.Unload(domain);
    }
    
    /// <summary>
    /// Executes an assembly in a different and permission restricted app domain.
    /// </summary>
    /// <param name="assemblyPath">Assembly path.</param>
    /// <param name="typeName">Type name.</param>
    /// <param name="method">Method.</param>
    /// 
    public string ExecuteAssembly(string assemblyPath, string typeName, string method) {
      System.Reflection.Assembly a = System.Reflection.Assembly.LoadFrom(assemblyPath);
      System.Reflection.Module module = a.GetModule("<Module>");
      Type namespaceType = module.GetType(typeName);
      System.Reflection.MethodInfo methodToExecute = namespaceType.GetMethod(method);
      System.Reflection.ParameterInfo[] parameters = methodToExecute.GetParameters();
      
      Random r = new Random();
      List<object> sampleArgs = new List<object>();
      
      // Only int parameters are supported for now
      foreach (System.Reflection.ParameterInfo param in parameters) {
        switch (Type.GetTypeCode(param.ParameterType)) {
          case TypeCode.Int16:
            sampleArgs.Add(r.Next(Int16.MinValue, Int16.MaxValue));
            break;
          case TypeCode.Int32:
          case TypeCode.Int64:
            sampleArgs.Add(r.Next(Int32.MinValue, Int32.MaxValue));
            break;
          default:
            System.Diagnostics.Debug.Assert(false, "Not supported: " + param.ParameterType.ToString());
            break;
        }
      }
      object[] sampleArgsArray = sampleArgs.ToArray();
      if (methodToExecute.Invoke(null, sampleArgsArray) != null)
        return methodToExecute.Invoke(null, sampleArgsArray).ToString();
      return "Nothing to return.";
    }
  }
}
