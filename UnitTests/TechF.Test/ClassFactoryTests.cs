using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Test
{
    //ncrunch: no coverage start
    public class ClassFactoryTests
    {
        [Test]
        public void TestResolve()
        {
            ClassFactory.SetFactoryType<TestClass, TestClass>();
            var o = ClassFactory.Resolve<TestClass>();
        }
        [Test]
        [ExpectedException(typeof(Exceptions.ClassFactoryTypeMismatchException))]
        public void TestTypeMismatch()
        {
            ClassFactory.SetFactoryType(typeof(TestClass3), typeof(TestClass));
        }
        [Test]
        public void ResolveTwice()
        {
            ClassFactory.SetFactoryType<TestClass, TestClass>();
            var o1 = ClassFactory.Resolve<TestClass>();
            var o2 = ClassFactory.Resolve<TestClass>();
            Assert.NotNull(o1);
            Assert.NotNull(o2);
        }
        [Test]
        [ExpectedException(typeof(Exceptions.ClassFactoryTypeMismatchException))]
        public void ResolveNonConfiguredType()
        {
            ClassFactory.Resolve<TestClass2>();
        }
    }
    //ncrunch: no coverage end
}
