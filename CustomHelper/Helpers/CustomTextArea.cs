using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace CustomHelper
{
    public static class CustomTextAreaHelper
    {
        #region Private Properties
        #endregion Private Properties


        #region Public Methods

        /// <summary>
        /// Custom TextArea
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes">
        /// Permitidos: @value, @class, @dynamicsAttr, @cols, @rows, @readonly
        /// @readonly:   true or false
        /// @dynamicsAttr:  es para agregar atributos dinamicos al helper.
        /// @cols:  Cantidad de columnas
        /// @rows:  Cantidad de filas
        /// </param>
        /// <returns></returns>
        public static MvcHtmlString CustomTextArea<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
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
            int columns = 18;
            int rows = 2;
            bool readOnly = false;


            string[] resultAttributes = Tools.ReadValues(htmlAttributes, new string[] { "value", "class", "dynamicsAttr", "cols", "rows", "readonly" });
            if (resultAttributes.Length > 0)
            {
                value = resultAttributes[0];
                cssClass = resultAttributes[1];
                dynamicsAttr = resultAttributes[2];
                columns = int.Parse(resultAttributes[3]);
                rows = int.Parse(resultAttributes[4]);
                readOnly = bool.Parse(resultAttributes[5]);
            }

            dynamicsAttr += validationObject.validations;
            return new MvcHtmlString(CustomHtml.BuildTextArea(express, id, cssClass, columns, rows, false, dynamicsAttr, value, modelStateValidation.error));
        }

        #endregion Public Methods

    }
}
