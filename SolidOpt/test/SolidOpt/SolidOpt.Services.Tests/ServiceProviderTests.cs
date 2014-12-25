// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using NUnit.Framework;

namespace SolidOpt.Services.Tests
{
    [TestFixture]
    public class ServiceProviderTests
    {
        [Test]
        public void MyServiceProvider() {
            MyServiceProvider myProvider = new MyServiceProvider();
            IMyAddService myAdd = myProvider.GetService<IMyAddService>();
            Assert.AreEqual(0, myAdd.Add(0,0));
            Assert.AreEqual(2, myAdd.Add(1,1));
            Assert.AreEqual(0, myAdd.Add(1,-1));
            Assert.AreEqual(5, myAdd.Add(4,1));

            IMySubService mySub = myProvider.GetService<IMySubService>();
            Assert.AreEqual(0, mySub.Sub(0,0));
            Assert.AreEqual(2, mySub.Sub(2,0));
            Assert.AreEqual(0, mySub.Sub(1,1));
            Assert.AreEqual(2, mySub.Sub(1,-1));
            Assert.AreEqual(3, mySub.Sub(4,1));
        }

        [Test]
        public void MyGenericServiceProviderInt() {
            var myProvider = new MyGenericServiceProvider<int>();
            var myList = myProvider.GetService<IMyGenericListService<int>>();

            myList.Append(1);
            myList.Append(2);
            myList.Append(3);
            Assert.AreEqual("1;2;3;", myList.ToString());
        }

        [Test]
        public void MyGenericServiceProviderString() {
            var myProvider = new MyGenericServiceProvider<string>();
            var myList = myProvider.GetService<IMyGenericListService<string>>();
            
            myList.Append("a");
            myList.Append("b");
            myList.Append("cde");
            Assert.AreEqual("a;b;cde;", myList.ToString());
        }

        [Test]
        public void MyGenericServiceProviderWildcard() {
            var myProvider = new MyReverseGenericServiceProvider<string>();
            var myList = myProvider.GetService(typeof(IMyGenericListService<>));
            Assert.IsNotNull(myList);
            ((IMyGenericListService<string>)myList).Append("aaa");
            ((IMyGenericListService<string>)myList).Append("bbb");
            ((IMyGenericListService<string>)myList).Append("ccc");
            Assert.AreEqual("ccc;bbb;aaa;", myList.ToString());
        }
        
        [Test]
        public void MyGenericServiceProviderWildcardDirect() {
            var myProvider = new MyReverseGenericServiceProvider<string>();
            var myList = myProvider.GetService(typeof(MyReverseGenericServiceProvider<>));
            Assert.IsNotNull(myList);
            ((MyReverseGenericServiceProvider<string>)myList).Append("aaa");
            ((MyReverseGenericServiceProvider<string>)myList).Append("bbb");
            ((MyReverseGenericServiceProvider<string>)myList).Append("ccc");
            Assert.AreEqual("ccc;bbb;aaa;", myList.ToString());
        }
        
        [Test]
        public void MyGenericServiceProviderWildcardParent2() {
            var myProvider = new MyReverseGenericServiceProvider<string>();
            var myList = myProvider.GetService(typeof(MyGenericServiceProvider<>));
            Assert.IsNotNull(myList);
            ((MyGenericServiceProvider<string>)myList).Append("aaa");
            ((MyGenericServiceProvider<string>)myList).Append("bbb");
            ((MyGenericServiceProvider<string>)myList).Append("ccc");
            Assert.AreEqual("ccc;bbb;aaa;", myList.ToString());
        }
        
        [Test]
        public void MyGenericServiceProvider2Wildcard() {
            var myProvider = new MyGenericService2Provider<string, int>();
            var myDict = myProvider.GetService(typeof(IMyGenericDictionaryService<,>));
            Assert.IsNotNull(myDict);
            ((IMyGenericDictionaryService<string,int>)myDict).Set("k1", 9);
            ((IMyGenericDictionaryService<string,int>)myDict).Set("k2", 8);
            ((IMyGenericDictionaryService<string,int>)myDict).Set("k3", 7);
            Assert.AreEqual("k1=9;k2=8;k3=7;", myDict.ToString());
        }

        internal class Hack: AbstractServiceProvider
        {
            public static new bool IsTypeProvideService(Type type, Type serviceTypeOrOpenGeneric) {
                return AbstractServiceProvider.IsTypeProvideService(type, serviceTypeOrOpenGeneric);
            }
        }

