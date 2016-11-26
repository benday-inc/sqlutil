using System;
using System.Linq;

namespace Benday.SqlServerUtilities.UnitTests.ViewModels
{
    public abstract class ViewModelFixtureBase
    {
        protected string _ValidConnectionStringUseIntegratedSecurity = "Server=the_server; Database=the_database_name; Integrated Security=True;";
        protected string _ValidConnectionStringUserNamePassword = "Server=the_server; Database=the_database_name; User Id=the_username; Password=the_password;";
    }
}
