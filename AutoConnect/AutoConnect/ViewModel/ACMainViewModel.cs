using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AutoConnect.BaseClass;
using AutoConnect.Converters;
using AutoConnect.Model;
using AutoConnect.View;
using AutoConnect.View.UserControls;
using Tekla.Structures;
using Tekla.Structures.Model;
using System.Collections.Generic;
using System.Linq;

namespace AutoConnect.ViewModel
{

    public class Data
    {
        public int PrimaryId { get; set; }
        public int SecondaryId { get; set; }
        public int[] SecondaryIds { get; set; }
        public object PrimaryObject { get; set; }
        public bool IsSingleConnection { get; set; }
        public object SecondaryObject { get; set; }
        public ArrayList SecondaryObjects { get; set; }
        public string ConnectionType { get; set; }
        public int Component { get; set; }
        public string AngleType { get; set; }

        public string ConnectingObjects { get; set; }
    }

    public class ACMainViewModel : BindableBase//, IDataErrorInfo
    {

        //Data Error Info
        #region DataErrorInfo
        //public string this[string columnName]
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        //public string Error
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        #endregion


        //public ACMainView _mainView;
        ArrayList Selection = new ArrayList();

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _visibility;
        public bool Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }

        }


        private int _objectSelected;
        public int ObjectsSelected
        {
            get { return _objectSelected; }
            set { SetProperty(ref _objectSelected, value); }

        }


        private bool _isChecked = true;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(ref _isChecked, value); }

        }

        private bool _included_BTBWS = true;
        public bool Included_BTBWS
        {
            get { return _included_BTBWS; }
            set { SetProperty(ref _included_BTBWS, value); }

        }

        private bool _included_BTBWD = true;
        public bool Included_BTBWD
        {
            get { return _included_BTBWD; }
            set { SetProperty(ref _included_BTBWD, value); }

        }

        private bool _included_BTCWS = true;
        public bool Included_BTCWS
        {
            get { return _included_BTCWS; }
            set { SetProperty(ref _included_BTCWS, value); }

        }

        private bool _included_BTCWD = true;
        public bool Included_BTCWD
        {
            get { return _included_BTCWD; }
            set { SetProperty(ref _included_BTCWD, value); }

        }

        private bool _included_BTCFS = true;
        public bool Included_BTCFS
        {
            get { return _included_BTCFS; }
            set { SetProperty(ref _included_BTCFS, value); }

        }


        //comboboxes
        #region Components
        private string _b2bWSingleComponentTypes;
        public string B2BWSingleComponentTypes
        {
            get { return _b2bWSingleComponentTypes; }
            set { SetProperty(ref _b2bWSingleComponentTypes, value); }
        }

        private string _b2bWMultiComponentTypes;
        public string B2BWMultiComponentTypes
        {
            get { return _b2bWMultiComponentTypes; }
            set { SetProperty(ref _b2bWMultiComponentTypes, value); }
        }

        private string _b2cWSingleComponentTypes;
        public string B2CWSingleComponentTypes
        {
            get { return _b2cWSingleComponentTypes; }
            set { SetProperty(ref _b2cWSingleComponentTypes, value); }
        }

        private string _b2cWMultiComponentTypes;
        public string B2CWMultiComponentTypes
        {
            get { return _b2cWMultiComponentTypes; }
            set { SetProperty(ref _b2cWMultiComponentTypes, value); }
        }

        private string _b2cfComponentTypes;
        public string B2CFComponentTypes
        {
            get { return _b2cfComponentTypes; }
            set { SetProperty(ref _b2cfComponentTypes, value); }
        }
        #endregion

        #region Bolt/Welds
        private string _b2bWSingleBoltWeldTypes;
        public string B2BWSingleBoltWeldTypes
        {
            get { return _b2bWSingleBoltWeldTypes; }
            set { SetProperty(ref _b2bWSingleBoltWeldTypes, value); }
        }

        private string _b2bWMultiBoltWeldTypes;
        public string B2BWMultiBoltWeldTypes
        {
            get { return _b2bWMultiBoltWeldTypes; }
            set { SetProperty(ref _b2bWMultiBoltWeldTypes, value); }
        }

        private string _b2cWSingleBoltWeldTypes;
        public string B2CWSingleBoltWeldTypes
        {
            get { return _b2cWSingleBoltWeldTypes; }
            set { SetProperty(ref _b2cWSingleBoltWeldTypes, value); }
        }

        private string _b2cWMultiBoltWeldTypes;
        public string B2CWMultiBoltWeldTypes
        {
            get { return _b2cWMultiBoltWeldTypes; }
            set { SetProperty(ref _b2cWMultiBoltWeldTypes, value); }
        }

        private string _b2cfBoltWeldTypes;
        public string B2CFBoltWeldTypes
        {
            get { return _b2cfBoltWeldTypes; }
            set { SetProperty(ref _b2cfBoltWeldTypes, value); }
        }
        #endregion

        #region AngleTypes
        private string _b2bWSingleAngleTypes;
        public string B2BWSingleAngleTypes
        {
            get { return _b2bWSingleAngleTypes; }
            set { SetProperty(ref _b2bWSingleAngleTypes, value); }
        }

        private string _b2bWMultiAngleTypes;
        public string B2BWMultiAngleTypes
        {
            get { return _b2bWMultiAngleTypes; }
            set { SetProperty(ref _b2bWMultiAngleTypes, value); }
        }

        private string _b2cWSingleAngleTypes;
        public string B2CWSingleAngleTypes
        {
            get { return _b2cWSingleAngleTypes; }
            set { SetProperty(ref _b2cWSingleAngleTypes, value); }
        }

        private string _b2cWMultiAngleTypes;
        public string B2CWMultiAngleTypes
        {
            get { return _b2cWMultiAngleTypes; }
            set { SetProperty(ref _b2cWMultiAngleTypes, value); }
        }

        private string _b2cfAngleTypes;
        public string B2CFAngleTypes
        {
            get { return _b2cfAngleTypes; }
            set { SetProperty(ref _b2cfAngleTypes, value); }
        }
    
        #endregion

        //private string _componentTypes;
        //public string ComponentTypes
        //{
        //    get { return _componentTypes; }
        //    set { SetProperty(ref _componentTypes, value); }
        //}

        private ObservableCollection<ConnectionSetting> _beamToBeamWebCollection;
        public ObservableCollection<ConnectionSetting> BeamToBeamWebCollection
        {
            get { return _beamToBeamWebCollection; }
            set { SetProperty(ref _beamToBeamWebCollection, value); }
        }

        private ObservableCollection<ConnectionSetting> _beamToBeamWebDoubleCollection;
        public ObservableCollection<ConnectionSetting> BeamToBeamWebDoubleCollection
        {
            get { return _beamToBeamWebDoubleCollection; }
            set { SetProperty(ref _beamToBeamWebDoubleCollection, value); }
        }

        private ObservableCollection<ConnectionSetting> _beamToColumnWebCollection;
        public ObservableCollection<ConnectionSetting> BeamToColumnWebCollection
        {
            get { return _beamToColumnWebCollection; }
            set { SetProperty(ref _beamToColumnWebCollection, value); }
        }

        private ObservableCollection<ConnectionSetting> _beamToColumnWebDoubleCollection;
        public ObservableCollection<ConnectionSetting> BeamToColumnWebDoubleCollection
        {
            get { return _beamToColumnWebDoubleCollection; }
            set { SetProperty(ref _beamToColumnWebDoubleCollection, value); }
        }

        private ObservableCollection<ConnectionSetting> _beamToColumnFlangeCollection;
        //private ACMainView _mainView;

        public ObservableCollection<ConnectionSetting> BeamToColumnFlangeCollection
        {
            get { return _beamToColumnFlangeCollection; }
            set { SetProperty(ref _beamToColumnFlangeCollection, value); }
        }

        //Constructor
        public ACMainViewModel()
        {
            _title = "Auto Connection";
        }

        #region Commands

        public ICommand Analyze
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //Do some sh!t
                    Selection = new ArrayList();
                    CreateModelObjects();
                   
                });
            }
        }

        public ICommand Apply
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    FilterConnection();
                    if (Included_BTBWD && BeamToBeamWebDoubleCollection.Count > 0) { ApplyConnection(BeamToBeamWebDoubleCollection, B2BWMultiComponentTypes, B2BWMultiBoltWeldTypes, B2BWMultiAngleTypes); }

                    if (Included_BTBWS && BeamToBeamWebCollection.Count > 0) { ApplyConnection(BeamToBeamWebCollection, B2BWSingleComponentTypes, B2BWSingleBoltWeldTypes, B2BWSingleAngleTypes); }

                    if (Included_BTCWD && BeamToColumnWebDoubleCollection.Count > 0) { ApplyConnection(BeamToColumnWebDoubleCollection,B2CWMultiComponentTypes, B2CWMultiBoltWeldTypes, B2CWMultiAngleTypes); }

                    if (Included_BTCWS && BeamToColumnWebCollection.Count > 0) { ApplyConnection(BeamToColumnWebCollection, B2CWSingleComponentTypes, B2CWSingleBoltWeldTypes, B2CWSingleAngleTypes); }

                    if (Included_BTCFS && BeamToColumnFlangeCollection.Count > 0) { ApplyConnection(BeamToColumnFlangeCollection, B2CFComponentTypes, B2CFBoltWeldTypes, B2CFAngleTypes); }

                });
            }
        }

        private void ApplyConnection(ObservableCollection<ConnectionSetting> collection, string component, string boltweldtype, string angletype)
        {

            if (collection.Count == 0 || collection == null) return;
            var model = new Tekla.Structures.Model.Model();
            var modelname = model.GetInfo().ModelName;
            var jobcode = GetJobCodeOnProjectName(modelname);


            foreach (var item in collection)
            {
                if (item.IsChecked)
                {
                    var EnumComponent = (int)Enum.Parse(typeof(ComponentType), component);
                    var EnumBoltweldtype = (int)Enum.Parse(typeof(BoltWeldOrientation), boltweldtype);
                    var EnumAngletype = (int)Enum.Parse(typeof(AngleTypes), angletype);

                    ConnectionChecker cnc = new ConnectionChecker();
                    if (item.IsSingleConnection) cnc.CreateConnection((Beam)item.PrimaryObject, (Beam)item.SecondaryObject, EnumComponent, jobcode, EnumBoltweldtype, EnumAngletype);
                    if (!item.IsSingleConnection) cnc.CreateConnection((Beam)item.PrimaryObject, item.SecondaryObjects, EnumComponent, jobcode, EnumBoltweldtype, EnumAngletype);
                }
            }

            model.CommitChanges();
        }

        private void FilterConnection()
        {
            if(Included_BTBWD && Included_BTBWS)
            {
                foreach (var item in BeamToBeamWebDoubleCollection)
                {
                   BeamToBeamWebCollection.Where(x => x.PrimaryId == item.PrimaryId && (x.SecondaryId == item.SecondaryIds[0] || x.SecondaryId == item.SecondaryIds[1])).Select(c => { c.IsChecked = false; return c; }).ToList();                                        
                }
            }

            if (!Included_BTBWD && Included_BTBWS)
            {
                foreach (var item in BeamToBeamWebDoubleCollection)
                {
                    BeamToBeamWebCollection.Where(x => x.PrimaryId == item.PrimaryId && (x.SecondaryId == item.SecondaryIds[0] || x.SecondaryId == item.SecondaryIds[1])).Select(c => { c.IsChecked = true; return c; }).ToList();
                }
            }

            if (Included_BTCWD && Included_BTCWS)
            {
                foreach (var item in BeamToColumnWebDoubleCollection)
                {
                    BeamToColumnWebCollection.Where(x => x.PrimaryId == item.PrimaryId && (x.SecondaryId == item.SecondaryIds[0] || x.SecondaryId == item.SecondaryIds[1])).Select(c => { c.IsChecked = false; return c; }).ToList();
                }
            }

            if (!Included_BTCWD && Included_BTCWS)
            {
                foreach (var item in BeamToColumnWebDoubleCollection)
                {
                    BeamToColumnWebCollection.Where(x => x.PrimaryId == item.PrimaryId && (x.SecondaryId == item.SecondaryIds[0] || x.SecondaryId == item.SecondaryIds[1])).Select(c => { c.IsChecked = true; return c; }).ToList();
                }
            }


            if ((Included_BTCWD || Included_BTCWS) && Included_BTBWS)
            {
                foreach (var item in BeamToColumnWebCollection)
                {
                    var secs = BeamToColumnFlangeCollection.Where(x => x.PrimaryId == item.PrimaryId).Select(b => b).ToList();
                    foreach (var sec in secs)
                    {
                        BeamToBeamWebCollection.Where(x => (x.PrimaryId == item.SecondaryId && x.SecondaryId == sec.SecondaryId) || (x.PrimaryId == sec.SecondaryId && x.SecondaryId == item.SecondaryId)).Select(c => { c.IsChecked = false; return c; }).ToList();
                    }
                }
            }

            if (!(Included_BTCWD || Included_BTCWS) && Included_BTBWS)
            {
                foreach (var item in BeamToColumnWebCollection)
                {
                    var secs = BeamToColumnFlangeCollection.Where(x => x.PrimaryId == item.PrimaryId).Select(b => b).ToList();
                    foreach (var sec in secs)
                    {
                        BeamToBeamWebCollection.Where(x => (x.PrimaryId == item.SecondaryId && x.SecondaryId == sec.SecondaryId) || (x.PrimaryId == sec.SecondaryId && x.SecondaryId == item.SecondaryId)).Select(c => { c.IsChecked = true; return c; }).ToList();
                    }
                }
            }

        }


        private string GetJobCodeOnProjectName(string modelname)
        {
            try
            {
                var split = modelname.Split('_');
                return split[1].ToString();
            }
            catch (Exception)
            {
                throw new ArgumentException("Model name is not on the right format. (Job #_Jobcode_Fabricator_TeklaVersionUsImp)");
            }
        }

        public ICommand Exit
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //Do some sh!t
                });
            }
        }

        public ICommand Settings
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //Do some sh!t
                });
            }
        }


        public ICommand CheckBox_CheckChanged
        {
            get
            {
                return new DelegateCommand((Text) =>
                {
                    //var model = new Tekla.Structures.Model.Model();
                    Tekla.Structures.Model.UI.ModelObjectSelector Selector = new Tekla.Structures.Model.UI.ModelObjectSelector();

                    var source = Text as string;
                    if (source != null)
                    {

                        var ids = source.Replace(" >>> ", "|").Split('|');
                        //ArrayList arr = new ArrayList();

                        foreach (var a in ids)
                        {
                            Beam b = new Beam
                            {
                                Identifier = { ID = Convert.ToInt32(a) }
                            };

                            Selection.Add(b);                                //arr.Add(b);
                        }

                        Selector.Select(Selection);

                    }
                });
            }
        }

        public ICommand CheckBox_UnChecked
        {
            get
            {
                return new DelegateCommand((Text) =>
                {
                    Tekla.Structures.Model.UI.ModelObjectSelector Selector = new Tekla.Structures.Model.UI.ModelObjectSelector();

                    var source = Text as string;
                    if (source != null)
                    {
                        var ids = source.Replace(" >>> ", "|").Split('|'); ;
                        List<Beam> results = Selection.Cast<Beam>().ToList();

                        foreach (var a in ids)
                        {
                            Beam b = new Beam
                            {
                                Identifier = { ID = Convert.ToInt32(a) }
                            };

                            var rem = results.Where(x => x.Identifier.ID == b.Identifier.ID).Select(y => y).ToList();
                            results.Remove(rem[0]);
                        }

                        Selection = new ArrayList(results);
                        Selector.Select(Selection);
                    }
                });
            }
        }


        #endregion


        public void CreateModelObjects()
        {

            ConnectionChecker con = new ConnectionChecker();
            con.Process();

            BeamToBeamWebCollection = con.BeamToBeamWebColl;
            BeamToColumnWebCollection = con.BeamToColumnWebColl;
            BeamToColumnFlangeCollection = con.BeamToColumnFlangeColl;
            //two-way connections
            BeamToBeamWebDoubleCollection = con.BeamToBeamWebDoubleColl;
            BeamToColumnWebDoubleCollection = con.BeamToColumnWebDoubleColl;

            //counts
            B2BW_SingleCollectionCount = BeamToBeamWebCollection.Count;
            B2BW_MultiCollectionCount = BeamToBeamWebDoubleCollection.Count;

            B2CW_SingleCollectionCount = BeamToColumnWebCollection.Count;
            B2CW_MultiCollectionCount = BeamToColumnWebDoubleCollection.Count;

            B2CF_CollectionCount = BeamToColumnFlangeCollection.Count;


            //System.Windows.Window win = this.GetCurrentWindow();
            //_mainView = (ACMainView)win;

            //class = con.BeamToColumnWebColl;
            //for (int i = 0; i < 1; i++)
            //{
            //    FrameOrientation frameorientation = new FrameOrientation();
            //    frameorientation.vm.BeamToBeamWebCollection = BeamToBeamWebCollection;
            //    _mainView.StackPopulate.Children.Add(frameorientation);
            //}

        }


        private int _b2bw_SingleCollectionCount;
        public int B2BW_SingleCollectionCount
        {
            get { return _b2bw_SingleCollectionCount; }
            set { SetProperty(ref _b2bw_SingleCollectionCount, value); B2BWSingle_IsCollapse = this.B2BW_SingleCollectionCount > 0 ? "Visibile" : "Collapsed"; }
        }

        private int _b2bw_MultiCollectionCount;
        public int B2BW_MultiCollectionCount
        {
            get { return _b2bw_MultiCollectionCount; }
            set { SetProperty(ref _b2bw_MultiCollectionCount, value); B2BWMulti_IsCollapse = this.B2BW_MultiCollectionCount > 0 ? "Visibile" : "Collapsed"; }
        }

        private int _b2cw_SingleCollectionCount;
        public int B2CW_SingleCollectionCount
        {
            get { return _b2cw_SingleCollectionCount; }
            set { SetProperty(ref _b2cw_SingleCollectionCount, value); B2CWSingle_IsCollapse = this.B2CW_SingleCollectionCount > 0 ? "Visibile" : "Collapsed"; }
        }

        private int _b2cw_MultiCollectionCount;
        public int B2CW_MultiCollectionCount
        {
            get { return _b2cw_MultiCollectionCount; }
            set { SetProperty(ref _b2cw_MultiCollectionCount, value); B2CWMulti_IsCollapse = this.B2CW_MultiCollectionCount > 0 ? "Visibile" : "Collapsed"; }
        }

        private int _b2cf_CollectionCount;
        public int B2CF_CollectionCount
        {
            get { return _b2cf_CollectionCount; }
            set { SetProperty(ref _b2cf_CollectionCount, value); B2CF_IsCollapsed = this.B2CF_CollectionCount > 0 ? "Visibile" : "Collapsed"; }
        }

        //visibility
        private string _b2bwSingle_isCollapse = "Collapsed";
        public string B2BWSingle_IsCollapse
        {
            get { return _b2bwSingle_isCollapse; }
            set { SetProperty(ref _b2bwSingle_isCollapse, value); }
        }

        private string _b2bwMulti_isCollapse = "Collapsed";
        public string B2BWMulti_IsCollapse
        {
            get { return _b2bwMulti_isCollapse; }
            set { SetProperty(ref _b2bwMulti_isCollapse, value); }
        }

        private string _b2cwSingle_isCollapse = "Collapsed";
        public string B2CWSingle_IsCollapse
        {
            get { return _b2cwSingle_isCollapse; }
            set { SetProperty(ref _b2cwSingle_isCollapse, value); }
        }

        private string _b2cwMulti_isCollapse = "Collapsed";
        public string B2CWMulti_IsCollapse
        {
            get { return _b2cwMulti_isCollapse; }
            set { SetProperty(ref _b2cwMulti_isCollapse, value); }
        }

        private string _b2cf_isCollapse = "Collapsed";
        public string B2CF_IsCollapsed
        { 
            get { return _b2cf_isCollapse; }
            set { SetProperty(ref _b2cf_isCollapse, value); }
        }
    }
}
