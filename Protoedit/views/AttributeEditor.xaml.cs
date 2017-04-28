using Protoedit.helper;
using Protoedit.project;
using Protoedit.views.fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Protoedit.views
{
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class AttributeEditor : UserControl
    {
        /// <summary>
        /// true, when the data is complete and safe for processing
        /// </summary>
        public bool IsComplete { get; private set; }
    
        public AttributeEditor()
        {
            InitializeComponent();
        //    this.checkInput(); //is nochnet da im kosntruktor
        }

        private void AddAttributeClick(object sender, RoutedEventArgs e)
        {
            AttributeEditorAttributeFragment aeaf = new AttributeEditorAttributeFragment(((AttributeDefinition)this.DataContext).AddAttribute(this.spAttributes.Children.Count));
   
            this.spAttributes.Children.Add(aeaf);
        }


        private void OnNameChanged(object sender, TextChangedEventArgs e)
        {
            this.checkInput();
        }

        private void checkInput()
        {
            if (String.IsNullOrEmpty(((AttributeDefinition)this.DataContext).Name.Value))
            {
                this.tbName.Background = Brushes.Red;
                this.IsComplete = false;
            }
            else
            {
                this.tbName.Background = Brushes.Transparent;
                this.IsComplete = true;
            }
        }
    }
}
