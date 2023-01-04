using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InformationWindow
{
    public class BonusProperties : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;

        public void SetText(string text) => _text.text = text;

        public void SetSprite(Sprite sprite) => _image.sprite = sprite;
    }
}