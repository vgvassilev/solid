/*
 * Created by SharpDevelop.
 * User: Sasho
 * Date: 11.8.2008
 * Time: 11:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Cache1
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using SolidOpt.Cache;
	
	/// <summary>
	/// Description of TestMemExpire.
	/// </summary>
	public class TestMemExpire: TestBase
	{
		public TestMemExpire(): base()
		{
			this.cache = new TestCacheManager<int,ResultClass>(
				new TestCacheManager<int,ResultClass>.InitDelegate(Init),
				new TestCacheManager<int,ResultClass>.TouchDelegate(Touch),
				new TestCacheManager<int,ResultClass>.ValidateDelegate(Validate),
				new TestCacheManager<int,ResultClass>.CalculateDelegate(Calculate),
				new TestCacheManager<int,ResultClass>.UpdateDelegate(Update),
				new TestCacheManager<int,ResultClass>.DeleteDelegate(Delete)
			);
		}
		
		public override void Test()
		{
			Console.WriteLine("Now: {0}", DateTime.UtcNow);
			base.Test();
		}
		
		public TestCacheManager<int,ResultClass>.CacheItem Init(int key, ResultClass value)
		{
			return new CacheExpireItem(value, DateTime.UtcNow.AddSeconds(1));
		}
		
		public TestCacheManager<int,ResultClass>.CacheItem Touch(int key, TestCacheManager<int,ResultClass>.CacheItem item)
		{
			((CacheExpireItem)item).ExpireAt = ((CacheExpireItem)item).ExpireAt.AddSeconds(1);
			return item;
		}
		
		public bool Validate(int key, TestCacheManager<int,ResultClass>.CacheItem item)
		{
			return ((CacheExpireItem)item).ExpireAt > DateTime.UtcNow;
		}
		
		public ResultClass Calculate(int key)
		{
			return ClassBuilder.BuildResult(0); //key
		}
		
		public ResultClass Update(int key, ResultClass old_value)
		{
			old_value.Text += "!";
			return old_value;
		}
		
		public void Delete(int key, TestCacheManager<int,ResultClass>.CacheItem item)
		{
			//
		}
		
		[Serializable]
		public class CacheExpireItem: TestCacheManager<int,ResultClass>.CacheItem
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
