using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.WebDriverTests
{
    //Add Application platform name
    public enum WebApplication
    {
        [System.ComponentModel.Description("INDIGO")]
        Indigo,

    }
    public enum EnvironmentURL
    {
        [System.ComponentModel.Description("Login URL")]
        IndigoLoginURL,

    }

    public static class Utility
    {
        public static string EnumToDescription(this Enum theEnum)
        {
            FieldInfo field = theEnum.GetType().GetField(theEnum.ToString());
            System.ComponentModel.DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
            return attribute == null ? theEnum.ToString() : attribute.Description;
        }
        public static string GenerateTimeStamp()
        {
            DateTime saveNow = DateTime.Now;
            string timeStamp = saveNow.ToString("MMddyyyy HHmmss");
            return timeStamp;
        }
        public static string EncodePasswordToBase64(string plainPassword)
        {
            try
            {
                byte[] encData_byte = new byte[plainPassword.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(plainPassword);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode " + ex.Message);
            }
        }
        public static string DecodePasswordFrom64(string encryptedPassword)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encryptedPassword);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception("Error in decoding " + ex.Message);
            }
        }
        public static string TakeScreenshot(IWebDriver driver, string fileName)
        {
            try
            {
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                fileName = fileName + ".Jpeg";
                ss.SaveAsFile(fileName);
                return fileName;
            }
            catch(Exception ex)
            {
                throw new Exception("Error in taking screenshot" + ex.Message);
            }
        }
    }
}
