using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mi9Pay.Web.Helpers
{
    public static class FormHelper
    {
        public static Dictionary<string, string> CovertToDictionary(this FormCollection form)
        {
            Dictionary<string, string> requestParameter = new Dictionary<string, string>();
            foreach (string key in form.AllKeys)
            {
                string value = form[key] as string;
                requestParameter.Add(key, value);
            }
            return requestParameter;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}