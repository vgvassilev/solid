/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 05.8.2008 ?.
 * Time: 14:41
 * 
 */

using System;

namespace SolidOpt.Cache
{
	/// <summary>
	/// IValidator checks the cached resource is currently valid without recaching.
	/// <param name="T">Type of the results (calculated source)</param>
	/// <param name="N">Type of the resource names (prime source)</param>
	/// </summary>
	public interface IValidator<T, N>
	{
		bool Validate(T calculatedSource);
		bool Validate(N primeSource);
	}
}
