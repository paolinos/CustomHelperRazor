using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomHelper
{
    public class JsonCustomValidation
    {
        public bool success { get; internal set; }
        public JsonCustomError[] errors { get { return listErrors.ToArray(); } }


        private List<JsonCustomError> listErrors;

        public JsonCustomValidation()
        {
            success = true;
            listErrors = new List<JsonCustomError>();
        }

        /// <summary>
        // TODO: Translate this
        /// Cuando se agrega un mensaje de error, automaticamente se coloca el success en false.
        /// </summary>
        /// <param name="key">Key or Field</param>
        /// <param name="message">Message</param>
        public void AddError(string key, string message)
        {
            if (success)
                success = false;

            JsonCustomError jsonError = new JsonCustomError();
            jsonError.field = key;
            jsonError.message = message;

            //listErrors.Add(  "{'field':'" + key + "','message':'"+message+"'}");ç
            listErrors.Add(jsonError);
        }
    }

    public struct JsonCustomError
    {
        public string field { get; set; }
        public string message { get; set; }
    }
}
