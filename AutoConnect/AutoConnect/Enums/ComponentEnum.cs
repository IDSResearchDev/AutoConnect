using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoConnect.View;
using AutoConnect.View.UserControls;

namespace AutoConnect.Enums
{
    public class ComponenSettingstEnum
    {
        enum ComponentType
        {
            c141 = 141,
            c143 = 143,
            c146 = 146,
            c147 = 147,
            c149 = 149,
            c184 = 184,
            c185 = 185,
            c186 = 186,
            c187 = 187,
            c188 = 188
        }
        /// <summary>
        /// Jfile attribute => btab5
        /// </summary>
        enum Angle
        {
            Single,
            Double
        }
        /// <summary>
        /// Jfile attribute => atab1
        /// </summary>
        enum FrameOrientationType
        {
            BoltedBolted,
            WeldedBolted,
            BoltedWelded,
            WeldedWelded
        }
    }
}
