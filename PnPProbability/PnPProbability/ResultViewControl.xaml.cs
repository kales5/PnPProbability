

using PnPProbability.Model;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PnPProbability
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultViewControl : StackLayout
    {
        public ResultViewControl()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty OutputListProperty = BindableProperty.Create(nameof(OutputList), typeof(ObservableCollection<TalentPropablityModel>), typeof(ResultViewControl));

        public ObservableCollection<TalentPropablityModel> OutputList
        {
            get
            {
                return (ObservableCollection<TalentPropablityModel>)GetValue(OutputListProperty);
            }
            set { SetValue(OutputListProperty, value); }
        }
    }
}