package so.model.vo
{
	import mx.collections.ArrayCollection;
	
	public class UserMethodVO
	{
		public function UserMethodVO(method:MethodVO=null)
		{
			this.methodName = method.methodName;
			this.description = method.description;
			this.selectedVersion = method.selectedVersion;
			this.versionsCollection = method.versionsCollection;
			this.configParams = method.configParams;
			this.selectedIndex = method.selectedIndex;
		}
		
		public var methodName:String = "";
		
		public var description:String = "";
		
		public var selectedVersion:String = "";
		
		public var selectedIndex:int = 0;
		
		public var versionsCollection:ArrayCollection = new ArrayCollection();
		
		public var configParams:ArrayCollection = new ArrayCollection();
	}
}