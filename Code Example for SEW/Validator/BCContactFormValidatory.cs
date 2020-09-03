using System;
using System.Globalization;
using FluentValidation;
using FluentValidation.Validators;
using OfficeFreedom.AppCode.ViewModels;

namespace OfficeFreedom.AppCode.Validation
{
    public class BCContactFormValidator : AbstractValidator<BCContactFormViewModel>
    {
        public BCContactFormValidator()
        {
            RuleFor(m => m.BCContactForm.FirstName)
				.NotEmpty().WithMessage("Please input a value for 'First Name' ")
				.Length(1, 50).WithMessage("Please input a value no longer than 50 characters.");
            RuleFor(m => m.BCContactForm.LastName)
                .NotEmpty().WithMessage("Please input a value for 'Last Name' ")
                .Length(1, 50).WithMessage("Please input a value no longer than 50 characters.");
            RuleFor(m => m.BCContactForm.CompanyName)
                .NotEmpty().WithMessage("Please input a value for 'Company Name' ")
                .Length(1, 50).WithMessage("Please input a value no longer than 50 characters.");

            RuleFor(m => m.BCContactForm.Email)
				.NotEmpty().WithMessage("Please input a value for 'Email'.")
				.EmailAddress().WithMessage("Please enter a valid email address.")
				.Length(1, 50).WithMessage("Please input a value no longer than 50 characters.");
			
			RuleFor(m => m.BCContactForm.Phone)
				.NotEmpty().WithMessage("Please input a value for 'Phone'.")
				.Length(10, 15).WithMessage("The value for telephone number should be 11 digits.");

            RuleFor(m => m.BCContactForm.SizeRequired)
                .NotEmpty().WithMessage("Please input a value for 'Size Required' ");

            RuleFor(m => m.BCContactForm.DatePicker)
                .NotEmpty().WithMessage("Please select a value for 'Viewing Date' ")
                .Length(1, 50).WithMessage("Please select a value.");

            RuleFor(m => m.BCContactForm.ViewingTime)
                .NotEmpty().WithMessage("Please select a value for 'Viewing Time' ")
                .Length(1, 50).WithMessage("Please select a value");
            RuleFor(m => m.BCContactForm.Message)
                .Length(1, 500).WithMessage("Your 'Message should be no longer than 500 characters.");


            //RuleFor(m => m.FormRealEstate.PolicyNumber)
            // .Length(1, 50).WithMessage("Please input a value no longer than 50 characters.");

            //RuleFor(m => m.FormRealEstate.Enquiry)
            // .NotEmpty().WithMessage("Please input a value for 'Enquiry' ")
            // .Length(1, 50).WithMessage("Please input a value no longer than 50 characters.");

            //RuleFor(m => m.FormRealEstate.Message)
            // .NotEmpty().WithMessage("Please input a value for 'Message' ")
            // .Length(1, 500).WithMessage("Please input a value no longer than 500 characters.");
        }

        // http://stackoverflow.com/questions/7461080/fastest-way-to-check-if-string-contains-only-digits
        private bool IsDigitsOnly(string str)
        {
            if (str == null)
            {
                return false;
            }

            var trimmedUnspacedValue = str.Trim().Replace(" ", "");
            foreach (char c in trimmedUnspacedValue)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        bool MustBeFutureDate(string date)
        {
            if (date == null)
                return false;

            var dateToParse = DateTime.ParseExact(date, "dd/MM/yyyy", new CultureInfo("en-GB"));
            DateTime timeSpan = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"), new CultureInfo("en-GB"));
            //var timeSpan = DateTime.Now(System.ContactForm.Globalization.CultureInfo.InvariantCulture););
            return dateToParse >= timeSpan;
        }
    }
}