using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures;
using Tekla.Structures.Model;
using TSMU = Tekla.Structures.Model.UI;

namespace AutoConnect.Model
{
    public class ConnectionChecker
    {
        private ObservableCollection<ConnectionSetting> _beamToBeamWebColl;
        public ObservableCollection<ConnectionSetting> BeamToBeamWebColl
        {
            get
            {
                return _beamToBeamWebColl;
            }
            set
            {
                _beamToBeamWebColl = value;
            }
        }
        private ObservableCollection<ConnectionSetting> _beamToColumnWebColl;
        public ObservableCollection<ConnectionSetting> BeamToColumnWebColl
        {
            get
            {
                return _beamToColumnWebColl;
            }
            set
            {
                _beamToColumnWebColl = value;
            }
        }

        private ObservableCollection<ConnectionSetting> _beamToColumnFlangeColl;
        public ObservableCollection<ConnectionSetting> BeamToColumnFlangeColl
        {
            get
            {
                return _beamToColumnFlangeColl;
            }
            set
            {
                _beamToColumnFlangeColl = value;
            }
        }

        public ConnectionChecker()
        {
            BeamToBeamWebColl = new ObservableCollection<ConnectionSetting>();
            BeamToColumnFlangeColl = new ObservableCollection<ConnectionSetting>();
            BeamToColumnWebColl = new ObservableCollection<ConnectionSetting>();
        }

        public void Process()
        {

            var model = new Tekla.Structures.Model.Model();
            var modelObjSelector = new TSMU.ModelObjectSelector();
            var selectedObj = modelObjSelector.GetSelectedObjects();

            var objlist = new List<Beam>();

            while (selectedObj.MoveNext())
            {
                Tekla.Structures.Model.Part thisPart = selectedObj.Current as Tekla.Structures.Model.Part;

                if (thisPart != null)
                {
                    var objBeam = thisPart as Beam;
                    objlist.Add(objBeam);
                }
            }

            List<ConnectionInfo> conInfo = new List<ConnectionInfo>();
            var pairlist = new List<CollectionPair>();

            foreach (var item in objlist)
            {
                var currentitem = item;


                var items = objlist.Where
                                        (layer =>
                                            Math.Round(layer.StartPoint.Z, 1) == Math.Round(currentitem.StartPoint.Z, 1) ||
                                            (
                                                Math.Round(layer.StartPoint.Z, 1) >= Math.Round(currentitem.StartPoint.Z, 1) &&
                                                Math.Round(layer.StartPoint.Z, 1) <= Math.Round(currentitem.EndPoint.Z, 1)
                                            ) ||
                                            (
                                                Math.Round(layer.EndPoint.Z, 1) >= Math.Round(currentitem.StartPoint.Z, 1) &&
                                                Math.Round(layer.EndPoint.Z, 1) <= Math.Round(currentitem.EndPoint.Z, 1)
                                            )
                                       ).ToList();

                foreach (var subitem in items)
                {
                    if (pairlist.Exists(
                                    c => (c.Primary == currentitem.Identifier.ToString() && c.Secondary == subitem.Identifier.ToString())
                                          || (c.Primary == subitem.Identifier.ToString() && c.Secondary == currentitem.Identifier.ToString())
                                )) continue;

                    if (currentitem.Identifier != subitem.Identifier)
                    {
                        ConnectionInfo info = new ConnectionInfo();
                        Line line1 = new Line(currentitem);
                        Line line2 = new Line(subitem);

                        var linechecker = new LineChecker(line1, line2);

                        if (linechecker.ValidConnection)
                        {
                            info.Primary = linechecker.GetPrimary();
                            info.PrimaryId = info.Primary.Id;
                            info.Secondary = linechecker.GetSecondary();
                            info.SecondaryProfile = GetSecondaryBeamProfile(info.Secondary.ObjSource);
                            info.PrimaryType = info.Primary.ObjSource.Type.ToString();
                            info.XIntersection = (double)linechecker.X_Coord;
                            info.YIntersection = (double)linechecker.Y1_Coord;
                            info.Level = info.Secondary.ObjSource.StartPoint.Z.RoundDown(3);


                            Beam primary = info.Primary.ObjSource;
                            Beam secondary = info.Secondary.ObjSource;


                            conInfo.Add(info);

                            // create pair of collection
                            // identify primary and secondary objects
                            // iterate pair of collection

                            var pairs = new CollectionPair()
                            {
                                Profile = (linechecker.GetSecondary().Id == currentitem.Identifier.ID) ? currentitem.Profile.ProfileString : subitem.Profile.ProfileString,
                                Primary = linechecker.GetPrimary().Id.ToString(),
                                Secondary = linechecker.GetSecondary().Id.ToString()
                            };
                            pairlist.Add(pairs);

                        }
                    }
                }
            }

            ProcessCollection(conInfo);

            Console.WriteLine("+++++++++++++++++++++++++++++");
            Console.WriteLine("Beam To Beam-Web Connection");
            Console.WriteLine();
            foreach (var i in BeamToBeamWebColl)
            {
                Console.WriteLine($"IsSingleConnection: {i.IsSingleConnection}");
                Console.Write($"{((Beam)i.PrimaryObject).Identifier.ID}");
                if (i.IsSingleConnection)
                { Console.Write($">>{((Beam)i.SecondaryObject).Identifier.ID}"); }
                else
                {
                    //Console.WriteLine($"Secondary ids:");
                    foreach (var l in i.SecondaryObjects)
                    {
                        var o = l as Beam;
                        Console.Write($">>{o.Identifier.ID}");
                    }

                }
                Console.WriteLine();
                Console.WriteLine("-------------------");
                Console.WriteLine();
            }
            Console.WriteLine("+++++++++++++++++++++++++++++");
            Console.WriteLine("Beam To Column-Web Connection");
            Console.WriteLine();
            foreach (var i in BeamToColumnWebColl)
            {
                Console.WriteLine($"IsSingleConnection: {i.IsSingleConnection}");
                Console.Write($"{((Beam)i.PrimaryObject).Identifier.ID}");
                if (i.IsSingleConnection)
                { Console.Write($">>{((Beam)i.SecondaryObject).Identifier.ID}"); }
                else
                {
                    //Console.WriteLine($"Secondary ids:");
                    foreach (var l in i.SecondaryObjects)
                    {
                        var o = l as Beam;
                        Console.Write($">>{o.Identifier.ID}");
                    }

                }
                Console.WriteLine();
                Console.WriteLine("-------------------");
                Console.WriteLine();
            }
            Console.WriteLine("+++++++++++++++++++++++++++++");
            Console.WriteLine("Beam To Column-Flange Connection");
            Console.WriteLine();
            foreach (var i in BeamToColumnFlangeColl)
            {
                Console.WriteLine($"IsSingleConnection: {i.IsSingleConnection}");
                Console.Write($"{((Beam)i.PrimaryObject).Identifier.ID}");
                if (i.IsSingleConnection)
                { Console.Write($">>{((Beam)i.SecondaryObject).Identifier.ID}"); }
                else
                {
                    //Console.WriteLine($"Secondary ids:");
                    foreach (var l in i.SecondaryObjects)
                    {
                        var o = l as Beam;
                        Console.Write($">>{o.Identifier.ID}");
                    }

                }
                Console.WriteLine();
                Console.WriteLine("-------------------");
                Console.WriteLine();
            }

            model.CommitChanges();
            Console.ReadKey();

        }

