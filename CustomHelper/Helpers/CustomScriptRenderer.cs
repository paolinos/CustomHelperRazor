using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages.Scope;

namespace CustomHelper
{
    public static class CustomScriptRenderer
    {
        #region Private Properties
        #endregion Private Properties



        #region Private Methods

        /// <summary>
        /// Get or Set ScriptScope
        /// </summary>
        private static string ScriptScope
        {
            get
            {
                object strValue = string.Empty;
                if (ScopeStorage.CurrentScope.TryGetValue("scriptString", out strValue))
                {
                    return strValue.ToString();
                }
                return string.Empty;
            }
            set
            {
                ScopeStorage.CurrentScope["scriptString"] = value;
            }

        }

        #endregion Private Methods



        #region Internal Methods

        /// <summary>
        /// Add Script 
        /// </summary>
        /// <param name="scriptString">Add {0} that it's the name</param>
        internal static void AddScript(string scriptString)
        {
            ScriptScope += scriptString;
        }

        #endregion Internal Methods



        #region Public Methods

        /// <summary>
        /// Herlper to render Javascript neededs to CustomHerlper
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <returns>Script needed's</returns>
        public static MvcHtmlString CustomScripts<TModel>(this HtmlHelper<TModel> helper)
        {
            return new MvcHtmlString("<script type=\"text/javascript\">$(function() {"+ScriptScope.ToString()+"});</script>");
        }

        #endregion Public Methods
    }
}
