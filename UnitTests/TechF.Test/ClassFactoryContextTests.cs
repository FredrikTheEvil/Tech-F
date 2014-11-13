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
    public enum TestEnum
    {
        Test1 = 0,
        Test2 = 1,
        Test3 = 2,
        Test4 = 3
    }
    static class TestClassPropertyValues
    {
        public const string StringProperty = "test";
        public const bool BooleanProperty = true;
        public const sbyte SignedByteProperty = 45;
        public const byte UnsignedByteProperty = 210;
        public const short SignedShortProperty = 2124;
        public const ushort UnsignedShortProperty = 3986;
        public const int SignedIntProperty = 849829;
        public const uint UnsignedIntProperty = 898099;
        public const long SignedLongProperty = 382918301;
        public const ulong UnsignedLongProperty = 382918301;
        public const float SingleProperty = 494.58f;
        public const double DoubleProperty = 890.112;
        public const decimal DecimalProperty = 123.456M;
        public const TestEnum EnumProperty = TestEnum.Test3;
        public static DateTimeOffset DateTimeOffsetProperty = new DateTimeOffset(1988, 2, 11, 0, 0, 0, TimeSpan.FromSeconds(0));
        public static DateTime DateTimeProperty = new DateTime(1988, 2, 11, 0, 0, 0);
    }
    public class TestClass
    {
        public string StringProperty { get; set; }
        public bool BooleanProperty { get; set; }
        public SByte SignedByteProperty { get; set; }
        public Byte UnsignedByteProperty { get; set; }
        public short SignedShortProperty { get; set; }
        public ushort UnsignedShortProperty { get; set; }
        public int SignedIntProperty { get; set; }
        public uint UnsignedIntProperty { get; set; }
        public long SignedLongProperty { get; set; }
        public ulong UnsignedLongProperty { get; set; }
        public float SingleProperty { get; set; }
        public double DoubleProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public TestEnum EnumProperty { get; set; }
        public TestEnum EnumPropertyNumeric { get; set; }
        public object NullProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public DateTimeOffset DateTimeOffsetProperty { get; set; }
    }
    public class TestClass2
    {
    }
    public class TestClass3 : TestClass
    {
    }
    public class TestClassConstructorInjectionValue
    {
        public int Property { get; private set; }

        public TestClassConstructorInjectionValue(int SignedIntProperty)
        {
            Property = SignedIntProperty;
        }
    }

    public class TestClassNoPublicConstructor
    {
        private TestClassNoPublicConstructor() { }
    }

    public class TestClassConstructiorInjectionRef
    {
        public TestClass Test { get; private set; }

        public TestClassConstructiorInjectionRef(TestClass test)
        {
            Test = test;
        }
    }

    public class ClassFactoryContextTests
    {
        private IReadOnlyDictionary<string, string> CreateProperties()
        {
            var d = new Dictionary<string, string>();

            d["StringProperty"] = TestClassPropertyValues.StringProperty;
            d["BooleanProperty"] = TestClassPropertyValues.BooleanProperty.ToString();
            d["SignedByteProperty"] = TestClassPropertyValues.SignedByteProperty.ToString();
            d["UnsignedByteProperty"] = TestClassPropertyValues.UnsignedByteProperty.ToString();
            d["SignedShortProperty"] = TestClassPropertyValues.SignedShortProperty.ToString();
            d["UnsignedShortProperty"] = TestClassPropertyValues.UnsignedShortProperty.ToString();
            d["SignedIntProperty"] = TestClassPropertyValues.SignedIntProperty.ToString();
            d["UnsignedIntProperty"] = TestClassPropertyValues.UnsignedIntProperty.ToString();
            d["SignedLongProperty"] = TestClassPropertyValues.SignedLongProperty.ToString();
            d["UnsignedLongProperty"] = TestClassPropertyValues.UnsignedLongProperty.ToString();
            d["SingleProperty"] = TestClassPropertyValues.SingleProperty.ToString();
            d["DoubleProperty"] = TestClassPropertyValues.DoubleProperty.ToString();
            d["DecimalProperty"] = TestClassPropertyValues.DecimalProperty.ToString();
            d["EnumProperty"] = TestClassPropertyValues.EnumProperty.ToString();
            d["NullProperty"] = null;
            d["EnumPropertyNumeric"] = ((int)TestClassPropertyValues.EnumProperty).ToString();
            d["DateTimeOffsetProperty"] = TestClassPropertyValues.DateTimeOffsetProperty.ToString();
            d["DateTimeProperty"] = TestClassPropertyValues.DateTimeProperty.ToString();

            return d;
        }
        private TType CreateWithProperties<TType, TImplementation>(bool persist = false) where TType : class where TImplementation : TType
        {
            var context = new ClassFactoryContext();
            var props = CreateProperties();
            context.SetFactoryType<TType, TImplementation>(props).Persist = persist;
            return context.Resolve<TType>();
        }

        [Test]
        public void TestFromGlobal()
        {
            ClassFactory.SetFactoryType<TestClass, TestClass>();
            var context = new ClassFactoryContext();
            var o = context.Resolve<TestClass>();
            ClassFactory.ClearFactoryTypeInformation<TestClass>();
            Assert.IsInstanceOf<TestClass>(o);
        }

        [Test]
        [ExpectedException(typeof(TechF.Exceptions.ClassFactoryException))]
        public void TestNonConfiguredType()
        {
            ClassFactory.ClearFactoryTypeInformation<TestClass>();
            var context = new ClassFactoryContext();
            var o = context.Resolve<TestClass>();
        }

        [Test]
        [ExpectedException(typeof(TechF.Exceptions.ClassFactoryConstructorException))]
        public void TestTypeWithoutPublicConstructor()
        {
            var context = new ClassFactoryContext();
            context.SetFactoryType<TestClassNoPublicConstructor, TestClassNoPublicConstructor>();
            var o = context.Resolve<TestClassNoPublicConstructor>();
        }
        [Test]
        public void TestContextPersistence()
        {
            var context = new ClassFactoryContext();
            context.SetFactoryType<TestClass, TestClass>(null).Persist = true;
            var testClass1 = context.Resolve<TestClass>();
            var testClass2 = context.Resolve<TestClass>();

            Assert.AreSame(testClass1, testClass2);
        }
        [Test]
        public void TestContextNonPersistence()
        {
            var context = new ClassFactoryContext();
            context.SetFactoryType<TestClass, TestClass>(null).Persist = false;
            var testClass1 = context.Resolve<TestClass>();
            var testClass2 = context.Resolve<TestClass>();

            Assert.AreNotSame(testClass1, testClass2);
        }
        [Test]
        [ExpectedException(typeof(Exceptions.ClassFactoryTypeMismatchException))]
        public void TestSetFactoryTypeTypeMismatch()
        {
            var props = CreateProperties();
            var context = new ClassFactoryContext();
            context.SetFactoryType(typeof(TestClass), typeof(TestClass2), null);
        }
        [Test]
        public void TestSetFactoryInheritance()
        {
            var context = new ClassFactoryContext();
            context.SetFactoryType(typeof(TestClass), typeof(TestClass3), null);
        }
        [Test]
        public void TestPropertyBindingString()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.StringProperty, TestClassPropertyValues.StringProperty);
        }
        [Test]
        public void TestPropertyBindingBoolean()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.BooleanProperty, TestClassPropertyValues.BooleanProperty);
        }
        [Test]
        public void TestPropertyBindingSignedByte()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.SignedByteProperty, TestClassPropertyValues.SignedByteProperty);
        }
        [Test]
        public void TestPropertyBindingUnsignedByte()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.UnsignedByteProperty, TestClassPropertyValues.UnsignedByteProperty);
        }
        [Test]
        public void TestPropertyBindingSignedShort()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.SignedShortProperty, TestClassPropertyValues.SignedShortProperty);
        }
        [Test]
        public void TestPropertyBindingUnsignedShort()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.SignedShortProperty, TestClassPropertyValues.SignedShortProperty);
        }
        [Test]
        public void TestPropertyBindingSignedInt()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.SignedIntProperty, TestClassPropertyValues.SignedIntProperty);
        }
        [Test]
        public void TestPropertyBindingUnsignedInt()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.UnsignedIntProperty, TestClassPropertyValues.UnsignedIntProperty);
        }
        [Test]
        public void TestPropertyBindingSignedLong()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.SignedLongProperty, TestClassPropertyValues.SignedLongProperty);
        }
        [Test]
        public void TestPropertyBindingUnsignedLong()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.UnsignedLongProperty, TestClassPropertyValues.UnsignedLongProperty);
        }
        [Test]
        public void TestPropertyBindingSingle()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.SingleProperty, TestClassPropertyValues.SingleProperty);
        }
        [Test]
        public void TestPropertyBindingDouble()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.DoubleProperty, TestClassPropertyValues.DoubleProperty);
        }
        [Test]
        public void TestPropertyBindingDecimal()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.DecimalProperty, TestClassPropertyValues.DecimalProperty);
        }
        [Test]
        public void TestPropertyEnum()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.EnumProperty, TestClassPropertyValues.EnumProperty);
        }
        [Test]
        public void TestPropertyEnumNumeric()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.EnumPropertyNumeric, TestClassPropertyValues.EnumProperty);
        }
        [Test]
        public void TestPropertyNull()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.NullProperty, null);
        }
        [Test]
        public void TestPropertyDateTime()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.DateTimeProperty, TestClassPropertyValues.DateTimeProperty);
        }
        [Test]
        public void TestPropertyDateTimeOffset()
        {
            var o = CreateWithProperties<TestClass, TestClass>();
            Assert.AreEqual(o.DateTimeOffsetProperty, TestClassPropertyValues.DateTimeOffsetProperty);
        }

        [Test]
        public void TestConstructorInjectionValues()
        {
            var o = CreateWithProperties<TestClassConstructorInjectionValue, TestClassConstructorInjectionValue>();
            Assert.AreEqual(TestClassPropertyValues.SignedIntProperty, o.Property);
        }
        [Test]
        [ExpectedException(typeof(Exceptions.ClassFactoryConstructorException))]
        public void TestConstructorInjectionMissingValues()
        {
            var context = new ClassFactoryContext();
            var props = new Dictionary<string, string>();
            context.SetFactoryType<TestClassConstructorInjectionValue, TestClassConstructorInjectionValue>(props);
            context.Resolve<TestClassConstructorInjectionValue>();
        }
        [Test]
        public void TestConstructorInjectionResolve()
        {
            var context = new ClassFactoryContext();
            context.SetFactoryType<TestClass, TestClass>();
            context.SetFactoryType<TestClassConstructiorInjectionRef, TestClassConstructiorInjectionRef>();
            var o = context.Resolve<TestClassConstructiorInjectionRef>();
            Assert.NotNull(o.Test);
        }
        [Test]
        public void TestNoPropertyBinding()
        {
            var context = new ClassFactoryContext();
            var props = new Dictionary<string, string>();
            props.Add("StringProperty", TestClassPropertyValues.StringProperty);
            context.SetFactoryType<TestClass, TestClass>(props).BindProperties = false;
            var o = context.Resolve<TestClass>();
            Assert.AreNotEqual(o.StringProperty, TestClassPropertyValues.StringProperty);
        }

        [Test]
        [ExpectedException(typeof(Exceptions.ClassFactoryTypeMismatchException))]
        public void TestTypeMismatch()
        {
            var context = new ClassFactoryContext();
            context.SetFactoryType(typeof(TestClass3), typeof(TestClass));
        }

        [Test]
        public void TestGetTypeOptions()
        {
            var context = new ClassFactoryContext();
            context.SetFactoryType<TestClass, TestClass>();
            context.GetTypeOptions<TestClass>().BindProperties = false;
            var o = context.Resolve<TestClass>();
            Assert.AreNotEqual(TestClassPropertyValues.StringProperty, o.StringProperty);
        }
        [Test]
        public void TestGetTypeOptionsFromRoot()
        {
            var context = new ClassFactoryContext();
            ClassFactory.SetFactoryType<TestClass, TestClass>();
            context.GetTypeOptions<TestClass>().BindProperties = false;
            var o = context.Resolve<TestClass>();
            Assert.AreNotEqual(TestClassPropertyValues.StringProperty, o.StringProperty);
        }
        [Test]
        [ExpectedException(typeof(Exceptions.ClassFactoryTypeMismatchException))]
        public void TestGetTypeOptionsFromInvalidType()
        {
            ClassFactory.ClearFactoryTypeInformation<TestClass>();
            new ClassFactoryContext().GetTypeOptions<TestClass>();
        }
        [Test]
        [ExpectedException(typeof(Exceptions.ClassFactoryException))]
        public void TestClearTypeInformation()
        {
            var context = new ClassFactoryContext();
            ClassFactory.ClearFactoryTypeInformation<TestClass>();
            context.SetFactoryType<TestClass, TestClass>();
            context.ClearFactoryTypeInformation<TestClass>();
            context.Resolve<TestClass>();
        }
    }
    //ncrunch: no coverage end
}
