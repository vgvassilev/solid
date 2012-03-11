/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Globalization;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
	/// <summary>
	/// Tries to resolve floats and doubles.
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
