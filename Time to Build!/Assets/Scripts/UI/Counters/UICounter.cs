using Newtonsoft.Json.Linq;
using Service;
using System;
using TMPro;
using UI.InformationWindow;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UICounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Transform _infoWindowPoint;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private int _value;

        public ResourceType Type { get => _resourceType; }
        public int Count { get => _value; }

        public static event Action<Vector2, string, BonusInfo[], PropertyInfo, string, ZoneType> ShowInfoWindow;
        public static event Action HideInfoWindow;

        public void SetValue(int value)
        {
            _value = value;
            _valueText.text = value.ToString();
        }

        public void AddValue(int value)
        {
            _value += value;
            UpdateText();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var title = Translation.GetResourceName(_resourceType);
            var description = Translation.GetResourceDescription(_resourceType);
            ShowInfoWindow?.Invoke(_infoWindowPoint.position, title, null, null, description, ZoneType.None);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideInfoWindow?.Invoke();
        }

        private void UpdateText() => _valueText.text = _value.ToString();

        private void Start()
        {
            UpdateText();
        }
    }
}
