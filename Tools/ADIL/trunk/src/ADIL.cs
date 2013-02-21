// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidOpt.Tools.ADIL
{
    public static class ADIL
    {
        public static MethodDefinition d(MethodDefinition f)
        {
            return d(f, 0);
        }

        public static MethodDefinition d(MethodDefinition f, int diffBy)
        {
            MethodDefinition df = CloneMethodWidhoutIL("d_" + f.Name, f);

            ILProcessor il = df.Body.GetILProcessor();
            il.Emit(OpCodes.Ldc_R4, 2.0f);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Mul);
            il.Emit(OpCodes.Ret);

            df.DeclaringType.Methods.Add(df);

            return df;
        }

        private static MethodDefinition CloneMethodWidhoutIL(string newName, MethodDefinition method)
        {
            MethodDefinition df = new MethodDefinition(newName, method.Attributes, method.ReturnType);
            //df.Body = new Mono.Cecil.Cil.MethodBody(df);
            //df.Attributes = method.Attributes;

            
            /// Attributes
            //public bool IsCompilerControlled {
            //public bool IsPrivate {
            //public bool IsFamilyAndAssembly {
            //public bool IsAssembly {
            //public bool IsFamily {
            //public bool IsFamilyOrAssembly {
            //public bool IsPublic {
            //public bool IsStatic {
            //public bool IsFinal {
            //public bool IsVirtual {
            //public bool IsHideBySig {
            //public bool IsReuseSlot {
            //public bool IsNewSlot {
            //public bool IsCheckAccessOnOverride {
            //public bool IsAbstract {
            //public bool IsSpecialName {
            //public bool IsPInvokeImpl {
            //public bool IsUnmanagedExport {
            //public bool IsRuntimeSpecialName {
            //public bool HasSecurity {
            /// MethodImplAttributes
            //public bool IsIL {
            //public bool IsNative {
            //public bool IsRuntime {
            //public bool IsUnmanaged {
            //public bool IsManaged {
            //public bool IsForwardRef {
            //public bool IsPreserveSig {
            //public bool IsInternalCall {
            //public bool IsSynchronized {
            //public bool NoInlining {
            //public bool NoOptimization {
            /// MethodSemanticsAttributes
            //public bool IsSetter {
            //public bool IsGetter {
            //public bool IsOther {
            //public bool IsAddOn {
            //public bool IsRemoveOn {
            //public bool IsFire {

            df.CallingConvention = method.CallingConvention;
            foreach (var cattr in method.CustomAttributes)
                df.CustomAttributes.Add(cattr);
            df.DeclaringType = method.DeclaringType;
            df.ExplicitThis = method.ExplicitThis;
            foreach (var gparam in method.GenericParameters)
                df.GenericParameters.Add(gparam);
            //df.HasSecurity
            df.HasThis = method.HasThis;
            df.ImplAttributes = method.ImplAttributes;
            //df.Module = method.Module;
            foreach (var param in method.Parameters) {
                ParameterDefinition p = new ParameterDefinition(param.Name, param.Attributes, param.ParameterType);
                if (param.HasConstant) p.Constant = param.Constant;
                foreach (var pcattr in param.CustomAttributes)
                    p.CustomAttributes.Add(pcattr);
                //p.HasConstant = param.HasConstant;
                //p.HasDefault = param.HasDefault;
                //p.IsIn = param.IsIn;
                //p.IsOptional = param.IsOptional;
                //p.IsOut = param.IsOut;
                p.MarshalInfo = param.MarshalInfo;
                p.Name = param.Name;
                df.Parameters.Add(p);
            }
            if (method.IsPInvokeImpl) df.PInvokeInfo = method.PInvokeInfo;
            df.SemanticsAttributes = method.SemanticsAttributes;

            df.Body = new Mono.Cecil.Cil.MethodBody(df);

            return df;
        }

    }
}
