using Gameplay.Cycle;
using UI.BuildingPanel;
using UnityEngine;

namespace UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class MoneyCounterHandler : MonoBehaviour
    {
        private UICounter _counter;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            BuildingLot.GetMoney += Get;
            DayCycle.GetMoney += Get;
        }

        private void OnDisable()
        {
            BuildingLot.GetMoney -= Get;
            DayCycle.GetMoney -= Get;
        }

        private int Get() => _counter.Count;
    }
}