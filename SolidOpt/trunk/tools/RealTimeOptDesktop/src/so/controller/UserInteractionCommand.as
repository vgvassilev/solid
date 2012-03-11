package so.controller
{
	import org.puremvc.as3.interfaces.INotification;
	import org.puremvc.as3.patterns.command.SimpleCommand;
	
	import so.model.ConfigProxy;

	public class UserInteractionCommand extends SimpleCommand
	{
		override public function execute(notification:INotification):void
		{
			switch(notification.getBody().type)
			{
				case "addUserMethod":
				{
					(facade.retrieveProxy(ConfigProxy.NAME) as ConfigProxy).setUserMethods(notification.getBody().method);
					break;
				}
				case "removeUserMethod":
				{
					(facade.retrieveProxy(ConfigProxy.NAME) as ConfigProxy).removeUserMethod(notification.getBody().method);
					break;
				}
				case "clearAllUserMethods":
				{
					(facade.retrieveProxy(ConfigProxy.NAME) as ConfigProxy).clearAllUserMethods();
					break;
				}
			}
		}
	}
}