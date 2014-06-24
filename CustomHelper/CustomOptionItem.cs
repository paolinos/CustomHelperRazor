using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomHelper
{
    public class CustomOptionItem
    {
        public string Value { get; internal set; }
        public string Text { get; internal set; }
        public bool IsSelected { get; internal set; }

        internal CustomOptionItem()
        {

        }
        public CustomOptionItem(string value, string text, bool selected)
        {
            Value = value;
            Text = text;
            IsSelected = IsSelected;
        }
    }

    public class ListCustomOptionItem
    {
        public List<CustomOptionItem> customOptionItem { get; set; }

        public ListCustomOptionItem()
        {
            customOptionItem = new List<CustomOptionItem>();
        }
    }

    public static class ToolCustomOptionItems
    {
        /// <summary>
        /// Get ListOptionItem, reading the properties.
        /// </summary>
        /// <param name="list">List to read</param>
        /// <param name="dataValueField">Value</param>
        /// <param name="dataTextField">Text</param>
        /// <param name="dataSelectedField">Check if Value it's inside of this array, and Select the value</param>
        /// <returns>A ListOptionItem, with a OptionList.</returns>
        public static ListCustomOptionItem GetListItems(IEnumerable list, string dataValueField, string dataTextField, string[] dataSelectedField)
        {
            ListCustomOptionItem listOptionItem = new ListCustomOptionItem();
            foreach (var item in list)
            {
                listOptionItem.customOptionItem.Add(GetItem(item, dataValueField, dataTextField, dataSelectedField));
            }
            return listOptionItem;
        }


        public static ListCustomOptionItem GetListItems(IEnumerable list, string dataValueField, string dataTextField)
        {
            ListCustomOptionItem listOptionItem = new ListCustomOptionItem();
            foreach (var item in list)
            {
                listOptionItem.customOptionItem.Add(GetItem(item, dataValueField, dataTextField));
            }
            return listOptionItem;
        }


        private static CustomOptionItem GetItem(object item, string dataValueField, string dataTextField, string dataSelectedField = "")
        {
            Type type = item.GetType();

            PropertyInfo propValue = type.GetProperty(dataValueField);
            PropertyInfo propText = type.GetProperty(dataTextField);
            PropertyInfo propSelected = null;
            if (dataSelectedField != string.Empty)
                propSelected = type.GetProperty(dataSelectedField);

            return new CustomOptionItem()
            {
                Value = propValue.GetValue(item, null).ToString(),
                Text = propText.GetValue(item, null).ToString(),
                IsSelected = dataSelectedField != string.Empty ? (bool)propSelected.GetValue(item, null) : false
            };
        }


        private static CustomOptionItem GetItem(object item, string dataValueField, string dataTextField, string[] dataSelectedField)
        {
            Type type = item.GetType();

            PropertyInfo propValue = type.GetProperty(dataValueField);
            PropertyInfo propText = type.GetProperty(dataTextField);

            //  TODO: Validar que existan las propiedades y que contenga valores ?

            CustomOptionItem optionItem = new CustomOptionItem()
            {
                Value = propValue.GetValue(item, null).ToString(),
                Text = propText.GetValue(item, null).ToString(),
                IsSelected = false
            };

            if (dataSelectedField.Where(q => q == optionItem.Value).FirstOrDefault() != null)
            {
                optionItem.IsSelected = true;
            }


            return optionItem;
        }
    }
}
