using System;
using System.ComponentModel;
using System.Globalization;
using HereAndThere.Models;
using Newtonsoft.Json;

namespace HereAndThere.TypeConvertors
{
    public class BoundaryTypeConvertor : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var s = value.ToString();
                var boundary = JsonConvert.DeserializeAnonymousType(s, new {latitude = 0.1M, longitude = 0.1M});


                return new Boundary {latitude = boundary.latitude, longitude = boundary.longitude};
            }
            return base.ConvertFrom(context, culture, value);
        }

    }
}