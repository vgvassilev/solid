using System;

using Mono.Cecil;

namespace SolidReflector.Plugins.AssemblyBrowser
{
  public class SelectionEventArgs : EventArgs {
    public MemberReference definition;
    public ModuleDefinition module;
    public AssemblyDefinition assembly;

    public SelectionEventArgs(MemberReference definition, ModuleDefinition module, 
                              AssemblyDefinition assembly) {
      this.definition = definition;
      this.module = module;
      this.assembly = assembly;
    }
}

  public interface IAssemblyBrowser
  {
    event EventHandler<SelectionEventArgs> SelectionChanged;
    AssemblyBrowser GetAssemblyBrowser();
  }
}

