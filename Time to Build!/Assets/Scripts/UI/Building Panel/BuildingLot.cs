using Service;
using Service.BuildingStorage;
using System;
using TMPro;
using UI.InformationWindow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.BuildingPanel
{
    public class BuildingLot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _costText;

        private Vector2 _infoWindowSpawnPoint;
        private BuildingType _buildingType = BuildingType.Road;
        private int _cost;
        private ZoneType _zoneType;

        public static event Func<BuildingType, Building> GetBuilding;
        public static event Func<Vector2> GetInfoWindowSpawnPoint;
        public static event Action<Vector2, string, BonusInfo[], PropertyInfo, string, ZoneType> ShowInfoWindow;
        public static event Action HideInfoWindow;
        public static event Action<BuildingType, int> StartBuilding;
        public static event Func<int> GetMoney;

        public void Set(BuildingType buildingType)
        {
            var building = GetBuilding?.Invoke(buildingType);
            _buildingType = buildingType;
            _cost = building.Cost;
            _zoneType = building.Zone;
            _icon.sprite = building.Icon;
            _title.text = Translation.GetBuildingName(buildingType);
            _costText.text = building.Cost.ToString();
        }

        public void UpdateColors()
        {
            var money = GetMoney();
            if (money < _cost)
                _costText.color = Color.red;
            else
                _costText.color = Color.white;
        }

        private void Start()
        {
            var infoWindowSpawnPoint = GetInfoWindowSpawnPoint?.Invoke();
            _infoWindowSpawnPoint = infoWindowSpawnPoint.GetValueOrDefault();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowInfoWindow?.Invoke(_infoWindowSpawnPoint, _title.text, null, null,
                Translation.GetBuildingDescription(_buildingType), _zoneType);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideInfoWindow?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StartBuilding?.Invoke(_buildingType, _cost);
        }
    }
}