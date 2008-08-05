/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 05.8.2008 ?.
 * Time: 14:55
 * 
 */

using System;

namespace SolidOpt.Cache
{
	/// <summary>
	/// ISource returns the result of the prime source. CacheManger receives the resource and
	/// passes it to the IStorage object. If needed transformator (via algoritm) is performed.
	/// <param name="T">Type of the results (calculated source)</param>
	/// <param name="N">Type of the resource names (prime source)</param>
	/// </summary>
	public interface ISource<T, N>
	{
		T CalculateResult(N primeSource);
	}
}
