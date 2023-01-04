using Service;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InformationWindow
{
    public class InfoWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private GameObject _instantsBonuses;
        [SerializeField] private GameObject _property;
        [SerializeField] private GameObject _propertyBonuses;
        [SerializeField] private TextMeshProUGUI _propertyText;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private GameObject _zone;
        [SerializeField] private Image _zoneSquare;
        [SerializeField] private TextMeshProUGUI _zoneText;
        [SerializeField] private GameObject _bonusPrefab;

        public static event Func<ZoneType, Color> GetZoneColor;

        public void Show(Vector2 position, string title, BonusInfo[] instantBonuses,
            PropertyInfo property, string description, ZoneType zone)
        {
            transform.position = position;
            _title.text = title;

            _description.text = description;
            SetInstantBonuses(instantBonuses);
            SetProperty(property);
            SetZone(zone);
            _panel.SetActive(true);
        }

        public void Hide() => _panel.SetActive(false);

        private void SetInstantBonuses(BonusInfo[] instantBonuses)
        {
            if (instantBonuses == null)
            {
                _instantsBonuses.SetActive(false);
                return;
            }
            _instantsBonuses.SetActive(true);
            foreach (var bonusInfo in instantBonuses)
            {
                var bonus = GetBonusObject(bonusInfo);
                bonus.transform.parent = _instantsBonuses.transform;
            }
        }

        private void SetProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                _property.SetActive(false);
                return;
            }
            _property.SetActive(true);
            foreach (var bonusInfo in propertyInfo.Bonuses)
            {
                var bonus = GetBonusObject(bonusInfo);
                bonus.transform.parent = _propertyBonuses.transform;
            }
            _propertyText.text = propertyInfo.Text;
        }

        private BonusProperties GetBonusObject(BonusInfo info)
        {
            var bonus = Instantiate(_bonusPrefab).GetComponent<BonusProperties>();
            bonus.SetSprite(info.Icon);
            bonus.SetText(info.Text);
            return bonus;
        }

        private void SetZone(ZoneType type)
        {
            if (type == ZoneType.None)
            {
                _zone.SetActive(false);
                return;
            }
            _zone.SetActive(true);
            var zoneColor = GetZoneColor?.Invoke(type);
            _zoneSquare.color = zoneColor.GetValueOrDefault();
            _zoneText.text = Translation.GetZoneName(type);
        }
    }
}
