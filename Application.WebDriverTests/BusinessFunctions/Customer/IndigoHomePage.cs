using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UITesting;
using System.Windows.Forms;
using static Application.TestFramework.Validate;

namespace Application.WebDriverTests
{
    public class IndigoHomePage : IndigoLoginPage
    {
        public IndigoHomePage(IWebDriver browserDriver) : base(browserDriver)
        {
        }

        private By FlightFrom => By.XPath("//input[@type='text' and @name='or-src']");
        private By SelectDelhi => By.XPath("//div[contains(text(), 'Delhi, India')]");
        private By SelectMuscat => By.XPath("//div[contains(text(), 'Muscat, Oman')]");
        private By FlightTo => By.XPath("//input[@type='text' and @name='or-dest']");
        private By SearchFlightLink => By.XPath("//span[contains(text(),'Search Flight')]");
        private By DoneLink => By.XPath("//a[contains(text(),'Done')]");
        private By OkButton => By.XPath("//button[@class='modal-btn-dark full-width float-right']");

        //Created below method for testing purpose...won't be run in normal run...would have to debug 
        public IndigoHomePage FlyingHigh(string fromPlace, string toPlace)
        {
            Sleep(4);
            EnterText(FlightFrom, fromPlace, true, "From option");
            EnterText(FlightTo, toPlace, true, "To option");
            //Click(FlightFrom, "From option");
            //Keyboard.SendKeys(fromPlace);
            //Click(SelectDelhi, "Delhi option");
            //Click(FlightTo, "To option");
            //Keyboard.SendKeys(toPlace);
            //Click(SelectMuscat, "Muscat option");
            Click(DoneLink, "Done");
            Click(SearchFlightLink, "Search Flight");
            Click(OkButton, "Ok");
            return this;
        }
        public IndigoHomePage VerifyHomePageTitle(string expectedTitle)
        {
            string actualTitle = driver.Title.Trim();
            AssertAreEqual(expectedTitle, actualTitle, "Expected title did not match to actual");

           
            return this;
        }


    }
}
