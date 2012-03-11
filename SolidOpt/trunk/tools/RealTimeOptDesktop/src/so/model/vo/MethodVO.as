package so.model.vo
{
	import mx.collections.ArrayCollection;
	
	[Bindable]
	public class MethodVO
	{	
		public var methodName:String = "";
		
		public var description:String = "";
		
		public var selectedVersion:String = "";
		
		public var selectedIndex:int = 0;
		
		public var versionsCollection:ArrayCollection = new ArrayCollection();
		
		public var configParams:ArrayCollection = new ArrayCollection();
		
		public var paramsArray:Array;
	}
}