        [Test]
        public void IsTypeProvideService() {

            //// Simple services

            // Base internal service interfaces
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(IService), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(IServiceProvider), typeof(IServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(IServiceProvider), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(IServiceContainer), typeof(IServiceContainer)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(IServiceContainer), typeof(IServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(IServiceContainer), typeof(IService)));
            // must be false
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(IService), typeof(IServiceProvider)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(IService), typeof(IServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(IServiceProvider), typeof(IServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(IService), typeof(IMyAddService)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(IServiceProvider), typeof(IMyAddService)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(IServiceContainer), typeof(IMyAddService)));

            // Simple service types and itself classes and inherited classes
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyAddService), typeof(MyAddService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyAddService1), typeof(MyAddService1)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyAddService1), typeof(AbstractService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(MyServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(AbstractServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(AbstractService)));
            // must be false
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyAddService), typeof(MySubService)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyAddService1), typeof(MySubService)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(AbstractServiceContainer)));

            // Simple service types and its service interfaces
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyAddService), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyAddService), typeof(IMyAddService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyAddService1), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyAddService1), typeof(IMyAddService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MySubService), typeof(IMySubService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(IServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(IMyAddService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(IMySubService)));
            // must be false
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyAddService), typeof(IMySubService)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyAddService), typeof(IServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyAddService1), typeof(IMySubService)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyAddService1), typeof(IServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MySubService), typeof(IMyAddService)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyServiceProvider), typeof(IServiceContainer)));


            //// Generics services

            // Generics service types and itself classes and inherited classes
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(AbstractService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(AbstractServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(MyGenericServiceProvider<int>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<string>), typeof(MyGenericServiceProvider<string>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(AbstractServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(MyGenericServiceProvider<int>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(MyReverseGenericServiceProvider<int>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(AbstractService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(AbstractServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(MyGenericServiceProvider<string>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(MyReverseGenericServiceProvider<string>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(AbstractService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(AbstractServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(MyGenericService2Provider<int,int>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,string>), typeof(AbstractService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,string>), typeof(AbstractServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,string>), typeof(MyGenericService2Provider<int,string>)));
            // must be false
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(AbstractServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(MyGenericServiceProvider<string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(AbstractServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(MyGenericServiceProvider<string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(MyReverseGenericServiceProvider<string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(AbstractServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(AbstractServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(MyGenericService2Provider<string,int>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(MyGenericService2Provider<string,string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,string>), typeof(AbstractServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,string>), typeof(MyGenericService2Provider<int,int>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,string>), typeof(MyGenericService2Provider<string,int>)));

            // Generics service types and its service interfaces
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(IServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(IMyGenericListService<int>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<string>), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<string>), typeof(IMyGenericListService<string>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(IServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(IMyGenericListService<int>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(IServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(IMyGenericListService<string>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(IServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(IMyGenericDictionaryService<int,int>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<string,string>), typeof(IService)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<string,string>), typeof(IServiceProvider)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<string,string>), typeof(IMyGenericDictionaryService<string,string>)));
            // must be false
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(IServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(IMyGenericListService<string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<string>), typeof(IMyGenericListService<int>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(IServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(IMyGenericListService<string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(IMyGenericListService<int>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(IServiceContainer)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(IMyGenericDictionaryService<int,string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(IMyGenericDictionaryService<string,int>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(IMyGenericDictionaryService<string,string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<string,string>), typeof(IMyGenericDictionaryService<string,int>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<string,string>), typeof(IMyGenericDictionaryService<int,string>)));
            Assert.IsFalse(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<string,string>), typeof(IMyGenericDictionaryService<int,int>)));

            ///// Open Generic serivices (Wildcards)

            // Open Generics service types and itself classes and inherited classes
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(MyGenericServiceProvider<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<string>), typeof(MyGenericServiceProvider<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(MyGenericServiceProvider<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(MyReverseGenericServiceProvider<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(MyGenericServiceProvider<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(MyReverseGenericServiceProvider<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(MyGenericService2Provider<,>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,string>), typeof(MyGenericService2Provider<,>)));

            // Open Generics service types and its service interfaces
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<int>), typeof(IMyGenericListService<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericServiceProvider<string>), typeof(IMyGenericListService<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<int>), typeof(IMyGenericListService<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyReverseGenericServiceProvider<string>), typeof(IMyGenericListService<>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<int,int>), typeof(IMyGenericDictionaryService<,>)));
            Assert.IsTrue(Hack.IsTypeProvideService(typeof(MyGenericService2Provider<string,string>), typeof(IMyGenericDictionaryService<,>)));
            
        }
        
    }
}
