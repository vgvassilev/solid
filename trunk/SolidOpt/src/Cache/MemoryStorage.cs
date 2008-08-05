/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 05.8.2008 ?.
 * Time: 15:06
 * 
 */

using System;

namespace SolidOpt.Cache
{
	/// <summary>
	/// MemoryStorage saves the input into RAM.
	/// </summary>
	public class MemoryStorage<T, N> : IStorage<T, N>
	{
		public MemoryStorage()
		{
		}
		
		bool IStorage<T, N>.Store(T calculatedSource)
		{
			throw new NotImplementedException();
		}
		
		T IStorage<T, N>.Load(N storedSource)
		{
			throw new NotImplementedException();
		}
	}
}
