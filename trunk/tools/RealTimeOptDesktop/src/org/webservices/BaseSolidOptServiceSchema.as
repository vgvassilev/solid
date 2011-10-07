package org.webservices
{
	 import mx.rpc.xml.Schema
	 public class BaseSolidOptServiceSchema
	{
		 public var schemas:Array = new Array();
		 public var targetNamespaces:Array = new Array();
		 public function BaseSolidOptServiceSchema():void
		{
			 var xsdXML0:XML = <s:schema xmlns:s="http://www.w3.org/2001/XMLSchema" xmlns:http="http://schemas.xmlsoap.org/wsdl/http/" xmlns:mime="http://schemas.xmlsoap.org/wsdl/mime/" xmlns:soap="http://schemas.xmlsoap.org/wsdl/soap/" xmlns:soap12="http://schemas.xmlsoap.org/wsdl/soap12/" xmlns:soapenc="http://schemas.xmlsoap.org/soap/encoding/" xmlns:tm="http://microsoft.com/wsdl/mime/textMatching/" xmlns:tns="http://www.solidopt.org/ws/SolidOptService.asmx" xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/" attributeFormDefault="unqualified" elementFormDefault="qualified" targetNamespace="http://www.solidopt.org/ws/SolidOptService.asmx">
    <s:element name="Login">
        <s:complexType>
            <s:sequence>
                <s:element minOccurs="0" name="userName" type="s:string"/>
                <s:element minOccurs="0" name="password" type="s:string"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="LoginResponse">
        <s:complexType>
            <s:sequence>
                <s:element name="LoginResult" type="s:boolean"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="Logout">
        <s:complexType/>
    </s:element>
    <s:element name="LogoutResponse">
        <s:complexType/>
    </s:element>
    <s:element name="GetTransformMethods">
        <s:complexType/>
    </s:element>
    <s:element name="GetTransformMethodsResponse">
        <s:complexType>
            <s:sequence>
                <s:element minOccurs="0" name="GetTransformMethodsResult" type="tns:TransformMethods"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:complexType name="TransformMethods">
        <s:sequence>
            <s:element maxOccurs="unbounded" minOccurs="0" name="TransformMethod" type="tns:TransformMethod"/>
        </s:sequence>
    </s:complexType>
    <s:complexType name="TransformMethod">
        <s:sequence>
            <s:element minOccurs="0" name="Name" type="s:string"/>
            <s:element minOccurs="0" name="Caption" type="s:string"/>
            <s:element minOccurs="0" name="Version" type="s:string"/>
            <s:element minOccurs="0" name="Status" type="s:string"/>
            <s:element minOccurs="0" name="FullName" type="s:string"/>
            <s:element minOccurs="0" name="Description" type="s:string"/>
            <s:element minOccurs="0" name="ConfigParamsDef" type="tns:ConfigParamsDef"/>
        </s:sequence>
    </s:complexType>
    <s:complexType name="ConfigParamsDef">
        <s:sequence>
            <s:element maxOccurs="unbounded" minOccurs="0" name="ConfigParamDef" type="tns:ConfigParamDef"/>
        </s:sequence>
    </s:complexType>
    <s:complexType name="ConfigParamDef">
        <s:sequence>
            <s:element minOccurs="0" name="Name" type="s:string"/>
            <s:element minOccurs="0" name="Caption" type="s:string"/>
            <s:element minOccurs="0" name="Value"/>
            <s:element minOccurs="0" name="DefaultValue"/>
            <s:element minOccurs="0" name="Description" type="s:string"/>
        </s:sequence>
    </s:complexType>
    <s:element name="NewOptimization">
        <s:complexType/>
    </s:element>
    <s:element name="NewOptimizationResponse">
        <s:complexType/>
    </s:element>
    <s:element name="AddOptimizationURI">
        <s:complexType>
            <s:sequence>
                <s:element minOccurs="0" name="uri" type="s:string"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="AddOptimizationURIResponse">
        <s:complexType>
            <s:sequence>
                <s:element name="AddOptimizationURIResult" type="s:boolean"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="AddOptimizationFile">
        <s:complexType>
            <s:sequence>
                <s:element minOccurs="0" name="binary" type="s:base64Binary"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="AddOptimizationFileResponse">
        <s:complexType>
            <s:sequence>
                <s:element name="AddOptimizationFileResult" type="s:boolean"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="SetOptimizationParams">
        <s:complexType>
            <s:sequence>
                <s:element minOccurs="0" name="config" type="tns:ConfigParams"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:complexType name="ConfigParams">
        <s:sequence>
            <s:element maxOccurs="unbounded" minOccurs="0" name="ConfigParam" type="tns:ConfigParam"/>
        </s:sequence>
    </s:complexType>
    <s:complexType name="ConfigParam">
        <s:sequence>
            <s:element minOccurs="0" name="Name" type="s:string"/>
            <s:element minOccurs="0" name="Value"/>
        </s:sequence>
    </s:complexType>
    <s:element name="SetOptimizationParamsResponse">
        <s:complexType>
            <s:sequence>
                <s:element name="SetOptimizationParamsResult" type="s:boolean"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="Optimize">
        <s:complexType/>
    </s:element>
    <s:element name="OptimizeResponse">
        <s:complexType>
            <s:sequence>
                <s:element name="OptimizeResult" type="s:int"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="GetResultURI">
        <s:complexType>
            <s:sequence>
                <s:element name="result" type="s:int"/>
            </s:sequence>
        </s:complexType>
    </s:element>
    <s:element name="GetResultURIResponse">
        <s:complexType>
            <s:sequence>
                <s:element minOccurs="0" name="GetResultURIResult" type="s:string"/>
            </s:sequence>
        </s:complexType>
    </s:element>
</s:schema>
;
			 var xsdSchema0:Schema = new Schema(xsdXML0);
			schemas.push(xsdSchema0);
			targetNamespaces.push(new Namespace('','http://www.solidopt.org/ws/SolidOptService.asmx'));
		}
	}
}