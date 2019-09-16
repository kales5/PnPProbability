using PnPProbability.Model;
using System;
using System.Collections.ObjectModel;

namespace PnPProbability.ViewModel
{
    public class DeadlandsViewModel : IPnPViewModel
    {
        private const int _wildCardValue = 6;

        private bool _hasWildCard;
        private int _talent;

        public bool HasWildCard
        {
            get => _hasWildCard;
            set
            {
                if (_hasWildCard != value)
                {
                    _hasWildCard = value;
                    CalcualteDicePropability();
                }
            }
        }

        public int Talent
        {
            get => _talent;
            set
            {
                if (_talent != value)
                {
                    _talent = value;
                    CalcualteDicePropability();
                }
            }
        }

        public int[] TalentValues => new[] { 4, 6, 8, 10, 12 };


        public int TawMax { get; set; } = 15;

        public DeadlandsViewModel()
        {
            _talent = 6;
            _hasWildCard = true;
            CalcualteDicePropability();
        }

        public ObservableCollection<TalentPropablityModel> OutputList { get; set; } = new ObservableCollection<TalentPropablityModel>();

        private void CalcualteDicePropability()
        {
            OutputList.Clear();
            OutputList.Add(GetTawOnePropability());
            for (int taW = 2; taW <= TawMax; taW++)
            {
                var talentPropablityModel = new TalentPropablityModel
                {
                    TaW = taW,
                    Propability = CalculatePropability(taW)
                };

                OutputList.Add(talentPropablityModel);
            }

        }

        private TalentPropablityModel GetTawOnePropability()
        {
            if (HasWildCard)
            {
                return new TalentPropablityModel
                {
                    TaW = 1,
                    Propability = 1m - ((decimal)(1m / Talent) * (1m / 6m))
                };
            }
            else
            {
                return new TalentPropablityModel
                {
                    TaW = 1,
                    Propability = (decimal)(Talent - 1) / Talent
                };
            }
        }


        private decimal CalculatePropability(int taW)
        {
            if (HasWildCard)
            {
                return CalculatePropability_WithWildCard(taW);
            }
            else
            {
                return CalculatePropability_NoWildCard(taW);
            }
        }

        private decimal CalculatePropability_NoWildCard(int taW)
        {
            if (taW <= Talent)
            {
                return (decimal)(Talent - taW + 1) / Talent;
            }
            else
            {
                return 1 / (decimal)Talent * (CalculatePropability_NoWildCard(taW - Talent));
            }
        }

        private decimal CalculatePropability_WithWildCard(int taW)
        {
            var resultTalentDice = CalculatePropability_NoWildCard(taW);
            return resultTalentDice + (1 - resultTalentDice) * CalculatePropability_NoWildCard(_wildCardValue);
        }

        public void InitializeOutputList()
        {
            if(OutputList.Count == 0)
            {
                CalcualteDicePropability();
            }
        }
    }
}
