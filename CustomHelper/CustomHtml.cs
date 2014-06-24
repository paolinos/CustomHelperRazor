using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomHelper
{
    internal class CustomHtml
    {
        #region Html Strings

        #region Input
        /// <summary>
        /// Input ( TextBox, Password, Checkbox )
        /// info: http://www.w3schools.com/tags/tag_input.asp
        /// 
        /// {0} = Type
        /// {1} = Name
        /// {2} = Id
        /// {3} = Value
        /// {4} = Required
        /// {5} = Placeholder
        /// {6} = Dynamic Attributes
        /// {7} = CSS Class
        /// {8} = Description. ( To use in checkbox )
        /// {9} = Span Validation
        /// </summary>
        /// HTML5 (IE7/IE8 NOT SUPPORT REQUIRED ATTRIBUTE)
        //private static string strTextBox = "<input type=\"{0}\" name=\"{1}\" id=\"{1}\" value=\"{2}\" required=\"{3}\" placeholder=\"{4}\" {5}>";
        /// HTML
        private static string Input = "<input type=\"{0}\" name=\"{1}\" id=\"{2}\" value=\"{3}\" {4} {5} {6} class=\"{7}\">{8} {9}";
        #endregion Input


        #region Select
        /// <summary>
        /// Select ( DropdownList )
        /// info: http://www.w3schools.com/tags/tag_select.asp
        /// {0} =>  Name
        /// {1} =>  Id
        /// {2} =>  Css Class
        /// {3} =>  Size to show. (size="3")
        /// {4} =>  To select multiples options. (multiple)
        /// {5} =>  Disabled option. (disabled)
        /// {6} =>  Dynamic Attributes
        /// {7} =>  List of options
        /// {8} =>  Span Validation
        /// </summary>
        private static string Select = "<select name=\"{0}\" id=\"{1}\" class=\"{2}\" {3} {4} {5} {6}>{7}</select>{8}";

        /// <summary>
        /// Options From DropDownList
        /// 
        /// {0} => Value
        /// {1} => Selected
        /// {2} => Description
        /// </summary>
        private static string Option = "<option value=\"{0}\"{1}>{2}</option>";
        #endregion Select


        #region TextArea
        /// <summary>
        /// TextArea
        /// 
        /// {0} =>  Name/Title
        /// {1} =>  Id
        /// {2} =>  Class
        /// {3} =>  Columns
        /// {4} =>  Rows
        /// {5} =>  Required
        /// {6} =>  Dynamic Attributes
        /// {7} =>  ReadOnly
        /// {8} =>  Value
        /// {9} =>  Span Validation
        /// </summary>
        private static string TextArea = "<textarea  name=\"{0}\" id=\"{1}\" title=\"{0}\" class=\"{2}\" cols=\"{3}\" rows=\"{4}\" {5} {6} {7}>{8}</textarea>{9}";
        #endregion TextArea


        #region jqGrid
        /// <summary>
        /// jqGrid - Table
        /// 
        /// {0} =>  Id
        /// </summary>
        private static string jqGridTable = "<table id=\"{0}\"></table><div id=\"p{0}\"></div>";

        /// <summary>
        /// jqGrid - Properties Script
        /// 
        /// {0} =>  Id/Name
        /// {1} =>  Width
        /// </summary>
        //private static string strPropertyGridScript = "{ name: '{0}', index: '{0}', width: {1} }";
        private static string jqGridPropertyScript = "{0} name: '{2}', index: '{2}' {1}";

        /// <summary>
        /// jqGrid - Script
        /// 
        /// {0} =>  Id
        /// {1} =>  url Update ajax
        /// {2} =>  Title columns
        /// {3} =>  Property Grid columns
        /// {4} =>  Count of Row
        /// {5} =>  First sort Field
        /// {6} =>  {
        /// {7} =>  }
        /// {8} =>  ajaxCalls To add new icons with calls. Ex=>   [{'title':'Export','url':'/url/export', 'icon':'cssName'},{'title':'ExportAll','url':'/url/exportAll', 'icon':'cssName'}]
        /// {9} => autoload
        /// {10} => Add string when autoload it's true
        /// </summary>
        //  Disable autoload: datatype: 'local'. Autoload : 'json'
        private static string jqGridScript = @"<script type='text/javascript'>App.Inst().onReady(function () {6} var $grid{0} = $('#{0}');$grid{0}.jqGrid({6} url: '{1}', datatype: '{9}', colNames: [{2}], colModel: [{3}], rowNum: {4}, rowList: [10, 20, 30], pager: '#p{0}', sortname: '{5}', viewrecords: true, sortorder: 'asc',height: '100%', caption: '' {7} ); $grid{0}.jqGrid('navGrid', '#p{0}', {6} edit: false, add: false, search:false, del: false, ajaxCalls:{8} {7});{10}{7});</script>";

        #endregion jqGrid

        #endregion Html Strings


        #region Public Methods


        public static string BuildInput(string type, string name, string id, string value, bool required, string placeholder = "", string dynamicsAttr = "", string cssClass = "", string innerHtml = "", string validation = "")
        {
            return string.Format(CustomHtml.Input, 
                                    type,
                                    name,
                                    id, 
                                    value, 
                                    required ? "required=\"true\"" : "", 
                                    placeholder != "" ? "placeholder=\"" + placeholder + "\"" : "", 
                                    dynamicsAttr, 
                                    cssClass, 
                                    innerHtml, 
                                    validation);
        }


        public static string BuildSelect(string name, string id, string cssClass, string listOptions, string dynamicAttr, string validations, bool readOnly, string size = "", bool multiple = false)
        {
            return string.Format(Select,name, id, cssClass, size != "" ? "size=\"" + size + "\"" : "", multiple ? "multiple" : "", readOnly ? "disabled" : "", dynamicAttr, listOptions, validations);
        }

        public static string BuildOption(string value, string name, string checkedOrSelected = "")
        {
            return string.Format(Option, value, checkedOrSelected, name);
        }


        public static string BuildTextArea(string name, string id, string cssClass, int columns, int rows, bool required, string dynamicsAttr, string value, string validations, bool readOnly = false)
        {
            return string.Format(TextArea,name, id, cssClass, columns, rows, required ? "required=\"true\"" : "", dynamicsAttr, readOnly ? "readonly=\"readonly\"" : "", value, validations);
        }


        public static string BuildGrid(string name)
        {
            return string.Format(jqGridTable, name);
        }

        public static string BuildGridScript(string id, string url, string titles, string properties, int rows, string firstSort, string ajaxCalls, bool autoload = true)
        {
            string setDataType = "json";
            string changeDataType = string.Empty;

            if(!autoload){
                setDataType = "local";
                changeDataType = string.Format("$grid{2}.jqGrid('setGridParam',{0}datatype: 'json'{1});", "{","}",id);
            }

            return string.Format(jqGridScript,
                id,
                url,
                titles,
                properties,
                rows,
                firstSort,
                "{",
                "}",
                ajaxCalls,
                setDataType,
                changeDataType);
        }

        public static string BuildGridPropertyScript(string name)
        {
            return string.Format(jqGridPropertyScript, "{", "}", name);
        }

        #endregion Public Methods
    }
}
