using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoConnect.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {

        string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribbArray = fieldInfo.GetCustomAttributes(false);

            //var attribDescription = attribbArray[0] as DescriptionAttribute;// may cause breakdown if different attribute ?

            var attribDesc = attribbArray.OfType<DescriptionAttribute>().FirstOrDefault();

            #region simplified so?
            //var description = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
            //                        .OfType<DescriptionAttribute>()
            //                        .FirstOrDefault(); 
            #endregion


            return attribbArray.Length == 0 ? enumObj.ToString() : attribDesc.Description;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum enumValue = (Enum)value;
            string description = GetEnumDescription(enumValue);
            return description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
