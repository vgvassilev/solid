/*
 * Created by SharpDevelop.
 * User: Sasho
 * Date: 08.8.2008
 * Time: 14:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Cache1
{
	/// <summary>
	/// Description of ClassBuilder.
	/// </summary>
	public class ClassBuilder
	{
		public ClassBuilder()
		{
		}
		
		public static ResultClass BuildResult(int i)
		{
			return new ResultClass("result" + i);
		}
	}
}
