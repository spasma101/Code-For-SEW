using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeFreedom.AppCode.Models
{
	public class BCContactForm
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public string CompanyName { get; set; }

		public string Email { get; set; }
		public string Phone { get; set; }
		public int SizeRequired { get; set; }
        public string DatePicker { get; set; }
        public string ViewingTime { get; set; }
		public string PageName { get; set; }
		public string PageURL { get; set; }
		public string bcRefId { get; set; }
		public string OriginalUrl { get; set; }
		public string Message { get; set; }

		//public string Enquiry { get; set; }
		//public string Message { get; set; }

	}
}