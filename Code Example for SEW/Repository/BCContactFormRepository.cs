using OfficeFreedom.AppCode.Helpers;
using OfficeFreedom.AppCode.Models;

namespace OfficeFreedom.AppCode.Repository
{
    public class BCContactFormRepository : Repository<BCContactForm>
    {
        private string umbracoDbDSN;

        public BCContactFormRepository()
            : this(AppSettings.umbracoDbDSN)
        {
        }

        public BCContactFormRepository(string connectionString) : base(connectionString)
        {
        }
    }
}