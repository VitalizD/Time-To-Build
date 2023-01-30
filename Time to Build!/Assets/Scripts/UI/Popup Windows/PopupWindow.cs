using Service.Sounds;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.PopupWindows
{
    public class PopupWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _window;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;

        private UnityAction _extraOnClick;

        public static event Action<bool> SetActiveCycle;

        public void Show(string title, string description, string buttonText, UnityAction onClick)
        {
            _title.text = title;
            _description.text = description;
            _buttonText.text = buttonText;
            if (onClick != null)
            {
                _extraOnClick = onClick;
                _button.onClick.AddListener(onClick);
            }
            _window.SetActive(true);
            SetActiveCycle?.Invoke(false);
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SoundManager.Instance.Play(Sound.Click, null);
            if (_extraOnClick != null)
                _button.onClick.RemoveListener(_extraOnClick);
            _extraOnClick = null;
            _window.SetActive(false);
            SetActiveCycle?.Invoke(true);
        }
    }
}