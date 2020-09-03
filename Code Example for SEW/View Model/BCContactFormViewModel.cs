using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using OfficeFreedom.AppCode.Models;
using OfficeFreedom.AppCode.Validation;
using OfficeFreedom.com.salesforce.partner;
using Umbraco.Web.Models;

namespace OfficeFreedom.AppCode.ViewModels
{
    [Validator(typeof(BCContactFormValidator))]
    public class BCContactFormViewModel
    {
        public BCContactFormViewModel()
        {
            ValidationErrors = new List<string>();
        }
        public List<string> ValidationErrors { get; internal set; }
        public BCContactForm BCContactForm { get; set; }
        public string AdminEmail { get; set; }
        public RenderModel UmbracoModel { get; set; }
    }
}