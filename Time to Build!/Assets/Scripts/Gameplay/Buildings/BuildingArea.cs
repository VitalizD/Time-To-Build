using Service.BuildingStorage;
using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Gameplay.Buildings
{
    [RequireComponent(typeof(AdjacentBuildings))]
    [RequireComponent(typeof(Timer))]
    public class BuildingArea : MonoBehaviour
    {
        public const float CellSize = 1f;

        [SerializeField] private MeshRenderer _platform;
        [SerializeField] private Transform _buildingPoint;
        [SerializeField] private Color _highlightColor;
        [Space]
        [SerializeField] private bool _empty = true;
        [SerializeField] private BuildingType _building;

        private AdjacentBuildings _adjacentBuildings;
        private RoadAdapter _roadAdapter;
        private Timer _timer;
        private Color _initialColor;

        public BuildingType Type { get; private set; } = BuildingType.BuildingSite;

        public static event Func<BuildingType, Building> GetBuilding;
        public static event Func<ZoneType, Material> GetZoneMaterial;
        public static event Func<Material> GetBuildingAreaMaterial;
        public static event Func<Material> GetRoadMaterial;
        public static event Action<BuildingArea> OpenBuildingPanel;
        public static event Func<bool> SelectedArea;

        public void UpdateRoadType()
        {
            if (_roadAdapter == null)
                return;
            _roadAdapter.CreateAdaptRoad(_buildingPoint.position);
        }

        public void RemoveHighlight() => _platform.material.color = _initialColor;

        public void StartBuilding(BuildingType buildingType)
        {
            if (Type != BuildingType.BuildingSite)
                return;

            Type = buildingType;
            var buildingTime = GetBuilding(buildingType).BuildingTime;
            _timer.Run(buildingTime, () => Build(buildingType));
        }

        private void Awake()
        {
            _adjacentBuildings = GetComponent<AdjacentBuildings>();
            _timer = GetComponent<Timer>();
            _initialColor = _platform.material.color;
        }

        private void Start()
        {
            if (!_empty)
            {
                Type = _building;
                Build(_building);
            }
            else
                SetDefault();
        }

        private void OnMouseEnter()
        {
            if (!SelectedArea() && Type == BuildingType.BuildingSite)
                Highlight();
        }

        private void OnMouseExit()
        {
            if (!SelectedArea() && Type == BuildingType.BuildingSite)
                RemoveHighlight();
        }

        private void OnMouseDown()
        {
            if (Type == BuildingType.BuildingSite)
                StartCoroutine(OpenBuildingPanelWithDelay());
        }

        private IEnumerator OpenBuildingPanelWithDelay()
        {
            yield return new WaitForEndOfFrame();
            OpenBuildingPanel?.Invoke(this);
            Highlight();
        }

        private void Build(BuildingType buildingType)
        {
            CheckRoad(buildingType, out bool isRoad);

            if (isRoad)
                return;

            var building = GetBuilding?.Invoke(buildingType);
            Instantiate(building.Prefab, _buildingPoint.position, Quaternion.identity, transform);
            SetMaterial(GetZoneMaterial?.Invoke(building.Zone));
        }

        private void CheckRoad(BuildingType buildingType, out bool isRoad)
        {

            if (buildingType == BuildingType.Road)
            {
                isRoad = true;
                _roadAdapter = gameObject.AddComponent<RoadAdapter>();
                UpdateRoadType();
                var adjacents = _adjacentBuildings.Get4Sides().Values;
                foreach (var adjacent in adjacents)
                {
                    if (adjacent == null)
                        continue;
                    adjacent.UpdateRoadType();
                }
                SetMaterial(GetRoadMaterial?.Invoke());
                _adjacentBuildings.CreateBuildingSites();
            }
            else isRoad = false;
        }

        private void SetDefault()
        {
            _platform.material = GetBuildingAreaMaterial?.Invoke();
        }

        private void Highlight() => _platform.material.color = _highlightColor;

        private void SetMaterial(Material material)
        {
            _platform.material = material;
            _initialColor = material.color;
        }
    }
}