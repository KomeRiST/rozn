using DevExpress.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace rozn
{
    class ViewModel:ViewModelBase
    {
        ApplicationContext db;
        public int Clicks { get; set; }
        public IEnumerable<tovar> Tovary { get; set; }


        public ICommand AddClick {
            get {
                return new DelegateCommand(() => { Clicks++; });
            }
        }


        public ViewModel()
        {
            //db = new ApplicationContext();
            //db.Tovars.Load();
            //Tovary = db.Tovars.Local.ToBindingList();
        }



    }
}
