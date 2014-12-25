// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
	// public static void Main(string[] args) {
	//   return;
	// }
	.method public hidebysig static void  Main(string[] args) cil managed
	{
	  .entrypoint
	  .maxstack  8
	  IL_0000:  nop
	  IL_0001:  ret
	} 
}
