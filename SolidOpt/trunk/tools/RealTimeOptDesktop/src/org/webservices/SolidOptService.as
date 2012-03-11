/**
 * SolidOptServiceService.as
 * This file was auto-generated from WSDL by the Apache Axis2 generator modified by Adobe
 * Any change made to this file will be overwritten when the code is re-generated.
 */
 /**
  * Usage example: to use this service from within your Flex application you have two choices:
  * Use it via Actionscript only
  * Use it via MXML tags
  * Actionscript sample code:
  * Step 1: create an instance of the service; pass it the LCDS destination string if any
  * var myService:SolidOptService= new SolidOptService();
  * Step 2: for the desired operation add a result handler (a function that you have already defined previously)  
  * myService.addLoginEventListener(myResultHandlingFunction);
  * Step 3: Call the operation as a method on the service. Pass the right values as arguments:
  * myService.Login(myuserName,mypassword);
  *
  * MXML sample code:
  * First you need to map the package where the files were generated to a namespace, usually on the <mx:Application> tag, 
  * like this: xmlns:srv="org.webservices.*"
  * Define the service and within its tags set the request wrapper for the desired operation
  * <srv:SolidOptService id="myService">
  *   <srv:Login_request_var>
  *		<srv:Login_request userName=myValue,password=myValue/>
  *   </srv:Login_request_var>
  * </srv:SolidOptService>
  * Then call the operation for which you have set the request wrapper value above, like this:
  * <mx:Button id="myButton" label="Call operation" click="myService.Login_send()" />
  */
