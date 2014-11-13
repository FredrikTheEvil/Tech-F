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
using TechF.Utilities;

namespace TechF.Test
{
    public class DictionaryComparisonTests
    {
        [Test]
        public void TestSameDictionariesAreEqual()
        {
            var d = new Dictionary<string, string>();
            Assert.IsTrue(DictionaryComparison<string, string>.Compare(d, d));
        }
        [Test]
        public void TestIdenticalDictionariesAreEqual()
        {
            var d1 = new Dictionary<string, string>();
            var d2 = new Dictionary<string, string>();
            d1.Add("test", "test");
            d2.Add("test", "test");
            Assert.IsTrue(DictionaryComparison<string, string>.Compare(d1, d2));
        }
        [Test]
        public void TestIdenticalKeysDifferentValuesAreNotEqual()
        {
            var d1 = new Dictionary<string, string>();
            var d2 = new Dictionary<string, string>();
            d1.Add("test", null);
            d2.Add("test", "test2");
            Assert.IsFalse(DictionaryComparison<string, string>.Compare(d1, d2));
        }
        [Test]
        public void TestDifferentKeysAreNotEqual()
        {
            var d1 = new Dictionary<string, string>();
            var d2 = new Dictionary<string, string>();
            d1.Add("test", "test2");
            d2.Add("test2", "test2");
            Assert.IsFalse(DictionaryComparison<string, string>.Compare(d1, d2));
        }
        [Test]
        public void TestDifferentNumberOfKeysAreNotEqual()
        {
            var d1 = new Dictionary<string, string>();
            var d2 = new Dictionary<string, string>();
            d1.Add("test", "test2");
            d2.Add("test", "test2");
            d1.Add("test1", "test2");
            Assert.IsFalse(DictionaryComparison<string, string>.Compare(d1, d2));
        }
    }
}
