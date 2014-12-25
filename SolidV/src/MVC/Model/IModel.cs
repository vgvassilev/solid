/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  /// <summary>
  /// A delegate, defining a method signature which will be invoked on model changes.
  /// </summary>
  public delegate void ModelChangedDelegate(object model);

  /// <summary>
  /// Defines an event to which users can subscribe to monitor changes in the model.
  /// </summary>
  public interface IModel
  {
    event ModelChangedDelegate ModelChanged;
  }
}
