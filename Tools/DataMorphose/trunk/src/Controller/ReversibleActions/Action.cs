/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace DataMorphose.Actions
{
  public abstract class Action : IReversibleAction
  {
    private string description;
    public string Description {
      get { return this.description; }
      set { description = value; }
    }

    #region IReversibleAction implementation
    public abstract void Undo();
    public abstract void Redo();
    #endregion
  }
}

