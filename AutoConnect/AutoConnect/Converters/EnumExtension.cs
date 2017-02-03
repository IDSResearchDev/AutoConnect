using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AutoConnect.Converters
{
    public enum ComponentType
    {
        [Description("141")]
        c141 = 141,
        [Description("143")]
        c143 = 143,
        [Description("146")]
        c146 = 146,
        [Description("147")]
        c147 = 147,
        [Description("149")]
        c149 = 149,
        [Description("184")]
        c184 = 184,
        [Description("185")]
        c185 = 185,
        [Description("186")]
        c186 = 186,
        [Description("187")]
        c187 = 187,
        [Description("188")]
        c188 = 188
    }

    public enum BoltWeldOrientation
    {
        [Description("Bolted-Bolted")]
        BoltedBolted = 2,
        [Description("Bolted-Welded")]
        BoltedWelded = 4,
        [Description("Welded-Bolted")]
        WeldedBolted = 3,
        [Description("Welded-Welded")]
        WeldedWelded = 5
    }

    public enum AngleTypes
    {
        [Description("Single")]
        Single = 2,
        [Description("Double")]
        Double = 3
    }

    public class EnumExtension : MarkupExtension
    {
        private Type _enumType;


        public EnumExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            EnumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            return (
              from object enumValue in enumValues
              select new EnumerationMember
              {
                  Value = enumValue,
                  Description = GetDescription(enumValue)
              }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = EnumType
              .GetField(enumValue.ToString())
              .GetCustomAttributes(typeof(DescriptionAttribute), false)
              .FirstOrDefault() as DescriptionAttribute;


            return descriptionAttribute != null
              ? descriptionAttribute.Description
              : enumValue.ToString();
        }

        public class EnumerationMember
        {
            public string Description { get; set; }
            public object Value { get; set; }
        }
    }
}
