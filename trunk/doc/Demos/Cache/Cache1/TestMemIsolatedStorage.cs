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
	using System.IO.IsolatedStorage;
	using System.Runtime.Serialization.Formatters.Binary;
	using OpenF.Lib.Cache;
	
	/// <summary>
	/// Description of TestMemIsolatedStorage.
	/// </summary>
	public class TestMemIsolatedStorage: TestBase
	{
		private IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForAssembly();
		
		public TestMemIsolatedStorage(): base()
		{
			isf.CreateDirectory("cache");
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
			Console.WriteLine("ISF: {0}", isf.GetDirectoryNames("*"));
			base.Test();
		}
		
		public CacheManager<int,ResultClass>.CacheItem Init(int key, ResultClass value)
		{
			return new CacheIsolatedStorageItem(value, isf, key);
		}
		
		public CacheManager<int,ResultClass>.CacheItem Touch(CacheManager<int,ResultClass>.CacheItem item)
		{
			//((CacheIsolatedStorageItem)item).ExpireAt = ((CacheIsolatedStorageItem)item).ExpireAt.AddSeconds(1);
			return item;
		}
		
		public bool Validate(int key, ResultClass value, CacheManager<int,ResultClass>.CacheItem item)
		{
			//return ((CacheIsolatedStorageItem)item).ExpireAt > DateTime.UtcNow;
			return true;
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
		
		public void Delete(CacheManager<int,ResultClass>.CacheItem item)
		{
			//
		}
		
		[Serializable]
		public class CacheIsolatedStorageItem: CacheManager<int,ResultClass>.CacheItem
		{
			[NonSerialized]
			private IsolatedStorageFileStream isfsValue;
			
			public CacheIsolatedStorageItem(): base()
			{
			}
				
//			public CacheIsolatedStorageItem(ResultClass value): base(value)
//			{
//				//this.expireAt = DateTime.MaxValue;
//			}
			
			public CacheIsolatedStorageItem(ResultClass value, IsolatedStorageFile isf, int key): base(value)
			{
				string path = Path.Combine("cache", key.GetHashCode().ToString());
				isf.CreateDirectory(path);
				
				GC.Collect(2, GCCollectionMode.Forced);
				GC.WaitForPendingFinalizers();
				
				this.isfsValue = new IsolatedStorageFileStream(Path.Combine(path, "value"), FileMode.Create, FileAccess.Write, isf);
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(isfsValue, value);
				isfsValue.Flush();
			}
			
			public override string ToString()
			{
				return string.Format("{0}", base.ToString());
			}
		}
	}
}
