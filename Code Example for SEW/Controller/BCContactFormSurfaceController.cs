using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using log4net;
using OfficeFreedom.AppCode.Helpers;
using OfficeFreedom.AppCode.Models;
using OfficeFreedom.AppCode.Repository;
using OfficeFreedom.AppCode.Services;
using OfficeFreedom.AppCode.Validation;
using OfficeFreedom.AppCode.ViewModels;
using OfficeFreedom.Controllers;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace OfficeFreedom.AppCode.Controllers
{
    public class BCContactFormSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        private new static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private SalesForceController _salesForceController;
        private readonly BCContactFormRepository _bCContactFormRepository;
        private readonly SendEmailService _sendEmailService;

        public BCContactFormSurfaceController()
            : this(
                new SendEmailService(),
                new BCContactFormRepository(AppSettings.umbracoDbDSN),
                new SalesForceController()
                )
        {
        }

        public BCContactFormSurfaceController(
            SendEmailService sendEmailService,
            BCContactFormRepository bCContactFormRepository,
            SalesForceController salesForceController
            )
        {
            _sendEmailService = sendEmailService;
            _bCContactFormRepository = bCContactFormRepository;
            _salesForceController = salesForceController;
        }

        [ValidateAntiForgeryToken]
        [System.Web.Mvc.HttpPost]
        public ActionResult BCContactForm(BCContactFormViewModel model)
        {
            TempData["Success"] = "false";

            if (!ModelState.IsValid)
            {
                var modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                ViewBag.FormValidatorErrors = modelErrors;
                return CurrentUmbracoPage();
            }

            var validator = new BCContactFormValidator();
            var results = validator.Validate(model);
            if (!results.IsValid)
            {
                foreach (var error in results.Errors) { model.ValidationErrors.Add(error.ErrorMessage); }
                ViewBag.FormValidatorErrors = model.ValidationErrors;
                return CurrentUmbracoPage();
            }

            var formModel = new BCContactForm
            {
                FirstName = model.BCContactForm.FirstName,
                LastName = model.BCContactForm.LastName,
                CompanyName = model.BCContactForm.CompanyName,
                Email = model.BCContactForm.Email,
                Phone = model.BCContactForm.Phone,
                SizeRequired = model.BCContactForm.SizeRequired,
                Message = model.BCContactForm.Message,
                DatePicker = model.BCContactForm.DatePicker,
                ViewingTime = model.BCContactForm.ViewingTime,
                PageName = model.BCContactForm.PageName,
                PageURL = model.BCContactForm.PageURL,
                bcRefId = model.BCContactForm.bcRefId,
                OriginalUrl = model.BCContactForm.OriginalUrl


                //PolicyNumber = model.FormRealEstate.PolicyNumber,
                //Enquiry = model.FormRealEstate.Enquiry,
                //Message = model.FormRealEstate.Message
            };

            var id = _bCContactFormRepository.Add(formModel);


            //Stringify data and post to salesforce using Mark salesforce connector

            var salesForceBCIntroduction = new Dictionary<String, String>
            {
                {"Lead_Email__c", model.BCContactForm.Email},
                {"Lead_First_Name__c", model.BCContactForm.FirstName},
                {"Lead_Last_Name__c", model.BCContactForm.LastName},
                {"Lead_Phone__c", model.BCContactForm.Phone},
                {"Related_Business_Centre__c", model.BCContactForm.bcRefId}
                //{"Related_Business_Centre__c", model.BCContactForm.bcRefId.ToString()}
            };

            var bCSfIntroStringified = _salesForceController.PushSfbcIntroduction(new JavaScriptSerializer().Serialize(salesForceBCIntroduction));

            if (bCSfIntroStringified == null)
            {
                LogHelper.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Method PushSfbcIntroduction: BC Post to salesforce failed");
            }
            else
            {
                LogHelper.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Method PushSfbcIntroduction: BC Post to salesforce success" + bCSfIntroStringified);
            }
            

            var isTestingEnvironment = AppSettings.IsTestingEnvironment;

            #region Send Confirmation Emails
            try
            {
                var to = new List<string>();

                // Set a dafault value

                var notificationEmailRecipient = EmailRecipient(model, isTestingEnvironment);

                model.AdminEmail = notificationEmailRecipient;
                const string subject = "Form - Business Center Contact Form - A new message has been successfully submitted";

                to.Add(notificationEmailRecipient);
                const string templatePath = "~/Views/Shared/Emails/BCContactFormTemplate.cshtml";
                _sendEmailService.SendEmail(to, Server.MapPath(templatePath), model, subject);

            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("ERROR: BCContactFormSurfaceController.Form (Send confirmation email): " + ex));
                TempData["Error"] = "true";
                return Redirect(model.BCContactForm.PageURL + "#bcErrorBlock");
            }
            #endregion Send email

            //Add a message in TempData which will be available 
            //in the View after the redirect 
            // TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);

            //redirect to current page to clear the form
            TempData["Success"] = "true";
            TempData["RedirectUrl"] = model.BCContactForm.OriginalUrl;
            return Redirect(model.BCContactForm.OriginalUrl);
        }



        private static string EmailRecipient(BCContactFormViewModel model, string isTestingEnvironment)
        {
            string emailRecipient = "";

            emailRecipient = (isTestingEnvironment == "false") ? AppSettings.ProductionEmailRecipient : AppSettings.TestingEmailRecipent;

            return emailRecipient;
        }



    }
}