using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private UIBar _progressBar;

        private Action _execute;
        private float _remainSeconds;

        public bool Activated { get; private set; } = false;

        public void Run(float seconds, Action execute)
        {
            _remainSeconds = seconds;
            _execute = execute;
            Activated = true;
            _progressBar.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!Activated)
                return;

            _progressBar.AddValue(Time.deltaTime / _remainSeconds);
            if (_progressBar.Filled)
                Stop();
        }

        private void Stop()
        {
            Activated = false;
            _progressBar.SetValue(0f);
            _progressBar.gameObject.SetActive(false);
            _execute?.Invoke();
        }
    }
}