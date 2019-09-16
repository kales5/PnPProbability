using PnPProbability.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PnPProbability
{
	public partial class MainPage : TabbedPage
	{
		public MainPage()
		{
			InitializeComponent();

            this.CurrentPageChanged += LoadData;
		}

        private void LoadData(object sender, EventArgs e)
        {
            ((IPnPViewModel)CurrentPage.BindingContext).InitializeOutputList();
        }
    }
}
