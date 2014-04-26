using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Helpers
{
    public static class EnumToSelectList
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = (int)Enum.Parse(typeof(TEnum), e.ToString()), Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObj);
        }
    }
}