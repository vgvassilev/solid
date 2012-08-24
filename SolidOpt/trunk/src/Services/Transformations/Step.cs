/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Transformations
{

  public abstract class Step : IStep
  {
    #region IStep implementation
    public abstract object Process (object codeModel);
    public abstract Type GetSourceType();
    public abstract Type GetTargetType();

    #endregion

  }
}
