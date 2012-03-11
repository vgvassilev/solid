package so.controller
{
	import mx.core.Application;
	
	import org.puremvc.as3.interfaces.INotification;
	import org.puremvc.as3.patterns.command.SimpleCommand;
	
	import so.view.MainViewMediator;

	public class ViewPrepCommand extends SimpleCommand
	{
		override public function execute(notification:INotification):void
		{
			facade.registerMediator(new MainViewMediator(Application.application.mainView));
		}
	}
}