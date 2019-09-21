using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using static Application.TestFramework.Validate;
using Application.TestFramework;
using static Application.TestFramework.DataMethods;
using System.Data;

namespace Application.WebDriverTests
{
    public class IndigoLoginPage : BasePage
    {
        public IndigoLoginPage(IWebDriver browserDriver, int pageLoadTimeoutSeconds) : base(browserDriver, pageLoadTimeoutSeconds)
        {
        }
        public IndigoLoginPage(IWebDriver browserDriver) : base(browserDriver)
        {
        }

        private By LoginLink => By.LinkText("Login");
        private By MobileNoTextBox => By.XPath("//input[@name='memberLogin.MemberMobileNo']");
        private By PasswordTextBox => By.XPath("//input[@name='memberLogin.Password']");
        private By LoginButton => By.XPath("//div[@class='ig-modal-login-btn']/button[@type='submit']");
        private By UserLoginLink => By.XPath("//span[@class='buttonText']");

        //Test
        private By timeSheetLink => By.XPath("//a[@href='showTimesheetDetails.do' and @class='active']");
        private By project => By.XPath("//select[@name='projectId']");
        private By taskDate => By.XPath("//input[@id='calendar']");
        private By videoSlider1 => By.XPath("//div[@id='ui_tpicker_hour_timein']/a[@class='ui-slider-handle ui-state-default ui-corner-all']");
        private By bar1 => By.XPath("//div[@id='ui_tpicker_hour_timein']");
        private By videoSlider2 => By.XPath("/div[@id='ui_tpicker_hour_timeout']/a");
        private By bar2 => By.XPath("//div[@id='ui_tpicker_hour_timeout']");
        private By duration => By.XPath("//input[@id='timediff']");
        private By estimation => By.XPath("//input[@id='estimatedTime']");
        private By status => By.XPath("//select[@name='taskStatusId']");
        private By desc => By.XPath("//textarea[@name='taskDescription']");
        private By submit => By.XPath("//input[@value='Add to Daily Work']");


        public IndigoLoginPage FillTimesheet()
        {
            Sleep(15);
            driver.FindElement(submit).GetAttribute("innertext");
            Click(timeSheetLink, "click");
            SelectComboBoxByIndex(project, 3, "jkl");
            EnterTextJS(taskDate, "08-22-2019", true, "tlk");
            DragVideoSliderHorizontally(videoSlider1,bar1, 1, "kl");
            DragVideoSliderHorizontally(videoSlider2,bar2, 1, "kl");
            EnterTextJS(duration, "4", true, "case");
            EnterTextJS(estimation, "4", true, "case");
            SelectComboBoxByIndex(status, 4, "ito");
            EnterText(desc, "workday", "klsl");
            Click(submit, "clo");






            return this;
        }








        public IndigoLoginPage ClickLogin()
        {
            Click(LoginLink, "Login");
            return this;
        }
        public IndigoLoginPage VerifyLoginPageTitle(string expectedTitle)
        {
            driver.Navigate().Refresh();
            string actualTitle = driver.Title.Trim();
            AssertAreEqual(expectedTitle, actualTitle, "Expected title did not match to actual");
            return this;
        }
        public IndigoLoginPage EnterMobileNumber(string number)
        {
            EnterText(MobileNoTextBox, number, "Mobile number");
            return this;
        }
        public IndigoLoginPage EnterPassword(string password)
        {
            EnterText(PasswordTextBox, password, "Password");
            return this;
        }
        public IndigoHomePage ClickUserLogin()
        {
            Click(UserLoginLink, "Login");
            return new IndigoHomePage(driver);
        }
        public IndigoHomePage LoginToIndigo(string number, string password)
        {
            Sleep(1);
            EnterMobileNumber(number);
            EnterPassword(password);
            ClickUserLogin();
            return new IndigoHomePage(driver);
        }
        //Getting login credential from login.xlsx
        public Dictionary<Login, string> ReadDataFromExcel(WebApplication application)
        {
            var app = application.EnumToDescription();
            string selectLogin = string.Format("SELECT phone, password FROM [logins$] WHERE app = '{0}'", app);
            DataTable  login = ImportExcelSheetToDatatTable(DataType.TestData, "logins.xlsx", selectLogin);

            try
            {
                string phone = login.Rows[0]["phone"].ToString();
                string password = login.Rows[0]["password"].ToString();

                var indigoLogin = new Dictionary<Login, string>()
                {
                    { Login.UserId, phone},
                    { Login.Password, password}
                };
                return indigoLogin;
            }
            catch
            {
                throw new ArgumentException(string.Format("There is no matching Login for Application: '{0}'", app));
            }
        }
        //write data to excel...can be used to create data and store it in excel. Using CreateData.xlsx
        public static void WriteDataToExcel(string sheetName, string fileName)
        {
            string insert = "INSERT INTO ["+sheetName+"$"+"] ([index],[col_1],[col_2]) VALUES(@index, @col_1, @col_2)";
            string index = Utility.GenerateTimeStamp();
            string firstColValue = Utility.GenerateTimeStamp();
            string SecondColValue = Utility.GenerateTimeStamp();
            var values = new Dictionary<string, string>()
                {
                    { "@index", index },
                    { "@col_1", firstColValue },
                    { "@col_2", SecondColValue }
                };
            ExportDataTableToExcelSheet(DataType.DataCreation, fileName, insert, values);
        }
    }
}
