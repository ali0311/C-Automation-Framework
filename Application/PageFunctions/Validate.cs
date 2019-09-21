using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TestFramework
{
    public class Validate
    {
        //This class will have custom assertions defined

        private static Logger log = WebDriverBase.log;

        public static void AssertAreEqual(string expected, string actual, string errorMessage)
        {
            try
            {
                Assert.AreEqual(expected, actual, errorMessage);
            }
            catch (AssertFailedException ex)
            {
                log.LogError(ex, errorMessage);
                throw;
            }
        }
        public static void AssertAreEqual(int expected, int actual, string message)
        {
            try
            {

                Assert.AreEqual(expected, actual, message);
            }
            catch (AssertFailedException ex)
            {
                log.LogError(ex, message);
                throw;
            }
        }
        public static void AssertAreEqual(bool expected, bool actual, string errorMessage)
        {
            try
            {
                Assert.AreEqual(expected, actual, errorMessage);
            }
            catch (AssertFailedException ex)
            {
                log.LogError(ex, errorMessage);
                throw;
            }
        }
        public static void AssertContains(string expected, string actual, string errorMessage)
        {
            try
            {
                if (!actual.Contains(expected))
                {
                    throw new AssertFailedException(string.Format("Actual string does not contain Expected string. Expected: {0}  Actual: {1}", expected, actual));
                }
            }
            catch (AssertFailedException ex)
            {
                log.LogError(ex, errorMessage);
                throw;
            }

        }
        public static void AssertIsTrue(bool actual, string errorMessage)
        {
            try
            {
                Assert.IsTrue(actual, errorMessage);
            }
            catch (AssertFailedException ex)
            {
                log.LogError(ex, errorMessage);
                throw;
            }
        }
        public static void AssertIsInconclusive(string errorMessage)
        {
            try
            {
                Assert.Inconclusive(errorMessage);
            }
            catch (AssertInconclusiveException ex)
            {
                log.LogError(ex, errorMessage);
                throw;
            }
        }
        public static void AssertIsFalse(bool actual, string errorMessage)
        {
            try
            {
                Assert.IsFalse(actual, errorMessage);
            }
            catch (AssertFailedException ex)
            {
                log.LogError(ex, errorMessage);
                throw;
            }
        }
    }
}
