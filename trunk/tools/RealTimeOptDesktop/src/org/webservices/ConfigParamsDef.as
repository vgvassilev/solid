/**
 * ConfigParamsDef.as
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

	public class ConfigParamsDef extends ArrayCollection
	{
		/**
		 * Constructor - initializes the array collection based on a source array
		 */
        
		public function ConfigParamsDef(source:Array = null)
		{
			super(source);
		}
        
        
		public function addConfigParamDefAt(item:ConfigParamDef,index:int):void 
		{
			addItemAt(item,index);
		}

		public function addConfigParamDef(item:ConfigParamDef):void 
		{
			addItem(item);
		} 

		public function getConfigParamDefAt(index:int):ConfigParamDef 
		{
			return getItemAt(index) as ConfigParamDef;
		}

		public function getConfigParamDefIndex(item:ConfigParamDef):int 
		{
			return getItemIndex(item);
		}

		public function setConfigParamDefAt(item:ConfigParamDef,index:int):void 
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
