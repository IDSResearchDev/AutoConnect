using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
//using System.Windows.Data;
using AutoConnect.View;
using AutoConnect.View.UserControls;

namespace AutoConnect.Enums
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
        [Description("Bolted-Bolted")]
        BoltedBolted = 1,
        [Description("Bolted-Welded")]
        BoltedWelded = 2,
        [Description("Welded-Bolted")]
        WeldedBolted = 3,
        [Description("Welded-Welded")]
        WeldedWelded = 4
    }

    //public enum FrameOrientationType
    //{
    //    [Description("BeamToColumnWeb")]
    //    BeamToColumnWeb = 1,
    //    [Description("BeamToColumnFlange")]
    //    BeamToColumnFlange = 2,
    //    [Description("BeamToBeamWeb")]
    //    BeamToBeamWeb = 3,
    //    [Description("BeamToBeamFlange")]
    //    BeamToBeamFlange = 4
    //};

    public class ComponentSettingsEnum
    {
        public static string[] GetComponentTypeDescription(Type enumType)
        {
            string[] descriptions = new string[Enum.GetNames(enumType).Length];
            int counter = 0;

            foreach (ComponentType item in Enum.GetValues(enumType))
            {
                var field = enumType.GetField(item.ToString());
                var attrib = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                descriptions[counter] = attrib.Length == 0 ? item.ToString() : ((DescriptionAttribute)attrib[0]).Description;
                counter++;
            }

            return descriptions;
        }

        public static string[] GetAngleTypeDescription(Type enumType)
        {
            string[] descriptions = new string[Enum.GetNames(enumType).Length];
            int counter = 0;

            foreach (Angle item in Enum.GetValues(enumType))
            {
                var field = enumType.GetField(item.ToString());
                var attrib = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                descriptions[counter] = attrib.Length == 0 ? item.ToString() : ((DescriptionAttribute)attrib[0]).Description;
                counter++;
            }

            return descriptions;
        }

        public static string[] GetBoltWeldOrientation(Type enumType)
        {
            string[] descriptions = new string[Enum.GetNames(enumType).Length];
            int counter = 0;

            foreach (BoltWeldOrientation item in Enum.GetValues(enumType))
            {
                var field = enumType.GetField(item.ToString());
                var attrib = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                descriptions[counter] = attrib.Length == 0 ? item.ToString() : ((DescriptionAttribute)attrib[0]).Description;
                counter++;
            }

            return descriptions;
        }

    }

}
