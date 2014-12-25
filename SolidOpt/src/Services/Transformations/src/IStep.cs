/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Transformations
{
  public interface IStep
  {
    /// <summary>
    /// Transforms the specified codeModel.
    /// </summary>
    /// <param name='codeModel'>
    /// Code model.
    /// </param>
    object Process(object codeModel);

    /// <summary>
    /// Calling that method returns metadata of the input type being transformed.
    /// </summary>
    /// <returns>
    /// The input (from) type.
    /// </returns>
    Type GetSourceType();

    /// <summary>
    /// Calling that method returns metadata of the input type being transformed to.
    /// </summary>
    /// <returns>
    /// The output (to) type.
    /// </returns>
    Type GetTargetType();
  }
}
