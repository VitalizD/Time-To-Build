using Gameplay.Buildings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BuildingPanel
{
    [RequireComponent(typeof(Animator))]
    public class BuildingPanel : MonoBehaviour
    {
        private const string SHOW_ANIMATOR_BOOL = "Show";

        [SerializeField] private GameObject _baseLots;
        [SerializeField] private GameObject _marketLots;
        [SerializeField] private GameObject _lotPrefab;
        [SerializeField] private BuildingType[] _baseBuildings;

        private Animator _animator;
        private BuildingArea _selectedArea;

        public bool BuildingSiteSelected { get => _selectedArea != null; }

        public static event Func<bool> CursorOverUIElement;
        public static event Action RemoveBuildingAreaSelection;
        public static event Action<ResourceType, int> AddMoney;
        public static event Action UpdateLotColors;

        public void Show(BuildingArea selectedArea)
        {
            _selectedArea = selectedArea;
            _animator.SetBool(SHOW_ANIMATOR_BOOL, true);
            UpdateLotColors?.Invoke();
        }

        public void Hide()
        {
            _selectedArea = null;
            _animator.SetBool(SHOW_ANIMATOR_BOOL, false);
            RemoveBuildingAreaSelection?.Invoke();
        }

        public void BuildOnSelectedArea(BuildingType buildingType, int cost)
        {
            if (_selectedArea == null)
                return;

            _selectedArea.StartBuilding(buildingType);
            Hide();
            AddMoney?.Invoke(ResourceType.Money, -cost);
        }

        public void HighlightAdjacentsInSelectedArea()
        {
            if (_selectedArea != null)
                _selectedArea.HighlightAdjacents();
        }

        public void RemoveHighlightingAdjacentsInSelectedArea()
        {
            if (_selectedArea != null)
                _selectedArea.RemoveHighlightingAdjacents();
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            foreach (var type in _baseBuildings)
            {
                var lot = Instantiate(_lotPrefab, _baseLots.transform);
                lot.GetComponent<BuildingLot>().Set(type);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!CursorOverUIElement())
                    Hide();
            }
        }
    }
}