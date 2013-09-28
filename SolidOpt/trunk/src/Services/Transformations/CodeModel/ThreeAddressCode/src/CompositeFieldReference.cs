/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using Mono.Cecil;

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public class CompositeFieldReference
  {
    private object instance;
    public object Instance {
      get { return instance; }
      set { instance = value; }
    }

    private FieldReference field;
    public FieldReference Field {
      get { return field; }
      set { field = value; }
    }

    public CompositeFieldReference(object instance, FieldReference field) {
      this.instance = instance;
      this.field = field;
    }
  }

}

