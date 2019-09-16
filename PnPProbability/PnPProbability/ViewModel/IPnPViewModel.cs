using PnPProbability.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PnPProbability.ViewModel
{
    public interface IPnPViewModel
    {
        ObservableCollection<TalentPropablityModel> OutputList { get; set; }

        void InitializeOutputList();
    }
}
