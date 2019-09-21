using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.TestFramework;
using static Application.WebDriverTests.BasePage;
using System.IO;
using OpenQA.Selenium;
using static System.Configuration.ConfigurationManager;
using static Application.WebDriverTests.Constants;
using System.Data;
using static Application.TestFramework.DataMethods;

namespace Application.WebDriverTests
{
    /// <summary>
    /// Summary description for IndigoDemoTest
    /// </summary>
    [TestClass]
    public class IndigoDemoTest
    {
        private string logFileName;
        private string testStartTime;
        private Logger log;
        private IWebDriver driver;
        private IndigoLoginPage userLogin;

        [TestInitialize()]
        public void TestInitialize()
        {
            testStartTime = Utility.GenerateTimeStamp();
            logFileName = string.Concat(TestContext.TestName, "_", testStartTime.Replace(" ", "_"));
            log = SetLogger(TestContext, Path.Combine(TestContext.DeploymentDirectory, logFileName + ".txt"));

            driver = LoadBrowserConfig(driver);
            int pageLoadTime = Convert.ToUInt16(AppSettings["PageLoadTimeOutSeconds"]);
            userLogin = new IndigoLoginPage(driver, pageLoadTime);
            userLogin.NavigateTo(EnvironmentURL.IndigoLoginURL);
        }

        [TestMethod]
        [TestCategory("Sanity")]
        public void WebDriverTimesheetTest()
        {
            log.LogMessage(TestStarted);
            var commonPage = userLogin.FillTimesheet();
            // var commonPage = userLogin.ClickLogin();

            // commonPage.VerifyLoginPageTitle("Online Flight Booking for Domestic & International Destinations | IndiGo");

            //Standard convention to design any test script ----------------------------------------------------------

            //Login
            /* commonPage.LoginToIndigo("8983424896", "Mind@2620")
                .FlyingHigh("Delhi", "Muscat")
                .VerifyHomePageTitle("Select Flights for Domestic & International Destinations | IndiGo"); */

            //--------------------------------------------------------------------------------------------------------

            log.LogMessage(TestCompleted);
        }

        [TestMethod]
        [TestCategory("Sanity")]
        public void ReadValueFromExcelTest()
        {
            log.LogMessage(TestStarted);

            //Login via reading file from excel
            string selectQuery = "SELECT app, phone, password FROM [login$] WHERE index=0";
            DataTable credData = ImportExcelSheetToDatatTable(DataType.TestData, "logins.xlsx", selectQuery);
            string app = credData.Rows[0]["app"].ToString();
            string phoneNo = credData.Rows[0]["phone"].ToString();
            string password = credData.Rows[0]["password"].ToString();

            Console.WriteLine("Check Values {0} , {1} , {2}", app, phoneNo, password);

            var commonPage = userLogin.ClickLogin();

            //Login
            commonPage.LoginToIndigo(phoneNo, password);

            log.LogMessage(TestCompleted);
        }

        [TestMethod]
        [TestCategory("Sanity")]
        public void WriteValueToExcelTest()
        {
            log.LogMessage(TestStarted);

            //Adding dynamic data to excel
            string insertQuery = "INSERT INTO [CreateData$] ([index],[col_1],[col_2]) VALUES(@index, @col_val1, @col_val2)";
            string indexValue = Utility.GenerateTimeStamp();
            string firstColValue = Utility.GenerateTimeStamp();
            string secondColValue = Utility.GenerateTimeStamp();
            var values = new Dictionary<string, string>
            {
                {"@index", indexValue},
                {"@col_val1", firstColValue},
                {"@col_val2", secondColValue}
            };
            ExportDataTableToExcelSheet(DataType.DataCreation, "CreateData.xlsx", insertQuery, values);

            log.LogMessage(TestCompleted);
        }

        [TestCleanup()]
        public void TestCleanUp()
        {
            log.AttachScreenShotIfTestCaseFailed(driver, logFileName);
            log.LogMessage("Test " + TestContext.CurrentTestOutcome.ToString());
            log.AttachLogFile(Path.Combine(TestContext.DeploymentDirectory, logFileName + ".txt"));
            driver.Quit();
        }

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;
    }
}
