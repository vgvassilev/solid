/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator
{
	/// <summary>
	/// The types, which come from the configuration Sources are string. Not every configuration
	/// parameter is meant to be string, though. 
	/// The type recognition is important for when configuration assembly is built, because its
	/// efficiency depends on the correct type recognition. The resolving must be in exact order,
	/// i.e if we put the StringResolver before the IntResolver every int will be resolved as string.
	/// The resolving is at two stages. The first is when the types are read from a Source, just 
	/// before they are entered into the configuration intermediate representation. The second
	/// stage is just before the configuration intermediate represenation is serialized as 
	/// assembly.
	/// </summary>
	public interface ITypeResolver
	{
		/// <summary>
		/// Tries to resolve the type of the parameter and returns the resolved type. 
		/// </summary>
		/// <param name="paramValue">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Object"/>
		/// </returns>
		object TryResolve(object paramValue);
	}
}
