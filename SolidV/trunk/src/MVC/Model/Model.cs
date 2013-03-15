// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
	public class Model : IModel
	{
		private int updateLockCount;

		private List<IModel> subModels;
		public List<IModel> SubModels {
			get {
				if (subModels == null)
					subModels = new List<IModel>();
				return subModels; 
			}
			set { subModels = value; }
		}
		
		public T GetSubModel<T>() where T: class, IModel
		{
			foreach (IModel item in SubModels){
				
				T result = item as T;
				if( result != null )
					return result;
			}
			return null;
		}
		
		public T UseSubModel<T>() where T: class, IModel, new()
		{
			foreach (IModel item in SubModels){
				
				T result = item as T;
				if( result != null )
					return result;
			}
			return RegisterSubModel<T>();
		}
		
		public T RegisterSubModel<T>() where T: class, IModel, new()
		{
			return this.RegisterSubModel<T>(new T());
		}
		
		public T RegisterSubModel<T>(T subModel) where T: class, IModel
		{
			if (GetSubModel<T>() == null){
				SubModels.Add(subModel);
				return subModel;
			}
			return null;
		}

		private List<IObserver> observers;

		public List<IObserver> Observers {
			get { 
				if (observers == null)
					observers = new List<IObserver>(); 
				return observers; 
			}
			set { observers = value; }
		}
		
		public Model()
		{
		}
		
		public void BeginUpdate()
		{
			updateLockCount++;
		}
		
		public void EndUpdate()
		{
			updateLockCount--;
			if (updateLockCount == 0)
				Notify();
		}
		
		public void Notify()
		{
			foreach (IObserver observer in Observers){
				observer.Update(this);
			}
		}
	}
}

