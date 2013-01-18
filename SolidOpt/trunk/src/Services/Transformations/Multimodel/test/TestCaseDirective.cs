/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolidOpt.Services.Transformations.Multimodel.Test
{
  /// <summary>
  /// Class used to model directives which can occur in a test case.
  /// </summary>
  internal class TestCaseDirective
  {
    internal enum Kinds {
      Run,
      XFail
    }

    private Kinds kind;
    public Kinds Kind {
      get { return this.kind; }
    }

    private string command;
    public string Command {
      get {
        if (kind != Kinds.Run)
          throw new InvalidOperationException("Getter should be called only if the kind is run");
        return command;
      }
      set {
        if (kind != Kinds.Run)
          throw new InvalidOperationException("Setter should be called only if the kind is run");
        command = value;
      }
    }

    private string arguments;

    public string Arguments {
      get {
        if (kind != Kinds.Run)
          throw new InvalidOperationException("Getter should be called only if the kind is run");
        return arguments;
      }
      set {
        if (kind != Kinds.Run)
          throw new InvalidOperationException("Setter should be called only if the kind is run");
        arguments = value;
      }
    }

    public TestCaseDirective(Kinds kind)
    {
      this.kind = kind;
    }
  }
}
