/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
	internal class CannotResolve
	{
		private static CannotResolve instance = new CannotResolve();
		
		public static CannotResolve Instance {
			get {return instance;}
		}
		
		private CannotResolve()
		{
		}
		
	}
}
