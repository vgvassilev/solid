package so.controller
{
	import org.puremvc.as3.interfaces.INotification;
	import org.puremvc.as3.patterns.command.SimpleCommand;
	
	import so.model.ConfigProxy;

	public class ModelPrepCommand extends SimpleCommand
	{
		override public function execute(notification:INotification):void
		{
			facade.registerProxy(new ConfigProxy());
		}
		
	}
}