using System;
using UI;
using UnityEngine;

namespace Gameplay.Cycle
{
    [RequireComponent(typeof(Timer))]
    [RequireComponent(typeof(UIBar))]
    public class DayCycle : MonoBehaviour
    {
        [SerializeField] private float _cycleTime;
        [SerializeField] private int[] _populationsForReduction;

        private Timer _timer;
        private UIBar _bar;
        private int _dayNumber = 1;
        private int _reductionIndex = 0;
        private int _previousPopulationForReduction;

        public static event Func<int> GetIncome;
        public static event Func<int> GetReputation;
        public static event Func<int> GetPopulation;
        public static event Func<int> GetMoney;
        public static event Action<ResourceType, int> AddMoney;
        public static event Action<ResourceType, int> SetMoney;
        public static event Action<ResourceType, int> AddPopulation;
        public static event Action<ResourceType, int> AddIncome;
        public static event Action<ResourceType, int> AddReputation;
        public static event Action NewDay;

        private void Awake()
        {
            _timer = GetComponent<Timer>();
            _bar = GetComponent<UIBar>();
        }

        private void Start()
        {
            UpdateDayNumberText();
            RunCycle();
        }

        private void UpdateStates()
        {
            var income = GetIncome();
            var currentMoney = GetMoney();
            var population = GetReputation();
            var totalMoney = currentMoney + income;
            if (totalMoney < 0)
            {
                population += totalMoney;
                SetMoney?.Invoke(ResourceType.Money, 0);
            }
            else
                AddMoney?.Invoke(ResourceType.Money, income);
            AddPopulation?.Invoke(ResourceType.Population, population);
            var reductionValue = GetReductionValue(GetPopulation());
            AddIncome?.Invoke(ResourceType.Income, -reductionValue);
            AddReputation?.Invoke(ResourceType.Reputation, -reductionValue);

            ++_dayNumber;
            UpdateDayNumberText();
            NewDay?.Invoke();
            RunCycle();
        }

        private void RunCycle()
        {
            _timer.Run(_cycleTime, UpdateStates);
        }

        private void UpdateDayNumberText() => _bar.SetTitle($"Δενό {_dayNumber}");

        private int GetReductionValue(int population)
        {
            var reductionValue = 0;
            if (_reductionIndex < _populationsForReduction.Length)
            {
                while (population >= _populationsForReduction[_reductionIndex])
                {
                    ++reductionValue;
                    ++_reductionIndex;

                    if (_reductionIndex == _populationsForReduction.Length)
                    {
                        _previousPopulationForReduction = _populationsForReduction[_reductionIndex - 1] + 2;
                        return reductionValue;
                    }
                }
            }
            else
            {
                while (population >= _previousPopulationForReduction)
                {
                    ++reductionValue;
                    _previousPopulationForReduction += 2;
                }
            }
            return reductionValue;
        }
    }
}