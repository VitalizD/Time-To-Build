using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI.Counters.Population
{
    public class PopulationRequirement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _populationValueText;
        [SerializeField] private TextMeshProUGUI _populationNeedText;
        [SerializeField] private Color _positiveColor;
        [SerializeField] private Color _negativeColor;
        [SerializeField] private PopulationRequirementsInfo[] _requirements;

        private readonly Dictionary<int, int[]> _requirementsDict = new();
        private Animation _populationValueAnimation;
        private UICounter _populationCounter;
        private int[] _currentRequirementSteps = new[] { 1 };
        private int _currentRequirementIndex = 0;
        private int _populationNeed = 1;
        private bool _lossIsClose = false;

        public static event Action Lost;
        public static event Action<string, string, string, UnityAction> ShowPopupWindow;

        public void IncreaseRequirement(int day)
        {
            _lossIsClose = _populationCounter.Count < _populationNeed;

            CheckNewRequirement(day);
            _populationNeed += _currentRequirementSteps[_currentRequirementIndex];
            UpdatePopulationNeedText();
            ++_currentRequirementIndex;
            if (_currentRequirementIndex >= _currentRequirementSteps.Length)
                _currentRequirementIndex = 0;
            ChangeVisual();
            CheckLost();
        }

        public void ChangeVisual()
        {
            if (_populationCounter.Count < _populationNeed)
            {
                _populationValueText.color = _negativeColor;
                _populationValueAnimation.Play();
            }
            else
            {
                if (_populationCounter.Count > _populationNeed)
                    _populationValueText.color = _positiveColor;
                else if (_populationCounter.Count == _populationNeed)
                    _populationValueText.color = Color.white;
                _populationValueAnimation.Stop();
                _populationValueText.transform.localScale = Vector3.one;
            }
        }

        private void Awake()
        {
            _populationValueAnimation = _populationValueText.GetComponent<Animation>();
            _populationCounter = GetComponent<UICounter>();
            foreach (var requirement in _requirements)
                _requirementsDict.Add(requirement.Day, requirement.RequirementSteps);
        }

        private void Start()
        {
            CheckNewRequirement(1);
            UpdatePopulationNeedText();
            ChangeVisual();
        }

        private void CheckNewRequirement(int day)
        {
            if (_requirementsDict.ContainsKey(day))
            {
                _currentRequirementSteps = _requirementsDict[day];
                _currentRequirementIndex = 0;
            }
        }

        private void UpdatePopulationNeedText() => _populationNeedText.text = $"/{_populationNeed}";

        private void CheckLost()
        {
            if (_lossIsClose)
            {
                Lost?.Invoke();
                ShowPopupWindow?.Invoke("Неудача", "К сожалению, вам не удалось привлечь достаточно населения, " +
                    "чтобы сделать город успешным.\n\n<color=red>Город утерян.", "Новая игра", 
                    () => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
            }
            else
                _lossIsClose = true;
        }
    }
}
