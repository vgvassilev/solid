package so.view
{
	import org.puremvc.as3.interfaces.INotification;
	import org.puremvc.as3.patterns.mediator.Mediator;
	
	import so.ApplicationFacade;
	import so.event.SolidOptEvent;

	public class MainViewMediator extends Mediator
	{
		public static const NAME:String = "MainViewMediator";
		
		public function MainViewMediator(viewComponent:Object=null)
		{
			super(NAME, viewComponent);
			mainView.addEventListener(SolidOptEvent.TRANSFORM_METHOD_SELECTED, methodSelected_eventHandler);
			mainView.addEventListener(SolidOptEvent.REMOVE_USER_METHOD, removeUserMethod_eventHandler);
			mainView.addEventListener(SolidOptEvent.CLEAR_ALL_USER_METHODS, clearAllUserMethods_eventHandler);
		}
		
		override public function listNotificationInterests():Array
		{
			return [ApplicationFacade.SERVICE_READY,
					ApplicationFacade.USER_METHODS_READY];
		}
		
		override public function handleNotification(notification:INotification):void
		{
			switch (notification.getName())
			{
				case ApplicationFacade.SERVICE_READY:
				{
					mainView.preparedMethodsList.dataProvider = notification.getBody().preparedMethods;
					break;
				}
				case ApplicationFacade.USER_METHODS_READY:
				{
					mainView.userMethodsList.dataProvider = notification.getBody().userMethods;
					break;
				}
			}
		}
		
		private function get mainView():MainView
		{
			return viewComponent as MainView;
		}
		
		private function methodSelected_eventHandler(event:SolidOptEvent):void
		{
			facade.sendNotification(ApplicationFacade.USER_METHODS_CHANGED, {type: "addUserMethod", method: event.selectedMethod});
		}
		
		private function removeUserMethod_eventHandler(event:SolidOptEvent):void
		{
			facade.sendNotification(ApplicationFacade.USER_METHODS_CHANGED, {type: "removeUserMethod", method: event.userMethod});
		}
		
		private function clearAllUserMethods_eventHandler(event:SolidOptEvent):void
		{
			facade.sendNotification(ApplicationFacade.USER_METHODS_CHANGED, {type: "clearAllUserMethods"});
		}
	}
}