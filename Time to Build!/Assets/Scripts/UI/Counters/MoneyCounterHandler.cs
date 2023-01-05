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
            BuildingPanel.BuildingPanel.GetMoney += Get;
            BuildingLot.GetMoney += Get;
        }

        private void OnDisable()
        {
            BuildingPanel.BuildingPanel.GetMoney -= Get;
            BuildingLot.GetMoney -= Get;
        }

        private int Get() => _counter.Count;
    }
}