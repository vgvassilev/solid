/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using Mono.Cecil;

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public class DeReference
  {
    private object addressTo;
    public object AddressTo {
      get { return addressTo; }
      set { addressTo = value; }
    }

    public DeReference(object addressTo) {
      this.addressTo = addressTo;
    }
  }
  
}
