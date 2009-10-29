/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 28.1.2009 г.
 * Time: 12:27
 * 
 */
using System;

namespace SolidOpt.Core.Configurator.TypeResolvers
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
