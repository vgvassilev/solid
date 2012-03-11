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
	/// The ChainResolver is part of the Composition Pattern and it should be used when the
	/// resolvers has to be in specific sequence and inserted into the resolvers list. The
	/// advantage is that there won't be restructuring of the others.
	/// For example imagine we have existing list of 
	/// ByteResolver-IntResolver-FloatResolver and we want to insert between ByteResolver
	/// and IntResolver a new custom chain. It can be done with the ChainResolver class.
	/// </summary>
	//FIXME: Has to be with the same name as the CustomMapperGroup
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
		
		/// <summary>
		/// Inserts new resolver after specific resolver 
		/// </summary>
		/// <param name="after">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		/// <param name="resolver">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		/// <returns>
		/// A <see cref="Resolver"/>
		/// </returns>
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
		
		/// <summary>
		/// Inserts new resolver at specific position 
		/// </summary>
		/// <param name="position">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="resolver">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		/// <returns>
		/// A <see cref="Resolver"/>
		/// </returns>
		public override Resolver InsertAtPosition(int position, ITypeResolver resolver)
		{
			ResolverChain.Insert(position, resolver);
			return this;
		}
		
		/// <summary>
		/// Inserts resolver at the first place in the list 
		/// </summary>
		/// <param name="resolver">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		/// <returns>
		/// A <see cref="Resolver"/>
		/// </returns>
		public override Resolver InsertFirst(ITypeResolver resolver)
		{
			ResolverChain.Insert(0, resolver);
			return this;
		}
		
		/// <summary>
		/// Adds resolver in the end of the list 
		/// </summary>
		/// <param name="resolver">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		/// <returns>
		/// A <see cref="Resolver"/>
		/// </returns>
		public override Resolver Add(ITypeResolver resolver)
		{
			ResolverChain.Add(resolver);
			return this;
		}
		
		/// <summary>
		/// Removes specific resolver. 
		/// </summary>
		/// <param name="resolver">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		public override void RemoveResolver(ITypeResolver resolver)
		{
			ResolverChain.Remove(resolver);
		}
		
		/// <summary>
		/// Triggers the type resolution. 
		/// </summary>
		/// <param name="paramValue">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Object"/>
		/// </returns>
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
