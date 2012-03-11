
/**
 * Service.as
 * This file was auto-generated from WSDL by the Apache Axis2 generator modified by Adobe
 * Any change made to this file will be overwritten when the code is re-generated.
 */
package org.webservices{
	import mx.rpc.AsyncToken;
	import flash.utils.ByteArray;
	import mx.rpc.soap.types.*;
               
    public interface ISolidOptService
    {
    	//Stub functions for the Login operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @param userName
    	 * @param password
    	 * @return An AsyncToken
    	 */
    	function login(userName:String,password:String):AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function login_send():AsyncToken;
        
        /**
         * The login operation lastResult property
         */
        function get login_lastResult():Boolean;
		/**
		 * @private
		 */
        function set login_lastResult(lastResult:Boolean):void;
       /**
        * Add a listener for the login operation successful result event
        * @param The listener function
        */
       function addloginEventListener(listener:Function):void;
       
       
        /**
         * The login operation request wrapper
         */
        function get login_request_var():Login_request;
        
        /**
         * @private
         */
        function set login_request_var(request:Login_request):void;
                   
    	//Stub functions for the Logout operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @return An AsyncToken
    	 */
    	function logout():AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function logout_send():AsyncToken;
        
        /**
         * The logout operation lastResult property
         */
        function get logout_lastResult():Object;
		/**
		 * @private
		 */
        function set logout_lastResult(lastResult:Object):void;
       /**
        * Add a listener for the logout operation successful result event
        * @param The listener function
        */
       function addlogoutEventListener(listener:Function):void;
       
       
    	//Stub functions for the GetTransformMethods operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @return An AsyncToken
    	 */
    	function getTransformMethods():AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function getTransformMethods_send():AsyncToken;
        
        /**
         * The getTransformMethods operation lastResult property
         */
        function get getTransformMethods_lastResult():TransformMethods;
		/**
		 * @private
		 */
        function set getTransformMethods_lastResult(lastResult:TransformMethods):void;
       /**
        * Add a listener for the getTransformMethods operation successful result event
        * @param The listener function
        */
       function addgetTransformMethodsEventListener(listener:Function):void;
       
       
    	//Stub functions for the NewOptimization operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @return An AsyncToken
    	 */
    	function newOptimization():AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function newOptimization_send():AsyncToken;
        
        /**
         * The newOptimization operation lastResult property
         */
        function get newOptimization_lastResult():Object;
		/**
		 * @private
		 */
        function set newOptimization_lastResult(lastResult:Object):void;
       /**
        * Add a listener for the newOptimization operation successful result event
        * @param The listener function
        */
       function addnewOptimizationEventListener(listener:Function):void;
       
       
    	//Stub functions for the AddOptimizationURI operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @param uri
    	 * @return An AsyncToken
    	 */
    	function addOptimizationURI(uri:String):AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function addOptimizationURI_send():AsyncToken;
        
        /**
         * The addOptimizationURI operation lastResult property
         */
        function get addOptimizationURI_lastResult():Boolean;
		/**
		 * @private
		 */
        function set addOptimizationURI_lastResult(lastResult:Boolean):void;
       /**
        * Add a listener for the addOptimizationURI operation successful result event
        * @param The listener function
        */
       function addaddOptimizationURIEventListener(listener:Function):void;
       
       
        /**
         * The addOptimizationURI operation request wrapper
         */
        function get addOptimizationURI_request_var():AddOptimizationURI_request;
        
        /**
         * @private
         */
        function set addOptimizationURI_request_var(request:AddOptimizationURI_request):void;
                   
    	//Stub functions for the AddOptimizationFile operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @param binary
    	 * @return An AsyncToken
    	 */
    	function addOptimizationFile(binary:flash.utils.ByteArray):AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function addOptimizationFile_send():AsyncToken;
        
        /**
         * The addOptimizationFile operation lastResult property
         */
        function get addOptimizationFile_lastResult():Boolean;
		/**
		 * @private
		 */
        function set addOptimizationFile_lastResult(lastResult:Boolean):void;
       /**
        * Add a listener for the addOptimizationFile operation successful result event
        * @param The listener function
        */
       function addaddOptimizationFileEventListener(listener:Function):void;
       
       
        /**
         * The addOptimizationFile operation request wrapper
         */
        function get addOptimizationFile_request_var():AddOptimizationFile_request;
        
        /**
         * @private
         */
        function set addOptimizationFile_request_var(request:AddOptimizationFile_request):void;
                   
    	//Stub functions for the SetOptimizationParams operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @param config
    	 * @return An AsyncToken
    	 */
    	function setOptimizationParams(config:ConfigParams):AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function setOptimizationParams_send():AsyncToken;
        
        /**
         * The setOptimizationParams operation lastResult property
         */
        function get setOptimizationParams_lastResult():Boolean;
		/**
		 * @private
		 */
        function set setOptimizationParams_lastResult(lastResult:Boolean):void;
       /**
        * Add a listener for the setOptimizationParams operation successful result event
        * @param The listener function
        */
       function addsetOptimizationParamsEventListener(listener:Function):void;
       
       
        /**
         * The setOptimizationParams operation request wrapper
         */
        function get setOptimizationParams_request_var():SetOptimizationParams_request;
        
        /**
         * @private
         */
        function set setOptimizationParams_request_var(request:SetOptimizationParams_request):void;
                   
    	//Stub functions for the Optimize operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @return An AsyncToken
    	 */
    	function optimize():AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function optimize_send():AsyncToken;
        
        /**
         * The optimize operation lastResult property
         */
        function get optimize_lastResult():Number;
		/**
		 * @private
		 */
        function set optimize_lastResult(lastResult:Number):void;
       /**
        * Add a listener for the optimize operation successful result event
        * @param The listener function
        */
       function addoptimizeEventListener(listener:Function):void;
       
       
    	//Stub functions for the GetResultURI operation
    	/**
    	 * Call the operation on the server passing in the arguments defined in the WSDL file
    	 * @param result
    	 * @return An AsyncToken
    	 */
    	function getResultURI(result:Number):AsyncToken;
        /**
         * Method to call the operation on the server without passing the arguments inline.
         * You must however set the _request property for the operation before calling this method
         * Should use it in MXML context mostly
         * @return An AsyncToken
         */
        function getResultURI_send():AsyncToken;
        
        /**
         * The getResultURI operation lastResult property
         */
        function get getResultURI_lastResult():String;
		/**
		 * @private
		 */
        function set getResultURI_lastResult(lastResult:String):void;
       /**
        * Add a listener for the getResultURI operation successful result event
        * @param The listener function
        */
       function addgetResultURIEventListener(listener:Function):void;
       
       
        /**
         * The getResultURI operation request wrapper
         */
        function get getResultURI_request_var():GetResultURI_request;
        
        /**
         * @private
         */
        function set getResultURI_request_var(request:GetResultURI_request):void;
                   
        /**
         * Get access to the underlying web service that the stub uses to communicate with the server
         * @return The base service that the facade implements
         */
        function getWebService():BaseSolidOptService;
	}
}