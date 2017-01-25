using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;

namespace AutoConnect.Model
{
    public static class UnitConverter
    {
        public static double RoundDown(this double value, double decimalPlaces)
        {
            var power = Math.Pow(10, decimalPlaces);
            return Math.Floor(value * power) / power;
        }

        public static double RoundOff(this double value, int decimalPlaces)
        {
            return Math.Round(value, decimalPlaces);
        }

        public static double RoundOff(this double? value, int decimalPlaces)
        {
            return Math.Round((double)value, decimalPlaces);
        }
    }

    public enum LineType
    {
        Slant = 0,
        Vertical = 1,
        Horizontal = 2,
        Column = 3
    }

    public enum ModelObjType
    {
        Primary = 1,
        Secondary
    }

    public class Line
    {
        public int Id;
        public double X1, Y1, X2, Y2, Z1, Z2;
        public double? M, B;
        public LineType Type;
        public ModelObjType Part;
        public Beam ObjSource;
        public Line()
        { }


        public Line(int id, double _x1, double _y1, double _x2, double _y2)
        {
            Id = id;
            X1 = _x1.RoundOff(3);
            Y1 = _y1.RoundOff(3);
            X2 = _x2.RoundOff(3);
            Y2 = _y2.RoundOff(3);
            InitializeValues();
        }

        public Line(Beam beam)
        {
            Id = beam.Identifier.ID;
            X1 = beam.StartPoint.X.RoundOff(3);
            Y1 = beam.StartPoint.Y.RoundOff(3);
            Z1 = beam.StartPoint.Z.RoundOff(3);
            X2 = beam.EndPoint.X.RoundOff(3);
            Y2 = beam.EndPoint.Y.RoundOff(3);
            Z2 = beam.EndPoint.Z.RoundOff(3);
            ObjSource = beam;
            InitializeValues();
        }

        private void InitializeValues()
        {
            this.SetLineType();
            M = GetSlope();
            if (M != null)
            { B = GetYIntercept(); }
        }

        private double? GetSlope()
        {
            // slope (m) = (y2-y1) / (x2-x1)
            double? slope = null;
            switch (Type)
            {
                case LineType.Slant:
                case LineType.Horizontal:
                    slope = (Y2 - Y1) / (X2 - X1);
                    break;
                case LineType.Vertical:
                    slope = null;
                    break;

                default:
                    break;
            }
            return slope;
        }

        private void SetLineType()
        {
            if (X1 == X2)
            {
                if (Y1 == Y2)
                {
                    Type = LineType.Column;
                }
                else
                {
                    Type = LineType.Vertical;
                }
            }
            else if (Y1 == Y2)
            {
                if (X1 == X2)
                {
                    Type = LineType.Column;
                }
                else
                {
                    Type = LineType.Horizontal;
                }
            }
            else
            {
                Type = LineType.Slant;
            }
        }

        private double? GetYIntercept()
        {
            // y-intercept = y - mx;
            return Y1 - (M * X1);
        }
    }

    public class LineChecker
    {
        private Line line1;
        private Line line2;
        private double? x_coord, y1_coord, y2_coord;
        private bool _intersects;
        private bool _validConnection;

        public double? X_Coord
        {
            get
            {
                return x_coord;//.RoundDown(3);
            }
            set
            {
                x_coord = value;
                //x_coord = (line1.M - (line2.M)) / (line2.B - (line1.B));
            }
        }

        public double? Y1_Coord
        {
            get
            {
                return y1_coord;//.RoundDown(3);
            }
            set
            {
                y1_coord = value;
                // y = mx + b
                //y1_coord = (line1.M * X_Coord) + line1.B;
            }
        }

        public double? Y2_Coord
        {
            get
            {
                return y2_coord;//.RoundDown(3);
            }
            set
            {
                y2_coord = value;
                // y = mx + b
                //y2_coord = (line2.M * X_Coord) + line2.B;
            }
        }

        public bool Intersects
        {
            get
            {
                return _intersects;
            }
            set
            {
                _intersects = value;
            }
        }

        public bool ValidConnection
        {
            get
            {
                return _validConnection;
            }
            set
            {
                _validConnection = value;
            }
        }

        public LineChecker(Line _line1, Line _line2)
        {
            line1 = _line1;
            line2 = _line2;
            this.Init();
        }

