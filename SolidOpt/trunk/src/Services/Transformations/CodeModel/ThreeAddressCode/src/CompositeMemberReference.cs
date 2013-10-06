/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using Mono.Cecil;

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public class CompositeMemberReference
  {
    private object instance;
    public object Instance {
      get { return instance; }
      set { instance = value; }
    }

    private MemberReference member;
    public MemberReference Member {
      get { return member; }
      set { member = value; }
    }

    public CompositeMemberReference(object instance, MemberReference member) {
      this.instance = instance;
      this.member = member;
    }
  }

}

