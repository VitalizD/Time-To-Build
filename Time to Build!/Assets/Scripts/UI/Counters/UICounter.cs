using Newtonsoft.Json.Linq;
using Service;
using Service.Sounds;
using System;
using TMPro;
using UI.InformationWindow;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Counters
{
    public class UICounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Transform _infoWindowPoint;
        [SerializeField] private IncreaseResourceAnimation _increaseResourceAnimation;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private int _value;
        [Space]
        [SerializeField] private bool _playSound;
        [SerializeField] private Sound _getValueSound;

        public ResourceType Type { get => _resourceType; }
        public int Count { get => _value; }

        public static event Action<Vector2, string, string> ShowInfoWindow;
        public static event Action HideInfoWindow;
        public static event Action UpdateBuildingLotColors;

        public void SetValue(int value)
        {
            if (_value + value > 0)
            {
                _increaseResourceAnimation.PlayIncrease($"+{value}");
                if (_playSound)
                    SoundManager.Instance.Play(_getValueSound, null);
            }
            else if (_value + value < 0)
                _increaseResourceAnimation.PlayDecrease(value.ToString());

            _value = value;
            _valueText.text = value.ToString();

            UpdateBuildingLotColors?.Invoke();
        }

        public void AddValue(int value)
        {
            _value += value;
            UpdateText();

            if (value > 0)
            {
                _increaseResourceAnimation.PlayIncrease($"+{value}");
                if (_playSound)
                    SoundManager.Instance.Play(_getValueSound, null);
            }
            else if (value < 0)
                _increaseResourceAnimation.PlayDecrease(value.ToString());

            UpdateBuildingLotColors?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var title = Translation.GetResourceName(_resourceType);
            var description = Translation.GetResourceDescription(_resourceType);
            ShowInfoWindow?.Invoke(_infoWindowPoint.position, title, description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideInfoWindow?.Invoke();
        }

        private void UpdateText()
        {
            _valueText.text = _value.ToString();
            _valueText.color = _value >= 0 ? Color.white : Color.red;
        }

        private void Start()
        {
            UpdateText();
        }
    }
}
