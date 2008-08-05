/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 05.8.2008 ?.
 * Time: 14:33
 * 
 */

using System;

namespace SolidOpt.Cache
{
	/// <summary>
	/// CacheManager makes the resource avaliable. If there is cached copy and the invalidation is ok then
	/// returns the cached one.
	/// <param name="T">Type of the results (calculated source)</param>
	/// <param name="N">Type of the resource names (prime source)</param>
	/// </summary>
	/// 
	public class CacheManager<T, N>
	{
		private ISource<T, N> source;
		private IValidator<T, N> validator;
		private IStorage<T, N> storage;
			
		public CacheManager()
		{
			
		}
		
		public CacheManager(ISource<T, N> source, IValidator<T, N> validator, IStorage<T, N> storage)
		{
			this.source = source;
			this.validator = validator;
			this.storage = storage;
		}
	}
}
