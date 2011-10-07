/**
 * GetTransformMethodsResultEvent.as
 * This file was auto-generated from WSDL
 * Any change made to this file will be overwritten when the code is re-generated.
*/
package org.webservices
{
	import mx.utils.ObjectProxy;
	import flash.events.Event;
	import flash.utils.ByteArray;
	import mx.rpc.soap.types.*;
	/**
	 * Typed event handler for the result of the operation
	 */
    
	public class GetTransformMethodsResultEvent extends Event
	{
		/**
		 * The event type value
		 */
		public static var GetTransformMethods_RESULT:String="GetTransformMethods_result";
		/**
		 * Constructor for the new event type
		 */
		public function GetTransformMethodsResultEvent()
		{
			super(GetTransformMethods_RESULT,false,false);
		}
        
		private var _headers:Object;
		private var _result:TransformMethods;
		public function get result():TransformMethods
		{
			return _result;
		}

		public function set result(value:TransformMethods):void
		{
			_result = value;
		}

		public function get headers():Object
		{
			return _headers;
		}

		public function set headers(value:Object):void
		{
			_headers = value;
		}
	}
}