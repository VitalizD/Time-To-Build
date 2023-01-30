using UI.Counters.Population;
using UI.PopupWindows;
using UnityEngine;

namespace Gameplay.Cycle
{
    [RequireComponent(typeof(DayCycle))]
    public class DayCycleHandler : MonoBehaviour
    {
        private DayCycle _cycle;

        private void Awake()
        {
            _cycle = GetComponent<DayCycle>();
        }

        private void OnEnable()
        {
            PopupWindow.SetActiveCycle += _cycle.SetActiveTimer;
        }

        private void OnDisable()
        {
            PopupWindow.SetActiveCycle -= _cycle.SetActiveTimer;
        }

        private int GetDay() => _cycle.Day;
    }
}