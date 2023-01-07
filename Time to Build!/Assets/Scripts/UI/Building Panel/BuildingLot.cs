using Service;
using Service.BuildingStorage;
using System;
using System.Linq;
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
        [SerializeField] private GameObject _block;

        private Vector2 _infoWindowSpawnPoint;
        private BuildingType _buildingType = BuildingType.Road;
        private int _cost;
        private ZoneType _zoneType;
        private int _reserve;
        private int _currentReserve;
        private int _daysForRefill;
        private int _currentDaysForRefill;

        public bool Locked { get; private set; } = false;

        public static event Func<BuildingType, Building> GetBuilding;
        public static event Func<Vector2> GetInfoWindowSpawnPoint;
        public static event Action<Vector2, BuildingType, bool> ShowInfoWindow;
        public static event Action HideInfoWindow;
        public static event Action<BuildingType, int> StartBuilding;
        public static event Func<int> GetMoney;
        public static event Action HighlightAdjacents;
        public static event Action RemoveHighlightingAdjacents;

        public void Set(BuildingType buildingType)
        {
            var building = GetBuilding?.Invoke(buildingType);
            _buildingType = buildingType;
            _cost = building.Cost;
            _zoneType = building.Zone;
            _reserve = building.Reserve;
            _currentReserve = building.Reserve;
            _daysForRefill = building.DaysForRefill;
            _currentDaysForRefill = building.DaysForRefill;
            _icon.sprite = building.Icon;
            _title.text = Translation.GetBuildingName(buildingType);
            _costText.text = building.Cost.ToString();
        }

        public void SetLock(bool value)
        {
            Locked = value;
            _block.SetActive(value);
        }

        public void UpdateColors()
        {
            var money = GetMoney();
            if (money < _cost)
                _costText.color = Color.red;
            else
                _costText.color = Color.white;
        }

        public void UpdateDaysForRefill()
        {
            --_currentDaysForRefill;
            if (_currentDaysForRefill <= 0)
            {
                _currentDaysForRefill = _daysForRefill;
                _currentReserve = _reserve;
                SetLock(false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowInfoWindow?.Invoke(_infoWindowSpawnPoint, _buildingType, Locked);

            if (ExistsAdjacentProperty())
                HighlightAdjacents?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideInfoWindow?.Invoke();
            RemoveHighlightingAdjacents?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var money = GetMoney();
            if (money < _cost || _currentReserve <= 0)
                return;

            RemoveHighlightingAdjacents?.Invoke();
            StartBuilding?.Invoke(_buildingType, _cost);
            --_currentReserve;
            if (_currentReserve <= 0)
                SetLock(true);
        }

        private void Start()
        {
            var infoWindowSpawnPoint = GetInfoWindowSpawnPoint?.Invoke();
            _infoWindowSpawnPoint = infoWindowSpawnPoint.GetValueOrDefault();
        }

        private bool ExistsAdjacentProperty()
        {
            var building = GetBuilding?.Invoke(_buildingType);
            foreach (var property in building.Properties)
            {
                if (property.Type == PropertyType.Adjacents)
                    return true;
            }
            return false;
        }
    }
}