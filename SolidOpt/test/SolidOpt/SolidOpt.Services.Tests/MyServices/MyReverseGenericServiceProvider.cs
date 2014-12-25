// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using System;
using System.Collections.Generic;
using NUnit.Framework;

using SolidOpt.Services;

namespace SolidOpt.Services.Tests
{
    public class MyReverseGenericServiceProvider<T>: MyGenericServiceProvider<T>
    {
        public override string ToString() {
            string sb = "";
            foreach (T item in list) sb = item.ToString() + ";" + sb; 
            return sb;
        }
    }
}