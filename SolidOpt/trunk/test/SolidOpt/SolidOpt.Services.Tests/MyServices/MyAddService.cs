// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using System;
using NUnit.Framework;

using SolidOpt.Services;

namespace SolidOpt.Services.Tests
{
    public class MyAddService: IMyAddService
    {
        public int Add(int a, int b) {
            return a + b;
        }
    }
    
}
