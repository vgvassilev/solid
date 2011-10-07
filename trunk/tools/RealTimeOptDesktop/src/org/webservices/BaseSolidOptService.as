/**
 * BaseSolidOptServiceService.as
 * This file was auto-generated from WSDL by the Apache Axis2 generator modified by Adobe
 * Any change made to this file will be overwritten when the code is re-generated.
 */
package org.webservices
{
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.net.URLLoader;
	import flash.net.URLRequest;
	import flash.utils.getDefinitionByName;
	import flash.utils.getQualifiedClassName;
	import mx.controls.treeClasses.DefaultDataDescriptor;
	import mx.utils.ObjectUtil;
	import mx.utils.ObjectProxy;
	import mx.messaging.events.MessageFaultEvent;
	import mx.messaging.MessageResponder;
	import mx.messaging.messages.SOAPMessage;
	import mx.messaging.messages.ErrorMessage;
   	import mx.messaging.ChannelSet;
	import mx.messaging.channels.DirectHTTPChannel;
	import mx.rpc.*;
	import mx.rpc.events.*;
	import mx.rpc.soap.*;
	import mx.rpc.wsdl.*;
	import mx.rpc.xml.*;
	import mx.rpc.soap.types.*;
	import mx.collections.ArrayCollection;
	
	/**
	 * Base service implementation, extends the AbstractWebService and adds specific functionality for the selected WSDL
	 * It defines the options and properties for each of the WSDL's operations
	 */ 
	public class BaseSolidOptService extends AbstractWebService
    {
		private var results:Object;
		private var schemaMgr:SchemaManager;
		private var BaseSolidOptServiceService:WSDLService;
		private var BaseSolidOptServicePortType:WSDLPortType;
		private var BaseSolidOptServiceBinding:WSDLBinding;
		private var BaseSolidOptServicePort:WSDLPort;
		private var currentOperation:WSDLOperation;
		private var internal_schema:BaseSolidOptServiceSchema;
	
		/**
		 * Constructor for the base service, initializes all of the WSDL's properties
		 * @param [Optional] The LCDS destination (if available) to use to contact the server
		 * @param [Optional] The URL to the WSDL end-point
		 */
		public function BaseSolidOptService(destination:String=null, rootURL:String=null)
		{
			super(destination, rootURL);
			if(destination == null)
			{
				//no destination available; must set it to go directly to the target
				this.useProxy = false;
			}
			else
			{
				//specific destination requested; must set proxying to true
				this.useProxy = true;
			}
			
			if(rootURL != null)
			{
				this.endpointURI = rootURL;
			} 
			else 
			{
				this.endpointURI = null;
			}
			internal_schema = new BaseSolidOptServiceSchema();
			schemaMgr = new SchemaManager();
			for(var i:int;i<internal_schema.schemas.length;i++)
			{
				internal_schema.schemas[i].targetNamespace=internal_schema.targetNamespaces[i];
				schemaMgr.addSchema(internal_schema.schemas[i]);
			}
BaseSolidOptServiceService = new WSDLService("BaseSolidOptServiceService");
			BaseSolidOptServicePort = new WSDLPort("BaseSolidOptServicePort",BaseSolidOptServiceService);
        	BaseSolidOptServiceBinding = new WSDLBinding("BaseSolidOptServiceBinding");
	        BaseSolidOptServicePortType = new WSDLPortType("BaseSolidOptServicePortType");
       		BaseSolidOptServiceBinding.portType = BaseSolidOptServicePortType;
       		BaseSolidOptServicePort.binding = BaseSolidOptServiceBinding;
       		BaseSolidOptServiceService.addPort(BaseSolidOptServicePort);
       		BaseSolidOptServicePort.endpointURI = "http://localhost/SolidOptWebService/SolidOptService.asmx";
       		if(this.endpointURI == null)
       		{
       			this.endpointURI = BaseSolidOptServicePort.endpointURI; 
       		} 
       		
			var requestMessage:WSDLMessage;
			var responseMessage:WSDLMessage;
			//define the WSDLOperation: new WSDLOperation(methodName)
            var login:WSDLOperation = new WSDLOperation("Login");
				//input message for the operation
    	        requestMessage = new WSDLMessage("Login");
            				requestMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","userName"),null,new QName("http://www.w3.org/2001/XMLSchema","string")));
            				requestMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","password"),null,new QName("http://www.w3.org/2001/XMLSchema","string")));
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
	            requestMessage.isWrapped = true;
	            requestMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","Login");
                
                responseMessage = new WSDLMessage("LoginResponse");
            				responseMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","LoginResult"),null,new QName("http://www.w3.org/2001/XMLSchema","boolean")));
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				
	            responseMessage.isWrapped = true;
	            responseMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","LoginResponse");
			login.inputMessage = requestMessage;
	        login.outputMessage = responseMessage;
            login.schemaManager = this.schemaMgr;
            login.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/Login";
            login.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(login);
			//define the WSDLOperation: new WSDLOperation(methodName)
            var logout:WSDLOperation = new WSDLOperation("Logout");
				//input message for the operation
    	        requestMessage = new WSDLMessage("Logout");
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
                
                responseMessage = new WSDLMessage("LogoutResponse");
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				logout.inputMessage = requestMessage;
	        logout.outputMessage = responseMessage;
            logout.schemaManager = this.schemaMgr;
            logout.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/Logout";
            logout.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(logout);
			//define the WSDLOperation: new WSDLOperation(methodName)
            var getTransformMethods:WSDLOperation = new WSDLOperation("GetTransformMethods");
				//input message for the operation
    	        requestMessage = new WSDLMessage("GetTransformMethods");
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
                
                responseMessage = new WSDLMessage("GetTransformMethodsResponse");
            				responseMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","GetTransformMethodsResult"),null,new QName("http://www.solidopt.org/ws/SolidOptService.asmx","TransformMethods")));
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				
	            responseMessage.isWrapped = true;
	            responseMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","GetTransformMethodsResponse");
			getTransformMethods.inputMessage = requestMessage;
	        getTransformMethods.outputMessage = responseMessage;
            getTransformMethods.schemaManager = this.schemaMgr;
            getTransformMethods.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/GetTransformMethods";
            getTransformMethods.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(getTransformMethods);
			//define the WSDLOperation: new WSDLOperation(methodName)
            var newOptimization:WSDLOperation = new WSDLOperation("NewOptimization");
				//input message for the operation
    	        requestMessage = new WSDLMessage("NewOptimization");
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
                
                responseMessage = new WSDLMessage("NewOptimizationResponse");
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				newOptimization.inputMessage = requestMessage;
	        newOptimization.outputMessage = responseMessage;
            newOptimization.schemaManager = this.schemaMgr;
            newOptimization.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/NewOptimization";
            newOptimization.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(newOptimization);
			//define the WSDLOperation: new WSDLOperation(methodName)
            var addOptimizationURI:WSDLOperation = new WSDLOperation("AddOptimizationURI");
				//input message for the operation
    	        requestMessage = new WSDLMessage("AddOptimizationURI");
            				requestMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","uri"),null,new QName("http://www.w3.org/2001/XMLSchema","string")));
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
	            requestMessage.isWrapped = true;
	            requestMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","AddOptimizationURI");
                
                responseMessage = new WSDLMessage("AddOptimizationURIResponse");
            				responseMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","AddOptimizationURIResult"),null,new QName("http://www.w3.org/2001/XMLSchema","boolean")));
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				
	            responseMessage.isWrapped = true;
	            responseMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","AddOptimizationURIResponse");
			addOptimizationURI.inputMessage = requestMessage;
	        addOptimizationURI.outputMessage = responseMessage;
            addOptimizationURI.schemaManager = this.schemaMgr;
            addOptimizationURI.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/AddOptimizationURI";
            addOptimizationURI.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(addOptimizationURI);
			//define the WSDLOperation: new WSDLOperation(methodName)
            var addOptimizationFile:WSDLOperation = new WSDLOperation("AddOptimizationFile");
				//input message for the operation
    	        requestMessage = new WSDLMessage("AddOptimizationFile");
            				requestMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","binary"),null,new QName("http://www.w3.org/2001/XMLSchema","base64Binary")));
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
	            requestMessage.isWrapped = true;
	            requestMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","AddOptimizationFile");
                
                responseMessage = new WSDLMessage("AddOptimizationFileResponse");
            				responseMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","AddOptimizationFileResult"),null,new QName("http://www.w3.org/2001/XMLSchema","boolean")));
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				
	            responseMessage.isWrapped = true;
	            responseMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","AddOptimizationFileResponse");
			addOptimizationFile.inputMessage = requestMessage;
	        addOptimizationFile.outputMessage = responseMessage;
            addOptimizationFile.schemaManager = this.schemaMgr;
            addOptimizationFile.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/AddOptimizationFile";
            addOptimizationFile.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(addOptimizationFile);
			//define the WSDLOperation: new WSDLOperation(methodName)
            var setOptimizationParams:WSDLOperation = new WSDLOperation("SetOptimizationParams");
				//input message for the operation
    	        requestMessage = new WSDLMessage("SetOptimizationParams");
            				requestMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","config"),null,new QName("http://www.solidopt.org/ws/SolidOptService.asmx","ConfigParams")));
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
	            requestMessage.isWrapped = true;
	            requestMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","SetOptimizationParams");
                
                responseMessage = new WSDLMessage("SetOptimizationParamsResponse");
            				responseMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","SetOptimizationParamsResult"),null,new QName("http://www.w3.org/2001/XMLSchema","boolean")));
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				
	            responseMessage.isWrapped = true;
	            responseMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","SetOptimizationParamsResponse");
			setOptimizationParams.inputMessage = requestMessage;
	        setOptimizationParams.outputMessage = responseMessage;
            setOptimizationParams.schemaManager = this.schemaMgr;
            setOptimizationParams.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/SetOptimizationParams";
            setOptimizationParams.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(setOptimizationParams);
			//define the WSDLOperation: new WSDLOperation(methodName)
            var optimize:WSDLOperation = new WSDLOperation("Optimize");
				//input message for the operation
    	        requestMessage = new WSDLMessage("Optimize");
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
                
                responseMessage = new WSDLMessage("OptimizeResponse");
            				responseMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","OptimizeResult"),null,new QName("http://www.w3.org/2001/XMLSchema","int")));
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				
	            responseMessage.isWrapped = true;
	            responseMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","OptimizeResponse");
			optimize.inputMessage = requestMessage;
	        optimize.outputMessage = responseMessage;
            optimize.schemaManager = this.schemaMgr;
            optimize.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/Optimize";
            optimize.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(optimize);
			//define the WSDLOperation: new WSDLOperation(methodName)
            var getResultURI:WSDLOperation = new WSDLOperation("GetResultURI");
				//input message for the operation
    	        requestMessage = new WSDLMessage("GetResultURI");
            				requestMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","result"),null,new QName("http://www.w3.org/2001/XMLSchema","int")));
                requestMessage.encoding = new WSDLEncoding();
                requestMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
			requestMessage.encoding.useStyle="literal";
	            requestMessage.isWrapped = true;
	            requestMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","GetResultURI");
                
                responseMessage = new WSDLMessage("GetResultURIResponse");
            				responseMessage.addPart(new WSDLMessagePart(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","GetResultURIResult"),null,new QName("http://www.w3.org/2001/XMLSchema","string")));
                responseMessage.encoding = new WSDLEncoding();
                responseMessage.encoding.namespaceURI="http://www.solidopt.org/ws/SolidOptService.asmx";
                responseMessage.encoding.useStyle="literal";				
				
	            responseMessage.isWrapped = true;
	            responseMessage.wrappedQName = new QName("http://www.solidopt.org/ws/SolidOptService.asmx","GetResultURIResponse");
			getResultURI.inputMessage = requestMessage;
	        getResultURI.outputMessage = responseMessage;
            getResultURI.schemaManager = this.schemaMgr;
            getResultURI.soapAction = "http://www.solidopt.org/ws/SolidOptService.asmx/GetResultURI";
            getResultURI.style = "document";
            BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.addOperation(getResultURI);
							SchemaTypeRegistry.getInstance().registerCollectionClass(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","TransformMethods"),org.webservices.TransformMethods);
							SchemaTypeRegistry.getInstance().registerClass(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","ConfigParam"),org.webservices.ConfigParam);
							SchemaTypeRegistry.getInstance().registerCollectionClass(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","ConfigParamsDef"),org.webservices.ConfigParamsDef);
							SchemaTypeRegistry.getInstance().registerCollectionClass(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","ConfigParams"),org.webservices.ConfigParams);
							SchemaTypeRegistry.getInstance().registerClass(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","ConfigParamDef"),org.webservices.ConfigParamDef);
							SchemaTypeRegistry.getInstance().registerClass(new QName("http://www.solidopt.org/ws/SolidOptService.asmx","TransformMethod"),org.webservices.TransformMethod);
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 * @param userName* @param password
		 * @return Asynchronous token
		 */
		public function login(userName:String,password:String):AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            out["userName"] = userName;
	            out["password"] = password;
	            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("Login");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 
		 * @return Asynchronous token
		 */
		public function logOut():AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("Logout");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 
		 * @return Asynchronous token
		 */
		public function getTransformMethods():AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("GetTransformMethods");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 
		 * @return Asynchronous token
		 */
		public function newOptimization():AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("NewOptimization");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 * @param uri
		 * @return Asynchronous token
		 */
		public function addOptimizationURI(uri:String):AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            out["uri"] = uri;
	            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("AddOptimizationURI");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 * @param binary
		 * @return Asynchronous token
		 */
		public function addOptimizationFile(binary:flash.utils.ByteArray):AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            out["binary"] = binary;
	            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("AddOptimizationFile");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 * @param config
		 * @return Asynchronous token
		 */
		public function setOptimizationParams(config:ConfigParams):AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            out["config"] = config;
	            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("SetOptimizationParams");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 
		 * @return Asynchronous token
		 */
		public function optimize():AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("Optimize");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
		/**
		 * Performs the low level call to the server for the operation
		 * It passes along the headers and the operation arguments
		 * @param result
		 * @return Asynchronous token
		 */
		public function getResultURI(result:Number):AsyncToken
		{
			var headerArray:Array = new Array();
            var out:Object = new Object();
            out["result"] = result;
	            currentOperation = BaseSolidOptServiceService.getPort("BaseSolidOptServicePort").binding.portType.getOperation("GetResultURI");
            var pc:PendingCall = new PendingCall(out,headerArray);
            call(currentOperation,out,pc.token,pc.headers);
            return pc.token;
		}
        /**
         * Performs the actual call to the remove server
         * It SOAP-encodes the message using the schema and WSDL operation options set above and then calls the server using 
         * an async invoker
         * It also registers internal event handlers for the result / fault cases
         * @private
         */
        private function call(operation:WSDLOperation,args:Object,token:AsyncToken,headers:Array=null):void
        {
	    	var enc:SOAPEncoder = new SOAPEncoder();
	        var soap:Object = new Object;
	        var message:SOAPMessage = new SOAPMessage();
	        enc.wsdlOperation = operation;
	        soap = enc.encodeRequest(args,headers);
	        message.setSOAPAction(operation.soapAction);
	        message.body = soap.toString();
	        message.url=endpointURI;
            var inv:AsyncRequest = new AsyncRequest();
            inv.destination = super.destination;
            //we need this to handle multiple asynchronous calls 
            var wrappedData:Object = new Object();
            wrappedData.operation = currentOperation;
            wrappedData.returnToken = token;
            if(!this.useProxy)
            {
            	var dcs:ChannelSet = new ChannelSet();	
	        	dcs.addChannel(new DirectHTTPChannel("direct_http_channel"));
            	inv.channelSet = dcs;
            }                
            var processRes:AsyncResponder = new AsyncResponder(processResult,faultResult,wrappedData);
            inv.invoke(message,processRes);
		}
        
        /**
         * Internal event handler to process a successful operation call from the server
         * The result is decoded using the schema and operation settings and then the events get passed on to the actual facade that the user employs in the application 
         * @private
         */
		private function processResult(result:Object,wrappedData:Object):void
           {
           		var headers:Object;
           		var token:AsyncToken = wrappedData.returnToken;
                var currentOperation:WSDLOperation = wrappedData.operation;
                var decoder:SOAPDecoder = new SOAPDecoder();
                decoder.resultFormat = "object";
                decoder.headerFormat = "object";
                decoder.multiplePartsFormat = "object";
                decoder.ignoreWhitespace = true;
                decoder.makeObjectsBindable=false;
                decoder.wsdlOperation = currentOperation;
                decoder.schemaManager = currentOperation.schemaManager;
                var body:Object = result.message.body;
                var stringResult:String = String(body);
                if(stringResult == null  || stringResult == "")
                	return;
                var soapResult:SOAPResult = decoder.decodeResponse(result.message.body);
                if(soapResult.isFault)
                {
	                var faults:Array = soapResult.result as Array;
	                for each (var soapFault:Fault in faults)
	                {
		                var soapFaultEvent:FaultEvent = FaultEvent.createEvent(soapFault,token,null);
		                token.dispatchEvent(soapFaultEvent);
	                }
                } else {
	                result = soapResult.result;
	                headers = soapResult.headers;
	                var event:ResultEvent = ResultEvent.createEvent(result,token,null);
	                event.headers = headers;
	                token.dispatchEvent(event);
                }
           }
           	/**
           	 * Handles the cases when there are errors calling the operation on the server
           	 * This is not the case for SOAP faults, which is handled by the SOAP decoder in the result handler
           	 * but more critical errors, like network outage or the impossibility to connect to the server
           	 * The fault is dispatched upwards to the facade so that the user can do something meaningful 
           	 * @private
           	 */
			private function faultResult(error:MessageFaultEvent,token:Object):void
			{
				//when there is a network error the token is actually the wrappedData object from above	
				if(!(token is AsyncToken))
					token = token.returnToken;
				token.dispatchEvent(new FaultEvent(FaultEvent.FAULT,true,true,new Fault(error.faultCode,error.faultString,error.faultDetail)));
			}
		}
	}

	import mx.rpc.AsyncToken;
	import mx.rpc.AsyncResponder;
	import mx.rpc.wsdl.WSDLBinding;
                
    /**
     * Internal class to handle multiple operation call scheduling
     * It allows us to pass data about the operation being encoded / decoded to and from the SOAP encoder / decoder units. 
     * @private
     */
    class PendingCall
    {
		public var args:*;
		public var headers:Array;
		public var token:AsyncToken;
		
		public function PendingCall(args:Object, headers:Array=null)
		{
			this.args = args;
			this.headers = headers;
			this.token = new AsyncToken(null);
		}
	}