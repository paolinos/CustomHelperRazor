using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace CustomHelper
{
    public static class CustomJQGridHelper
    {
        #region Public Methods

        /// <summary>
        /// Custom JQ Grid - Grilla con javascript
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="url">Url de update</param>
        /// <param name="ajaxCalls"></param>
        /// <param name="htmlAttributes">
        /// Permitidos: @titles, @properties, @rows, @firtSort, @class.
        /// </param>
        /// <returns></returns>
        public static MvcHtmlString CustomJQGrid<TModel>(this HtmlHelper<TModel> helper, string name, Type type, string url, string ajaxCalls = "", object htmlAttributes = null, bool autoload = true)
        {
            string titles = string.Empty;
            string properties = string.Empty;
            int rows = 10;
            string firtSort = string.Empty;
            string cssClass = string.Empty;
            string extra = string.Empty;

            string[] resultAttributes = Tools.ReadValues(htmlAttributes, new string[] { "titles", "properties", "rows", "firtSort", "class", "extra" });
            if (resultAttributes.Length > 0)
            {
                titles = resultAttributes[0];
                properties = resultAttributes[1];
                rows = resultAttributes[2] == string.Empty ? 10 : int.Parse(resultAttributes[2]);
                firtSort = resultAttributes[3];
                cssClass = resultAttributes[4];
                extra = resultAttributes[5];
            }

            //  Si no existe el Titulo y las Properties, hacemos Reflection y generamos la tabla con todos las propiedades
            if (titles == string.Empty && properties == string.Empty)
            {
                string tmpTitles = string.Empty;
                string tmpProperties = string.Empty;
                PropertyInfo[] propValue = type.GetProperties();
                foreach (var item in propValue)
                {
                    if (tmpTitles != string.Empty)
                    {
                        tmpTitles += ",";
                        tmpProperties += ",";
                    }

                    tmpTitles += string.Format("'{0}'", item.Name);
                    tmpProperties += CustomHtml.BuildGridPropertyScript(item.Name);
                }

                titles = tmpTitles;
                properties = tmpProperties;
            }

            if (ajaxCalls == string.Empty)
                ajaxCalls = "[]";

            return new MvcHtmlString(CustomHtml.BuildGrid(name) + CustomHtml.BuildGridScript(name, url, titles, properties, rows, firtSort, ajaxCalls, autoload));
        }

        /// <summary>
        /// Actualizamos la query para refrescar la grilla
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gridDate"></param>
        /// <param name="list"></param>
        /// <param name="extrajson">Para agregar valor extra al JSON</param>
        /// <returns></returns>
        public static JsonResult UpdateJQGrid<T>(JqGridData gridDate, IEnumerable<T> list, string extrajson = "")
        {
            int from = ((gridDate.page - 1) * gridDate.rows);
            int to = (gridDate.page * gridDate.rows);

            /*
            int maxRows = maxRowsValue;
            if(maxRowsValue == -1)
                maxRows = list.ToList().Count();
            */
            int maxRows = list.Count();
            int maxPages = (maxRows / gridDate.rows)+1;

            IEnumerable<T> tmpList = QueryExtension.Get<T>(list, gridDate.sidx, gridDate.sord == "desc" ? false : true, from, to);

            return new JsonResult()
            {
                Data = jqGridReturn.Generate(gridDate.page.ToString(), maxPages, maxRows, tmpList, extrajson),
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// Update List query
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="gridDate">Data</param>
        /// <param name="list">List</param>
        /// <returns>IEnumerable to continuing with the query.</returns>
        public static IEnumerable<T> UpdateJqGridList<T>(JqGridData gridDate, IEnumerable<T> list)
        {
            int from = ((gridDate.page - 1) * gridDate.rows);
            int to = (gridDate.page * gridDate.rows);

            int maxRows = list.Count();
            int maxPages = maxRows / gridDate.rows;

            IEnumerable<T> tmpList = QueryExtension.Get<T>(list, gridDate.sidx, gridDate.sord == "desc" ? false : true, from, to);
            return tmpList;
        }


        #endregion Public Methods
    }
}
