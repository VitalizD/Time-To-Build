using Gameplay.Cycle;
using UI;
using UI.Counters.Population;
using UnityEngine;

namespace UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class PopulationCounterHandler : MonoBehaviour
    {
        private UICounter _counter;
        private PopulationRequirement _populationRequirement;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
            _populationRequirement = GetComponent<PopulationRequirement>();
        }

        private void OnEnable()
        {
            DayCycle.GetPopulation += Get;
            DayCycle.NewDay += _populationRequirement.IncreaseRequirement;
            CounterHandler.PopulationChanged += _populationRequirement.ChangeVisual;
        }

        private void OnDisable()
        {
            DayCycle.GetPopulation -= Get;
            DayCycle.NewDay -= _populationRequirement.IncreaseRequirement;
            CounterHandler.PopulationChanged -= _populationRequirement.ChangeVisual;
        }

        private int Get() => _counter.Count;
    }
}