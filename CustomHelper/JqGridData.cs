using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomHelper
{
    public class JqGridData
    {
        public bool _search { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public string sidx { get; set; }
        public string sord { get; set; }
        public bool export { get; set; }

        //  To pass an extra parameter
        public string extra { get; set; }
    }

    public class jqGridReturn
    {
        public string page { get; internal set; }
        public int total { get; internal set; }
        public int records { get; internal set; }
        public object[] rows { get; internal set; }
        public string extra { get; internal set; }


        public static jqGridReturn Generate<T>(string p, int t, int rec, IEnumerable<T> list, string extraJson = "")
        {
            return new jqGridReturn()
            {
                page = p,
                total = t,
                records = rec,
                rows = list != null ? list.Cast<object>().ToArray() : new object[0],
                extra = extraJson
            };
        }


        private static T[] GetList<T>(IEnumerable<T> list)
        {
            T[] array = list.Cast<T>().ToArray();
            return array;
        }
    }
}
