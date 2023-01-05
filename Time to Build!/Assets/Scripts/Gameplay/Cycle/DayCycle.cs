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

        private Timer _timer;
        private UIBar _bar;
        private int _dayNumber = 1;

        public static event Func<int> GetIncome;
        public static event Func<int> GetReputation;
        public static event Action<ResourceType, int> AddMoney;
        public static event Action<ResourceType, int> AddPopulation;

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
            AddMoney?.Invoke(ResourceType.Money, GetIncome());
            AddPopulation?.Invoke(ResourceType.Population, GetReputation());
            ++_dayNumber;
            UpdateDayNumberText();
            RunCycle();
        }

        private void RunCycle()
        {
            _timer.Run(_cycleTime, UpdateStates);
        }

        private void UpdateDayNumberText() => _bar.SetTitle($"Δενό {_dayNumber}");
    }
}