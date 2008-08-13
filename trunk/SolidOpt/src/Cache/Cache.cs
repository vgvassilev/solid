/*
 * Created by SharpDevelop.
 * User: Alexander Penev
 * Date: 08.8.2008
 * Time: 09:04
 * 
 */

namespace OpenF.Lib.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	
	/// <summary>
	/// CacheManager makes the resource avaliable. If there is cached copy and the invalidation is ok then
	/// returns the cached one.
	/// <param name="TKey">Type of the resource names (prime source)</param>
	/// <param name="TValue">Type of the results (calculated source)</param>
	/// </summary>
	/// 
	[Serializable]
	public class CacheManager<TKey,TValue>
	{
		public delegate CacheItem InitDelegate(TKey key, TValue value);
		public delegate CacheItem TouchDelegate(CacheItem item);
		public delegate bool ValidateDelegate(TKey key, TValue value, CacheItem item);
		public delegate TValue CalculateDelegate(TKey key);
		public delegate TValue UpdateDelegate(TKey key, TValue old_value);
		public delegate void DeleteDelegate(CacheItem item);
		
		private Dictionary<TKey,CacheItem> storage;
		
		private InitDelegate initDelegate;
		private TouchDelegate touchDelegate;
		private ValidateDelegate validateDelegate;
		private CalculateDelegate calculateDelegate;
		private UpdateDelegate updateDelegate;
		private DeleteDelegate deleteDelegate;
		
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
		
//		public CacheManager()
//		{
//			this.storage = new Dictionary<TKey,CacheItem>();
//			this.validateValue = DefaultValidateDelegate;
//			this.calculateValue = DefaultCalculateValueDelegate;
//			this.updateValue = DefaultUpdateValueDelegate;
//		}
//		
//		public CacheManager(ValidateDelegate validateValue)
//		{
//			this.storage = new Dictionary<TKey,CacheItem>();
//			this.validateValue = validateValue;
//			this.calculateValue = DefaultCalculateValueDelegate;
//			this.updateValue = DefaultUpdateValueDelegate;
//		}
//		
//		public CacheManager(ValidateDelegate validateValue, CalculateValueDelegate calculateValue)
//		{
//			this.storage = new Dictionary<TKey,CacheItem>();
//			this.validateValue = validateValue;
//			this.calculateValue = calculateValue;
//			this.updateValue = DefaultUpdateValueDelegate;
//		}
//		
//		public CacheManager(ValidateDelegate validateValue, CalculateValueDelegate calculateValue, UpdateValueDelegate updateValue)
//		{
//			this.storage = new Dictionary<TKey,CacheItem>();
//			this.validateValue = validateValue;
//			this.calculateValue = calculateValue;
//			this.updateValue = updateValue;
//		}
		
		private CacheItem DefaultInitDelegate(TKey key, TValue value)
		{
			return new CacheItem(value);
		}
		
		private CacheItem DefaultTouchDelegate(CacheItem item)
		{
			return item;
		}
		
		private bool DefaultValidateDelegate(TKey key, TValue value, CacheItem item)
		{
			return true;
		}
		
		private TValue DefaultCalculateDelegate(TKey key)
		{
			return default(TValue);
		}
		
		private TValue DefaultUpdateDelegate(TKey key, TValue old_value)
		{
			return calculateDelegate(key);
		}
		
		private void DefaultDeleteDelegate(CacheItem item)
		{
			//
		}
		
		//
		
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
				DeleteItem(cacheItem);
				return storage.Remove(key);
			} else {
				return false;
			}
		}
		
		public void Scavenge()
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
		
		//
		internal virtual CacheItem InitItem(TKey key, TValue value) {
			Console.WriteLine("InitItem({0})", value);
			return initDelegate(key, value);
		}
		
		internal virtual CacheItem TouchItem(CacheItem item) {
			string str_item = string.Format("{0}", item);
			CacheItem result = touchDelegate(item);
			Console.WriteLine("TouchItem({0})->{1}", str_item, item);
			return result;
		}
		
		internal virtual bool ValidateItem(TKey key, TValue value, CacheItem item) {
			bool result = validateDelegate(key, value, item);
			Console.WriteLine("ValidateItem({0},{1},{2})->{3}", key, value, item, result);
			return result;
		}
		
		internal virtual TValue CalculateItem(TKey key) {
			TValue result = calculateDelegate(key);
			Console.WriteLine("CalculateItem({0})->{1}", key, result);
			return result;
		}
		
		internal virtual TValue UpdateItem(TKey key, TValue old_value) {
			string str_old_value = string.Format("{0}", old_value);
			TValue result = updateDelegate(key, old_value);
			Console.WriteLine("UpdateItem({0},{1})->{2}", key, str_old_value, result);
			return result;
		}
		
		internal virtual void DeleteItem(CacheItem item) {
			Console.WriteLine("DeleteItem({0})", item);
			deleteDelegate(item);
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
		
		// Properties
		public TValue this[TKey key]
		{
			get {
//				CacheItem cacheItem;
//				
//				if (storage.TryGetValue(key, out cacheItem)) {
//					if (ValidateItem(key, cacheItem.Value, cacheItem)) {
//						return TouchItem(cacheItem).Value;
//					} else {
//						return cacheItem.Value = UpdateItem(key, cacheItem.Value);
//					}
//				} else {
//					return (storage[key] = CalculateItem(key)).Value;
//				}
				
				CacheItem cacheItem;
				
				if (storage.TryGetValue(key, out cacheItem)) {
					if (!ValidateItem(key, cacheItem.Value, cacheItem)) {
						cacheItem.Value = UpdateItem(key, cacheItem.Value);
					}
				} else {
					cacheItem = storage[key] = InitItem(key, CalculateItem(key));
				}
				
				return TouchItem(cacheItem).Value;
			}
			
			set {
				storage[key] = InitItem(key, value);
			}
		}
		
		public object State
		{
			get {
				return storage;
			}
			
			set {
				storage = new Dictionary<TKey,CacheItem>((IDictionary<TKey,CacheItem>)value);
			}
		}			
		
		/// <summary>
		/// 
		/// </summary>
		[Serializable]
		public class CacheItem
		{
			private TValue value;
			
			public CacheItem()
			{
			}
				
			public CacheItem(TValue value)
			{
				this.value = value;
			}
			
			public override string ToString()
			{
				return string.Format("{0}", value);
			}
			
			public TValue Value {
				get { return value; }
				set { this.value = value; }
			}
		}
	}
}
