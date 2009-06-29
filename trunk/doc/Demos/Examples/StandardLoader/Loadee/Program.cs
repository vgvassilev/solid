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
			Triangle1();
			
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
		
		public static void Triangle1()
		{
			int a;
			int b;
			int c;
			int P;
			do {
				a = 5;
				b = 8;
				c = 10;
				P = a + b + c;
				Console.WriteLine("Perimeter is {0}", P);
				
				int p = P/2;
				double S = Math.Sqrt(p * (p-a) * (p-b) * (p-c));
				if (S == 0)
					Console.WriteLine("Area is Zero");
				else
					Console.WriteLine("Area is {0}", S);
			} while (a == 5);
		}
	}
}