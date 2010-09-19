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
	/// Групата CustomMapperGroup е част от шаблона композиция и би трябвало да се използва тогава,
	/// когато mapping-и трябва да се извършат в точно определена последователност. С други думи
	/// когато се построява съставен mapping и е необходимо map-a да се извърши в определен ред се
	/// съставя група, която се добавя в MapManager-a и бива разглеждана от него като хомогенен обект.
	/// </summary>
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
