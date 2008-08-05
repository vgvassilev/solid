/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 05.8.2008 ?.
 * Time: 14:45
 * 
 */

using System;

namespace SolidOpt.Cache
{
	/// <summary>
	/// IStorage saves the information in concrete representation.
	/// <param name="T">Type of the results (calculated source)</param>
	/// <param name="N">Type of the resource names (prime source)</param>
	/// </summary>
	public interface IStorage<T, N>
	{
		bool Store(T calculatedSource);
		T Load(N storedSource);
	}
}
