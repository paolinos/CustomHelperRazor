using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace CustomHelper
{
    public static class CustomTextBoxHelper
    {
        #region Public Methods

        /// <summary>
        /// Custom Text, es un campo de texto
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes">
        /// Permitidos: @value, @placeholder, @class y @dynamicsAttr
        /// @dynamicsAttr:  es para agregar atributos dinamicos al helper
        /// </param>
        /// <returns>Helper</returns>
        public static MvcHtmlString CustomText<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string express = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            string id = express.Replace(".", "_");
            ModelStateValidation modelStateValidation = CustomValidation.GetModelStateValidation(helper.ViewData.ModelState, express);

            string value = string.Empty;
            string placeholder = string.Empty;
            string cssClass = string.Empty;
            string dynamicsAttr = string.Empty;
            string newId = string.Empty;
            string newName = string.Empty;

            if (data.Model != null)
            {
                string strModel = data.Model.ToString();
                if (strModel != string.Empty)
                {
                    value = strModel;
                }
            }

            string[] resultAttributes = Tools.ReadValues(htmlAttributes, new string[] { "value", "placeholder", "dynamicsAttr", "class", "id", "name" });
            if (resultAttributes.Length > 0)
            {
                value = resultAttributes[0];
                placeholder = resultAttributes[1];
                dynamicsAttr = resultAttributes[2];
                cssClass = resultAttributes[3];
                newId = resultAttributes[4];
                newName = resultAttributes[5];
            }

            // Obtenemos las validadciones del Modelo
            ValidationObject validationObject = CustomValidation.GetValidationFromDataAnnotations(helper, data, express, modelStateValidation.error);

            dynamicsAttr += validationObject.validations;

            string validationMessage = validationObject.message != null ? validationObject.message : "";

            if (modelStateValidation.hasError)
            {
                cssClass = cssClass == string.Empty ? "input-validation-error" : cssClass + " input-validation-error";
            }

            if (newId == string.Empty)
            {
                newId = id;
            }

            if (newName == string.Empty)
            {
                newName = express;
            }

            return new MvcHtmlString(CustomHtml.BuildInput(InputType.Text, newName, newId, value, false, placeholder, dynamicsAttr, cssClass, "", validationMessage));
        }
        #endregion Public Methods
    }
}
