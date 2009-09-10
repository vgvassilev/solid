/*
 *
 * User: Vassil Vassilev
 * Date: 09.9.2009 г.
 * Time: 14:01
 * 
 */
using System;
using System.Collections.ObjectModel;

namespace Cecil.Decompiler.Ast
{
	/// <summary>
	/// Description of CodeNodeCollection.
	/// </summary>
	public class CodeNodeCollection<TElement> : Collection<TElement>, ICodeNode
	{
		public CodeNodeCollection()
		{
		}
		
		public CodeNodeType CodeNodeType
		{
			get { return CodeNodeType.CodeNodeCollection; }
		}
	}
}
