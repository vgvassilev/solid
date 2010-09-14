/*
 * Created by SharpDevelop.
 * User: Alexander Penev
 * Date: 08.8.2008
 * Time: 09:04
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SolidOpt.Services.Subsystems.Cache
{

	/// <summary>
	/// CacheManager makes the resource avaliable. If there is cached copy and the invalidation is ok then
	/// returns the cached one.
	/// <param name="TKey">Type of the resource names (prime source)</param>
	/// <param name="TValue">Type of the results (calculated source)</param>
	/// </summary>

	[Serializable]
	public class CacheManager<TKey,TValue>
	{
		private Dictionary<TKey,CacheItem> storage;
		
		#region Cache System Delegates
		/// <summary>
		/// Creates new cache item. 
		/// </summary>
		/// <param name="key">Cache item name (key)</param>
		/// <param name="value">Cache item value</param>
		/// <returns>Cache item object</returns>
		public delegate CacheItem InitDelegate(TKey key, TValue value);
		
		/// <summary>
		/// Cache system informs the user when the cache object is accessed.
		/// </summary>
		/// <param name="key">Cache item name (key)</param>
		/// <param name="item">Cache item object</param>
		/// <returns>Cache item object</returns>
		public delegate CacheItem TouchDelegate(TKey key, CacheItem item);
		
		/// <summary>
		/// Checks the cache value whether it is valid.
		/// </summary>
		/// <param name="key">Cache item name (key)</param>
		/// <param name="item">Cache item object</param>
		/// <returns>Returns if the given key-item pair is valid.</returns>
		public delegate bool ValidateDelegate(TKey key, CacheItem item);
		
		/// <summary>
		/// Deletes given cache object from the system.
		/// </summary>
		/// <param name="key">Cache item name (key)</param>
		/// <param name="item">Cache item object</param>
		public delegate void DeleteDelegate(TKey key, CacheItem item);		
		
		/// <summary>
		/// Calculates the value of the element with given key. 
		/// It is done when keys do not have values in the system.
		/// </summary>
		/// <param name="key">Cache item name (key)</param>
		/// <returns>Cache item object</returns>
		public delegate CacheItem CalculateDelegate(TKey key);
		
		/// <summary>
		/// Renews the value of the cache item.
		/// </summary>
		/// <param name="key">Cache item name (key)</param>
		/// <param name="item">Cache item object</param>
		public delegate void UpdateDelegate(TKey key, CacheItem item);
				
		private InitDelegate initDelegate;
		private TouchDelegate touchDelegate;
		private ValidateDelegate validateDelegate;
		private CalculateDelegate calculateDelegate;
		private UpdateDelegate updateDelegate;
		private DeleteDelegate deleteDelegate;
		#endregion
		
		
		#region Constructors
		
		/// <summary>
		/// Attach default methods to the delegates. If there isn't specified concrete methods the default 
		/// are executed. There is no difference in the parameter order.
		/// </summary>
		/// <param name="delegates">Methods to handle the cache system functionality.</param>
		public CacheManager(params Delegate[] delegates)
		{
			this.storage = new Dictionary<TKey,CacheItem>();

			foreach (Delegate d in delegates) {
				if (d is InitDelegate) this.initDelegate = (InitDelegate)d;
				else if (d is TouchDelegate) this.touchDelegate = (TouchDelegate)d;
				else if (d is ValidateDelegate) this.validateDelegate = (ValidateDelegate)d;
				else if (d is CalculateDelegate) this.calculateDelegate = (CalculateDelegate)d;
				else if (d is UpdateDelegate) this.updateDelegate = (UpdateDelegate)d;
				else if (d is DeleteDelegate) this.deleteDelegate = (DeleteDelegate)d;
			}
			
			if (this.initDelegate == null) this.initDelegate = DefaultInitDelegate;
			if (this.touchDelegate == null) this.touchDelegate = DefaultTouchDelegate;
			if (this.validateDelegate == null) this.validateDelegate = DefaultValidateDelegate;
			if (this.calculateDelegate == null) this.calculateDelegate = DefaultCalculateDelegate;
			if (this.updateDelegate == null) this.updateDelegate = DefaultUpdateDelegate;
			if (this.deleteDelegate == null) this.deleteDelegate = DefaultDeleteDelegate;
		}
		#endregion
		
		/// <summary>
		/// NOTE: All methods are virtual because of the faster execution. We noticed that the execution
		/// of delegate binded with virtual method is even faster than execution of Interface. The test has
		/// been taken place on MS JIT 2.0
		/// </summary>

		#region Delegates default implementation
		/// <summary>
		/// The basic idea is a memory cache to be created. 
		/// If the delegate methods remains unchanged in the memory will have values which are constantly valid.
		/// </summary>
		
		
		protected virtual CacheItem DefaultInitDelegate(TKey key, TValue value)
		{
			return new MemoryCacheItem(value);
		}
		
		protected virtual CacheItem DefaultTouchDelegate(TKey key, CacheItem item)
		{
			return item;
		}
		
		protected virtual bool DefaultValidateDelegate(TKey key, CacheItem item)
		{
			return true;
		}
		
		protected virtual CacheItem DefaultCalculateDelegate(TKey key)
		{
			return new MemoryCacheItem(default(TValue));
		}
		
		protected virtual void DefaultUpdateDelegate(TKey key, CacheItem item)
		{
			item.Value = calculateDelegate(key).Value;
		}
		
		protected virtual void DefaultDeleteDelegate(TKey key, CacheItem item)
		{
			//
		}
		
		//
		#endregion
		
		#region Cache Manager Routines
		public void Add(TKey key, TValue value)
		{
			this[key] = value;
		}
		
		public bool ContainsKey(TKey key)
		{
			return storage.ContainsKey(key);
		}
		
		public bool Remove(TKey key)
		{
			CacheItem cacheItem;
			
			if (storage.TryGetValue(key, out cacheItem)) {
				DeleteItem(key, cacheItem);
				return storage.Remove(key);
			} else {
				return false;
			}
		}
		
		public void Compact()
		{
			//
		}
		
		public void Save()
		{
			//
		}
		
		public void Load()
		{
			//
		}
		#endregion
		//
		
		#region Testing helpers
		internal virtual CacheItem InitItem(TKey key, TValue value) {
			Console.WriteLine("InitItem({0})", value);
			return initDelegate(key, value);
		}
		
		internal virtual CacheItem TouchItem(TKey key, CacheItem item) {
			string str_item = string.Format("{0}", item);
			CacheItem result = touchDelegate(key, item);
			Console.WriteLine("TouchItem({0})->{1}", str_item, item);
			return result;
		}
		
		internal virtual bool ValidateItem(TKey key, CacheItem item) {
			bool result = validateDelegate(key, item);
			Console.WriteLine("ValidateItem({0},{1},{2})->{3}", key, item.Value, item, result);
			return result;
		}
		
		internal virtual CacheItem CalculateItem(TKey key) {
			CacheItem item = calculateDelegate(key);
			Console.WriteLine("CalculateItem({0})->{1}", key, item);
			return item;
		}
		
		internal virtual void UpdateItem(TKey key, CacheItem oldItem) {
			string str_old_item = string.Format("{0}", oldItem);
			
			updateDelegate(key, oldItem);
			Console.WriteLine("UpdateItem({0},{1})->{2}", key, str_old_item, oldItem);
		}
		
		internal virtual void DeleteItem(TKey key, CacheItem item) {
			Console.WriteLine("DeleteItem({0})", item);
			deleteDelegate(key, item);
		}
		
		
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<TKey,CacheItem> p in storage) {
				sb.Append(p.ToString());
				sb.Append(", ");
			}
			return sb.ToString();
		}
		#endregion
		
		#region Properties

		/// <summary>
		/// Gets or sets cache item object.
		/// </summary>
		public TValue this[TKey key]
		{
			get {
				
				CacheItem cacheItem;
				
				if (storage.TryGetValue(key, out cacheItem)) {
					if (!ValidateItem(key, cacheItem)) {
						UpdateItem(key, cacheItem);
						return cacheItem.Value;
					}
					else {
						return TouchItem(key, cacheItem).Value;
					}
				} 
				else {
					cacheItem = storage[key] = CalculateItem(key);
					return cacheItem.Value;
				}
				
			}
			
			set {
				CacheItem cacheItem;
				
				if (!storage.TryGetValue(key, out cacheItem)) {
					storage[key] = InitItem(key, value);
				}
				else {
					cacheItem.Value = value;
				}
				
			}
		}
		
		/// <summary>
		/// We do not want to serialize all the delegates. 
		/// When serializing we simply serialize the dictionary of the class. 
		/// This way we omit unnecessary data.
		/// </summary>
		public object State
		{
			get {
				return storage;
			}
			
			set {
				storage = new Dictionary<TKey,CacheItem>((IDictionary<TKey,CacheItem>)value);
			}
		}	
		#endregion
		
		/// <summary>
		/// Base class for all cache items.
		/// The idea of the cache items is to store information for the value of the cached object.
		/// </summary>
		[Serializable]
		public abstract class CacheItem
		{						
			public abstract TValue Value {
				get;
				set;
			}
		}
		
		
		/// <summary>
		/// The class implements memory caching functionallity.
		/// It adds value property to store cached object values.
		/// </summary>
		[Serializable]
		public class MemoryCacheItem : CacheItem
		{
			private TValue value;
			
			public MemoryCacheItem()
			{
			}
				
			public MemoryCacheItem(TValue value)
			{
				this.value = value;
			}
			
			public override string ToString()
			{
				return string.Format("{0}", value);
			}
			
			public override TValue Value {
				get { return value; }//TODO: Add more memory cache logics. Include weak pointers
				set { this.value = value; }
			}
		}
		
		
	}
}
