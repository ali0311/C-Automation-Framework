using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.IO;

namespace Application.TestFramework
{
    public class Logger
    {
        //We will defined Log context like log Error message, Take screenshot, Log Exception

        private int step = 1;
        private TestContext testContext;
        private string filePath;
        private StreamWriter outputFile;
        public readonly string element = "element";
        public Logger(TestContext testContext, string filePath)
        {
            this.testContext = testContext;
            outputFile = new StreamWriter(this.filePath = filePath, true);
        }

        public void LogMessage(string message)
        {
            outputFile.WriteLine("{0}{1}{2}{3}", step, ".", " ", message);
            outputFile.Flush();
            step++;
        }
        public void LogError(Exception ex, string customMsg)
        {
            LogMessage("\nERROR: " + customMsg);
            LogMessage("\nException: " + ex.Message);
            LogMessage("\nStackTrace: " + ex.StackTrace);
        }
        public void LogException(Exception ex, string customMsg)
        {
            LogMessage("\nERROR: " + customMsg);
            LogMessage("\nException: " + ex.Message);
            LogMessage("\nStackTrace: " + ex.StackTrace);
        }

        public void AttachScreenShotIfTestCaseFailed(IWebDriver driver, string fileName)
        {
            if (driver != null)
            {
                if (testContext.CurrentTestOutcome.ToString().Equals("Failed"))
                {
                    Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                    fileName = fileName + ".Jpeg";
                    ss.SaveAsFile(fileName);
                    testContext.AddResultFile(fileName);
                }
            }
            else
            {
                LogMessage("Oops!..Looks like browser is closed before taking a screenshot!");
            }
            }
        public void AttachLogFile(string logFilePath)
        {
            {
                testContext.AddResultFile(logFilePath);
            }
        }
       
    }
}
