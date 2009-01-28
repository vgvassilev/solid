/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 25.1.2009 г.
 * Time: 13:35
 * 
 */
using System;
using System.Globalization;

namespace SolidOpt.Core.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of FloatResolver.
	/// </summary>
	public class FloatResolver : Resolver
	{
		public FloatResolver()
		{
		}
		
		public override object TryResolve(object paramValue)
		{
			Single SingleResult;
			if (Single.TryParse(paramValue.ToString(), NumberStyles.Float,
			                    NumberFormatInfo.InvariantInfo, out SingleResult)) {
				return SingleResult;
			}
			else {
				Double DoubleResult;
				if (Double.TryParse(paramValue.ToString(), NumberStyles.Float,
			                    NumberFormatInfo.InvariantInfo, out DoubleResult))
					return DoubleResult;
				else {
					return base.TryResolve(paramValue);
				}
			}
		}
		
	}
}
