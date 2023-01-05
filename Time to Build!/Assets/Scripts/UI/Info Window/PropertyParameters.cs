using TMPro;
using UnityEngine;

namespace UI.InformationWindow
{
    public class PropertyParameters : MonoBehaviour
    {
        [SerializeField] private Transform _bonuses;
        [SerializeField] private TextMeshProUGUI _text;

        public void AddBonus(Transform bonus) => bonus.SetParent(_bonuses);

        public void SetText(string text) => _text.text = text;
    }
}
