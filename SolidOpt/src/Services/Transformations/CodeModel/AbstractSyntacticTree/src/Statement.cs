/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree.Nodes
{
  public abstract class Statement
  {
    protected Statement() {}

    /// <summary>
    /// Determines whether this instance is of the specified kind.
    /// </summary>
    /// <description>This avoids RTTI requests and going through virtual methods. Subclassess must
    /// implement it too.
    /// </description>
    /// <returns><c>true</c> if this instance is a; otherwise, <c>false</c>.</returns>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    bool IsA<T>() where T : Statement {
      return typeof(T) != null;
    }

    T DynCast<T>() where T : Statement {
      if (typeof(T) == this.GetType())
          return (T)this;
      return null;
    }
  }
}

