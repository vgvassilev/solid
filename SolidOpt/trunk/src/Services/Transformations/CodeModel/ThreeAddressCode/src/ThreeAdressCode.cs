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
            int i = 0;

            sb.AppendLine(method.Name);

            foreach (Triplet triplet in RawTriplets) {
                sb.AppendLine(String.Format("L{0}: {1}", i++, triplet.ToString()));
            }
            
            return sb.ToString();   
        }
    }
}

