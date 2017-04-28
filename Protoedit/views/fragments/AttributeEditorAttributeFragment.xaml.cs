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

namespace Protoedit.views.fragments
{
    /// <summary>
    /// Interaktionslogik für AttributeEditorAttributeFragment.xaml
    /// </summary>
    public partial class AttributeEditorAttributeFragment : UserControl
    {

        /// <summary>
        /// true, if all information is complete
        /// </summary>
        public bool IsComplete { get; private set; }

        public AttributeEditorAttributeFragment(dynamic dataContext)
        {
            this.DataContext = dataContext;
            this.CheckInput();
            InitializeComponent();

        }

        private void OnTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.CheckInput();
        }

        private void OnNameChanged(object sender, TextChangedEventArgs e)
        {
            this.CheckInput();
        }

        private void CheckInput()
        {
            if (String.IsNullOrEmpty(((dynamic)this.DataContext).TypeName.Value) || String.IsNullOrEmpty(((dynamic)this.DataContext).AttributeName.Value))
            {
                this.Background = Brushes.Red;
                this.IsComplete = false;
            }
            else
            {
                this.Background = Brushes.Transparent;
                this.IsComplete = true;
            }
        }
    }
}
