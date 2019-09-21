using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.TestFramework;
using OpenQA.Selenium;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using static System.Configuration.ConfigurationManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using Microsoft.Office.Interop.Excel;

namespace Application.WebDriverTests
{
    public class BasePage : WebDriverBase
    {
        public BasePage(IWebDriver browserDriver) : base(browserDriver)
        {
        }

        public BasePage(IWebDriver browserDriver, int pageLoadTimeoutSeconds) : base(browserDriver, pageLoadTimeoutSeconds)
        {
        }

        private static string BrowserName => AppSettings["Browser"];
        private static string ChromeDriverPath => AppSettings["ChromeDriverPath"];
        private static string IEDriverPath => AppSettings["IEDriverPath"];
        private static string TestDataFolder => AppSettings["TestDataFolder"];
        public static string EnvironmentType => ReadEnvironmentSetting("Type");

        public void NavigateTo(EnvironmentURL urlName)
        {
            string appUrl = ReadEnvironmentUrl(urlName).ToString();
            NavigateTo(appUrl);
            log.LogMessage("Navigated to url : " + appUrl);
        }
        public static Uri ReadEnvironmentUrl(EnvironmentURL urlName)
        {
            return new Uri(ReadEnvironmentSetting(urlName.EnumToDescription()));
        }
        public static string ReadEnvironmentSetting(string settingName)
        {
            var filePath = GetSettingsFilePath();
            string returnVal = string.Empty;
            try
            {
                Dictionary<string, string> settings = ReadAllSettingsFromFile(filePath);
                if (settings.ContainsKey(settingName))
                {
                    returnVal = settings[settingName];
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Could not ReadEnvironmentSetting because: " + e.Message);
            }
            return returnVal;
        }
        public static Logger SetLogger(TestContext testContext, string logFilePath)
        {
            return WebDriverBase.log = new Logger(testContext, logFilePath);
        }
        private static Dictionary<string, string> ReadAllSettingsFromFile(string filePath)
        {
            var results = new Dictionary<string, string>();

            using (var reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Setting")
                    {
                        results.Add(reader.GetAttribute("Name"), reader.ReadElementContentAsString());
                    }
                }
            }
            return results;
        }
        private static string GetSettingsFilePath()
        {
            string configFile = String.Concat(AppSettings["TestEnvironment"].ToString(), ".config");
            if (File.Exists(configFile))
            {
                return configFile;
            }
            else
            {
                return Path.Combine(AppSettings["ConfigFolder"], configFile);
            }
        }
        public static string ReadEnvironmentType()
        {
            return EnvironmentType;
        }
        public static IWebDriver LoadBrowserConfig(IWebDriver driver)
        {
            switch (BrowserName.ToLower())
            {
                case "chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--start-maximized");
                    options.AddArguments("--allow-running-insecuontent");
                    driver = new ChromeDriver(ChromeDriverPath, options);
                    break;
                default:
                    InternetExplorerOptions capabilitiesInternetOptions = new InternetExplorerOptions();
                    capabilitiesInternetOptions.EnableNativeEvents = false;
                    driver = new InternetExplorerDriver(IEDriverPath, capabilitiesInternetOptions);
                    break;
            }
            driver.Url = @"http://www.google.com/";
            log.LogMessage("Browser driver is : " + driver.GetType().Name.ToString());
            return driver;
        }
        public void ClickIESecurityCertLink()
        {
            if (GetBrowserType().Equals(Constants.IEDriver))
            {
                if (GetPageTitle().Equals(Constants.IECertErrPageTitle))
                    ClickSecurityCertLink();
            }
        }
    }
}
