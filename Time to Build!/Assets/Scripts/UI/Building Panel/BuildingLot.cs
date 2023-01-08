using Gameplay.Buildings;
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
        [SerializeField] private GameObject _newIcon;
        [SerializeField] private GameObject _soonDisappearIcon;
        [SerializeField] private GameObject _markupIcon;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private GameObject _block;

        private Transform _infoWindowSpawnPoint;
        private BuildingType _buildingType = BuildingType.Road;
        private int _cost;
        private int _markup = 0;
        private ZoneType _zoneType;
        private int _reserve;
        private int _currentReserve;
        private int _daysForRefill;
        private int _currentDaysForRefill;
        private Property[] _properties;
        private bool _single;

        public bool Locked { get; private set; } = false;

        public static event Func<BuildingType, Building> GetBuilding;
        public static event Func<Transform> GetInfoWindowSpawnPoint;
        public static event Action<Vector2, BuildingType, bool, bool, int> ShowInfoWindow;
        public static event Action HideInfoWindow;
        public static event Action<BuildingType, int> StartBuilding;
        public static event Func<int> GetMoney;
        public static event Action HighlightAdjacents;
        public static event Action RemoveHighlightingAdjacents;
        public static event Action<ZoneType[]> HighlightBuildingsByZone;
        public static event Action RemoveighlightingBuildingsByZone;
        public static event Action<bool> AddLotToMarket;

        public void Set(BuildingType buildingType, bool single)
        {
            var building = GetBuilding?.Invoke(buildingType);
            _buildingType = buildingType;
            _cost = building.Cost;
            _zoneType = building.Zone;
            _reserve = building.Reserve;
            _currentReserve = building.Reserve;
            _daysForRefill = building.DaysForRefill;
            _currentDaysForRefill = building.DaysForRefill;
            _properties = building.Properties;
            _icon.sprite = building.Icon;
            _single = single;
            _title.text = Translation.GetBuildingName(buildingType);
            UpdateCostText();

            if (single)
                _newIcon.SetActive(true);
        }

        public void SetMarkup(int value)
        {
            _markup = value;
            _markupIcon.SetActive(value > 0);
            UpdateCostText();
        }

        public void ActivateSoonDisappear() => _soonDisappearIcon.SetActive(true);

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
                _costText.color = Color.black;
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
            ShowInfoWindow?.Invoke(_infoWindowSpawnPoint.position, _buildingType, Locked, _soonDisappearIcon.activeSelf, _markup);

            if (BuildingArea.ExistsPropertyOf(PropertyType.Adjacents, _buildingType))
                HighlightAdjacents?.Invoke();
            HighlightBuildingsForEachProperty();

            _newIcon.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideInfoWindow?.Invoke();
            RemoveHighlightingAdjacents?.Invoke();
            RemoveighlightingBuildingsByZone?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var money = GetMoney();
            if (money < _cost || _currentReserve <= 0)
                return;

            HideInfoWindow?.Invoke();
            RemoveHighlightingAdjacents?.Invoke();
            StartBuilding?.Invoke(_buildingType, _cost);

            if (_single)
            {
                Destroy(gameObject);
                AddLotToMarket?.Invoke(true);
            }
            else
            {
                --_currentReserve;
                if (_currentReserve <= 0)
                    SetLock(true);
            }
        }

        private void Start()
        {
            _infoWindowSpawnPoint = GetInfoWindowSpawnPoint?.Invoke();
        }

        private void HighlightBuildingsForEachProperty()
        {
            foreach (var property in _properties)
            {
                if (property.Type != PropertyType.Each)
                    return;
                HighlightBuildingsByZone(property.Zones);
            }
        }

        private void UpdateCostText()
        {
            _costText.text = (_cost + _markup).ToString();
            //_extraCostText.text = _markup == 0 ? "" : $"(+{_markup})";
        }
    }
}