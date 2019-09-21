using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UITesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using Keys = System.Windows.Forms.Keys;

namespace Application.TestFramework
{
    public class WebDriverBase
    {
        //Will have WebDriver basic handling logic
        //This class will have defined constructor

        protected internal IWebDriver driver;
        public static Logger log;
        public static int pageLoadTimeout;

        public WebDriverBase(IWebDriver broweserDriver)
        {
            this.driver = broweserDriver;
            WaitForPageLoad(pageLoadTimeout);
        }
        public WebDriverBase(IWebDriver broweserDriver, int pageLoadTimeoutSeconds)
        {
            this.driver = broweserDriver;
            WaitForPageLoad(pageLoadTimeout = pageLoadTimeoutSeconds);
        }

        public enum FrameIdentifier
        {
            Name,
            Id
        }
        public enum DialogButton
        {
            ok,
            cancel
        }

        public IWebDriver NavigateTo(string url)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
            return driver;
        }
        public void EnterText(By elementLocator, string text, string logMsg)
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(elementLocator);
                element.Clear();
                element.SendKeys(text);
                log.LogMessage(string.Format("Entered '{0}' in {1} textbox | {2}", text, logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to enter '{0}' in {1} textbox | {2}", text, logMsg, elementLocator));
                throw;
            }
        }
        public void EnterText(IWebElement element, string text, string logMsg)
        {
            try
            {
                element.Clear();
                element.SendKeys(text);
                log.LogMessage(string.Format("Entered '{0}' in {1} textbox | {2}", text, logMsg, element));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to enter '{0}' in {1} textbox | {2}", text, logMsg, element));
                throw;
            }
        }
        public void EnterText(By elementLocator, string text, bool clearDefaultText, string logMsg)
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(elementLocator);
                if (clearDefaultText)
                {
                    element.Clear();
                }
                element.SendKeys(text);
                log.LogMessage(string.Format("Entered '{0}' in {1} textbox | {2}", text, logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to enter '{0}' in {1} textbox | {2}", text, logMsg, elementLocator));
                throw;
            }
        }
        public void EnterTextJS(By elementLocator, string text, bool clearDefaultText, string logMsg)
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(elementLocator);
                if (clearDefaultText)
                {
                    element.Clear();
                }
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].value='" + text + "'", element);
                log.LogMessage(string.Format("Entered '{0}' in {1} textbox | {2}", text, logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to enter '{0}' in {1} textbox | {2}", text, logMsg, elementLocator));
                throw;
            }
        }
        public void ClickSecurityCertLink()
        {
            try
            {
                driver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
                log.LogMessage("Clicked on security certificate link");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to click on security certificate link");
                throw;
            }
        }
        public void EnterTextRadEditor(FrameIdentifier value, string iFrameValue, string text, string logMessage)
        {
            IWebElement iFrame = null;

            if (value == FrameIdentifier.Name)
            {
                iFrame = driver.FindElement(By.Name(iFrameValue));
            }
            if (value == FrameIdentifier.Id)
            {
                iFrame = driver.FindElement(By.Id(iFrameValue));
            }

            ScrollWindowJS(200);
            driver.SwitchTo().Frame(iFrame);
            Click(By.XPath(".//html/body"), logMessage);
            EnterText(By.XPath(".//html/body"), text, logMessage);
            driver.SwitchTo().DefaultContent();
        }
        public void EnterTextRadEditorJS(string iFrameValue, string text, string logMsg)
        {
            try
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript(string.Format("document.getElementById('{0}').contentWindow.document.body.innerHTML='{1}'", iFrameValue, text));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to enter '{0}' in {1} textbox | {2}", text, logMsg, iFrameValue));
                throw;
            }
        }
        public void Click(By elementLocator, string logMsg)
        {
            try
            {
                WaitForElementToBeVisible(elementLocator, 10);
                SetFocus(elementLocator, "");
                driver.FindElement(elementLocator).Click();
                log.LogMessage(string.Format("Clicked {0} | {1}", logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to click {0} | {1} ", logMsg, elementLocator));
                throw;
            }
        }
        public void Click(IWebElement element, string logMsg)
        {
            try
            {
                element.Click();
                log.LogMessage(string.Format("Clicked {0} | {1}", logMsg, element));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to click {0} | {1} ", logMsg, element));
                throw;
            }
        }
        public void ClickJS(By elementLocator, string logMsg)
        {
            try
            {
                WaitForElementToBeVisible(elementLocator, 10);
                IWebElement element = driver.FindElement(elementLocator);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].click();", element);
                log.LogMessage(string.Format("Clicked {0} | {1}", logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to click {0} | {1}", logMsg, elementLocator));
                throw;
            }
        }
        public void ClickJS(IWebElement element, string logMsg)
        {
            try
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].click();", element);
                log.LogMessage(string.Format("Clicked {0} | {1}", logMsg, element));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to click {0} | {1}", logMsg, element));
                throw;
            }
        }
        public void SetFocus(By elementLocator, string logMsg)
        {
            try
            {
                IWebElement element = driver.FindElement(elementLocator);
                Actions actions = new Actions(driver);
                actions.MoveToElement(element).Perform();
                log.LogMessage(string.Format("Set focus on {0} | {1}", logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to set focus on {0} | {1}", logMsg, elementLocator));
            }
        }
        public void SetFocus(IWebElement element, string logMsg)
        {
            try
            {
                Actions actions = new Actions(driver);
                actions.MoveToElement(element).Perform();
                log.LogMessage(string.Format("Set focus on {0} | {1}", logMsg, element));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to set focus on {0} | {1}", logMsg, element));
            }
        }
        public void CheckCheckbox(By elementLocator, bool isChecked, string logMsg)
        {
            try
            {
                IWebElement element = driver.FindElement(elementLocator);
                if (element.Selected)
                {
                    if (!isChecked)
                    {
                        element.Click();
                        log.LogMessage(string.Format("Checked checkbox {0} | {1}", logMsg, elementLocator));
                    }
                }
                else
                {
                    if (isChecked)
                    {
                        element.Click();
                        log.LogMessage(string.Format("Checked checkbox {0} | {1}", logMsg, elementLocator));
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to check checkbox {0} | {1}", logMsg, elementLocator));
                throw;
            }
        }
        public void CheckCheckbox(IWebElement element, bool isChecked, string logMsg)
        {
            try
            {
                if (element.Selected)
                {
                    if (!isChecked)
                    {
                        element.Click();
                        log.LogMessage(string.Format("Checked checkbox {0} | {1}", logMsg, element));
                    }
                }
                else
                {
                    if (isChecked)
                    {
                        element.Click();
                        log.LogMessage(string.Format("Checked checkbox {0} | {1}", logMsg, element));
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to check checkbox {0} | {1}", logMsg, element));
                throw;
            }
        }
        public void CheckCheckboxByLabel(By labelLocator, bool isChecked, string logMsg)
        {
            string label = GetExactElementInnerText(labelLocator);
            By elementLocator = By.XPath("//label[text()=\"" + label + "\"]/preceding-sibling::input[@type='checkbox'][1]");

            try
            {
                ScrollToExposeControl(elementLocator);
                IWebElement element = driver.FindElement(elementLocator);
                if (element.Selected)
                {
                    if (!isChecked)
                    {
                        SetFocus(elementLocator, "");
                        element.Click();
                        log.LogMessage(string.Format("Checked checkbox {0} | {1}", logMsg, elementLocator));
                    }
                }
                else
                {
                    if (isChecked)
                    {
                        SetFocus(elementLocator, "");
                        element.Click();
                        log.LogMessage(string.Format("Checked checkbox {0} | {1}", logMsg, elementLocator));
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to check checkbox {0} | {1}", logMsg, elementLocator));
                throw;
            }
        }
        public void CheckCheckboxJS(By elementLocator, bool isChecked, string logMsg)
        {
            string check = isChecked.ToString().ToLower();

            try
            {
                IWebElement element = driver.FindElement(elementLocator);
                if (element.Enabled)
                {
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                    jse.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                    jse.ExecuteScript("arguments[0].checked = " + check, element);
                    log.LogMessage(string.Format("Checked checkbox {0} to {1} | {2}", logMsg, check, elementLocator));
                }
                else
                {
                    throw new InvalidElementStateException(string.Format("Failed to check disabled checkbox {0} | {1}", logMsg, elementLocator));
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to check checkbox {0} | {1}", logMsg, elementLocator));
                throw;
            }
        }
        public void CheckCheckboxJS(IWebElement element, bool isChecked, string logMsg)
        {
            string check = isChecked.ToString().ToLower();

            try
            {
                if (element.Enabled)
                {
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                    jse.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                    jse.ExecuteScript("arguments[0].checked = " + check, element);
                    log.LogMessage(string.Format("Checked checkbox {0} to {1} | {2}", logMsg, check, element));
                }
                else
                {
                    throw new InvalidElementStateException(string.Format("Failed to check disabled checkbox {0} | {1}", logMsg, element));
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to check checkbox {0} | {1}", logMsg, element));
                throw;
            }
        }
        public void SelectComboBoxByText(By elementLocator, string text, string logMsg)
        {
            try
            {
                IWebElement element = driver.FindElement(elementLocator);
                var combo = new SelectElement(element);
                combo.SelectByText(text);
                log.LogMessage(string.Format("Selected {0} combobox text '{1}' | {2}", logMsg, text, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to select {0} combobox text '{1}' | {2}", logMsg, text, elementLocator));
                throw;
            }
        }
        public void SelectComboBoxByText(IWebElement element, string text, string logMsg)
        {
            try
            {
                var combo = new SelectElement(element);
                combo.SelectByText(text);
                log.LogMessage(string.Format("Selected {0} combobox text '{1}' | {2}", logMsg, text, element));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to select {0} combobox text '{1}' | {2}", logMsg, text, element));
                throw;
            }
        }
        public void SelectComboBoxByValue(By elementLocator, string value, string logMsg)
        {
            try
            {
                IWebElement element = driver.FindElement(elementLocator);
                var combo = new SelectElement(element);
                combo.SelectByValue(value);
                log.LogMessage(string.Format("Selected {0} combobox text '{1}' | {2}", logMsg, value, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to select {0} combobox text '{1}' | {2}", logMsg, value, elementLocator));
                throw;
            }
        }
        public void SelectComboBoxByIndex(By elementLocator, int index, string logMsg)
        {
            try
            {
                IWebElement element = driver.FindElement(elementLocator);
                var combo = new SelectElement(element);
                combo.SelectByIndex(index);
                log.LogMessage(string.Format("Selected {0} combobox index {1} | {2}", logMsg, index, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to select {0} combobox index {1} | {2}", logMsg, index, elementLocator));
                throw;
            }
        }
        public int ComboBoxSearchTextToIndexValue(By elementLocator, string value, string logMsg)
        {
            IWebElement element = driver.FindElement(elementLocator);
            var combo = new SelectElement(element);
            IList<IWebElement> options = combo.Options;

            for (var i = 0; i < options.Count; i++)
            {
                string itemText = options[i].GetAttribute("innerText");

                if (itemText.Contains(value))
                {
                    log.LogMessage(string.Format("Search text {0} found inside combobox {1} at index {2} | {3}", value, logMsg, i, elementLocator));
                    return i;
                }
            }
            throw new NoSuchElementException(string.Format("Cannot find '{0}' in an option in comboBox - '{1}'", value, logMsg));
        }
        public void SelectRadioButtonByLabel(string radioLabel, string logMsg)
        {
            By elementLocator = By.XPath("//label[text()=\"" + radioLabel + "\"]/preceding-sibling::input[@type='radio'][1]");
            try
            {
                ScrollToExposeControl(elementLocator);
                driver.FindElement(elementLocator).Click();
                log.LogMessage(string.Format("Selected radio button {0} | {1}", logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to select radio button {0} | {1}", logMsg, elementLocator));
                throw;
            }
        }
        public void SelectRadioButton(By elementLocator, string logMsg)
        {
            try
            {
                driver.FindElement(elementLocator).Click();
                log.LogMessage(string.Format("Selected radio button {0} | {1}", logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to click radio button {0} | {1}", logMsg, elementLocator));
                throw;
            }
        }
        public void SelectRadioButton(IWebElement element, string logMsg)
        {
            try
            {
                element.Click();
                log.LogMessage(string.Format("Selected radio button {0} | {1}", logMsg, element));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to click radio button {0} | {1}", logMsg, element));
                throw;
            }
        }
        public void PerformDialogAction(DialogButton buttonName)
        {
            try
            {
                Sleep(1);
                IAlert alert = driver.SwitchTo().Alert();
                if (buttonName.Equals(DialogButton.ok))
                {
                    alert.Accept();
                }
                else if (buttonName.Equals(DialogButton.cancel))
                {
                    alert.Dismiss();
                }
                log.LogMessage("Clicked " + buttonName + " on windows pop up");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to click " + buttonName + " on windows pop up");
                throw;
            }
            driver.SwitchTo().DefaultContent();
        }
        public string GetAttribute(By elementLocator, string attributeName, string logMsg)
        {
            try
            {
                return driver.FindElement(elementLocator).GetAttribute(attributeName);
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to get attribute {0} from element {1} | {2}", attributeName, logMsg, elementLocator));
                throw;
            }
        }
        public string GetCSSPropertyValue(By elementLocator, string propertyName, string logMsg)
        {
            try
            {
                return driver.FindElement(elementLocator).GetCssValue(propertyName);
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to get CSS value {0} from element {1} | {2}", propertyName, logMsg, elementLocator));
                throw;
            }
        }
        public bool WaitForElementToBeVisible(By elementLocator, int timeoutInSeconds)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(elementLocator));
                return true;
            }
            catch (Exception ex)
            {
                log.LogException(ex, "The element " + elementLocator + " is not visible.");
                throw;
            }
        }
        public bool WaitForElementToBeVisible(IWebElement element, int timeoutInSeconds)
        {
            int counter = 0;

            try
            {
                while (!IsElementDisplayed(element) && counter < timeoutInSeconds)
                {
                    Sleep(1);
                    counter++;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.LogException(ex, "The element " + element + " is not visible after waiting " + timeoutInSeconds + "seconds.");
                throw;
            }
        }
        public bool WaitForElementToBeInvisible(By elementLocator, int timeoutInSeconds)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(elementLocator));
                return true;
            }
            catch (Exception ex)
            {
                log.LogException(ex, "The element " + elementLocator + " is still visible.");
                throw;
            }
        }
        public bool WaitForPageLoad(int timeoutInSeconds)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript(("return document.readyState")).ToString().Equals("complete"));
                return true;
            }
            catch (Exception ex)
            {
                log.LogException(ex, "The page " + driver.Url + " not loaded.");
                return false;
            }
        }
        public string GetBrowserType()
        {
            return driver.GetType().Name.ToString();
        }
        public string GetPageUrl()
        {
            return driver.Url.ToString();
        }
        public void PageRefresh()
        {
            driver.Navigate().Refresh();
        }
        public bool IsElementDisplayed(By elementLocator)
        {
            bool isDisplayed;
            try
            {
                isDisplayed = driver.FindElement(elementLocator).Displayed;
            }
            catch
            {
                isDisplayed = false;
            }
            return isDisplayed;
        }
        public void WaitForElementToBeDisplayedWithPageRefresh(By elementLocator, int secondsTimeout)
        {
            do
            {
                if (IsElementDisplayed(elementLocator))
                {
                    break;
                }
                else
                {
                    Sleep(1);
                    PageRefresh();
                    secondsTimeout--;
                }
            }
            while (!IsElementDisplayed(elementLocator) && secondsTimeout != 0);
        }
        public void WaitForElementNotToBeDisplayedWithPageRefresh(By elementLocator, int secondsTimeout)
        {
            do
            {
                if (IsElementDisplayed(elementLocator))
                {
                    Sleep(1);
                    PageRefresh();
                    secondsTimeout--;
                }
                else
                {
                    break;
                }
            }
            while (IsElementDisplayed(elementLocator) && secondsTimeout != 0);
        }
        public bool IsElementDisplayed(IWebElement element)
        {
            bool isDisplayed;
            try
            {
                isDisplayed = element.Displayed;
            }
            catch
            {
                isDisplayed = false;
            }
            return isDisplayed;
        }
        public bool IsElementEnabled(By elementLocator)
        {
            bool isEnabled;
            try
            {
                isEnabled = driver.FindElement(elementLocator).Enabled;
            }
            catch
            {
                isEnabled = false;
            }
            return isEnabled;
        }
        public bool IsElementEnabled(IWebElement element)
        {
            bool isEnabled;
            try
            {
                isEnabled = element.Enabled;
            }
            catch
            {
                isEnabled = false;
            }
            return isEnabled;
        }
        public void ScrollWindowJS(int scrollByLength)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("window.scrollBy(0," + scrollByLength + ")", "");
        }
        public void ScrollToExposeControl(By elementLocator)
        {
            IWebElement element = driver.FindElement(elementLocator);
            if (driver.GetType().Name.ToString() == "InternetExplorerDriver")
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            }
            else
            {
                Actions actions = new Actions(driver);
                actions.MoveToElement(element);
                actions.Perform();
                ScrollWindowJS(75);
            }
        }
        public void ScrollToExposeControl(IWebElement element)
        {
            if (driver.GetType().Name.ToString() == "InternetExplorerDriver")
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            }
            else
            {
                Actions actions = new Actions(driver);
                actions.MoveToElement(element);
                actions.Perform();
                ScrollWindowJS(75);
            }
        }
        public bool IsTextPresentOnPage(string text)
        {
            bool isPresent;
            try
            {
                isPresent = driver.FindElement(By.XPath("//*[text()='" + text + "']")).Displayed;
            }
            catch
            {
                return false;
            }

            return isPresent;
        }
        public void Sleep(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }
        public string GetElementInnerText(By elementLocator, string logMsg)
        {
            string innerText = null;
            try
            {
                innerText = driver.FindElement(elementLocator).GetAttribute("innerText").Trim();
                log.LogMessage(string.Format("Retrieved innerText {0} for an element {1} | {2}", innerText, logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to retrieve innerText for an element {0} | {1}", logMsg, elementLocator));
                throw;
            }
            return innerText;
        }
        public string GetElementInnerText(IWebElement element, string logMsg)
        {
            string innerText = null;
            try
            {
                innerText = element.GetAttribute("innerText").Trim();
                log.LogMessage(string.Format("Retrieved innerText {0} for an element {1} | {2}", innerText, logMsg, element));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to retrieve innerText for an element {0} | {1}", logMsg, element));
                throw;
            }
            return innerText;
        }
        public string GetDropDownInnerText(By elementLocator, string logMsg)
        {
            string innerText = null;
            try
            {
                IWebElement element = driver.FindElement(elementLocator);
                var combo = new SelectElement(element);
                innerText = combo.SelectedOption.GetAttribute("innerText");
                log.LogMessage(string.Format("Retrieved innerText {0} for an element {1} | {2}", innerText, logMsg, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to retrieve innerText for an element {0} | {1}", logMsg, elementLocator));
                throw;
            }
            return innerText;
        }
        public string GetPageDomain()
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            return (String)jse.ExecuteScript("return document.domain");
        }
        public string GetPageTitle()
        {
            return driver.Title.ToString();
        }
        public void Highlight(By element)
        {
            var highlighted = driver.FindElement(element);

            var jsDriver = (IJavaScriptExecutor)driver;
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { highlighted });
        }
        public void Highlight(IWebElement element)
        {
            var jsDriver = (IJavaScriptExecutor)driver;
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
        }
        public bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                log.LogMessage("Alert is present");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsRadioButtonSelectedByLabel(string radioLabel, string logMsg)
        {
            bool isSelected;
            By elementLocator = By.XPath("//label[text()=\"" + radioLabel + "\"]/preceding-sibling::input[@type='radio'][1][@checked='checked']");
            try
            {
                isSelected = driver.FindElement(elementLocator).Selected;
                log.LogMessage(string.Format("Checked radio button {0} | {1}", logMsg, elementLocator));
                return isSelected;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void ClearTextbox(By elementLocator)
        {
            ClearTextbox(driver.FindElement(elementLocator));
        }
        public void ClearTextbox(IWebElement element)
        {
            element.Clear();
        }
        public string GetExactElementInnerText(By elementLocator)
        {
            string innerText = null;
            try
            {
                innerText = driver.FindElement(elementLocator).GetAttribute("innerText");
                log.LogMessage(string.Format("Retrieved innerText {0} for an element | {1}", innerText, elementLocator));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to retrieve innerText for an element | {0}", elementLocator));
                throw;
            }
            return innerText;
        }
        public string GetExactElementInnerText(IWebElement element, string logMsg)
        {
            string innerText = null;
            try
            {
                innerText = element.GetAttribute("innerText");
                log.LogMessage(string.Format("Retrieved innerText {0} for an element {1} | {2}", innerText, logMsg, element));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to retrieve innerText for an element {0} | {1}", logMsg, element));
                throw;
            }
            return innerText;
        }
        public void DragVideoSliderHorizontally(By videoSliderx, By videoBarx, int dragLimit, string logMsg)
        {
            IWebElement videoSlider = driver.FindElement(videoSliderx);
            IWebElement videoBar = driver.FindElement(videoBarx);
            try
            {
                Actions action = new Actions(driver);
                var sliderWidth = videoBar.Size.Width;
                action.ClickAndHold(videoSlider).MoveByOffset((sliderWidth / dragLimit), 0).Release().Build().Perform();
                log.LogMessage(string.Format("Dragged video slider for an element {0} | {1}", logMsg, videoSlider));
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to drag video slider for an element {0} | {1}", logMsg, videoSlider));
            }
        }

        public void UploadFileFromWindows(By element, string path, string logMsg)
        {
            try
            {
                Click(element, "Click");
                Keyboard.SendKeys(path);
                Keyboard.SendKeys("o", System.Windows.Input.ModifierKeys.Alt);
            }

            catch (Exception ex)
            {
                log.LogError(ex, string.Format("Failed to upload file {0} | {1}", logMsg, element));
            }
        }
    }
}
