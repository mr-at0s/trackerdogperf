using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrackerDog;
using TrackerDog.Configuration;

namespace PerfTest
{
    [TestClass]
    public class UnitTest1
    {
        private ITrackableObjectFactory _trackableObjectFactory;

        public class Class0
        {
            public virtual Class1 Class1 { get; set; }
        }

        public class Class1
        {
            public virtual ISet<Class2> Class2s { get; set; }
        }

        public class Class2
        {
            public int Number { get; set; }
        }
        

        [TestInitialize()]
        public void Initialize()
        {
            IObjectChangeTrackingConfiguration config = ObjectChangeTracking.CreateConfiguration();
            config.TrackThisTypeRecursive<Class0>();

            _trackableObjectFactory = config.CreateTrackableObjectFactory();
        }

        [TestMethod]
        [DataRow(1 /* number of repeats */, 10 /* number of list items */)]
        public void PerfTest(int n, int m)
        {
            var class0 = new Class0
            {
                Class1 = new Class1
                {
                    Class2s = new HashSet<Class2>(
                        Enumerable.Range(0, m).Select(i => new Class2{ Number = i })
                        )
                }
            };

            var proxyList = new List<Class0>();
            for (int i = 0; i < n; ++i)
            {
                var proxy = _trackableObjectFactory.CreateFrom(class0);
                proxyList.Add(proxy);
            }

            Assert.AreEqual( proxyList.Count, n);
        }
    }
}
