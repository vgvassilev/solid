/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Core.Loader.Demo.Loadee
{
	// Disable warnings for never used and unreachable code, because this is test
	// 168 - variable declared but never used
	// 162 - unreachable code detected
	// 219 - variable assigned but the value never used
	
	#pragma warning disable 168, 162, 219
	
	/// <summary>
	/// This is a test program. It should be removed when appropraite tests are created. 
	/// </summary>
	//TODO: Replace this with test cases
	class Program
	{
		public static void Main(string[] args)
		{
//			Triangle();
//			Triangle1();
			Console.WriteLine("test");
			InlineTest();
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
//		
//		public static void PreTestLoop()
//		{
//			int a = (int)Math.Sqrt(150*150);
//			double s = 0;
//			;
//			;
//			;
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
			double a = 5;
			double result;
			result = CalculateDiag(a);
			Console.WriteLine(a);
		}
		
		public static double CalculateDiag(double a)
		{
			return Math.Sqrt(2) * a;
		}

		public static void InlineTest1()
		{
//			int a = 0;
//			Random rnd = new Random();
//			int p = 4;
//			p = Inlinee(2, 18);
			double s, a, b, c, x;
			a = 1;
//			a = 1 + 2;
//			b = (2 + 7) * 5;
//			c = 3/2;
//			Random rnd = new Random();
//			a = Math.Sqrt(25.25);
//			b = 8.2d;
//			c = 1d;
			s = CalculateArea(a, 1 + 2, 1 + 2 + 3);
			Console.WriteLine(s);
			s = CalculateArea1(9, 8, 7);
			Console.WriteLine(s);
			
//			Console.WriteLine(p);
//			TestThis.Inc();
//			TestThis.Inc(6);
//			TestThis td = new TestThis();
//			int t = td.Test();
//			Console.WriteLine(t);
//			int f = 5;
//			float f1 = 5.5f;
//			Console.WriteLine(f + f1);
			
//			Console.WriteLine(p);
//			Inlinee1(0, 1, 2);
//			byte f = 2;
//			OutTest(ref f);
//			Console.WriteLine(f);
		}
//		public static void OutTest(ref byte p) {
//			p = 5;
//			
//		}
		
//		public static void CalculateAreas()
//		{
//			double s;
//			Random rnd = new Random();
//			do {
//				s = CalculateArea(rnd.Next(), rnd.Next(), rnd.Next());
//			} while (s < 100);
// 		}
		
//		[MethodInliner.Inlineable]
		[SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
//		[MethodInliner.SideEffects(false)]
		public static double CalculateArea(double a, double b, double c)
		{
			if (a==0 || b==0 || c==0) return 0;
			
			double p = (a + b + c) / 2 ;
			return Math.Sqrt(p * (p-a) * (p-b) * (p-c));
		}
		
		[SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
		public static double CalculateArea1(double a, double b, double c)
		{
			try {
				if (a==0 || b==0 || c==0) return 0;
				
				double p = (a + b + c) / 2 ;
				return Math.Sqrt(p * (p-a) * (p-b) * (p-c));
			}
			catch {
				return -1;
			}
		}
		
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
		
//		[MethodInliner.Inlineable]
		[SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
//		[MethodInliner.SideEffects(true)]
		public static int Inlinee(int p, int q)
		{
			byte x = 5;
			int y = 2;
//			p=5;
//			q=6;
			if (x == 6) {
				Console.WriteLine(x);
				return p + q;
				
			}
			else {
				Console.WriteLine(p+q);
				return 10;
			}
////			byte v;
////			Console.WriteLine(x);
//			return 1;
			return p+q;
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
//		[MethodInliner.Inlineable]
//		public static void Inlinee1(params object [] ints)
//		{
//			for (int i = 0; i < ints.Length; i++) {
//				Console.WriteLine(ints[i]);
//			}
//			return;
//		}
	}
	#pragma warning restore 168, 162, 219
}
	class TestThis
	{
		private static int number = 0;
		
//		[MethodInliner.Inlineable]
		[SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
		public int Test()
		{
			Console.WriteLine("This is this");
			return 0;
		}
		
//		[MethodInliner.Inlineable]
		[SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
		public static int Inc ()
		{
			return number + 1;
		}
		
//		[MethodInliner.Inlineable]
		[SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
		public static int Inc (int number)
		{
			return (TestThis.number + number + 1) * (number + TestThis.number + 1);
			//return (TestThis.number + number + 1) * (1 + number + TestThis.number);
		}
	}