﻿using Xunit;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Mc2.CrudTest.Presentation.Application.Common.Validation;

namespace Mc2.CrudTest.AcceptanceTests.UnitTests.Common.Validation
{
    public class CustomerValidationTest
    {

        [Theory]
        [InlineData("+989386623004")]
        [InlineData("+989333336058")]
        [InlineData("+989196625065")]
        public void PhoneNumberValidate_ValidatePhoneNumber_NullErrorMessage(string input)
        {

            var phoneNumberValidateAttribute = new PhoneNumberValidateAttribute();
            var result = phoneNumberValidateAttribute.GetValidationResult(input, new ValidationContext(input));
            result?.ErrorMessage.Should().Be(null);

        }

        [Theory]
        [InlineData("989386623004")]
        [InlineData("09386623004")]
        public void PhoneNumberValidate_ValidatePhoneNumber_ErrorMessage(string input)
        {

            var phoneNumberValidateAttribute = new PhoneNumberValidateAttribute();
            var result = phoneNumberValidateAttribute.GetValidationResult(input, new ValidationContext(input));
            result?.ErrorMessage.Should().NotBe(null);

        }
    }
}
