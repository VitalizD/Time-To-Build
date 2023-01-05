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

        }

        private void OnDisable()
        {

        }

        private int Get() => _counter.Count;
    }
}