package org.webservices
{
	import mx.rpc.AsyncToken;
	import flash.events.EventDispatcher;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.events.FaultEvent;
	import flash.utils.ByteArray;
	import mx.rpc.soap.types.*;

    /**
     * Dispatches when a call to the operation Login completes with success
     * and returns some data
     * @eventType LoginResultEvent
     */
    [Event(name="Login_result", type="org.webservices.LoginResultEvent")]
    
    /**
     * Dispatches when a call to the operation Logout completes with success
     * and returns some data
     * @eventType LogoutResultEvent
     */
    [Event(name="Logout_result", type="org.webservices.LogoutResultEvent")]
    
    /**
     * Dispatches when a call to the operation GetTransformMethods completes with success
     * and returns some data
     * @eventType GetTransformMethodsResultEvent
     */
    [Event(name="GetTransformMethods_result", type="org.webservices.GetTransformMethodsResultEvent")]
    
    /**
     * Dispatches when a call to the operation NewOptimization completes with success
     * and returns some data
     * @eventType NewOptimizationResultEvent
     */
    [Event(name="NewOptimization_result", type="org.webservices.NewOptimizationResultEvent")]
    
    /**
     * Dispatches when a call to the operation AddOptimizationURI completes with success
     * and returns some data
     * @eventType AddOptimizationURIResultEvent
     */
    [Event(name="AddOptimizationURI_result", type="org.webservices.AddOptimizationURIResultEvent")]
    
    /**
     * Dispatches when a call to the operation AddOptimizationFile completes with success
     * and returns some data
     * @eventType AddOptimizationFileResultEvent
     */
    [Event(name="AddOptimizationFile_result", type="org.webservices.AddOptimizationFileResultEvent")]
    
    /**
     * Dispatches when a call to the operation SetOptimizationParams completes with success
     * and returns some data
     * @eventType SetOptimizationParamsResultEvent
     */
    [Event(name="SetOptimizationParams_result", type="org.webservices.SetOptimizationParamsResultEvent")]
    
    /**
     * Dispatches when a call to the operation Optimize completes with success
     * and returns some data
     * @eventType OptimizeResultEvent
     */
    [Event(name="Optimize_result", type="org.webservices.OptimizeResultEvent")]
    
    /**
     * Dispatches when a call to the operation GetResultURI completes with success
     * and returns some data
     * @eventType GetResultURIResultEvent
     */
    [Event(name="GetResultURI_result", type="org.webservices.GetResultURIResultEvent")]
    
	/**
	 * Dispatches when the operation that has been called fails. The fault event is common for all operations
	 * of the WSDL
	 * @eventType mx.rpc.events.FaultEvent
	 */
    [Event(name="fault", type="mx.rpc.events.FaultEvent")]

	public class SolidOptService extends EventDispatcher implements ISolidOptService
	{
    	private var _baseService:BaseSolidOptService;
        
        /**
         * Constructor for the facade; sets the destination and create a baseService instance
         * @param The LCDS destination (if any) associated with the imported WSDL
         */  
        public function SolidOptService(destination:String=null,rootURL:String=null)
        {
        	_baseService = new BaseSolidOptService(destination,rootURL);
        }
        
		//stub functions for the Login operation
          

        /**
         * @see ISolidOptService#Login()
         */
        public function login(userName:String,password:String):AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.login(userName,password);
            _internal_token.addEventListener("result",_Login_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#Login_send()
		 */    
        public function login_send():AsyncToken
        {
        	return login(_Login_request.userName,_Login_request.password);
        }
              
		/**
		 * Internal representation of the request wrapper for the operation
		 * @private
		 */
		private var _Login_request:Login_request;
		/**
		 * @see ISolidOptService#Login_request_var
		 */
		[Bindable]
		public function get login_request_var():Login_request
		{
			return _Login_request;
		}
		
		/**
		 * @private
		 */
		public function set login_request_var(request:Login_request):void
		{
			_Login_request = request;
		}
		
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _login_lastResult:Boolean;
		[Bindable]
		/**
		 * @see ISolidOptService#Login_lastResult
		 */	  
		public function get login_lastResult():Boolean
		{
			return _login_lastResult;
		}
		/**
		 * @private
		 */
		public function set login_lastResult(lastResult:Boolean):void
		{
			_login_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addLogin()
		 */
		public function addloginEventListener(listener:Function):void
		{
			addEventListener(LoginResultEvent.Login_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _Login_populate_results(event:ResultEvent):void
		{
			var e:LoginResultEvent = new LoginResultEvent();
		            e.result = event.result as Boolean;
		                       e.headers = event.headers;
		             login_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//stub functions for the Logout operation
          

        /**
         * @see ISolidOptService#Logout()
         */
        public function logout():AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.logOut();
            _internal_token.addEventListener("result",_Logout_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#Logout_send()
		 */    
        public function logout_send():AsyncToken
        {
        	return logout();
        }
              
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _logout_lastResult:Object;
		[Bindable]
		/**
		 * @see ISolidOptService#Logout_lastResult
		 */	  
		public function get logout_lastResult():Object
		{
			return _logout_lastResult;
		}
		/**
		 * @private
		 */
		public function set logout_lastResult(lastResult:Object):void
		{
			_logout_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addLogout()
		 */
		public function addlogoutEventListener(listener:Function):void
		{
			addEventListener(LogoutResultEvent.Logout_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _Logout_populate_results(event:ResultEvent):void
		{
			var e:LogoutResultEvent = new LogoutResultEvent();
		            e.result = event.result as Object;
		                       e.headers = event.headers;
		             logout_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//stub functions for the GetTransformMethods operation
          

        /**
         * @see ISolidOptService#GetTransformMethods()
         */
        public function getTransformMethods():AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.getTransformMethods();
            _internal_token.addEventListener("result",_GetTransformMethods_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#GetTransformMethods_send()
		 */    
        public function getTransformMethods_send():AsyncToken
        {
        	return getTransformMethods();
        }
              
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _getTransformMethods_lastResult:TransformMethods;
		[Bindable]
		/**
		 * @see ISolidOptService#GetTransformMethods_lastResult
		 */	  
		public function get getTransformMethods_lastResult():TransformMethods
		{
			return _getTransformMethods_lastResult;
		}
		/**
		 * @private
		 */
		public function set getTransformMethods_lastResult(lastResult:TransformMethods):void
		{
			_getTransformMethods_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addGetTransformMethods()
		 */
		public function addgetTransformMethodsEventListener(listener:Function):void
		{
			addEventListener(GetTransformMethodsResultEvent.GetTransformMethods_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _GetTransformMethods_populate_results(event:ResultEvent):void
		{
			var e:GetTransformMethodsResultEvent = new GetTransformMethodsResultEvent();
		            e.result = event.result as TransformMethods;
		                       e.headers = event.headers;
		             getTransformMethods_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//stub functions for the NewOptimization operation
          

        /**
         * @see ISolidOptService#NewOptimization()
         */
        public function newOptimization():AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.newOptimization();
            _internal_token.addEventListener("result",_NewOptimization_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#NewOptimization_send()
		 */    
        public function newOptimization_send():AsyncToken
        {
        	return newOptimization();
        }
              
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _newOptimization_lastResult:Object;
		[Bindable]
		/**
		 * @see ISolidOptService#NewOptimization_lastResult
		 */	  
		public function get newOptimization_lastResult():Object
		{
			return _newOptimization_lastResult;
		}
		/**
		 * @private
		 */
		public function set newOptimization_lastResult(lastResult:Object):void
		{
			_newOptimization_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addNewOptimization()
		 */
		public function addnewOptimizationEventListener(listener:Function):void
		{
			addEventListener(NewOptimizationResultEvent.NewOptimization_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _NewOptimization_populate_results(event:ResultEvent):void
		{
			var e:NewOptimizationResultEvent = new NewOptimizationResultEvent();
		            e.result = event.result as Object;
		                       e.headers = event.headers;
		             newOptimization_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//stub functions for the AddOptimizationURI operation
          

        /**
         * @see ISolidOptService#AddOptimizationURI()
         */
        public function addOptimizationURI(uri:String):AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.addOptimizationURI(uri);
            _internal_token.addEventListener("result",_AddOptimizationURI_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#AddOptimizationURI_send()
		 */    
        public function addOptimizationURI_send():AsyncToken
        {
        	return addOptimizationURI(_AddOptimizationURI_request.uri);
        }
              
		/**
		 * Internal representation of the request wrapper for the operation
		 * @private
		 */
		private var _AddOptimizationURI_request:AddOptimizationURI_request;
		/**
		 * @see ISolidOptService#AddOptimizationURI_request_var
		 */
		[Bindable]
		public function get addOptimizationURI_request_var():AddOptimizationURI_request
		{
			return _AddOptimizationURI_request;
		}
		
		/**
		 * @private
		 */
		public function set addOptimizationURI_request_var(request:AddOptimizationURI_request):void
		{
			_AddOptimizationURI_request = request;
		}
		
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _addOptimizationURI_lastResult:Boolean;
		[Bindable]
		/**
		 * @see ISolidOptService#AddOptimizationURI_lastResult
		 */	  
		public function get addOptimizationURI_lastResult():Boolean
		{
			return _addOptimizationURI_lastResult;
		}
		/**
		 * @private
		 */
		public function set addOptimizationURI_lastResult(lastResult:Boolean):void
		{
			_addOptimizationURI_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addAddOptimizationURI()
		 */
		public function addaddOptimizationURIEventListener(listener:Function):void
		{
			addEventListener(AddOptimizationURIResultEvent.AddOptimizationURI_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _AddOptimizationURI_populate_results(event:ResultEvent):void
		{
			var e:AddOptimizationURIResultEvent = new AddOptimizationURIResultEvent();
		            e.result = event.result as Boolean;
		                       e.headers = event.headers;
		             addOptimizationURI_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//stub functions for the AddOptimizationFile operation
          

        /**
         * @see ISolidOptService#AddOptimizationFile()
         */
        public function addOptimizationFile(binary:flash.utils.ByteArray):AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.addOptimizationFile(binary);
            _internal_token.addEventListener("result",_AddOptimizationFile_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#AddOptimizationFile_send()
		 */    
        public function addOptimizationFile_send():AsyncToken
        {
        	return addOptimizationFile(_AddOptimizationFile_request.binary);
        }
              
		/**
		 * Internal representation of the request wrapper for the operation
		 * @private
		 */
		private var _AddOptimizationFile_request:AddOptimizationFile_request;
		/**
		 * @see ISolidOptService#AddOptimizationFile_request_var
		 */
		[Bindable]
		public function get addOptimizationFile_request_var():AddOptimizationFile_request
		{
			return _AddOptimizationFile_request;
		}
		
		/**
		 * @private
		 */
		public function set addOptimizationFile_request_var(request:AddOptimizationFile_request):void
		{
			_AddOptimizationFile_request = request;
		}
		
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _addOptimizationFile_lastResult:Boolean;
		[Bindable]
		/**
		 * @see ISolidOptService#AddOptimizationFile_lastResult
		 */	  
		public function get addOptimizationFile_lastResult():Boolean
		{
			return _addOptimizationFile_lastResult;
		}
		/**
		 * @private
		 */
		public function set addOptimizationFile_lastResult(lastResult:Boolean):void
		{
			_addOptimizationFile_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addAddOptimizationFile()
		 */
		public function addaddOptimizationFileEventListener(listener:Function):void
		{
			addEventListener(AddOptimizationFileResultEvent.AddOptimizationFile_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _AddOptimizationFile_populate_results(event:ResultEvent):void
		{
			var e:AddOptimizationFileResultEvent = new AddOptimizationFileResultEvent();
		            e.result = event.result as Boolean;
		                       e.headers = event.headers;
		             addOptimizationFile_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//stub functions for the SetOptimizationParams operation
          

        /**
         * @see ISolidOptService#SetOptimizationParams()
         */
        public function setOptimizationParams(config:ConfigParams):AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.setOptimizationParams(config);
            _internal_token.addEventListener("result",_SetOptimizationParams_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#SetOptimizationParams_send()
		 */    
        public function setOptimizationParams_send():AsyncToken
        {
        	return setOptimizationParams(_SetOptimizationParams_request.config);
        }
              
		/**
		 * Internal representation of the request wrapper for the operation
		 * @private
		 */
		private var _SetOptimizationParams_request:SetOptimizationParams_request;
		/**
		 * @see ISolidOptService#SetOptimizationParams_request_var
		 */
		[Bindable]
		public function get setOptimizationParams_request_var():SetOptimizationParams_request
		{
			return _SetOptimizationParams_request;
		}
		
		/**
		 * @private
		 */
		public function set setOptimizationParams_request_var(request:SetOptimizationParams_request):void
		{
			_SetOptimizationParams_request = request;
		}
		
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _setOptimizationParams_lastResult:Boolean;
		[Bindable]
		/**
		 * @see ISolidOptService#SetOptimizationParams_lastResult
		 */	  
		public function get setOptimizationParams_lastResult():Boolean
		{
			return _setOptimizationParams_lastResult;
		}
		/**
		 * @private
		 */
		public function set setOptimizationParams_lastResult(lastResult:Boolean):void
		{
			_setOptimizationParams_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addSetOptimizationParams()
		 */
		public function addsetOptimizationParamsEventListener(listener:Function):void
		{
			addEventListener(SetOptimizationParamsResultEvent.SetOptimizationParams_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _SetOptimizationParams_populate_results(event:ResultEvent):void
		{
			var e:SetOptimizationParamsResultEvent = new SetOptimizationParamsResultEvent();
		            e.result = event.result as Boolean;
		                       e.headers = event.headers;
		             setOptimizationParams_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//stub functions for the Optimize operation
          

        /**
         * @see ISolidOptService#Optimize()
         */
        public function optimize():AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.optimize();
            _internal_token.addEventListener("result",_Optimize_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#Optimize_send()
		 */    
        public function optimize_send():AsyncToken
        {
        	return optimize();
        }
              
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _optimize_lastResult:Number;
		[Bindable]
		/**
		 * @see ISolidOptService#Optimize_lastResult
		 */	  
		public function get optimize_lastResult():Number
		{
			return _optimize_lastResult;
		}
		/**
		 * @private
		 */
		public function set optimize_lastResult(lastResult:Number):void
		{
			_optimize_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addOptimize()
		 */
		public function addoptimizeEventListener(listener:Function):void
		{
			addEventListener(OptimizeResultEvent.Optimize_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _Optimize_populate_results(event:ResultEvent):void
		{
			var e:OptimizeResultEvent = new OptimizeResultEvent();
		            e.result = event.result as Number;
		                       e.headers = event.headers;
		             optimize_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//stub functions for the GetResultURI operation
          

        /**
         * @see ISolidOptService#GetResultURI()
         */
        public function getResultURI(result:Number):AsyncToken
        {
         	var _internal_token:AsyncToken = _baseService.getResultURI(result);
            _internal_token.addEventListener("result",_GetResultURI_populate_results);
            _internal_token.addEventListener("fault",throwFault); 
            return _internal_token;
		}
        /**
		 * @see ISolidOptService#GetResultURI_send()
		 */    
        public function getResultURI_send():AsyncToken
        {
        	return getResultURI(_GetResultURI_request.result);
        }
              
		/**
		 * Internal representation of the request wrapper for the operation
		 * @private
		 */
		private var _GetResultURI_request:GetResultURI_request;
		/**
		 * @see ISolidOptService#GetResultURI_request_var
		 */
		[Bindable]
		public function get getResultURI_request_var():GetResultURI_request
		{
			return _GetResultURI_request;
		}
		
		/**
		 * @private
		 */
		public function set getResultURI_request_var(request:GetResultURI_request):void
		{
			_GetResultURI_request = request;
		}
		
	  		/**
		 * Internal variable to store the operation's lastResult
		 * @private
		 */
        private var _getResultURI_lastResult:String;
		[Bindable]
		/**
		 * @see ISolidOptService#GetResultURI_lastResult
		 */	  
		public function get getResultURI_lastResult():String
		{
			return _getResultURI_lastResult;
		}
		/**
		 * @private
		 */
		public function set getResultURI_lastResult(lastResult:String):void
		{
			_getResultURI_lastResult = lastResult;
		}
		
		/**
		 * @see ISolidOptService#addGetResultURI()
		 */
		public function addgetResultURIEventListener(listener:Function):void
		{
			addEventListener(GetResultURIResultEvent.GetResultURI_RESULT,listener);
		}
			
		/**
		 * @private
		 */
        private function _GetResultURI_populate_results(event:ResultEvent):void
		{
			var e:GetResultURIResultEvent = new GetResultURIResultEvent();
		            e.result = event.result as String;
		                       e.headers = event.headers;
		             getResultURI_lastResult = e.result;
		             dispatchEvent(e);
	        		}
		
		//service-wide functions
		/**
		 * @see ISolidOptService#getWebService()
		 */
		public function getWebService():BaseSolidOptService
		{
			return _baseService;
		}
		
		/**
		 * Set the event listener for the fault event which can be triggered by each of the operations defined by the facade
		 */
		public function addSolidOptServiceFaultEventListener(listener:Function):void
		{
			addEventListener("fault",listener);
		}
		
		/**
		 * Internal function to re-dispatch the fault event passed on by the base service implementation
		 * @private
		 */
		 
		 private function throwFault(event:FaultEvent):void
		 {
		 	dispatchEvent(event);
		 }
    }
}
