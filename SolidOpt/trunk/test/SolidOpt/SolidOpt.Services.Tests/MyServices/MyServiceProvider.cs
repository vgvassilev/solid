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
    public class MyServiceProvider: AbstractServiceProvider, IMyAddService, IMySubService
    {
        public int Add(int a, int b) {
            return a + b;
        }

        public int Sub(int a, int b) {
            return a - b;
        }

    }
    
}