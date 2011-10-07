package so.utils
{
	import mx.managers.SystemManager;
	import mx.core.Singleton;
	import mx.core.IFlexModuleFactory;
	import flash.events.Event;
	import mx.managers.ISystemManager;
	
	import mx.core.mx_internal;
	
	use namespace mx_internal;

	public class FlexDragSystemManager extends SystemManager implements IFlexModuleFactory, ISystemManager
	{
		public function FlexDragSystemManager()
		{
			super();
		}
		
		override mx_internal function docFrameHandler(event:Event = null):void
		{
			Singleton.registerClass("mx.managers::IDragManager",
					Class(getDefinitionByName("mx.managers::DragManagerImpl")));
					
			super.mx_internal::docFrameHandler(event);
		} 
		
		
		
	}
}