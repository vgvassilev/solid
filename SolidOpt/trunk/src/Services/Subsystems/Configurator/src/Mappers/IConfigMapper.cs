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
	/// The interface represent the renaming (mapping) between the symbols. The
	/// configuration subsystem contains logics for parameter renaming. The
	/// renaming of parameters (mapping) is process, which changes part or the
	/// entire name of the parameters.
	/// <list> There are several types of mapping:
	/// <item>
	/// 	User mapping - it is applicablewhen the developer decides, that the
	///		new names are going to be more useful, meaningful and intuitive.
	/// </item>
	/// <item>
	/// 	Validation mapping - it is applicable, when the target format of 
	/// 	transformation (intermediate representation or configuration file) 
	/// 	does not support specific symbols, used in the names. I. e. in the
	/// 	INI file format specific are “ [“, “] “,”=“. If there are parameters,
	/// 	which must be stored into INI and contain in their names specific or
	/// 	reserved symbols, they have to be mapped, because the configuration 
	/// 	file wouldn't be valid.
	/// </item>
	/// <item>
	/// 	Optimizational mapping - applicable when there are unneccessary long
	/// 	names. The long names use more storage space and it is better to be
	/// 	reduced.
	/// </item>
	/// </list>
	/// </summary>
	public interface IConfigMapper<TParamName>
	{
		Dictionary<TParamName, object> Map(Dictionary<TParamName, object> mmCIR);
		Dictionary<TParamName, object> UnMap(Dictionary<TParamName, object> mmCIR);
	}
}
