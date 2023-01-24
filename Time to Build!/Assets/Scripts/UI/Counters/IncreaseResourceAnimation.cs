using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Counters
{
    [RequireComponent(typeof(Animator))]
    public class IncreaseResourceAnimation : MonoBehaviour
    {
        private const string INCREASE_ANIMATOR_BOOL = "Increase";
        private const string START_DECREASE_ANIMATOR_TRIGGER = "Start Decrease";
        private const string START_INCREASE_ANIMATOR_TRIGGER = "Start Increase";

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;
        [SerializeField] private Color _redColor;
        [SerializeField] private Color _greenColor;

        private Animator _animator;

        public void PlayIncrease(string text, Sprite icon = null) => Play(true, text, icon);

        public void PlayDecrease(string text, Sprite icon = null) => Play(false, text, icon);

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Play(bool value, string text, Sprite icon)
        {
            _text.text = text;
            if (_image != null)
                _image.sprite = icon;
            _text.color = value ? _greenColor: _redColor;
            gameObject.SetActive(true);
            _animator.SetBool(INCREASE_ANIMATOR_BOOL, value);
            _animator.SetTrigger(value ? START_INCREASE_ANIMATOR_TRIGGER : START_DECREASE_ANIMATOR_TRIGGER);
        }
    }
}