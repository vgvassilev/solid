using Mono.Cecil;
using System;

namespace SolidReflector.Plugins.AssemblyBrowser
{
  /// <summary>
  /// Provides information about the currently selected assembly element.
  /// </summary>
  /// 
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

  /// <summary>
  /// Methods and events an assembly browser plugin has to implement in order to be compatible with
  /// the SolidReflector/DataMorphose project
  /// </summary>
  /// 
  public interface IAssemblyBrowser
  {
    /// <summary>
    /// Occurs when the current selected assembly element is changed.
    /// </summary>
    /// 
    event EventHandler<SelectionEventArgs> SelectionChanged;

    /// <summary>
    /// Gets the AssemblyBrowser plugin.
    /// </summary>
    /// <returns>
    /// The AssemblyBrowser plugin.
    /// </returns>
    /// 
    AssemblyBrowser GetAssemblyBrowser();
  }
}