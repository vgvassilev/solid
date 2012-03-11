package so
{
	import org.puremvc.as3.patterns.facade.Facade;
	
	import so.controller.StartupCommand;
	import so.controller.UserInteractionCommand;

	public class ApplicationFacade extends Facade
	{
		public static const STARTUP:String = "STARTUP";
		public static const SERVICE_READY:String = "SERVICE_READY";
		public static const USER_METHODS_CHANGED:String = "USER_METHODS_CHANGED";
		public static const USER_METHODS_READY:String = "USER_METHODS_READY";
				
		public function ApplicationFacade()
		{
			super();
		}
		
		public static function getInstance():ApplicationFacade
		{
			if (instance == null)
				instance = new ApplicationFacade();
				
			return instance as ApplicationFacade;
		}
		
		override protected function initializeController():void
		{
			super.initializeController();
			registerCommand(STARTUP, StartupCommand);
			registerCommand(USER_METHODS_CHANGED, UserInteractionCommand);
		}
		
		public function startup(app:SolidOptDesktop):void
		{
			sendNotification(STARTUP, app);
		}
	}
}