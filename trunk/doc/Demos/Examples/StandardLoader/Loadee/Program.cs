/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.6.2009 г.
 * Time: 16:47
 * 
 */
using System;
using System.Collections.Generic;

namespace Loadee
{
	class Program
	{
		public static void Main(string[] args)
		{
//			Triangle();
//			Triangle1();
			
			Console.ReadKey(true);
		}
		
//		public static void Triangle()
//		{
//			int a = 5;
//			int b = 8;
//			int c = 10;
//			int P = a + b + c;
//			Console.WriteLine("Perimeter is {0}", P);
//			
//			int p = P/2;
//			double S = Math.Sqrt(p * (p-a) * (p-b) * (p-c));
//			Console.WriteLine("Area is {0}", S);
//		}
//		
//		public static void PostTestLoop()
//		{
//			int a;
//			int b;
//			int s = 0;
//			do {
//				a = 5;
//				b = 8;
//				s = a + b;
//				s = b;
//			} while (a == 100);
//		}
		
//		public static void PreTestLoop()
//		{
//			int a = (int)Math.Sqrt(150*150);
//			double s = 0;
//			while (a >= 100) {
//				a ++;
//				int b = 8;
//				s = Math.Sqrt(a + b);
//				b = ++a;
//				b = a++;
//			} 
//			Console.WriteLine(s);
//		}
		
//		public static void PreTestLoop1()
//		{
//			while (new Random().Next() > 0.5) {
//				int b = 8;
//				b --;
//				
//				Console.WriteLine(b);
//			} 
//		}
//		
//		public static void Triangle1()
//		{
//			int a;
//			int b;
//			int c;
//			int P;
////			Console.WriteLine("a");
//			do {
//				a = 5;
//				b = 8;
//				c = 10;
//				P = a + b + c;
//				Console.WriteLine("Perimeter is {0}", P);
//				
//				int p = P/2;
//				double S = Math.Sqrt(p * (p-a) * (p-b) * (p-c));
//				if (S == 0)
//					Console.WriteLine("Area is Zero");
//				else
//					Console.WriteLine("Area is {0}", S);
//			} while (a == 5);
//			a++;
//			Console.WriteLine(a);
//			b++;
//			Console.WriteLine(b);
//		}
//		
		

		public static void InlineTest()
		{
			int a = 0;
			Random rnd = new Random();
			
			int p = Inlinee(a + rnd.Next(), a + 18);
			Console.WriteLine(a);
			Console.WriteLine(p);
//			Inlinee1(0, 1, 2);
//			byte f = 2;
//			OutTest(ref f);
//			Console.WriteLine(f);
		}
//		public static void OutTest(ref byte p) {
//			p = 5;
//			
//		}
		
//		[MethodInliner.InlineAttribute]
//		public static void OutParamAssign()
//		{
//			Dictionary<int, string> dict = new Dictionary<int, string>();
//			dict[0] = "abc";
//			string s;
//			if (!dict.TryGetValue(0, out s)){
//				s = "10";
//			}
//			Console.WriteLine(s);
//			
//			string s1;
//			dict.TryGetValue(0, out s1);
//			s1 = "11";
//			Console.WriteLine(s1);
//		}
		
		[MethodInliner.InlineableAttribute]
		public static int Inlinee(int p, int q)
		{
			byte x = 5;
			if (x == 6) {
				Console.WriteLine(x);
				return p + q;
				
			}
			else {
				Console.WriteLine(p+q);
				
			}
			byte v;
			Console.WriteLine(x);
			return 10;
		}
		
//		[MethodInliner.Inlineable]
//		public static void Inlinee1(params byte [] ints)
//		{
//			int sum = 0;
//			for (int i = 0; i < ints.Length; i++) {
//				sum += ints[i];
//			}
//			return;
//		}
		[MethodInliner.Inlineable]
		public static void Inlinee1(params object [] ints)
		{
			for (int i = 0; i < ints.Length; i++) {
				Console.WriteLine(ints[i]);
			}
			return;
		}
	}
}