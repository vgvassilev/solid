/*
 * Created by SharpDevelop.
 * User: Sasho
 * Date: 08.8.2008
 * Time: 13:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Cache1
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
