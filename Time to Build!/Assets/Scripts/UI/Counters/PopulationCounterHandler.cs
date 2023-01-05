using Gameplay.Cycle;
using UI;
using UnityEngine;

namespace UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class PopulationCounterHandler : MonoBehaviour
    {
        private UICounter _counter;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            DayCycle.GetPopulation += Get;
        }

        private void OnDisable()
        {
            DayCycle.GetPopulation -= Get;
        }

        private int Get() => _counter.Count;
    }
}