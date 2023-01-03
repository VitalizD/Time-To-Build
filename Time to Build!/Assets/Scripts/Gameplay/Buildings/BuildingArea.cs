using Service.BuildingStorage;
using System;
using UI;
using UnityEngine;

namespace Gameplay.Buildings
{
    [RequireComponent(typeof(AdjacentBuildings))]
    public class BuildingArea : MonoBehaviour
    {
        public const float CellSize = 1f;

        [SerializeField] private MeshRenderer _platform;
        [SerializeField] private Transform _buildingPoint;
        [SerializeField] private UIBar _progressBar;
        [Space]
        [SerializeField] private bool _empty = true;
        [SerializeField] private BuildingType _building;

        private AdjacentBuildings _adjacentBuildings;
        private RoadAdapter _roadAdapter;

        public BuildingType Type { get; private set; } = BuildingType.BuildingSite;

        public static event Func<BuildingType, Building> GetBuilding;
        public static event Func<ZoneType, Material> GetZoneMaterial;
        public static event Func<Material> GetBuildingAreaMaterial;
        public static event Func<Material> GetRoadMaterial;

        public void UpdateRoadType()
        {
            if (_roadAdapter == null)
                return;
            _roadAdapter.CreateAdaptRoad(_buildingPoint.position);
        }

        private void Awake()
        {
            _adjacentBuildings = GetComponent<AdjacentBuildings>();
            _progressBar.gameObject.SetActive(false);
        }

        private void Start()
        {
            if (!_empty)
                Build(_building);
            else
                SetDefault();
        }

        private void Build(BuildingType buildingType)
        {
            Type = buildingType;
            CheckRoad(buildingType, out bool isRoad);

            if (isRoad)
                return;

            var building = GetBuilding?.Invoke(buildingType);
            Instantiate(building.Prefab, _buildingPoint.position, Quaternion.identity, transform);
            _platform.material = GetZoneMaterial?.Invoke(building.Zone);
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
                _platform.material = GetRoadMaterial?.Invoke();
                _adjacentBuildings.CreateBuildingSites();
            }
            else isRoad = false;
        }

        private void SetDefault()
        {
            _platform.material = GetBuildingAreaMaterial?.Invoke();
        }
    }
}