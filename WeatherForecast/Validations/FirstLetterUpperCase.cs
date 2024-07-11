﻿using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.Validations
{
    public class FirstLetterUpperCase: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) 
            {
                return ValidationResult.Success;
            }

            var firstLetter = value.ToString()[0].ToString();

            if (firstLetter!= firstLetter.ToUpper())
            {
                return new ValidationResult("First letter must be upper case, dude.");
            }

            return ValidationResult.Success;
        }
    }
}
