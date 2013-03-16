/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public delegate void ModelChangedDelegate(object model);

  public interface IModel
  {
    event ModelChangedDelegate ModelChanged;
  }
}
