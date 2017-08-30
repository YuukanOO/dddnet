using DDDNet.Attributes;
using DDDNet.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDDNet.Tests
{
    [RaiseEvent(typeof(string))]
    class User
    {
        public string Name { get; set; }
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
        }
    }
}
