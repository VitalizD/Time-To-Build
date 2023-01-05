using Service;
using System;
using System.Collections;
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
        [SerializeField] private GameObject _properties;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private GameObject _zone;
        [SerializeField] private Image _zoneSquare;
        [SerializeField] private TextMeshProUGUI _zoneText;
        [SerializeField] private GameObject _bonusPrefab;
        [SerializeField] private GameObject _propertyPrefab;

        public static event Func<ZoneType, Color> GetZoneColor;
        public static event Func<ResourceType, Sprite> GetResourceIcon;

        public void Show(Vector2 position, string title, BonusInfo[] instantBonuses,
            PropertyInfo[] properties, string description, ZoneType zone)
        {
            transform.position = position;
            _title.text = title;

            _description.text = description;
            SetInstantBonuses(instantBonuses);
            SetProperties(properties);
            SetZone(zone);
            _panel.SetActive(true);

            // Костыль для корректного отображения 
            StartCoroutine(Reshow());
        }

        public void Hide()
        {
            RemoveContent(_properties.transform);
            RemoveContent(_instantsBonuses.transform);
            _panel.SetActive(false);
        }

        private IEnumerator Reshow()
        {
            yield return new WaitForEndOfFrame();
            _panel.SetActive(false);
            _panel.SetActive(true);
        }

        private void RemoveContent(Transform obj)
        {
            for (var i = obj.childCount - 1; i >= 0; --i)
                Destroy(obj.GetChild(i).gameObject);
        }

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
                bonus.transform.SetParent(_instantsBonuses.transform);
            }
        }

        private void SetProperties(PropertyInfo[] propertyInfos)
        {
            if (propertyInfos == null)
            {
                _properties.SetActive(false);
                return;
            }
            _properties.SetActive(true);
            foreach (var propertyInfo in propertyInfos)
            {
                var property = Instantiate(_propertyPrefab).GetComponent<PropertyParameters>();
                foreach (var bonusInfo in propertyInfo.Bonuses)
                {
                    var bonus = GetBonusObject(bonusInfo);
                    property.AddBonus(bonus.transform);
                }
                property.SetText(propertyInfo.ConditionText);
                property.transform.SetParent(_properties.transform);
            }
        }

        private BonusProperties GetBonusObject(BonusInfo info)
        {
            var bonus = Instantiate(_bonusPrefab).GetComponent<BonusProperties>();
            bonus.SetSprite(GetResourceIcon?.Invoke(info.Resource));
            var text = info.Value.ToString();
            bonus.SetText(info.Value > 0 ? '+' + text : text);
            if (info.Value < 0)
                bonus.SetRedColor();
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
