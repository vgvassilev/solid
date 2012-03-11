/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using SolidOptWebServiceClient.www.solidopt.org;

namespace SolidOpt.Services.WebServices.SolidOptWebServiceClient
{
	class Program 
	{
		public static void Main(string[] args)
		{
			SolidOptService ws = new SolidOptService();
			ws.CookieContainer = new System.Net.CookieContainer();
			
			TransformMethod[] tms = ws.GetTransformMethods();
			foreach (TransformMethod tm in tms) {
				Console.WriteLine(tm.FullName);
			}
			
			Console.ReadKey(true);
			
			ws.NewOptimization();
			ws.AddOptimizationURI("http://www.solidopt.org/logo.png");
			Console.WriteLine(ws.Optimize());
			Console.WriteLine(ws.GetResultURI(0));
			
			Console.ReadKey(true);
		}
	}
}
