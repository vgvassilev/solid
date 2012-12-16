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
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void MyAddService() {
            MyAddService my = new MyAddService();
            Assert.AreEqual(0, my.Add(0,0));
            Assert.AreEqual(2, my.Add(1,1));
            Assert.AreEqual(0, my.Add(1,-1));
            Assert.AreEqual(5, my.Add(4,1));
        }

        [Test]
        public void MyAddService1() {
            MyAddService1 my = new MyAddService1();
            Assert.AreEqual(0, my.Add(0,0));
            Assert.AreEqual(2, my.Add(1,1));
            Assert.AreEqual(0, my.Add(1,-1));
            Assert.AreEqual(5, my.Add(4,1));
        }

    }
}

