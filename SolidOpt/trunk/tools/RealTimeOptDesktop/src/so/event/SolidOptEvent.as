package so.event
{
	import flash.events.Event;
	
	import so.model.vo.MethodVO;
	import so.model.vo.UserMethodVO;

	public class SolidOptEvent extends Event
	{
		public static const TRANSFORM_METHOD_SELECTED:String = "TRANSFORM_METHOD_SELECTED";
		public static const REMOVE_USER_METHOD:String = "REMOVE_USER_METHOD";
		public static const CLEAR_ALL_USER_METHODS:String = "CLEAR_ALL_USER_METHODS";
		
		public var selectedMethod:MethodVO = null;
		public var userMethod:UserMethodVO = null;
		
		public function SolidOptEvent(type:String, bubbles:Boolean=false, cancelable:Boolean=false)
		{
			super(type, bubbles, cancelable);
		}
		
	}
}