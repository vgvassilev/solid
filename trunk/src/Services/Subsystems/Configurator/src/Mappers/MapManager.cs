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
	/// MapManager е отговорен за map-ването на стойностите, при преминаване от едно представяне в друго.
	/// Той трябва да се конструира, настройва и предава на Target-а, когато се прави потребителски mapping,
	/// т.е. когато mapping-a не е задължителен. След като се предаде настроения мениджър, опционално
	/// Target-a закача допълнителни, задължителни (валидиращи) mapper-и, които се съобразяват с формата 
	/// на изходния ресурс. Например, когато формата не поддържа интервал в имената на идентификаторите 
	/// е необходимо да се добави механизъм за валидното представяне на ресурса.
	/// </summary>
	public class MapManager<TParamName> : IConfigMapper<TParamName>
	{
		private List<Mapper<TParamName>> mapperList = new List<Mapper<TParamName>>();
		
		private Dictionary<TParamName, object> mmCIR;
		public Dictionary<TParamName, object> MmCIR {
			get { return mmCIR; }
			set { mmCIR = value; }
		}
		
		public MapManager()
		{
		}
		
		public Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR)
		{
			foreach (Mapper<TParamName> mapper in mapperList){
				this.mmCIR = mapper.Map(mmCIR);
			}
			return this.mmCIR;
		}
		
		public Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR)
		{
			foreach (Mapper<TParamName> mapper in mapperList){
				this.mmCIR = mapper.UnMap(mmCIR);
			}
			return this.mmCIR;
		}
		
		public MapManager<TParamName> Add(Mapper<TParamName> mapper)
		{
			mapperList.Add(mapper);
			return this;
		}
		
		public MapManager<TParamName> AddFirst(Mapper<TParamName> mapper)
		{
			mapperList.Insert(0, mapper);
			return this;
		}
		
		public MapManager<TParamName> AddLast(Mapper<TParamName> mapper)
		{
			mapperList.Insert(mapperList.Count - 1, mapper);
			return this;
		}
		
		public void Remove(Mapper<TParamName> mapper)
		{
			mapperList.Remove(mapper);
		}
	}
}
