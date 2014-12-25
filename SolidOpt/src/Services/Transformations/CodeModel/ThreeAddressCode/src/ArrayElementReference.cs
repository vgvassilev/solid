/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using Mono.Cecil;

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public class ArrayElementReference
  {
    private object array;
    public object Array {
      get { return array; }
      set { array = value; }
    }
    
    private object index;
    public object Index {
      get { return index; }
      set { index = value; }
    }
    
    public ArrayElementReference(object array, object index) {
      this.array = array;
      this.index = index;
    }
  }

}
