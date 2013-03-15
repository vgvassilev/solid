using System;

namespace SolidV.MVC
{
	public interface IObserver
	{
		void Update(Model subject);
	}
}

