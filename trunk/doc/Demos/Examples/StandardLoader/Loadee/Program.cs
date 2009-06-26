/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.6.2009 г.
 * Time: 16:47
 * 
 */
using System;

namespace Loadee
{
	class Program
	{
		public static void Main(string[] args)
		{
			Triangle();
			
			Console.ReadKey(true);
		}
		
		public static void Triangle()
		{
			int a = 5;
			int b = 8;
			int c = 10;
			int P = a + b + c;
			Console.WriteLine("Perimeter is {0}", P);
			
			int p = P/2;
			double S = Math.Sqrt(p * (p-a) * (p-b) * (p-c));
			Console.WriteLine("Area is {0}", S);
		}
	}
}