using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CustomHelper.Attributes
{
    public class TextoAttribute : ValidationAttribute, IClientValidatable
    {
        public enum Type
        {
            Texto,
            Texto_Number,
            Texto_Number_Spaces,
            Texto_Spaces
        }

        private string _regularExpression = string.Empty;
        private string _mask = string.Empty;
        
        public TextoAttribute(TextoAttribute.Type textoType)
        {
            /*
                Por defecto el texto incluye letras de la a a la z, tanto en mayuscula como en minuscula( abecedario simple ) 
                Y se incluyen las letras con acento como: á é í ó ú
                tambien las letras con dieresis: ä ë ï ö ü
                y tambien los caracteres ñ y ç
            */
            switch (textoType)
            {
                case Type.Texto:
                    _mask = @"[A-Za-záéíóúñÁÉÍÓÚÑäëïöüÄËÏÜÖçÇ]";
                    break;
                case Type.Texto_Number:
                    _mask = @"[A-Za-záéíóúñÁÉÍÓÚÑäëïöüÄËÏÜÖçÇ0-9]";
                    break;
                case Type.Texto_Number_Spaces:
                    _mask = @"[A-Za-záéíóúñÁÉÍÓÚÑäëïöüÄËÏÜÖçÇ0-9\s]";
                    break;
                case Type.Texto_Spaces:
                    _mask = @"[A-Za-záéíóúñÁÉÍÓÚÑäëïöüÄËÏÜÖçÇ\s]";
                    break;
            }
            _regularExpression = "^" + _mask + "+$";

        }

        public override string FormatErrorMessage(string name)
        {
            return "El campo " + name + " no es un texto valido.";
        }

        public TextoAttribute(string regularExpression)
        {
            _regularExpression = regularExpression;
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
            rule.ValidationParameters.Add("mask", _mask);
            rule.ValidationType = "exclude";
            yield return rule;
        }
    }
}