        public string GetSecondaryBeamProfile(Beam secondaryBeam)
        {
            var prof = secondaryBeam.Profile.ProfileString.Split('X');
            return prof[0];
        }

        private void ProcessCollection(List<ConnectionInfo> conInfo)
        {
            var result = conInfo.GroupBy(x => x.PrimaryId).Select(x => x.First()).ToList();
            // per primary
            foreach (var key in result)
            {
                // get secondaries per primary
                var secondaries = conInfo.Where(x => x.PrimaryId == key.PrimaryId).ToList();

                // get distinct levels of secondaries
                var level = secondaries.GroupBy(l => l.Level).Select(x => x.First()).ToList();
                foreach (var l in level)
                {
                    // group secondaries by level 
                    var perLevel = secondaries.Where(x => x.Level == l.Level).ToList();
                    if (perLevel.Count() > 1)
                    {
                        // get common start and end points of secondary
                        var multi = perLevel.Where(z => (z.Secondary.X1.RoundOff(3) == z.XIntersection.RoundOff(3) && z.Secondary.Y1.RoundOff(3) == z.YIntersection.RoundOff(3)) || (z.Secondary.X2.RoundOff(3) == z.XIntersection.RoundOff(3) && z.Secondary.Y2.RoundOff(3) == z.YIntersection.RoundOff(3))).Select(p => p).ToList();
                        var tmpArr = new List<int>();
                        // analyze common points
                        for (int i = 0; i < multi.Count(); i++)
                        {
                            if (tmpArr.Contains(multi[i].Secondary.Id))
                                continue;
                            ArrayList arr = new ArrayList();
                            for (int j = i + 1; j < multi.Count(); j++)
                            {
                                if (multi[i].XIntersection.RoundOff(3) == multi[j].XIntersection.RoundOff(3) && multi[j].XIntersection.RoundOff(3) == multi[j].XIntersection.RoundOff(3))
                                {
                                    var pos = key.Primary.ObjSource.Position.Rotation.ToString();
                                    var axis = multi[i].Secondary.Type;
                                    var axis2 = multi[j].Secondary.Type;

                                    if (axis == axis2)
                                    {
                                        if ((axis == LineType.Vertical && axis2 == LineType.Vertical && pos == "TOP") || (axis == LineType.Horizontal && axis2 == LineType.Horizontal && pos == "FRONT"))
                                        {
                                            // double - clip angle
                                            // beam-column-beam-web | beam-beam-beam-web
                                            arr.Add(multi[i].Secondary.ObjSource);
                                            arr.Add(multi[j].Secondary.ObjSource);
                                            tmpArr.Add(multi[i].Secondary.Id);
                                            tmpArr.Add(multi[j].Secondary.Id);
                                        }

                                        //if ((axis == LineType.Vertical && pos == "FRONT") || (axis == LineType.Horizontal && pos == "TOP"))
                                        //{
                                        //    //flange

                                        //}
                                    }
                                }
                            }
                            if (arr.Count > 0)
                            {
                                ValidateCollection(key, arr);

                                // multiple connection : beam-column-beam-web | beam-beam-beam-web
                                //Console.WriteLine(CreateConnection(key.Primary.ObjSource, arr, 143, code));
                            }
                            else
                            {
                                ValidateCollection(key, multi[i].Secondary.ObjSource);
                                // add single connection
                                // check beam-flange / beam-web
                                //Console.WriteLine(CreateConnection(key.Primary.ObjSource, multi[i].Secondary.ObjSource, 141, code));
                            }
                        }
                    }
                    else
                    {
                        ValidateCollection(key, perLevel);
                    }
                }

            }
        }

