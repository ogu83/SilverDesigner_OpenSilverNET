using SilverDesigner.DOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverDesigner
{
    public partial class ObjectTree : UserControl
    {
        private DocumentVM _myVM { get { return DataContext as DocumentVM; } }
        //private bool _selectByCode = false;
        private bool _selectByUser = false;

        public ObjectTree()
        {
            InitializeComponent();
            DataContextChanged += ObjectTree_DataContextChanged;
        }

        void ObjectTree_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldVM = (e.OldValue as DocumentVM);
            if (oldVM != null)
            {
                oldVM.AnObjectSelected -= OnObjectSelected;
                oldVM.SelectedAnyChildChanged += OnObjectSelected;
            }

            _myVM.AnObjectSelected += OnObjectSelected;
            _myVM.SelectedAnyChildChanged += OnObjectSelected;

            //mainList.ExpandAll();
        }

        private void OnObjectSelected(UIObjectVM sender, UIObjectVM selected)
        {
            if (_selectByUser)
            {
                _selectByUser = false;
                return;
            }

            //new Action(() =>
            //{
            //_selectByCode = true;
            //mainList.ExpandAll();
            mainList.SelectItem(selected);
            //}).StartAfter(TimeSpan.FromMilliseconds(50));
        }

        private void mainList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var myObj = (e.NewValue as UIObjectVM);
            if (myObj != null)
            {
                _selectByUser = true;

                if (_myVM.SelectedAnyChild != myObj)
                    myObj.SelectCommand();
            }
        }
    }
}