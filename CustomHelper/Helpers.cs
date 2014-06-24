using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CustomHelper
{
    internal class InputType
    {
        public static string Text = "text";
        public static string Password = "password";
        public static string Letters = "letters";
        public static string Numeric = "numeric";
        public static string Datetime = "datetime";
        public static string Checkbox = "checkbox";
        public static string Button = "button";
        public static string Submit = "submit";
    }

    internal class Tools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <param name="arrayNames">Array must contain: "value", "placeholder", "dynamicsAttr", "class", "checked"</param>
        /// <returns></returns>
        public static string[] ReadValues(object htmlAttributes, string[] arrayNames)
        {
            string[] values = new string[0];
            if (htmlAttributes != null)
            {
                //  Reading aditional htmlAttributes
                IDictionary<string, object> additionalAttributes = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                string strTemp = string.Empty;
                values = new string[arrayNames.Length];
                for (int i = 0; i < arrayNames.Length; i++)
                {
                    strTemp = arrayNames[i];
                    values[i] = "";
                    object tempObj = additionalAttributes[strTemp];
                    if (tempObj != null)
                    {
                        values[i] = tempObj.ToString();
                    }
                }
            }
            return values;
        }
    }
}
