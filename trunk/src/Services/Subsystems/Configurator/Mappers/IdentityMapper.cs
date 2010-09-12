/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 30.12.2008 г.
 * Time: 19:20
 * 
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.Mappers
{
	/// <summary>
	/// Единичният (прозрачният) mapper. Използва се за тестване и демонстрация какво би трябвало
	/// да включва един mapper.
	/// </summary>
	public class IdentityMapper<TParamName> : Mapper<TParamName>
	{
		public IdentityMapper()
		{
		}
		
		public override Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR)
		{
			return mmCIR;
		}
		
		public override Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR)
		{
			return mmCIR;
		}
	}
}
