using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoConnect.View;
using AutoConnect.View.UserControls;

namespace AutoConnect.Enums
{
    public class ComponentSettingsEnum
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
        /// <summary>
        /// Jfile attribute => btab5
        /// </summary>
        public enum Angle
        {
            [Description("Single")]
            Single = 1,
            [Description("Double")]
            Double = 2
        }
        /// <summary>
        /// Jfile attribute => atab1
        /// </summary>
        public enum BoltWeldOrientation
        {
            [Description("BoltedBolted")]
            BoltedBolted = 1,
            [Description("WeldedBolted")]
            WeldedBolted = 2,
            [Description("BoltedWelded")]
            BoltedWelded = 3,
            [Description("WeldedWelded")]
            WeldedWelded = 4
        }

        public enum FrameOrientationType
        {
            [Description("BeamToColumnWeb")]
            BeamToColumnWeb = 1,
            [Description("BeamToColumnFlange")]
            BeamToColumnFlange = 2,
            [Description("BeamToBeamWeb")]
            BeamToBeamWeb = 3,
            [Description("BeamToBeamFlange")]
            BeamToBeamFlange = 4
        }

        public static class EnumCollection
        {
            public static string[] GetComponentTypeDescription(Type enumType)
            {
                string[] descriptions = new string[Enum.GetNames(enumType).Length];
                int counter = 0;

                foreach (var item in Enum.GetValues(enumType))
                {
                    var field = enumType.GetField(item.ToString());
                    var attrib = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    descriptions[0] = attrib.Length == 0 ? item.ToString() : ((DescriptionAttribute)attrib[0]).Description;
                    counter++;
                   

                }

                return descriptions;
            }

        }
    }
}
