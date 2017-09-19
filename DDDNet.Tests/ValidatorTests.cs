using DDDNet.Attributes;
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
        public void TestBuiltInValidators()
        {
            var name = "john";

            var validator = Validator.For<User>()
                                .IsRequired("Name", name)
                                .HasMinimumLength("Name", name, 2)
                                .HasMaximumLength("Name", name, 3);

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(validator.Errors.Count, 1);
            Assert.AreEqual(validator.Errors[0].Resource, "User");
            Assert.AreEqual(validator.Errors[0].Field, "Name");
            Assert.AreEqual(validator.Errors[0].Code, "HasMaximumLength");
            Assert.AreEqual(validator.Errors[0].CodeData, 3);

            Assert.ThrowsException<ValidationException>(() => validator.Throw());

            validator = Validator.For<User>()
                .IsUnique("Name", () => true);

            Assert.IsFalse(validator.HasError);

            validator = Validator.For<User>()
                .IsUnique("Name", () => false);

            Assert.IsTrue(validator.HasError);

            validator = Validator.For<User>()
                .IsEmail("Email", "email@");

            Assert.IsTrue(validator.HasError);

            validator = Validator.For<User>()
                .IsEmail("Email", "email@something.com");

            Assert.IsFalse(validator.HasError);

            var items = new[] {
                new Item() { Code = "code1", Label = "First code" },
                new Item(),
                new Item() { Code = "code3"},
            };

            validator = Validator.For<User>()
                .Each(nameof(Item), items, (nestedValidator, o) =>
                {
                    nestedValidator
                        .IsRequired(nameof(o.Code), o.Code)
                        .IsRequired(nameof(o.Label), o.Label);
                });

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(validator.Errors[0].Resource, "User");
            Assert.AreEqual(validator.Errors[0].Field, "Item[1].Code");
            Assert.AreEqual(validator.Errors[1].Field, "Item[1].Label");
            Assert.AreEqual(validator.Errors[2].Field, "Item[2].Label");

            validator = Validator.For<User>()
                .IsRequired<Item>(nameof(Item), null);

            Assert.IsTrue(validator.HasError);

            validator = Validator.For<User>()
                .IsRequired<Item>(nameof(Item), new Item());

            Assert.IsFalse(validator.HasError);

            validator = Validator.For<User>()
                .AreEqual(nameof(User.Password), "password", nameof(User.PasswordConfirm), "password");

            Assert.IsFalse(validator.HasError);

            validator = Validator.For<User>()
                .AreEqual(nameof(User.Password), "password", nameof(User.PasswordConfirm), "doesnotmatch");

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(1, validator.Errors.Count);
            Assert.AreEqual(validator.Errors[0].Field, "Password");
            Assert.AreEqual(validator.Errors[0].Code, "AreEqual");
            Assert.AreEqual(validator.Errors[0].CodeData, "PasswordConfirm");

            var user = new User();

            validator = Validator.For<User>()
                .For(nameof(User.Item), user.Item, (v, o) =>
                {
                    v
                        .IsRequired(nameof(Item.Code), o?.Code)
                        .IsRequired(nameof(Item.Label), o?.Label);
                });

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(validator.Errors[0].Field, "Item.Code");
            Assert.AreEqual(validator.Errors[1].Field, "Item.Label");

            validator = Validator.For<DateTime>()
                .IsLessThan(nameof(DateTime), DateTime.UtcNow, DateTime.UtcNow.AddSeconds(30))
                .IsLessThan("Int", 5, 6);

            Assert.IsFalse(validator.HasError);

            validator = Validator.For<DateTime>()
                .IsLessThan(nameof(DateTime), DateTime.UtcNow, DateTime.UtcNow.AddSeconds(-30))
                .IsLessThan("Int", 5, 5);

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(validator.Errors.Count, 2);

            validator = Validator.For<DateTime>()
                .IsGreaterThan(nameof(DateTime), DateTime.UtcNow, DateTime.UtcNow.AddSeconds(-30))
                .IsGreaterThan("Int", 6, 5);

            Assert.IsFalse(validator.HasError);

            validator = Validator.For<DateTime>()
                .IsGreaterThan(nameof(DateTime), DateTime.UtcNow, DateTime.UtcNow.AddSeconds(30))
                .IsGreaterThan("Int", 5, 5)
                .IsGreaterThan("Int", 4, 5);

            Assert.IsTrue(validator.HasError);
            Assert.AreEqual(validator.Errors.Count, 3);

            var now = DateTime.UtcNow;

            validator = Validator.For<DateTime>()
                .IsLessThanOrEqual(nameof(DateTime), now, now)
                .IsLessThanOrEqual("Int", 5, 5)
                .IsLessThanOrEqual("Int", 3, 5);

            Assert.IsFalse(validator.HasError);

            validator = Validator.For<DateTime>()
                .IsLessThanOrEqual(nameof(DateTime), now, now)
                .IsLessThanOrEqual("Int", 5, 2);

            Assert.IsTrue(validator.HasError);
        }
    }
}
