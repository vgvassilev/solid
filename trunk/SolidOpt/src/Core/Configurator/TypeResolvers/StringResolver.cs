/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 25.1.2009 г.
 * Time: 13:19
 * 
 */
using System;
using System.Text;

namespace SolidOpt.Core.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of StringResolver.
	/// </summary>
	public class StringResolver : Resolver
	{
		public StringResolver()
		{
		}
		
		public override object TryResolve(object paramValue)
		{
			return paramValue.ToString();
		}
	}
}
