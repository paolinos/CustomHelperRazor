using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CustomHelper.Attributes
{
    public class EmailAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _regularExpression = @"^([_A-Za-z0-9-+]+(.[_A-Za-z0-9-]+)*){3,}@([A-Za-z0-9-]+(.[A-Za-z0-9]+)*){3,}(.[A-Za-z]{2,})$";

        public override string FormatErrorMessage(string name)
        {
            return "El campo " + name + " es un email invalido. Ej.: xxx@yyy.com, xxx@yyy.com.ar";
        }
        public override bool IsValid(object value)
        {
            if (value != null)
                return Regex.IsMatch(value.ToString(), _regularExpression);
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("reg", _regularExpression);
            rule.ValidationParameters.Add("mask", @"[_A-Za-z0-9-.@]");
            rule.ValidationType = "exclude";
            yield return rule;
        }
    }
}
