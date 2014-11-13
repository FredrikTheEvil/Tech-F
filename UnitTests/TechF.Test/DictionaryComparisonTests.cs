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