        private void ValidateCollection(ConnectionInfo key, List<ConnectionInfo> perLevel)
        {
            var pos = key.Primary.ObjSource.Position.Rotation.ToString();
            var axis = perLevel[0].Secondary.Type;
            var pair = new ConnectionSetting
            {
                IsSingleConnection = true,
                PrimaryObject = key.Primary.ObjSource,
                SecondaryObject = perLevel[0].Secondary.ObjSource,
                SecondaryObjects = null
            };
            //AddToCollection(key.PrimaryType, pos, axis, pair);
            if (key.PrimaryType == "BEAM")
            {
                pair.ConnectionType = "B2BW";
                BeamToBeamWebColl.Add(pair);
            }
            else
            {
                if ((axis == LineType.Vertical && pos == "TOP") || (axis == LineType.Horizontal && pos == "FRONT"))
                {
                    pair.ConnectionType = "B2CW";
                    BeamToColumnWebColl.Add(pair);
                }
                else
                {
                    pair.ConnectionType = "B2CF";
                    BeamToColumnFlangeColl.Add(pair);
                }
            }
        }

        private void ValidateCollection(ConnectionInfo key, Beam secondary)
        {
            var pair1 = new ConnectionSetting
            {
                IsSingleConnection = true,
                PrimaryObject = key.Primary.ObjSource,
                SecondaryObject = secondary,
                SecondaryObjects = null
            };
            //AddToCollection(key.PrimaryType, pair1);
            if (key.PrimaryType == "BEAM")
            {
                pair1.ConnectionType = "B2BW";
                BeamToBeamWebColl.Add(pair1);
            }
            else
            {
                pair1.ConnectionType = "B2CF";
                BeamToColumnFlangeColl.Add(pair1);
            }
        }

        private void ValidateCollection(ConnectionInfo key, ArrayList arr)
        {
            var multiCon = new ConnectionSetting
            {
                IsSingleConnection = false,
                PrimaryObject = key.Primary.ObjSource,
                SecondaryObject = null,
                SecondaryObjects = arr
            };

            var pair1 = new ConnectionSetting
            {
                IsSingleConnection = true,
                PrimaryObject = key.Primary.ObjSource,
                SecondaryObject = arr[0],
                SecondaryObjects = null
            };

            var pair2 = new ConnectionSetting
            {
                IsSingleConnection = true,
                PrimaryObject = key.Primary.ObjSource,
                SecondaryObject = arr[1],
                SecondaryObjects = null
            };
            //AddToCollection(key.PrimaryType, multiCon, pair1, pair2);

            if (key.PrimaryType == "BEAM")
            {
                multiCon.ConnectionType = "B2BW";
                BeamToBeamWebColl.Add(multiCon);

                pair1.ConnectionType = "B2BW";
                BeamToBeamWebColl.Add(pair1);

                pair2.ConnectionType = "B2BW";
                BeamToBeamWebColl.Add(pair2);
            }
            else
            {
                multiCon.ConnectionType = "B2CW";
                BeamToColumnWebColl.Add(multiCon);

                pair1.ConnectionType = "B2CW";
                BeamToColumnWebColl.Add(pair1);

                pair2.ConnectionType = "B2CW";
                BeamToColumnWebColl.Add(pair2);
            }
        }

