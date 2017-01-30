using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AutoConnect.BaseClass;
using AutoConnect.Model;
using AutoConnect.View;
using AutoConnect.View.UserControls;

namespace AutoConnect.ViewModel
{
    public class Temp
    {
        public string Nene { get; set; }
        public ObservableCollection<string> components { get; set; }
        public ObservableCollection<string> types { get; set; }
        public ObservableCollection<string> angles { get; set; }

        public Temp()
        {
            components = new ObservableCollection<string>()
            {
                "141","142","413","144"
            };

            types = new ObservableCollection<string>()
            {
                "Welded/Bolted","Bolted/Bolted","Bolted/Welded","Welded/Welded"
            };

            angles = new ObservableCollection<string>()
            {
                "Single Clip", "Double Clip"
            };
        }
    }
    
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
        

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ACMainView _mainView;


        private bool _visibility;

        public bool Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }

        }

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
                    BeamToBeamWebCollection = new ObservableCollection<ConnectionSetting>();
                    BeamToColumnWebCollection = new ObservableCollection<ConnectionSetting>();
                    BeamToColumnFlangeCollection = new ObservableCollection<ConnectionSetting>();
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
                    //Do some sh!t
                });
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

     
        #endregion

        public void CreateModelObjects()
        {

            ConnectionChecker con = new ConnectionChecker();
            con.Process();

            System.Windows.Window win = this.GetCurrentWindow();
            _mainView = (ACMainView)win;
            _mainView.b2bw.vm.BeamToBeamWebCollection = con.BeamToBeamWebColl;
            _mainView.b2cw.vm.BeamToColumnWebCollection = con.BeamToColumnWebColl;
            _mainView.b2cf.vm.BeamToColumnFlangeCollection = con.BeamToColumnFlangeColl;


            ComponentTypes = new ObservableCollection<string>()
            {
                "143","147","183"
            };


            //MyClass = con.BeamToColumnWebColl;
            //for (int i = 0; i < 4; i++)
            //{
            //UsrCtrlBeamToColumnWeb usrctrl = new UsrCtrlBeamToColumnWeb();
            //usrctrl.vm.MyClass = con.BeamToColumnWebColl;
            //_mainView.StackPopulate.Children.Add(usrctrl);
            //}


        }

        private bool CheckVisibility(ObservableCollection<ConnectionSetting> collection)
        {
            return collection.Count > 0 ? true : false;
        }
        


    }
}
