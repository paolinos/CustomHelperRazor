using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CustomHelper.Attributes
{
    public class HourAttribute : ValidationAttribute, IClientValidatable
    {
        public enum Type
        {
            Horas,
            HorasMinutos,
            HorasMinutosSegundos
        }

        private string _regularExpression = string.Empty;
        private string _example = string.Empty;

        public HourAttribute(HourAttribute.Type type = Type.HorasMinutos )
        {
            switch (type)
            {
                case Type.Horas:
                    //_regularExpression = @"^\d{1,2}$";
                    _regularExpression = @"^([01]?[0-9]|2[0-4])$";
                    _example = "1, 01, 10, 23";
                    break;
                case Type.HorasMinutosSegundos:
                    //_regularExpression = @"^\d{1,2}:{1}\d{2}:{1}\d{2}$";
                    _regularExpression = @"^([01]?[0-9]|2[0-4]):{1}(0[0-9]|[0-5][0-9]):{1}(0[0-9]|[0-5][0-9])$";
                    _example = "1:20:00, 01:20:23, 10:00:59, 23:59:52";
                    break;

                default:
                    //_regularExpression = @"^\d{1,2}:{1}\d{2}$";
                    //_regularExpression = @"^([01]?[0-9]|2[0-3]):{1}(0[0-9]|[0-5][0-9])$";
                    _regularExpression = @"^([01]?[0-9]|2[0-3]):(0[0-9]|[0-5][0-9])$";
                    _example = "1:20, 01:20, 10:00, 23:59";
                    break;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return "El campo " + name + " tiene que ser una hora valida.Ej.:" + _example;
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
            rule.ValidationParameters.Add("mask", "[0-9:]");
            rule.ValidationType = "exclude";
            yield return rule;
        }
    }
}
