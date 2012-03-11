/**
 * TransformMethods.as
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

	public class TransformMethods extends ArrayCollection
	{
		/**
		 * Constructor - initializes the array collection based on a source array
		 */
        
		public function TransformMethods(source:Array = null)
		{
			super(source);
		}
        
        
		public function addTransformMethodAt(item:TransformMethod,index:int):void 
		{
			addItemAt(item,index);
		}

		public function addTransformMethod(item:TransformMethod):void 
		{
			addItem(item);
		} 

		public function getTransformMethodAt(index:int):TransformMethod 
		{
			return getItemAt(index) as TransformMethod;
		}

		public function getTransformMethodIndex(item:TransformMethod):int 
		{
			return getItemIndex(item);
		}

		public function setTransformMethodAt(item:TransformMethod,index:int):void 
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
