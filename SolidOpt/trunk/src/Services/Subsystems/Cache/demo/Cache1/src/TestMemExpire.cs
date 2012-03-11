/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//using SolidOpt.Cache;

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{	
	
	/// <summary>
	/// Description of TestMemExpire.
	/// </summary>
	public class TestMemExpire: TestBase
	{
		public TestMemExpire(): base()
		{
			this.cache = new CacheManager<int,ResultClass>(
				new CacheManager<int,ResultClass>.InitDelegate(Init),
				new CacheManager<int,ResultClass>.TouchDelegate(Touch),
				new CacheManager<int,ResultClass>.ValidateDelegate(Validate),
				new CacheManager<int,ResultClass>.CalculateDelegate(Calculate),
				new CacheManager<int,ResultClass>.UpdateDelegate(Update),
				new CacheManager<int,ResultClass>.DeleteDelegate(Delete)
			);
		}
		
		public override void Test()
		{
			Console.WriteLine("Now: {0}", DateTime.UtcNow);
			base.Test();
		}
		
		public virtual CacheManager<int,ResultClass>.CacheItem Init(int key, ResultClass value)
		{
			return new CacheExpireItem(value, DateTime.UtcNow.AddSeconds(1));
		}
		
		public virtual CacheManager<int,ResultClass>.CacheItem Touch(int key, CacheManager<int,ResultClass>.CacheItem item)
		{
			((CacheExpireItem)item).ExpireAt = ((CacheExpireItem)item).ExpireAt.AddSeconds(1);
			return item;
		}
		
		public virtual bool Validate(int key, CacheManager<int,ResultClass>.CacheItem item)
		{
			return ((CacheExpireItem)item).ExpireAt > DateTime.UtcNow;
		}
		
		public virtual CacheManager<int,ResultClass>.CacheItem Calculate(int key)
		{
			return new CacheExpireItem(ClassBuilder.BuildResult(0), DateTime.UtcNow.AddSeconds(1)); //key
		}
		
		public virtual void Update(int key,  CacheManager<int,ResultClass>.CacheItem oldItem)
		{
			oldItem.Value.Text += "!";
		}
		
		public virtual void Delete(int key, CacheManager<int,ResultClass>.CacheItem item)
		{
			//
		}
		
		[Serializable]
		public class CacheExpireItem: CacheManager<int,ResultClass>.MemoryCacheItem
		{
			private DateTime expireAt;
			
			public CacheExpireItem(): base()
			{
			}
				
			public CacheExpireItem(ResultClass value): base(value)
			{
				this.expireAt = DateTime.MaxValue;
			}
			
			public CacheExpireItem(ResultClass value, DateTime expireAt): base(value)
			{
				this.expireAt = expireAt;
			}
			
			public override string ToString()
			{
				return string.Format("{0}:exp={1}", base.ToString(), ExpireAt);
			}
			
			public DateTime ExpireAt {
				get { return expireAt; }
				set { expireAt = value; }
			}
		}
	}
}
