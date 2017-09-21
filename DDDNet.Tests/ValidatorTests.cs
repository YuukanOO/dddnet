using DDDNet.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DDDNet.Tests
{
    class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public Item Item { get; set; }
    }

    class Item
    {
        public string Code { get; set; }
        public string Label { get; set; }
    }

    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void TestStringIsRequired()
        {
            var validator = Validator.For(nameof(TestStringIsRequired))
                .IsRequired("EmptyString", "")
                .IsRequired("NullString", (string)null)
                .IsRequired("WhiteSpacedString", "  ")
                .IsRequired("ValidString", "somestring");

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(3, validator.Errors.Count);
            Assert.AreEqual("EmptyString", validator.Errors[0].Field);
            Assert.AreEqual("NullString", validator.Errors[1].Field);
            Assert.AreEqual("WhiteSpacedString", validator.Errors[2].Field);
        }

        [TestMethod]
        public void TestGuidIsRequired()
        {
            var validator = Validator.For(nameof(TestGuidIsRequired))
                .IsRequired("EmptyGuid", Guid.Empty)
                .IsRequired("ValidGuid", Guid.NewGuid());

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(1, validator.Errors.Count);
            Assert.AreEqual("EmptyGuid", validator.Errors[0].Field);
        }

        [TestMethod]
        public void TestGenericIsRequired()
        {
            var validator = Validator.For(nameof(TestGenericIsRequired))
                .IsRequired("NullClass", (User)null)
                .IsRequired("NullableGuid", (Guid?)null)
                .IsRequired("NullableInteger", (int?)null)
                .IsRequired("Integer", 0)
                .IsRequired("ValidClass", new User());

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(3, validator.Errors.Count);
            Assert.AreEqual("NullClass", validator.Errors[0].Field);
            Assert.AreEqual("NullableGuid", validator.Errors[1].Field);
            Assert.AreEqual("NullableInteger", validator.Errors[2].Field);
        }

        [TestMethod]
        public void TestHasMinimumLength()
        {
            var validator = Validator.For(nameof(TestHasMinimumLength))
                .HasMinimumLength("TooShortString", "a", 3)
                .HasMinimumLength("TooShortWhitespacedString", "abc    ", 4)
                .HasMinimumLength("ExactLengthString", "abc", 3)
                .HasMinimumLength("ValidString", "somethinglonger", 5)
                .HasMinimumLength("EmptyString", "", 3);
            
            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(2, validator.Errors.Count);
            Assert.AreEqual("TooShortString", validator.Errors[0].Field);
            Assert.AreEqual("TooShortWhitespacedString", validator.Errors[1].Field);
        }

        [TestMethod]
        public void TestHasMaximumLength()
        {
            var validator = Validator.For(nameof(TestHasMaximumLength))
                .HasMaximumLength("TooLongString", "somelongstring", 3)
                .HasMaximumLength("TooShortWhitespacedString", "aze     ", 4)
                .HasMaximumLength("ExactLengthString", "abc", 3)
                .HasMaximumLength("EmptyString", "", 3);
            
            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(1, validator.Errors.Count);
            Assert.AreEqual("TooLongString", validator.Errors[0].Field);
        }

        [TestMethod]
        public void TestIsEmail()
        {
            var validator = Validator.For(nameof(TestIsEmail))
                .IsEmail("WrongEmail", "john@doe")
                .IsEmail("WrongEmail2", "john.doe.fr")
                .IsEmail("WrongEmail3", "john@doe.")
                .IsEmail("EmptyEmail", "")
                .IsEmail("ValidEmail", "john@doe.fr");

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(3, validator.Errors.Count);
            Assert.AreEqual("WrongEmail", validator.Errors[0].Field);
            Assert.AreEqual("WrongEmail2", validator.Errors[1].Field);
            Assert.AreEqual("WrongEmail3", validator.Errors[2].Field);
        }

        [TestMethod]

        public void TestIsUnique()
        {
            var validator = Validator.For(nameof(TestIsUnique))
                .IsUnique("Falsy", () => false)
                .IsUnique("Truthy", () => true);

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(1, validator.Errors.Count);
            Assert.AreEqual("Falsy", validator.Errors[0].Field);
        }

        [TestMethod]
        public void TestAreEqual()
        {
            var now = DateTime.UtcNow;

            var validator = Validator.For(nameof(TestAreEqual))
                .AreEqual("NotEqualIntegers", 2, 3)
                .AreEqual("EqualIntegers", 3, 3)
                .AreEqual("NotEqualDates", now, now.AddDays(3))
                .AreEqual("EqualDates", now, now)
                .AreEqual("NullDate", (DateTime?)null, now);

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(2, validator.Errors.Count);
            Assert.AreEqual("NotEqualIntegers", validator.Errors[0].Field);
            Assert.AreEqual("NotEqualDates", validator.Errors[1].Field);
        }

        [TestMethod]
        public void TestForObject()
        {
            
        }

        [TestMethod]
        public void TestEach()
        {

        }
        
        [TestMethod]
        public void TestIsLessThan()
        {

        }

        [TestMethod]
        public void TestIsLessThanOrEqual()
        {

        }

        [TestMethod]
        public void TestIsGreaterThan()
        {

        }

        [TestMethod]
        public void TestIsGreaterThanOrEqual()
        {

        }
    }
}
