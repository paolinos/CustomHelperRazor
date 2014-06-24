using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CustomHelper
{
    internal class ValidationObject
    {
        public string validations { get; internal set; }
        public string message { get; internal set; }
    }

    public static class ValidationHelper
    {
        public static MvcHtmlString ShowListErrors(this HtmlHelper htmlHelper)
        {
            return new MvcHtmlString(CustomValidation.ValidationErrors);
        }

        public static JsonCustomValidation GetErrors(ViewDataDictionary ViewData)
        {
            JsonCustomValidation jsonValidation = new CustomHelper.JsonCustomValidation();

            int i = 0;
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    jsonValidation.AddError(ViewData.ModelState.Keys.ElementAt(i), error.ErrorMessage);
                }
                i++;
            }
            return jsonValidation;
        }
    }

    internal struct ModelStateValidation
    {
        public bool hasError { get; set; }
        public string error { get; set; }
    }

    internal static class CustomValidation
    {
        //         If it's error by default we load class="field-validation-error"
        //         else load class="field-validation-valid"
        /// <summary>
        /// 
        /// </summary>
        private const string validationMsg = "<span class=\"field-validation-valid\" data-valmsg-replace=\"true\" data-valmsg-for=\"{0}\"{1}>{2}</span>";

        public static string ValidationErrors { get; internal set; }

        /// <summary>
        /// Check if field is valid
        /// </summary>
        /// <param name="modelState">ModelState del helper. Ej: helper.ViewData.ModelState</param>
        /// <param name="propertyName">PropertyName. Ej: data.PropertyName</param>
        /// <returns>true = Valid / false = Not Valid</returns>
        public static ModelStateValidation GetModelStateValidation(ModelStateDictionary modelState, string propertyName)
        {
            ModelStateValidation modelStateValidation = new ModelStateValidation()
            {
                hasError = false,
                error = ""
            };

            if (!modelState.IsValid)
            {
                int pos = modelState.Keys.ToList().IndexOf(propertyName);
                if (pos > -1)
                {
                    ModelErrorCollection modelErrorCollection = modelState.Values.ElementAt(pos).Errors;

                    if (modelErrorCollection.Count() > 0)
                    {
                        modelStateValidation.hasError = true;

                        modelStateValidation.error = modelErrorCollection[0].ErrorMessage;
                    }
                }
            }
            return modelStateValidation;
        }


        public static ValidationObject GetValidationFromDataAnnotations(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, string expression, string errorMsg)
        {
            StringBuilder validations = new StringBuilder();

            //  Read metadata to know DataAnnotations
            var metadata = modelMetadata;
            var prop = metadata.ContainerType.GetProperty(metadata.PropertyName);
            var attrs = prop.GetCustomAttributes(false);
            string strTemp = string.Empty;

            //  TODO: translate this
            //  Obtenemos los valores de DataAnnotation y tambien obtenemos los valores por default, por si no agregamos algun mensaje.
            //  Esto se obtubo de la "System.Web.Mvc.Html.dll", de la clase "ValidationExtensions" del metodo privado "ApplyFieldValidationMetadata"
            IEnumerable<ModelClientValidationRule> dataAnnotations = Enumerable.SelectMany<ModelValidator, ModelClientValidationRule>(ModelValidatorProviders.Providers.GetValidators(modelMetadata, (ControllerContext)htmlHelper.ViewContext), (Func<ModelValidator, IEnumerable<ModelClientValidationRule>>)(v => v.GetClientValidationRules()));

            //  TODO: translate this
            //  Si tiene DataAnnotation, entonces tiene que validarse
            if (dataAnnotations.Count() > 0)
                validations.Append("data-val=\"true\"");

            foreach (ModelClientValidationRule clientValidationRule in dataAnnotations)
            {
                Type tmpType = clientValidationRule.GetType();
                if (tmpType != null)
                {
                    string tmp = string.Empty;


                    /*
                     * (EN)Add different tipes of validation
                     * (ESP)Se agregan los distintos tipos de validaciones
                     * More info:
                     * http://msdn.microsoft.com/en-us/library/system.web.mvc.modelclientvalidationrule%28v=vs.111%29.aspx
                    */
                    switch (tmpType.Name)
                    {
                        case "ModelClientValidationRequiredRule":
                            tmp = " data-val-required=\"" + clientValidationRule.ErrorMessage + "\"";
                            break;

                        case "ModelClientValidationStringLengthRule":
                            tmp = " data-val-length=\"" + clientValidationRule.ErrorMessage + "\"";

                            if (clientValidationRule.ValidationParameters.ContainsKey("min"))
                                tmp += " data-val-length-min=\"" + clientValidationRule.ValidationParameters["min"] + "\"";

                            if (clientValidationRule.ValidationParameters.ContainsKey("max"))
                                tmp += " data-val-length-max=\"" + clientValidationRule.ValidationParameters["max"] + "\"";
                            break;

                        case "ModelClientValidationRangeRule":
                            tmp = " data-val-range=\"" + clientValidationRule.ErrorMessage + "\"";
                            tmp += " data-val-range-min=\"" + clientValidationRule.ValidationParameters["min"] + "\"";
                            tmp += " data-val-range-max=\"" + clientValidationRule.ValidationParameters["max"] + "\"";
                            break;

                        case "ModelClientValidationRule":
                            tmp = " data-val-rule=\"" + clientValidationRule.ErrorMessage + "\"";
                            if (clientValidationRule.ValidationParameters.Count() > 0)
                            {
                                tmp += " data-val-rule-reg=\"" + clientValidationRule.ValidationParameters["reg"] + "\"";
                                tmp += " data-val-rule-mask=\"" + clientValidationRule.ValidationParameters["mask"] + "\"";
                            }
                            break;
                    }
                    
                    if (clientValidationRule.ValidationParameters.Count > 0)
                    {
                        object objResult = string.Empty;
                        if (clientValidationRule.ValidationParameters.TryGetValue("script", out objResult))
                        {
                            CustomScriptRenderer.AddScript(objResult.ToString().Replace("{0}", expression));
                        }
                    }

                    if (tmp != string.Empty)
                    {
                        validations.Append(tmp);
                    }
                }

            }
            string msg = string.Empty;

            if (errorMsg != string.Empty)
            {
                msg = string.Format(validationMsg, expression, " style=\"display:inline;\"", errorMsg);
            }
            else
            {
                msg = string.Format(validationMsg, expression, string.Empty, string.Empty);
            }

            ValidationErrors += msg;

            return new ValidationObject() { validations = validations.ToString(), message = msg };
        }
    }
}
