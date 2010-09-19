/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{
	/// <summary>
	/// Description of Resolver.
	/// </summary>
	public abstract class Resolver : ITypeResolver
	{
		
		public virtual object TryResolve(object paramValue)
		{
			return CannotResolve.Instance;
		}
		
		public virtual Resolver Add(ITypeResolver resolver)
		{
			return this;
		}
		public virtual Resolver InsertFirst(ITypeResolver resolver)
		{
			return this;
		}
		public virtual Resolver InsertAfter(ITypeResolver after, ITypeResolver resolver)
		{
			return this;
		}
		public virtual Resolver InsertAtPosition(int position, ITypeResolver resolver)
		{
			return this;
		}
		
		public virtual void RemoveResolver(ITypeResolver resolver)
		{
		}
	}
}
