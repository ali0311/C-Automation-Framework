using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WebDriverTests
{
    public class Constants
    {
        public static readonly string IEDriver = "InternetExplorerDriver";
        public static readonly string IECertErrPageTitle = "Certificate Error: Navigation Blocked";
        public static readonly string ChromeDriver = "ChromeDriver";
        public static readonly string TeamEnv = "TEAM";
        public static readonly string QAEnv = "QA";
        public static readonly string ProdEnv = "PROD";
        public static readonly string GlobalPassword = "@Pass1234";
        public static readonly string QAKey = "QA";
        public static readonly string UserPassword = "pass123";
        public static readonly string TestStarted = "Test execution started..";
        public static readonly string TestCompleted = "Test execution completed\n";
        public static readonly string DateFormatMMDDYYYY = "MM/dd/yyyy";
    }
}
