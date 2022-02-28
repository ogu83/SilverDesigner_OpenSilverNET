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
    public partial class IDE : UserControl
    {
        private IDEVM _myVM;

        public IDE()
        {
            loadDummyObjects();

            InitializeComponent();
            this.DataContext = _myVM = new IDEVM();
            _myVM.ShowNewDocumentDialog += _myVM_ShowNewDocumentDialog;
            _myVM.ShowResourcesDialog += _myVM_ShowResourcesDialog;
        }

        private void _myVM_ShowResourcesDialog(object sender, EventArgs e)
        {
            ResourcesWindow d = new ResourcesWindow(_myVM.EditArea.Document.Resources);
            d.Show();
        }

        private void _myVM_ShowNewDocumentDialog(IDEVM sender, DOM.DocumentVM document)
        {
            NewDocument newDocDialog = new NewDocument() { DataContext = document };
            newDocDialog.Closed += (object sender1, EventArgs e1) =>
            {
                if (newDocDialog.DialogResult.GetValueOrDefault(false))
                    _myVM.CreateDocument(document);
            };
            newDocDialog.Show();
        }

        private void loadDummyObjects()
        {

        }
    }
}
