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
    //

    [TestFixture]
    public class ServiceContainerTests
    {
        [Test]
        public void MyServiceContainerSimple() {
            ServiceContainer serviceContainer = new ServiceContainer();
            serviceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyAddService1());

            List<IMyAddService> serviceList = serviceContainer.GetServices<IMyAddService>();
            Assert.AreEqual(2, serviceList.Count);

            foreach (IMyAddService my in serviceList) {
                Assert.AreEqual(0, my.Add(0,0));
                Assert.AreEqual(2, my.Add(1,1));
                Assert.AreEqual(0, my.Add(1,-1));
                Assert.AreEqual(5, my.Add(4,1));
            }
        }

        [Test]
        public void MyServiceContainerTwoServiceTypes() {
            ServiceContainer serviceContainer = new ServiceContainer();
            serviceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyAddService1());
            serviceContainer.AddService(new MySubService());

            List<IMyAddService> serviceList = serviceContainer.GetServices<IMyAddService>();
            Assert.AreEqual(2, serviceList.Count);
            
            foreach (IMyAddService my in serviceList) {
                Assert.AreEqual(0, my.Add(0,0));
                Assert.AreEqual(2, my.Add(1,1));
                Assert.AreEqual(0, my.Add(1,-1));
                Assert.AreEqual(5, my.Add(4,1));
            }
        }

        [Test]
        public void MyServiceContainerTwoServiceTypesAndProvider() {
            ServiceContainer serviceContainer = new ServiceContainer();
            serviceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyAddService1());
            serviceContainer.AddService(new MySubService());
            serviceContainer.AddService(new MyServiceProvider());

            List<IMyAddService> serviceList = serviceContainer.GetServices<IMyAddService>();
            Assert.AreEqual(3, serviceList.Count);
            
            foreach (IMyAddService my in serviceList) {
                Assert.AreEqual(0, my.Add(0,0));
                Assert.AreEqual(2, my.Add(1,1));
                Assert.AreEqual(0, my.Add(1,-1));
                Assert.AreEqual(5, my.Add(4,1));
            }
        }

        [Test]
        public void MyServiceContainerTwoProviders() {
            ServiceContainer serviceContainer = new ServiceContainer();
            serviceContainer.AddService(new MySubService());
            serviceContainer.AddService(new MyServiceProvider());
            serviceContainer.AddService(new MyServiceProvider());

            List<IMyAddService> serviceList = serviceContainer.GetServices<IMyAddService>();
            Assert.AreEqual(2, serviceList.Count);
            
            foreach (IMyAddService my in serviceList) {
                Assert.AreEqual(0, my.Add(0,0));
                Assert.AreEqual(2, my.Add(1,1));
                Assert.AreEqual(0, my.Add(1,-1));
                Assert.AreEqual(5, my.Add(4,1));
            }
        }
        
        [Test]
        public void MyServiceContainerTwoSubContainers() {
            ServiceContainer serviceContainer = new ServiceContainer();
            ServiceContainer serviceContainer1 = new ServiceContainer();
            ServiceContainer serviceContainer2 = new ServiceContainer();
            serviceContainer.AddService(serviceContainer1);
            serviceContainer.AddService(serviceContainer2);
            serviceContainer1.AddService(new MySubService());
            serviceContainer1.AddService(new MyAddService());
            serviceContainer1.AddService(new MyServiceProvider());
            serviceContainer2.AddService(new MyAddService1());
            serviceContainer2.AddService(new MyServiceProvider());

            List<IMyAddService> serviceList = serviceContainer.GetServices<IMyAddService>();
            Assert.AreEqual(4, serviceList.Count);
            
            foreach (IMyAddService my in serviceList) {
                Assert.AreEqual(0, my.Add(0,0));
                Assert.AreEqual(2, my.Add(1,1));
                Assert.AreEqual(0, my.Add(1,-1));
                Assert.AreEqual(5, my.Add(4,1));
            }
        }
        
        [Test]
        public void MyServiceContainerParentContainer() {
            ServiceContainer parentServiceContainer = new ServiceContainer();
            ServiceContainer serviceContainer = new ServiceContainer(parentServiceContainer);
            parentServiceContainer.AddService(new MySubService());
            parentServiceContainer.AddService(new MyAddService());
            parentServiceContainer.AddService(new MyServiceProvider());
            serviceContainer.AddService(new MyAddService1());

            List<IMyAddService> serviceList = serviceContainer.GetServices<IMyAddService>();
            Assert.AreEqual(3, serviceList.Count);
            
            foreach (IMyAddService my in serviceList) {
                Assert.AreEqual(0, my.Add(0,0));
                Assert.AreEqual(2, my.Add(1,1));
                Assert.AreEqual(0, my.Add(1,-1));
                Assert.AreEqual(5, my.Add(4,1));
            }
        }
        
        [Test]
        public void MyGenericServiceContainer() {
            ServiceContainer parentServiceContainer = new ServiceContainer();
            ServiceContainer serviceContainer = new ServiceContainer(parentServiceContainer);
            ServiceContainer serviceSubContainer = new ServiceContainer();
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyAddService1());
            serviceContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<string>());
            serviceContainer.AddService(serviceSubContainer);
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<string>());

            List<IMyGenericListService<int>> serviceList = serviceContainer.GetServices<IMyGenericListService<int>>();
            Assert.AreEqual(4, serviceList.Count);

            foreach (IMyGenericListService<int> my in serviceList) {
                ((IMyGenericListService<int>)my).Append(1);
                ((IMyGenericListService<int>)my).Append(2);
                ((IMyGenericListService<int>)my).Append(3);
                Assert.IsTrue(my.ToString() == "1;2;3;" || my.ToString() == "3;2;1;");
            }
        }
        
        [Test]
        public void MyGenericServiceContainerWildcard() {
            ServiceContainer parentServiceContainer = new ServiceContainer();
            ServiceContainer serviceContainer = new ServiceContainer(parentServiceContainer);
            ServiceContainer serviceSubContainer = new ServiceContainer();
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyAddService1());
            serviceContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<string>());
            serviceContainer.AddService(serviceSubContainer);
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<string>());
            
            List<IService> serviceList = serviceContainer.GetServices(typeof(IMyGenericListService<>));
            Assert.AreEqual(6, serviceList.Count);
            
            foreach (IService my in serviceList) {
                if (my is IMyGenericListService<int>) {
                    ((IMyGenericListService<int>)my).Append(1);
                    ((IMyGenericListService<int>)my).Append(2);
                    ((IMyGenericListService<int>)my).Append(3);
                } else if (my is IMyGenericListService<string>) {
                    ((IMyGenericListService<string>)my).Append("1");
                    ((IMyGenericListService<string>)my).Append("2");
                    ((IMyGenericListService<string>)my).Append("3");
                }
                Assert.IsTrue(my.ToString() == "1;2;3;" || my.ToString() == "3;2;1;");
            }
        }

        [Test]
        public void MyGenericServiceContainerWildcard2() {
            ServiceContainer parentServiceContainer = new ServiceContainer();
            ServiceContainer serviceContainer = new ServiceContainer(parentServiceContainer);
            ServiceContainer serviceSubContainer = new ServiceContainer();
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyAddService1());
            serviceContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<string>());
            serviceContainer.AddService(serviceSubContainer);
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<string>());
            
            List<IService> serviceList = serviceContainer.GetServices(typeof(IMyGenericListService<>));
            Assert.AreEqual(6, serviceList.Count);

            string sb = "";
            foreach (object my in serviceList) {
                Type myType = my.GetType();
                sb += myType.Name;
                int cnt = myType.GetGenericArguments().Length;
                if (cnt > 0) {
                    sb += "<";
                    foreach (var gp in myType.GetGenericArguments()) {
                        sb += gp.ToString();
                        if (--cnt > 0) sb += ",";
                    }
                    sb += ">";
                };
                sb += "; ";
            }
            Assert.AreEqual("MyReverseGenericServiceProvider`1<System.Int32>; MyGenericServiceProvider`1<System.Int32>; MyGenericServiceProvider`1<System.String>; MyReverseGenericServiceProvider`1<System.Int32>; MyReverseGenericServiceProvider`1<System.String>; MyGenericServiceProvider`1<System.Int32>; ", sb);
        }
        
        [Test]
        public void MyServiceContainerGetAllServices() {
            ServiceContainer parentServiceContainer = new ServiceContainer();
            ServiceContainer serviceContainer = new ServiceContainer(parentServiceContainer);
            ServiceContainer serviceSubContainer = new ServiceContainer();
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyAddService1());
            serviceContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<string>());
            serviceContainer.AddService(serviceSubContainer);
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<string>());
            serviceSubContainer.AddService(new MyGenericService2Provider<string, int>());

            List<IService> serviceList = serviceContainer.GetServices();
            Assert.AreEqual(9, serviceList.Count);
            
            string sb = "";
            foreach (object my in serviceList) {
                Type myType = my.GetType();
                sb += myType.Name;
                int cnt = myType.GetGenericArguments().Length;
                if (cnt > 0) {
                  sb += "<";
                  foreach (var gp in myType.GetGenericArguments()) {
                      sb += gp.ToString();
                      if (--cnt > 0) sb += ",";
                  }
                  sb += ">";
                };
                sb += "; ";
            }
            Assert.AreEqual("MyAddService1; MyReverseGenericServiceProvider`1<System.Int32>; MyGenericServiceProvider`1<System.Int32>; MyGenericServiceProvider`1<System.String>; MyReverseGenericServiceProvider`1<System.Int32>; MyReverseGenericServiceProvider`1<System.String>; MyGenericService2Provider`2<System.String,System.Int32>; MyGenericServiceProvider`1<System.Int32>; MyAddService; ", sb);
        }
        
        [Test]
        public void MyServiceContainerGetFirstServiceGeneric() {
            ServiceContainer parentServiceContainer = new ServiceContainer();
            ServiceContainer serviceContainer = new ServiceContainer(parentServiceContainer);
            ServiceContainer serviceSubContainer = new ServiceContainer();
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyGenericServiceProvider<double>());
            parentServiceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<string>());
            serviceContainer.AddService(serviceSubContainer);
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<long>());
            serviceSubContainer.AddService(new MyGenericService2Provider<string, int>());
            serviceSubContainer.AddService(new MyGenericService2Provider<string, float>());

            IMyGenericListService<int> service1 = serviceContainer.GetService<IMyGenericListService<int>>();
            Assert.IsNotNull(service1);

            IMyGenericListService<double> service2 = serviceContainer.GetService<IMyGenericListService<double>>();
            Assert.IsNotNull(service2);
            
            IMyAddService service3 = serviceContainer.GetService<IMyAddService>();
            Assert.IsNotNull(service3);

            MyGenericServiceProvider<string> service4 = serviceContainer.GetService<MyGenericServiceProvider<string>>();
            Assert.IsNotNull(service4);

            MyReverseGenericServiceProvider<long> service5 = serviceContainer.GetService<MyReverseGenericServiceProvider<long>>();
            Assert.IsNotNull(service5);

            MyGenericService2Provider<string, int> service6 = serviceContainer.GetService<MyGenericService2Provider<string, int>>();
            Assert.IsNotNull(service6);

            IMyGenericListService<float> service7 = serviceContainer.GetService<IMyGenericListService<float>>();
            Assert.IsNull(service7);

            MyGenericService2Provider<string, string> service8 = serviceContainer.GetService<MyGenericService2Provider<string, string>>();
            Assert.IsNull(service8);

            IMySubService service9 = serviceContainer.GetService<IMySubService>();
            Assert.IsNull(service9);
        }

        [Test]
        public void MyServiceContainerGetFirstService() {
            ServiceContainer parentServiceContainer = new ServiceContainer();
            ServiceContainer serviceContainer = new ServiceContainer(parentServiceContainer);
            ServiceContainer serviceSubContainer = new ServiceContainer();
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyGenericServiceProvider<double>());
            parentServiceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<string>());
            serviceContainer.AddService(serviceSubContainer);
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<long>());
            serviceSubContainer.AddService(new MyGenericService2Provider<string, int>());
            
            IService service;

            service = serviceContainer.GetService(typeof(IMyGenericListService<int>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(IMyGenericListService<int>), service);

            service = serviceContainer.GetService(typeof(IMyGenericListService<double>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(IMyGenericListService<double>), service);

            service = serviceContainer.GetService(typeof(IMyAddService));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(IMyAddService), service);

            service = serviceContainer.GetService(typeof(MyGenericServiceProvider<string>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(MyGenericServiceProvider<string>), service);

            service = serviceContainer.GetService(typeof(MyReverseGenericServiceProvider<long>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(MyReverseGenericServiceProvider<long>), service);

            service = serviceContainer.GetService(typeof(MyGenericService2Provider<string, int>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(MyGenericService2Provider<string, int>), service);

            service = serviceContainer.GetService(typeof(IMyGenericListService<float>));
            Assert.IsNull(service);

            service = serviceContainer.GetService(typeof(MyGenericService2Provider<string, string>));
            Assert.IsNull(service);

            service = serviceContainer.GetService(typeof(IMySubService));
            Assert.IsNull(service);
        }
        
        [Test]
        public void MyServiceContainerGetFirstServiceGenericWildcard() {
            ServiceContainer parentServiceContainer = new ServiceContainer();
            ServiceContainer serviceContainer = new ServiceContainer(parentServiceContainer);
            ServiceContainer serviceSubContainer = new ServiceContainer();
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyGenericServiceProvider<int>());
            parentServiceContainer.AddService(new MyGenericServiceProvider<double>());
            parentServiceContainer.AddService(new MyAddService());
            serviceContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<int>());
            serviceContainer.AddService(new MyGenericServiceProvider<string>());
            serviceContainer.AddService(serviceSubContainer);
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<int>());
            serviceSubContainer.AddService(new MyReverseGenericServiceProvider<long>());
            serviceSubContainer.AddService(new MyGenericService2Provider<string, int>());
            serviceSubContainer.AddService(new MyGenericService2Provider<string, float>());

            IService service;
            
            service = serviceContainer.GetService(typeof(IMyGenericListService<>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(IMyGenericListService<int>), service);
            
            service = serviceContainer.GetService(typeof(MyGenericServiceProvider<>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(MyGenericServiceProvider<int>), service);
            
            service = serviceContainer.GetService(typeof(MyReverseGenericServiceProvider<>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(MyReverseGenericServiceProvider<int>), service);
            
            service = serviceContainer.GetService(typeof(MyGenericService2Provider<,>));
            Assert.IsNotNull(service);
            Assert.IsInstanceOf(typeof(MyGenericService2Provider<string, int>), service);

            service = serviceContainer.GetService(typeof(MyGenericService3Provider<,,>));
            Assert.IsNull(service);
        }
        
    }
}

