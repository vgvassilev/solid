/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	[Serializable]
	public class ResultClass
	{
		private string text;
		
		public ResultClass()
		{
		}
		
		public ResultClass(string text)
		{
			this.text = text;
		}
		
		public override string ToString()
		{
			return "R<" + text + ">";
		}
		
		public string Text {
			get { return text; }
			set { text = value; }
		}
	}
}
