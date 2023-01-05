using Gameplay.Cycle;
using UI;
using UnityEngine;

namespace UI.Counters
{
    [RequireComponent(typeof(UICounter))]
    public class ReputationCounterHandler : MonoBehaviour
    {
        private UICounter _counter;

        private void Awake()
        {
            _counter = GetComponent<UICounter>();
        }

        private void OnEnable()
        {
            DayCycle.GetReputation += Get;
        }

        private void OnDisable()
        {
            DayCycle.GetReputation -= Get;
        }

        private int Get() => _counter.Count;
    }
}
