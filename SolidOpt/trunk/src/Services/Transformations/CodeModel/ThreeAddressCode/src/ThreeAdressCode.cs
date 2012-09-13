/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Text;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

    public class ThreeAdressCode
    {
        private Triplet root;
        public Triplet Root {
            get { return root; }
        }
        
        private MethodDefinition method;
        public MethodDefinition Method {
            get { return method; }
        }
        
        private List<Triplet> rawTriplets;
        public List<Triplet> RawTriplets {
            get { return rawTriplets; }
        }

        private List<VariableDefinition> temporaryVariables;
        public List<VariableDefinition> TemporaryVariables {
            get { return temporaryVariables; }
        }

        public ThreeAdressCode(MethodDefinition method, Triplet root, List<Triplet> rawTriplets, List<VariableDefinition> temporaryVariables)
        {
            this.method = method;
            this.root = root;
            this.rawTriplets = rawTriplets;
            this.temporaryVariables = temporaryVariables;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} ", method.ReturnType.ToString());
            sb.AppendFormat("{0}::{1}(", method.DeclaringType.ToString(), method.Name);
            ParameterDefinition paramDef;
            for(int i = 0; i < method.Parameters.Count; i++) {
              paramDef = method.Parameters[i];
              sb.AppendFormat("{0} {1}{2}", paramDef.ParameterType.ToString(), paramDef.Name,
                              (i < method.Parameters.Count-1) ? ", " : "");
            }
            sb.AppendLine(") {");

            int index = 0;
            foreach (Triplet triplet in RawTriplets) {
                sb.AppendLine(String.Format("  L{0}: {1}", index++, triplet.ToString()));
            }

            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}

