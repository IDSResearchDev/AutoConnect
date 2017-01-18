using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoConnect.BaseClass;
using AutoConnect.View;
using AutoConnect.View.UserControls;

namespace AutoConnect.ViewModel
{
    public class ACMainViewModel : BindableBase, IDataErrorInfo
    {

        //Data Error Info
        #region DataErrorInfo
        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

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

        public ACMainView _mainView;

        public void CreateModelObjects()
        {
            for (int i = 0; i < 3; i++)
            {
                var usrctrl = new FrameOrientations();
                System.Windows.Window win = this.GetCurrentWindow();
                _mainView = (ACMainView)win;
                _mainView.StackPopulate.Children.Add(usrctrl);
            }
        }
        

    }
}
