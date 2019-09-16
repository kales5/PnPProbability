﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using PnPProbability.Model;
using System.Collections.Generic;

namespace PnPProbability.ViewModel
{
    public class SplittermondViewModel : IPnPViewModel
    {
        private int _eigenschaft1 = 2;
        private int _eigenschaft2 = 2;

        private int _difficulty = 20;
        private SplitterMondRollType _rollType = SplitterMondRollType.Normal;

        public int Eigentschaft1
        {
            get { return _eigenschaft1; }
            set
            {
                if (_eigenschaft1 != value)
                {
                    _eigenschaft1 = value;
                    CalcualteDicePropability();
                }
            }
        }

        public IList<SplitterMondRollType> RollTypes => Enum.GetValues(typeof(SplitterMondRollType)).Cast<SplitterMondRollType>().ToList();

        public int Eigentschaft2
        {
            get { return _eigenschaft2; }
            set
            {
                if (_eigenschaft2 != value)
                {
                    _eigenschaft2 = value;
                    CalcualteDicePropability();
                }
            }
        }

        public int Difficulty
        {
            get { return _difficulty; }
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    CalcualteDicePropability();
                }
            }
        }

        public SplitterMondRollType RollType
        {
            get { return _rollType; }
            set
            {
                if (_rollType != value)
                {
                    _rollType = value;
                    CalcualteDicePropability();
                }
            }
        }

        public int TawMax { get; set; } = 15;

        public ObservableCollection<TalentPropablityModel> OutputList { get; set; } = new ObservableCollection<TalentPropablityModel>();


        public void CalcualteDicePropability()
        {
            OutputList.Clear();
            for (int taW = 0; taW <= TawMax; taW++)
            {
                var talentPropablityModel = new TalentPropablityModel
                {
                    TaW = taW,
                    Propability = CountOfValidScores(taW)
                };

                OutputList.Add(talentPropablityModel);
            }
        }

        public decimal CountOfValidScores(int taw)
        {
            var effectiveTaw = taw + Eigentschaft1 + Eigentschaft2;
            switch (_rollType)
            {
                case SplitterMondRollType.Normal:
                    {
                        return CountOfValidScoresRoll(effectiveTaw, CombinationIsValidNormalRoll) / 100m;
                    }
                case SplitterMondRollType.Risk:
                    {
                        return CountOfValidScoresRiskRoll(effectiveTaw) / 10000m;
                    }
                case SplitterMondRollType.Cautious:
                    {
                        return CountOfValidScoresRoll(effectiveTaw, CombinationIsValidCautiousRoll) / 100m;
                    }
                default:
                    {
                        throw new NotSupportedException($"This {nameof(SplitterMondRollType)} is not Supported: {_rollType}");
                    }
            }
        }

        private decimal CountOfValidScoresRiskRoll(int taw)
        {
            int numberOfValid = 0;
            for (int w1 = 1; w1 <= 10; w1++)
            {
                for (int w2 = 1; w2 <= 10; w2++)
                {
                    for (int w3 = 1; w3 <= 10; w3++)
                    {
                        for (int w4 = 1; w4 <= 10; w4++)
                        {
                            if (CombinationIsValidRiskRoll(w1, w2, w3, w4, taw, _difficulty))
                                numberOfValid++;
                        }
                    }
                }
            }
            return numberOfValid;
        }

        private bool CombinationIsValidRiskRoll(int d1, int d2, int d3, int d4, int taw, int difficulty)
        {
            var dices = new[] { d1, d2, d3, d4 };

            int max1 = dices.Max();
            var dicesWithoutMax = dices.Where(d => d != max1).ToList();
            int max2 = dicesWithoutMax.Count == 3 ? dicesWithoutMax.Max() : max1;

            int min1 = dices.Min();
            var dicesWithoutMin = dices.Where(d => d != min1).ToList();
            int min2 = dicesWithoutMax.Count == 3 ? dicesWithoutMax.Min() : min1;

            if (max1 + max2 > 18)
                return true;
            if (min1 + min2 < 4)
                return false;

            return max1 + max2 + taw >= difficulty;
        }

        public int CountOfValidScoresRoll(int taw, Func<int, int, int, int, bool> combinationIsValid)
        {
            int numberOfValid = 0;
            for (int w1 = 1; w1 <= 10; w1++)
            {
                for (int w2 = 1; w2 <= 10; w2++)
                {
                    if (combinationIsValid(w1, w2, taw, _difficulty))
                        numberOfValid++;
                }
            }
            return numberOfValid;
        }

        private bool CombinationIsValidNormalRoll(int w1, int w2, int taw, int difficulty)
        {
            var ws = new[] { w1, w2 };
            if (w1 + w2 > 18)
                return true;
            if (w1 + w2 < 4)
                return false;

            return w1 + w2 + taw >= difficulty;
        }

        private bool CombinationIsValidCautiousRoll(int w1, int w2, int taw, int difficulty)
        {
            return Math.Max(w1, w2) + taw >= difficulty;
        }

        public void InitializeOutputList()
        {
            if (OutputList.Count == 0)
            {
                CalcualteDicePropability();
            }
        }
    }
}
