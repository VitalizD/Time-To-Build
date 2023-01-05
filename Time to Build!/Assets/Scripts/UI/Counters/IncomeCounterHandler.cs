using Gameplay.Cycle;
using UI;
using UnityEngine;

namespace UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class IncomeCounterHandler : MonoBehaviour
    {
        private UICounter _counter;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            DayCycle.GetIncome += Get;
        }

        private void OnDisable()
        {
            DayCycle.GetIncome -= Get;
        }

        private int Get() => _counter.Count;
    }

}