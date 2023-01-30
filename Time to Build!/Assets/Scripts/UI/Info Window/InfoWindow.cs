using Service;
using Service.BuildingStorage;
using System;
using System.Collections;
using System.Linq;
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
        [SerializeField] private GameObject _categories;
        [SerializeField] private GameObject _bonusPrefab;
        [SerializeField] private GameObject _propertyPrefab;
        [SerializeField] private GameObject _categoryPrefab;
        [Space]
        [SerializeField] private GameObject _lineInstantsBonuses;
        [SerializeField] private GameObject _lineProperty;
        [SerializeField] private GameObject _lineZone;
        [SerializeField] private GameObject _catogoriesLine;

        public static event Func<ZoneType, Color> GetZoneColor;
        public static event Func<ResourceType, Sprite> GetResourceIcon;
        public static event Func<BuildingCategory, Sprite> GetCategoryIcon;
        public static event Func<BuildingType, Building> GetBuilding;

        public void Show(Vector2 position, BuildingType buildingType, bool noReserve, bool soonDisappear, int markup)
        {
            var building = GetBuilding?.Invoke(buildingType);
            var instantBonuses = building.InstantBonuses.Length == 0 ? null : building.InstantBonuses;
            var propertyInfos = building.Properties.Length == 0 ? null
                : building.Properties.Select(property => new PropertyInfo(property.Bonuses,
                Translation.GetPropertyDescription(property.Type, property.Zones, property.Categories))).ToArray();

            var description = Translation.GetBuildingDescription(buildingType);
            if (noReserve)
                description += "\n\n<color=red>нет в запасе</color>";
            if (markup > 0)
                description += $"\n\n<color=red>наценка +{markup}</color>";
            if (soonDisappear)
                description += "\n\n<color=red>скоро исчезнет с рынка</color>";
            if (description != null)
                description = description.TrimStart('\n');

            Show(position, Translation.GetBuildingName(buildingType), building.InstantBonuses, propertyInfos,
                description, building.Zone, building.Categories);
        }

        public void Show(Vector2 position, string title, string description)
        {
            Show(position, title, null, null, description, ZoneType.None, null);
        }

        public void Show(Vector2 position, string title, BonusInfo[] instantBonuses,
            PropertyInfo[] properties, string description, ZoneType zone, BuildingCategory[] categories)
        {
            transform.position = position;
            _title.text = title;

            _description.text = description;
            SetInstantBonuses(instantBonuses);
            SetProperties(properties);
            SetZone(zone);
            SetCategories(categories);
            _lineInstantsBonuses.SetActive(instantBonuses != null && instantBonuses.Length > 0);
            _lineProperty.SetActive((properties != null && properties.Length > 0) || (description != null && description != ""));
            _lineZone.SetActive(zone != ZoneType.None);
            _catogoriesLine.SetActive(categories != null && categories.Length > 0);
            _panel.SetActive(true);

            //  остыль дл€ корректного отображени€ 
            StartCoroutine(Reshow());
        }

        public void Hide()
        {
            RemoveContent(_properties.transform);
            RemoveContent(_instantsBonuses.transform);
            RemoveContent(_categories.transform);
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
                bonus.transform.localScale = Vector3.one;
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
                property.transform.localScale = Vector3.one;
            }
        }

        private UIElementProperties GetBonusObject(BonusInfo info)
        {
            var bonus = Instantiate(_bonusPrefab).GetComponent<UIElementProperties>();
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

        private void SetCategories(BuildingCategory[] buildingCategories)
        {
            if (buildingCategories == null || buildingCategories.Length == 0)
                return;
            _categories.SetActive(true);
            foreach (var category in buildingCategories)
            {
                var element = Instantiate(_categoryPrefab, _categories.transform).GetComponent<UIElementProperties>();
                element.SetSprite(GetCategoryIcon?.Invoke(category));
                element.SetText(Translation.GetCategoryName(category));
            }
        }
    }
}
