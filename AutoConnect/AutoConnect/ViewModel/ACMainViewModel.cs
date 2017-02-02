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
        
        //private bool _isChecked;
        //public bool IsChecked
        //{
        //    get { return _isChecked; }
        //    set { SetProperty(ref _isChecked, value); }

        //}

        private ObservableCollection<string> _componentTypes;
        public ObservableCollection<string> ComponentTypes
        {
            get { return _componentTypes; }
            set { SetProperty(ref _componentTypes, value); }
        }

        private ObservableCollection<ConnectionSetting> _beamToBeamWebCollection;
        public ObservableCollection<ConnectionSetting> BeamToBeamWebCollection
        {
            get { return _beamToBeamWebCollection; }
            set { SetProperty(ref _beamToBeamWebCollection, value); }
        }

        private ObservableCollection<ConnectionSetting> _beamToColumnWebCollection;
        public ObservableCollection<ConnectionSetting> BeamToColumnWebCollection
        {
            get { return _beamToColumnWebCollection; }
            set { SetProperty(ref _beamToColumnWebCollection, value); }
        }

        private ObservableCollection<ConnectionSetting> _beamToColumnFlangeCollection;
        public ObservableCollection<ConnectionSetting> BeamToColumnFlangeCollection
        {
            get { return _beamToColumnFlangeCollection; }
            set { SetProperty(ref _beamToColumnFlangeCollection, value); }
        }

        //Constructor
        public ACMainViewModel()
        {
            _title = "Autokek";
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
                    ApplyConnection(BeamToBeamWebCollection);
                    ApplyConnection(BeamToColumnWebCollection);
                    ApplyConnection(BeamToColumnFlangeCollection);
                });
            }
        }

        private void ApplyConnection(ObservableCollection<ConnectionSetting> collection)
        {

            if (collection.Count == 0 || collection == null) return; 
            var model = new Tekla.Structures.Model.Model();
            var modelname = model.GetInfo().ModelName;
            var jobcode = GetJobCodeOnProjectName(modelname);


            foreach (var item in collection)
            {
                if (item.IsChecked)
                {
                    var component = (int)Enum.Parse(typeof(ComponentType), item.Component);
                    var boltweld = (int)Enum.Parse(typeof(BoltWeldOrientation), item.BoltWeldOrientation);
                    var angletype = (int)Enum.Parse(typeof(AngleTypes), item.AngleType);

                    ConnectionChecker cnc = new ConnectionChecker();
                    if (item.IsSingleConnection) cnc.CreateConnection((Beam)item.PrimaryObject, (Beam)item.SecondaryObject, component, jobcode, boltweld, angletype);
                    if (!item.IsSingleConnection) cnc.CreateConnection((Beam)item.PrimaryObject, item.SecondaryObjects, component, jobcode, boltweld, angletype);
                }
            }
            
            model.CommitChanges();
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
                    if(source != null)
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
                        var ids = source.Replace(" >>> ", "|").Split('|');;
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

            //System.Windows.Window win = this.GetCurrentWindow();
            //_mainView = (ACMainView)win;

            BeamToBeamWebCollection = con.BeamToBeamWebColl;
            BeamToColumnWebCollection = con.BeamToColumnWebColl;
            BeamToColumnFlangeCollection = con.BeamToColumnFlangeColl;


        }



        private bool CheckVisibility(ObservableCollection<ConnectionSetting> collection)
        {
            return collection.Count > 0;
        }
        


    }
}