        private void Init()
        {
            X_Coord = line1.Type == LineType.Vertical || line1.Type == LineType.Column ? line1.X1 : line2.Type == LineType.Vertical || line2.Type == LineType.Column ? line2.X1 : (line2.B - (line1.B)) / (line1.M - (line2.M));
            Y1_Coord = line1.Type == LineType.Horizontal || line1.Type == LineType.Column ? line1.Y1 : line2.Type == LineType.Horizontal || line2.Type == LineType.Column ? line2.Y1 : (line1.M * X_Coord) + line1.B;
            Y2_Coord = line1.Type == LineType.Horizontal || line1.Type == LineType.Column ? line1.Y1 : line2.Type == LineType.Horizontal || line2.Type == LineType.Column ? line2.Y1 : (line2.M * X_Coord) + line2.B;

            if (X_Coord != null && Y1_Coord != null && Y2_Coord != null)
            {
                if (!Double.IsInfinity((double)X_Coord) && !Double.IsInfinity((double)Y1_Coord) && !Double.IsInfinity((double)Y2_Coord))
                {
                    double y1 = (double)Y1_Coord;
                    double y2 = (double)Y2_Coord;
                    double x = (double)X_Coord;
                    List<Line> lines = new List<Line>();

                    lines.Add(line1);
                    lines.Add(line2);
                    //Intersects = y1.ToString("0.###") == y2.ToString("0.###");
                    //Console.WriteLine($"x:{((double)X_Coord).ToString("0.###")}\ny1:{ ((double)Y1_Coord).ToString("0.###")}\ny2:{((double)Y2_Coord).ToString("0.###")}");
                    // 1st validation for intersection
                    if (y1.ToString("0.###") == y2.ToString("0.###"))
                    {
                        // check secondary 

                        var sec = lines.Where(z => (z.X1.RoundOff(3) == X_Coord.RoundOff(3) && z.Y1.RoundOff(3) == Y1_Coord.RoundOff(3)) || (z.X2.RoundOff(3) == X_Coord.RoundOff(3) && z.Y2.RoundOff(3) == Y2_Coord.RoundOff(3))).Select(p => p).ToList();

                        // 2nd validation for intersection
                        if (sec.Count > 0)
                        {
                            line1.Part = line1.Type == LineType.Column ? ModelObjType.Primary : sec[0].Id == line1.Id ? ModelObjType.Secondary : ModelObjType.Primary;
                            line2.Part = line1.Type == LineType.Column ? ModelObjType.Secondary : sec[0].Id == line2.Id ? ModelObjType.Secondary : ModelObjType.Primary;

                            int counter = 0;
                            foreach (var line in lines)
                            {
                                var x_min = (line.X1 < line.X2) ? line.X1 : line.X2;
                                var x_max = (line.X1 < line.X2) ? line.X2 : line.X1;

                                var y_min = (line.Y1 < line.Y2) ? line.Y1 : line.Y2;
                                var y_max = (line.Y1 < line.Y2) ? line.Y2 : line.Y1;

                                if ((X_Coord.RoundOff(3) >= x_min.RoundOff(3) && X_Coord.RoundOff(3) <= x_max.RoundOff(3)) &&
                                    (Y1_Coord.RoundOff(3) >= y_min.RoundOff(3) && Y1_Coord.RoundOff(3) <= y_max.RoundOff(3)))
                                {
                                    counter++;
                                }
                            }
                            // 3rd validation for intersection
                            Intersects = counter == lines.Count;

                            //if (Intersects)
                            //{
                            //    if (sec.Count() > 1)
                            //    {
                            //        ValidConnection = (line1.Type != LineType.Column && line2.Type != LineType.Column) ? false : true;
                            //    }
                            //    else
                            //    {
                            //        ValidConnection = true;
                            //    }
                            //}
                            ValidConnection = Intersects;
                        }


                    }
                }
            }
        }


        public Line GetPrimary()
        {
            Line primary = new Line();
            if (Intersects)
            {
                primary = line1.Part == ModelObjType.Primary ? line1 : line2;
            }

            return primary;
        }

        public Line GetSecondary()
        {
            Line primary = new Line();
            if (Intersects)
            {
                primary = line2.Part == ModelObjType.Secondary ? line2 : line1;
            }

            return primary;
        }

    }
}
