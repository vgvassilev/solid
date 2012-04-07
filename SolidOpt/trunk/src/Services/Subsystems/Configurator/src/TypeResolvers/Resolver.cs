/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Configurator.TypeResolvers
{

	public abstract class Resolver : ITypeResolver
	{
		/// <summary>
		/// If nobody overrides the function, i.e the type has not been resolved
		/// or it cannot be resolved, then the type of the old type is not 
		/// resolvable
		/// </summary>
		/// <param name="paramValue">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Object"/>
		/// </returns>
		public virtual object TryResolve(object paramValue)
		{
			//FIXME: add implementation
			throw new NotImplementedException();
			//return CannotResolve.Instance;
		}
		
		/// <summary>
		/// Adds specified resolver in the end of the list. 
		/// </summary>
		/// <param name="resolver">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		/// <returns>
		/// A <see cref="Resolver"/>
		/// </returns>
		public virtual Resolver Add(ITypeResolver resolver)
		{
			//FIXME: add implementation
			throw new NotImplementedException();
			//return this;
		}
		
		/// <summary>
		/// Inserts specified resolver in the first place of the list. 
		/// </summary>
		/// <param name="resolver">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		/// <returns>
		/// A <see cref="Resolver"/>
		/// </returns>
		public virtual Resolver InsertFirst(ITypeResolver resolver)
		{
			//FIXME: add implementation
			throw new NotImplementedException();
			//return this;
		}
		
		/// <summary>
		/// Inserts specified resolver after specified position in the list. 
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
		public virtual Resolver InsertAfter(ITypeResolver after, ITypeResolver resolver)
		{
			//FIXME: add implementation
			throw new NotImplementedException();
			//return this;
		}
		
		/// <summary>
		/// Inserts specified resolver at specified position in the list. 
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
		public virtual Resolver InsertAtPosition(int position, ITypeResolver resolver)
		{
			//FIXME: add implementation
			throw new NotImplementedException();
			//return this;
		}
		
		/// <summary>
		/// Removes specified resolver. 
		/// </summary>
		/// <param name="resolver">
		/// A <see cref="ITypeResolver"/>
		/// </param>
		public virtual void RemoveResolver(ITypeResolver resolver)
		{
			//FIXME: add implementation
			throw new NotImplementedException();
		}
	}
}
