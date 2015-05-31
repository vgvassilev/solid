/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidV.MVC;

namespace SolidIDE.Domains
{

  [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
  public sealed class ToolAttribute : Attribute
  {
    readonly Type toolProviderType;
    public Type ToolProviderType {
      get { return toolProviderType; }
    }

    public ToolAttribute(Type toolProviderType)
    {
      this.toolProviderType = toolProviderType;
    }

    public class ToolProvider<E, C, M>
    {
      public virtual Tool<E, C, M> GetTool() {
        return default(Tool<E,C,M>);
      }
    }

  }

}