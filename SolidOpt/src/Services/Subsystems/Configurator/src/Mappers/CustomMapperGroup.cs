/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.Mappers
{
  /// <summary>
  /// The CustomMapperGroup is part of the Composition Pattern and it should be used when the
  /// mappers are going to be used in specific sequence. In other words when a composite mapper
  /// is built and we need the mapping to be performed in specific order this group should be 
  /// created. Then it is added to the MapManager and from now onit is seen as homogeneous 
  /// object.
  /// </summary>
  //FIXME:Has to be ChainMapper and to be similar to the TypeResolvers because the idea is the same...
  public class CustomMapperGroup<TParamName> : Mapper<TParamName>
  {
    private List<Mapper<TParamName>> mapperList = new List<Mapper<TParamName>>();
    
    public CustomMapperGroup()
    {
    }
    
    public override Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR)
    {
      foreach (Mapper<TParamName> mapper in mapperList){
        mmCIR = mapper.Map(mmCIR);
      }
      return mmCIR;
    }
    
    public override Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR)
    {
      foreach (Mapper<TParamName> mapper in mapperList){
        mmCIR = mapper.UnMap(mmCIR);
      }
      return mmCIR;
    }
    
    public override Mapper<TParamName> AddMapper(Mapper<TParamName> mapper)
    {
      mapperList.Add(mapper);
      return this;
    }
    
    public override void RemoveMapper(Mapper<TParamName> mapper)
    {
      mapperList.Remove(mapper);
    }  
  }
}
