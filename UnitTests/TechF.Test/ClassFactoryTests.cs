// Tech-F Framework
// (C) Copyright 2014 Fredrik A.Kristiansen<fredrikaxk@gmail.com>
//
// All rights reserved.This program and the accompanying materials
// are made available under the terms of the GNU Lesser General Public License
// (LGPL) version 2.1 which accompanies this distribution, and is available at
// http://www.gnu.org/licenses/lgpl-2.1.html
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU
// Lesser General Public License for more details.
//
// Contributors:
//     Fredrik A. Kristiansen<fredrikaxk@gmail.com>
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
