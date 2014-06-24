using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;


/*
 
 http://msdn.microsoft.com/en-us/library/az24scfc%28v=vs.110%29.aspx
 * 
 * http://regexstorm.net/Tester.aspx
 
 */


namespace CustomHelper.Attributes
{
    public class NumberAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _regularExpression = "^[0-9]+$";

        public override string FormatErrorMessage(string name)
        {
            return "El campo " + name + " tiene que ser un numero";
        }

        public override bool IsValid(object value)
        {
            if (value != null)
                return Regex.IsMatch(value.ToString(), _regularExpression);

                //return Regex.IsMatch(value.ToString(), "^\\d");
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("reg", _regularExpression);
            rule.ValidationParameters.Add("mask", @"[0-9]");
            rule.ValidationType = "exclude";
            yield return rule;
        }
    }
}
