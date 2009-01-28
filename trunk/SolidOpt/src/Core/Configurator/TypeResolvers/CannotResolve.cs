/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 28.1.2009 г.
 * Time: 12:27
 * 
 */
using System;

namespace SolidOpt.Core.Configurator.TypeResolvers
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
