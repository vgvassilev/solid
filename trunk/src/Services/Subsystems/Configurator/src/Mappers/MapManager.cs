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
	/// The MapManager is responsible for the mapping of the names/values when transforming from one
	/// format to another. When one needs user or optimization mapping the manager has to be 
	/// constructed, set up and passed to the Target.
	/// </summary>
	public class MapManager<TParamName> : IConfigMapper<TParamName>
	{
		private List<Mapper<TParamName>> mapperList = new List<Mapper<TParamName>>();
		
		private Dictionary<TParamName, object> mmCIR;
		/// <summary>
		/// The MapManager Configuration Intermediate Representation. 
		/// </summary>
		public Dictionary<TParamName, object> MmCIR {
			get { return mmCIR; }
			set { mmCIR = value; }
		}
		#region Constructors
		
		public MapManager()
		{
		}
		
		#endregion
		
		/// <summary>
		/// Triggers the mapping, using the mapper list. 
		/// </summary>
		/// <param name="mmCIR">
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </param>
		/// <returns>
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </returns>
		public Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR)
		{
			foreach (Mapper<TParamName> mapper in mapperList){
				this.mmCIR = mapper.Map(mmCIR);
			}
			return this.mmCIR;
		}
		
		/// <summary>
		/// Triggers the unmapping, using the mapper list. 
		/// </summary>
		/// <param name="mmCIR">
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </param>
		/// <returns>
		/// A <see cref="Dictionary<TParamName, System.Object>"/>
		/// </returns>
		public Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR)
		{
			foreach (Mapper<TParamName> mapper in mapperList){
				this.mmCIR = mapper.UnMap(mmCIR);
			}
			return this.mmCIR;
		}
		
		/// <summary>
		/// Adds specific mapper. 
		/// </summary>
		/// <param name="mapper">
		/// A <see cref="Mapper<TParamName>"/>
		/// </param>
		/// <returns>
		/// A <see cref="MapManager<TParamName>"/>
		/// </returns>
		public MapManager<TParamName> Add(Mapper<TParamName> mapper)
		{
			mapperList.Add(mapper);
			return this;
		}
		
		/// <summary>
		/// Adds mapper at the first place of the list.  
		/// </summary>
		/// <param name="mapper">
		/// A <see cref="Mapper<TParamName>"/>
		/// </param>
		/// <returns>
		/// A <see cref="MapManager<TParamName>"/>
		/// </returns>
		public MapManager<TParamName> AddFirst(Mapper<TParamName> mapper)
		{
			mapperList.Insert(0, mapper);
			return this;
		}
		
		/// <summary>
		/// Adds mapper at the end of the list. 
		/// </summary>
		/// <param name="mapper">
		/// A <see cref="Mapper<TParamName>"/>
		/// </param>
		/// <returns>
		/// A <see cref="MapManager<TParamName>"/>
		/// </returns>
		public MapManager<TParamName> AddLast(Mapper<TParamName> mapper)
		{
			mapperList.Insert(mapperList.Count - 1, mapper);
			return this;
		}
		
		/// <summary>
		/// Removes specific mapper. 
		/// </summary>
		/// <param name="mapper">
		/// A <see cref="Mapper<TParamName>"/>
		/// </param>
		public void Remove(Mapper<TParamName> mapper)
		{
			mapperList.Remove(mapper);
		}
	}
}
