package so.model
{
	import mx.collections.ArrayCollection;
	
	import org.puremvc.as3.interfaces.IProxy;
	import org.puremvc.as3.patterns.proxy.Proxy;
	import org.webservices.GetTransformMethodsResultEvent;
	import org.webservices.SolidOptService;
	
	import so.ApplicationFacade;
	import so.model.vo.MethodVO;
	import so.model.vo.UserMethodVO;
	import so.utils.HashMap;

	public class ConfigProxy extends Proxy implements IProxy
	{
		public static const NAME:String = "ConfigProxy";
		
		public var userMethods:ArrayCollection = new ArrayCollection()
		
		public function ConfigProxy(data:Object=null)
		{
			super(NAME, data);
			loadService();
		}
		
		private function loadService():void
		{
			var soService:SolidOptService = new SolidOptService();
			soService.getTransformMethods();
			soService.addgetTransformMethodsEventListener(getTransformMethods_eventHandler);
		}
		
		private function getTransformMethods_eventHandler(event:GetTransformMethodsResultEvent):void
		{
			var transformMethods:ArrayCollection = event.result;
			var preparedMethods:ArrayCollection = new ArrayCollection();
			
			var methodsMap:HashMap = new HashMap();
			
			var methodVO:MethodVO;
			
			for (var i:int=0; i < transformMethods.length; i++)
			{	
				if(methodsMap.containsKey(transformMethods[i].Caption))
				{
					methodVO = methodsMap.getValue(transformMethods[i].Caption);
					methodVO.versionsCollection.addItem(transformMethods[i].Version + " " + transformMethods[i].Status);
					methodVO.configParams.addItem(transformMethods[i].ConfigParamsDef);
				}
				else
				{
					methodVO = new MethodVO();
				
					methodVO.methodName = transformMethods[i].Caption;
					methodVO.description = transformMethods[i].Description;
					methodVO.selectedVersion = transformMethods[i].Version + " " + transformMethods[i].Status;
					methodVO.versionsCollection.addItem(transformMethods[i].Version + " " + transformMethods[i].Status);
					methodVO.configParams.addItem(transformMethods[i].ConfigParamsDef);
					
					methodsMap.put(transformMethods[i].Caption, methodVO);
					
					preparedMethods.addItem(methodVO);
				}			
			}
			
			facade.sendNotification(ApplicationFacade.SERVICE_READY, {preparedMethods: preparedMethods});
		}
		
		public function setUserMethods(method:MethodVO):void
		{
			var userMethod:UserMethodVO = new UserMethodVO(method)
			userMethods.addItem(userMethod);
			
			facade.sendNotification(ApplicationFacade.USER_METHODS_READY, {userMethods: userMethods});
		}
		
		public function removeUserMethod(method:UserMethodVO):void
		{
			trace ("item index: " + userMethods.getItemIndex(method));
			 
			userMethods.removeItemAt(userMethods.getItemIndex(method));
			
			facade.sendNotification(ApplicationFacade.USER_METHODS_READY, {userMethods: userMethods});
		}
		
		public function clearAllUserMethods():void
		{
			userMethods.removeAll();
			
			facade.sendNotification(ApplicationFacade.USER_METHODS_READY, {userMethods: userMethods});
		}
	}
}