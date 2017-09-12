using DDDNet.Attributes;
using DDDNet.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDDNet.Tests
{
    class User
    {
        public string Name { get; set; }
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
        }
    }
}
