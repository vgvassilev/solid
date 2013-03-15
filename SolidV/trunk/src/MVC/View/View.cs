// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;
using System.Collections.Generic;

using Cairo;

namespace SolidV.MVC
{
	public class View : IView, IObserver
	{
		private Dictionary<Type, IViewer> viewers = new Dictionary<Type, IViewer>();
		public Dictionary<Type, IViewer> Viewers {
			get { return viewers; }
			set { viewers = value; }
		}

		private Model model;
		public Model Model {
			get { return model; }
			set { model = value; }
		}

		private Context context;
		public Context Context {
			get { return context; }
			set { context = value; }
		}
    
		public View()
		{
		}

		public View(Context context, Model model)
		{
			this.Context = context;
			this.Model = model;
		}

		public void Draw(Context context, Model model)
		{
			foreach (Model mod in model.SubModels) {
				DrawItem(context, mod);
			}
		}

		public void DrawItem(Context context, object item) {
			IViewer viewer;
			if (Viewers.TryGetValue(item.GetType(), out viewer)){
				viewer.DrawItem(context, item);
			}
		}
    
		public void Update(Model subject)
		{
			throw new System.NotImplementedException();
		}
	}
}

