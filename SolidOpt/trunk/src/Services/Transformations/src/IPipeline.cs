/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Transformations
{


  public interface IPipeline<Source, Target> 
    where Source : class
    where Target : class
  {
    Target Run(Source source);
  }
}
