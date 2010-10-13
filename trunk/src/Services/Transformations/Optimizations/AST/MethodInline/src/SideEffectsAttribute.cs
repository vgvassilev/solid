/*
 * $Id: InlineTransformer.cs 349 2010-09-26 08:15:46Z apenev $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Transformations.Optimizations.AST.MethodInline
{
	public class SideEffectsAttribute : Attribute
	{
		public bool HasSideEffects = false;
		
		public SideEffectsAttribute(bool HasSideEffects)
		{
			this.HasSideEffects = HasSideEffects;
		}
	}
}
