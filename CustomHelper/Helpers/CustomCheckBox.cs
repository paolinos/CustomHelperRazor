using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace CustomHelper
{
    public static class CustomCheckBoxHelper
    {
        #region Private Properties
        #endregion Private Properties


        #region Public Methods

        /// <summary>
        /// Custom CheckBox
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes">
        /// Permitidos: @value, @checked, @class y @dynamicsAttr
        /// @checked:   true or false
        /// @dynamicsAttr:  es para agregar atributos dinamicos al helper
        /// </param>
        /// <returns></returns>
        public static MvcHtmlString CustomCheckBox<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string express = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            string id = express.Replace(".", "_");

            ModelStateValidation modelStateValidation = CustomValidation.GetModelStateValidation(helper.ViewData.ModelState, express);

            string value = string.Empty;
            bool isChecked = false;
            string cssClass = string.Empty;
            string dynamicsAttr = string.Empty;

            string[] resultAttributes = Tools.ReadValues(htmlAttributes, new string[] { "value", "checked", "dynamicsAttr", "class" });
            if (resultAttributes.Length > 0)
            {
                value = resultAttributes[0];
                isChecked = bool.Parse(resultAttributes[1]);
                cssClass = resultAttributes[2];
                dynamicsAttr = resultAttributes[3];
            }

            // Obtenemos las validadciones del Modelo
            ValidationObject validationObject = CustomValidation.GetValidationFromDataAnnotations(helper, data, express, modelStateValidation.error);

            dynamicsAttr += validationObject.validations;
            dynamicsAttr += isChecked ? " checked" : "";

            string validationMessage = validationObject.message != null ? validationObject.message : "";

            return new MvcHtmlString(CustomHtml.BuildInput(InputType.Checkbox, express, id, value == "" ? "true" : value, false, string.Empty, dynamicsAttr, cssClass, data.PropertyName, validationMessage));
        }


        /// <summary>
        /// Custom CheckBox List
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="listItems">Objeto ListCustomOptionItem</param>
        /// <param name="htmlAttributes">
        /// Permitidos: @value, @checked, @class y @dynamicsAttr
        /// @checked:   true or false
        /// @dynamicsAttr:  es para agregar atributos dinamicos al helper
        /// </param>
        /// <returns></returns>
        public static MvcHtmlString CustomCheckBoxList<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, ListCustomOptionItem listItems, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string express = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            string id = express.Replace(".", "_");
            ModelStateValidation modelStateValidation = CustomValidation.GetModelStateValidation(helper.ViewData.ModelState, express);

            string value = string.Empty;
            bool isChecked = false;
            string cssClass = string.Empty;
            string dynamicsAttr = string.Empty;

            //  Leemos los atributos
            string[] resultAttributes = Tools.ReadValues(htmlAttributes, new string[] { "value", "checked", "dynamicsAttr", "class" });
            if (resultAttributes.Length > 0)
            {
                value = resultAttributes[0];
                isChecked = bool.Parse(resultAttributes[1]);
                cssClass = resultAttributes[2];
                dynamicsAttr = resultAttributes[3];
            }

            // Obtenemos las validadciones del Modelo
            ValidationObject validationObject = CustomValidation.GetValidationFromDataAnnotations(helper, data, express, modelStateValidation.error);

            dynamicsAttr += validationObject.validations;

            string validationMessage = validationObject.message != null ? validationObject.message : "";

            StringBuilder resultList = new StringBuilder();
            resultList.Append(string.Format("<ul name=\"{0}\" id=\"{0}\" style=\"list-style-type:none;\" data-val-array=\"true\"{1}>", data.PropertyName, dynamicsAttr != string.Empty ? " " + dynamicsAttr : ""));
            string strChequed = string.Empty;
            if (listItems != null)
            {
                foreach (var item in listItems.customOptionItem)
                {
                    strChequed = string.Empty;
                    if (item.IsSelected)
                        strChequed = " checked";

                    resultList.Append("<li>" + CustomHtml.BuildInput(InputType.Checkbox, express, id, item.Value, false, string.Empty, strChequed, cssClass, item.Text, "") + "</li>");
                }
            }
            resultList.Append("</ul>");

            return new MvcHtmlString(resultList.ToString() + validationMessage);
        }

        #endregion Public Methods
    }
}
