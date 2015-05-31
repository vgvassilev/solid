/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidOpt.Services;

using SolidIDE;

namespace SolidIDE.Domains.ControlFlow
{
  public class ControlFlowDomain : IPlugin
  {
    private ISolidIDE solidIDE;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      solidIDE = context as ISolidIDE;
      //
    }

    void IPlugin.UnInit(object context) {
      // throw new NotImplementedException();
    }

    #endregion

  }
}
