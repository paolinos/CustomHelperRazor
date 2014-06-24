using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace CustomHelper
{
    public static class CustomDropDownListHelper
    {
        #region Public Methods

        /// <summary>
        /// Custom DropDownList
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="listItems">Objeto ListCustomOptionItem</param>
        /// <param name="htmlAttributes">
        /// Permitidos: @class, @dynamicsAttr, @size, @multiple, @readonly.
        /// @readonly:   true or false
        /// @dynamicsAttr:  es para agregar atributos dinamicos al helper.
        /// @multiple:  true or false. Multiples selecciones o no.
        /// </param>
        /// <returns></returns>
        public static MvcHtmlString CustomDropDownList<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, ListCustomOptionItem listItems, object htmlAttributes = null)
        {
            return CustomDropDownList(helper, expression, listItems, "", htmlAttributes);
        }

        /// <summary>
        /// Custom DropDownList con valor por defecto
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="listItems">Objeto ListCustomOptionItem</param>
        /// <param name="defaultOption">Valor por defecto</param>
        /// <param name="htmlAttributes">
        /// Permitidos: @class, @dynamicsAttr, @size, @multiple, @readonly.
        /// @readonly:   true or false
        /// @dynamicsAttr:  es para agregar atributos dinamicos al helper.
        /// @multiple:  true or false. Multiples selecciones o no.
        /// </param>
        /// <returns></returns>
        public static MvcHtmlString CustomDropDownList<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, ListCustomOptionItem listItems, string defaultOption, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string express = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            string id = express.Replace(".", "_");
            ModelStateValidation modelStateValidation = CustomValidation.GetModelStateValidation(helper.ViewData.ModelState, express);

            
            // Obtenemos las validadciones del Modelo
            ValidationObject validationObject = CustomValidation.GetValidationFromDataAnnotations(helper, data, express, modelStateValidation.error);


            string value = string.Empty;
            string cssClass = string.Empty;
            string dynamicsAttr = string.Empty;
            string newId = string.Empty;
            string newName = string.Empty;

            string size = string.Empty;
            bool multiple = false;
            bool readOnly = false;

            //  Leemos los atributos
            string[] resultAttributes = Tools.ReadValues(htmlAttributes, new string[] { "class", "dynamicsAttr", "size", "multiple", "readonly", "id", "name" });
            if (resultAttributes.Length > 0)
            {
                cssClass = resultAttributes[0];
                dynamicsAttr = resultAttributes[1];
                size = resultAttributes[2];
                multiple = resultAttributes[3] != string.Empty ? bool.Parse(resultAttributes[3]) : false;
                readOnly = resultAttributes[4] != string.Empty ? bool.Parse(resultAttributes[4]) : false;
                newId = resultAttributes[5];
                newName = resultAttributes[6];
            }

            dynamicsAttr += validationObject.validations;
            string validationMessage = validationObject.message != null ? validationObject.message : "";

            StringBuilder listOptions = new StringBuilder();
            if (defaultOption != string.Empty)
                listOptions.Append(CustomHtml.BuildOption("", defaultOption));

            if (listItems != null)
            {
                foreach (var item in listItems.customOptionItem)
                {
                    listOptions.Append(CustomHtml.BuildOption(item.Value, item.Text, item.IsSelected ? " selected" : ""));
                }
            }

            if (newId == string.Empty)
            {
                newId = id;
            }

            if (newName == string.Empty)
            {
                newName = express;
            }

            return new MvcHtmlString(CustomHtml.BuildSelect(newName, newId, cssClass, listOptions.ToString(), dynamicsAttr, validationMessage, readOnly, size, multiple));
        }

        #endregion Public Methods
    }
}