        public bool CreateConnection(Beam primary, Beam secondary, int component, string code)
        {
            var beamsize = GetSecondaryBeamProfile(secondary);
            var attribute = string.Concat(code, "_", beamsize);
            try
            {

                Tekla.Structures.Model.Connection C = new Tekla.Structures.Model.Connection();
                C.Name = "Test End Plate";
                C.Number = component;
                C.LoadAttributesFromFile("standard");

                C.SetPrimaryObject(primary);
                C.SetSecondaryObject(secondary);
                C.PositionType = PositionTypeEnum.COLLISION_PLANE;

                if (!C.Insert())
                {

                    Console.WriteLine("Connection Insert failed ");
                    return false;
                }
                else
                {
                    Console.WriteLine(C.Identifier.ID);
                }

                return true;
            }
            catch (Exception x)
            {

                Console.WriteLine(x.GetBaseException().Message);
                return false;
            }

        }

        public bool CreateConnection(Beam primary, ArrayList secondaries, int component, string code)
        {
            var beamsize = GetSecondaryBeamProfile(((Beam)secondaries[0]));
            var attribute = string.Concat(code, "_", beamsize);
            try
            {

                Tekla.Structures.Model.Connection C = new Tekla.Structures.Model.Connection();
                C.Name = "Test End Plate";
                C.Number = component;
                C.LoadAttributesFromFile("standard");

                C.SetPrimaryObject(primary);
                C.SetSecondaryObjects(secondaries);

                //C.UpVector = new Vector(0, 0, 1000);
                C.PositionType = PositionTypeEnum.COLLISION_PLANE;



                if (!C.Insert())
                {

                    Console.WriteLine("Connection Insert failed ");
                    return false;
                }
                else
                {
                    Console.WriteLine(C.Identifier.ID);
                }

                return true;
            }
            catch (Exception x)
            {

                Console.WriteLine(x.GetBaseException().Message);
                return false;
            }

        }

        public bool CreateConnection(Beam primary, Beam secondary)
        {
            var beamsize = GetSecondaryBeamProfile(secondary);
            //var attribute = string.Concat(code, "_", beamsize);
            try
            {

                Tekla.Structures.Model.Connection C = new Tekla.Structures.Model.Connection();
                C.Name = "Test End Plate";
                C.Number = 141;
                C.LoadAttributesFromFile("standard");

                C.SetPrimaryObject(primary);
                C.SetSecondaryObject(secondary);

                if (!C.Insert())
                {

                    Console.WriteLine("Connection Insert failed ");
                    return false;
                }
                else
                {
                    Console.WriteLine(C.Identifier.ID);
                }

                return true;
            }
            catch (Exception x)
            {

                Console.WriteLine(x.GetBaseException().Message);
                return false;
            }

        }

        public bool CreateConnection(Beam primary, Beam secondary, string code)
        {
            var beamsize = GetSecondaryBeamProfile(secondary);
            //var attribute = string.Concat(code, "_", beamsize);
            try
            {

                Tekla.Structures.Model.Connection C = new Tekla.Structures.Model.Connection();
                C.Name = "Test End Plate";
                C.Number = 143;
                C.LoadAttributesFromFile("standard");
                //C.LoadAttributesFromFile(attribute);
                //C.UpVector = new Vector(0, 0, 1000);
                //C.PositionType = PositionTypeEnum.COLLISION_PLANE;

                C.SetPrimaryObject(primary);
                C.SetSecondaryObject(secondary);


                //C.SetAttribute("e2", 10.0);
                //C.SetAttribute("e1", 10.0);

                if (!C.Insert())
                {

                    Console.WriteLine("Connection Insert failed ");
                    return false;
                }
                else
                {
                    Console.WriteLine(C.Identifier.ID);
                }

                return true;
            }
            catch (Exception x)
            {

                Console.WriteLine(x.GetBaseException().Message);
                return false;
            }

        }
    }
}
