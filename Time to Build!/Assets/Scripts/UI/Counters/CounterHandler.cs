using Gameplay.Buildings;
using Gameplay.Cycle;
using Service.Sounds;
using UI.BuildingPanel;
using UnityEngine;

namespace UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class CounterHandler : MonoBehaviour
    {
        private UICounter _counter;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            BuildingPanel.BuildingPanel.AddMoney += AddResource;
            DayCycle.AddPopulation += AddResource;
            DayCycle.AddMoney += AddResource;
            DayCycle.SetMoney += SetResource;
            DayCycle.AddIncome += AddResource;
            DayCycle.AddReputation += AddResource;
            RewardsCalculator.AddResource += AddResource;
        }

        private void OnDisable()
        {
            BuildingPanel.BuildingPanel.AddMoney -= AddResource;
            DayCycle.AddPopulation -= AddResource;
            DayCycle.AddMoney -= AddResource;
            DayCycle.SetMoney -= SetResource;
            DayCycle.AddIncome -= AddResource;
            DayCycle.AddReputation -= AddResource;
            RewardsCalculator.AddResource -= AddResource;
        }

        private void AddResource(ResourceType type, int value)
        {
            if (type != _counter.Type)
                return;

            _counter.AddValue(value);
        }

        private void SetResource(ResourceType type, int value)
        {
            if (type != _counter.Type)
                return;

            _counter.SetValue(value);
        }
    }
}