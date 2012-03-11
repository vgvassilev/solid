/**
 * ConfigParams.as
 * This file was auto-generated from WSDL by the Apache Axis2 generator modified by Adobe
 * Any change made to this file will be overwritten when the code is re-generated.
 */
package org.webservices
{
	import mx.utils.ObjectProxy;
	import mx.collections.ArrayCollection;
	import mx.collections.IList;
	import mx.collections.ICollectionView;
	import mx.rpc.soap.types.*;
	/**
	 * Typed array collection
	 */

	public class ConfigParams extends ArrayCollection
	{
		/**
		 * Constructor - initializes the array collection based on a source array
		 */
        
		public function ConfigParams(source:Array = null)
		{
			super(source);
		}
        
        
		public function addConfigParamAt(item:ConfigParam,index:int):void 
		{
			addItemAt(item,index);
		}

		public function addConfigParam(item:ConfigParam):void 
		{
			addItem(item);
		} 

		public function getConfigParamAt(index:int):ConfigParam 
		{
			return getItemAt(index) as ConfigParam;
		}

		public function getConfigParamIndex(item:ConfigParam):int 
		{
			return getItemIndex(item);
		}

		public function setConfigParamAt(item:ConfigParam,index:int):void 
		{
			setItemAt(item,index);
		}

		public function asIList():IList 
		{
			return this as IList;
		}
        
		public function asICollectionView():ICollectionView 
		{
			return this as ICollectionView;
		}
	}
}
