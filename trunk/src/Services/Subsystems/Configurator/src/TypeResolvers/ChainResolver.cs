/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of ChainResolver.
	/// </summary>
	public class ChainResolver : Resolver
	{
		private List<ITypeResolver> resolverChain = new List<ITypeResolver>();
		public List<ITypeResolver> ResolverChain {
			get { return resolverChain; }
			set { resolverChain = value; }
		}
		
		public ChainResolver()
		{
		}
		
		public override Resolver InsertAfter(ITypeResolver after, ITypeResolver resolver)
		{
			int index = ResolverChain.IndexOf(after);
			if (index >= 0) {
				ResolverChain.Insert(index, resolver);
			}
			else {
				ResolverChain.Add(resolver);
			}
			return this;
		}
		
		public override Resolver InsertAtPosition(int position, ITypeResolver resolver)
		{
			ResolverChain.Insert(position, resolver);
			return this;
		}
		
		public override Resolver InsertFirst(ITypeResolver resolver)
		{
			ResolverChain.Insert(0, resolver);
			return this;
		}
		
		public override Resolver Add(ITypeResolver resolver)
		{
			ResolverChain.Add(resolver);
			return this;
		}
		
		public override void RemoveResolver(ITypeResolver resolver)
		{
			ResolverChain.Remove(resolver);
		}
		
		public override object TryResolve(object paramValue)
		{
			object val = base.TryResolve(paramValue);
			foreach(ITypeResolver item in ResolverChain) {
				val = item.TryResolve(paramValue);
				if (!(val is CannotResolve))
					break;
			}
			return val;
		}
		
		
	}
}
