using Service.BuildingStorage;
using Service.Sounds;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UI.InformationWindow;
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
        [SerializeField] private GameObject _highlightArea;
        [SerializeField] private GameObject _highlightBuilding;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Color _highlightColor;
        [Space]
        [SerializeField] private bool _empty = true;
        [SerializeField] private BuildingType _building;

        private AdjacentBuildings _adjacentBuildings;
        private RoadAdapter _roadAdapter;
        private RewardsCalculator _rewardsCalculator;
        private Transform _infoWindowPoint;
        private Timer _timer;
        private Color _initialColor;
        private bool _builded = false;

        public BuildingType Type { get; private set; } = BuildingType.BuildingSite;

        public static event Func<BuildingType, Building> GetBuilding;
        public static event Func<ZoneType, Material> GetZoneMaterial;
        public static event Func<Material> GetBuildingAreaMaterial;
        public static event Func<Material> GetRoadMaterial;
        public static event Action<BuildingArea> OpenBuildingPanel;
        public static event Func<bool> AreaIsSelected;
        public static event Func<Transform> GetInfoWindowPoint;
        public static event Action<Vector2, BuildingType, bool, bool, int> ShowInfoWindow;
        public static event Action HideInfoWindow;
        public static event Action<ZoneType, BuildingCategory[], BuildingArea> AddToBuildingManager;
        public static event Action<BuildingArea> AddToBuildingManagerWithEachProperty;
        public static event Func<bool> CursorOverUIElement;
        public static event Action<BuildingArea> RemoveBuildingSite;

        public static bool ExistsPropertyOf(PropertyType propertyType, BuildingType buildingType)
        {
            var building = GetBuilding?.Invoke(buildingType);
            foreach (var property in building.Properties)
            {
                if (property.Type == propertyType)
                    return true;
            }
            return false;
        }

        public void UpdateRoadType()
        {
            if (_roadAdapter == null)
                return;
            _roadAdapter.CreateAdaptRoad(_buildingPoint.position);
        }

        public void RemoveIllumination() => _platform.material.color = _initialColor;

        public void StartBuilding(BuildingType buildingType)
        {
            if (Type != BuildingType.BuildingSite)
                return;

            Type = buildingType;
            var buildingTime = GetBuilding(buildingType).BuildingTime;
            SoundManager.Instance.PlayLoop(Sound.Building, _audioSource);
            _timer.Run(buildingTime, () => Build(buildingType));
        }

        public void Build(BuildingType buildingType)
        {
            if (_builded)
                return;

            SoundManager.Instance.Stop(Sound.Building);
            _builded = true;
            Type = buildingType;
            RemoveBuildingSite?.Invoke(this);
            CheckRoad(buildingType, out bool isRoad);
            if (isRoad)
                return;

            var buildingInfo = GetBuilding?.Invoke(buildingType);
            var building = Instantiate(buildingInfo.Prefab, _buildingPoint.position, Quaternion.identity, transform);
            var material = GetZoneMaterial?.Invoke(buildingInfo.Zone);
            if (material != null)
                SetMaterial(GetZoneMaterial?.Invoke(buildingInfo.Zone));
            if (ExistsPropertyOf(PropertyType.Each, Type))
                AddToBuildingManagerWithEachProperty?.Invoke(this);
            AddToBuildingManager?.Invoke(buildingInfo.Zone, buildingInfo.Categories, this);
            TurnToRoad(building.transform);
            _rewardsCalculator.GetAllRewards(buildingInfo);
        }

        public Dictionary<ResourceType, int> GetRewardsInThis()
        {
            if (!_builded)
                return new Dictionary<ResourceType, int>();
            return _rewardsCalculator.GetRewardsInThis(GetBuilding?.Invoke(Type));
        }

        public void HighlightAdjacents() => _highlightArea.SetActive(true);

        public void RemoveHighlightingAdjacents() => _highlightArea.SetActive(false);

        public void HighlightThis() => _highlightBuilding.SetActive(true);

        public void RemoveHighlightingThis() => _highlightBuilding.SetActive(false);

        public void SetActiveTimer(bool value) => _timer.SetActive(value);

        private void Awake()
        {
            _adjacentBuildings = GetComponent<AdjacentBuildings>();
            _timer = GetComponent<Timer>();
            _rewardsCalculator = GetComponent<RewardsCalculator>();
            _initialColor = _platform.material.color;
        }

        private void Start()
        {
            _infoWindowPoint = GetInfoWindowPoint();
            if (!_empty)
            {
                Type = _building;
                Build(_building);
            }
            else
                SetDefaultMaterial();
        }

        private void OnMouseEnter()
        {
            if (CursorOverUIElement())
                return;

            if (!AreaIsSelected() && Type == BuildingType.BuildingSite)
                Illuminate();

            if (Type != BuildingType.Road && Type != BuildingType.BuildingSite && Type != BuildingType.Obstacle)
            {
                ShowInfoWindow?.Invoke(_infoWindowPoint.position, Type, false, false, 0);
            }
        }

        private void OnMouseExit()
        {
            if (CursorOverUIElement())
                return;

            if (!AreaIsSelected() && Type == BuildingType.BuildingSite)
                RemoveIllumination();

            if (Type != BuildingType.Road && Type != BuildingType.BuildingSite)
                HideInfoWindow?.Invoke();
        }

        private void OnMouseDown()
        {
            if (CursorOverUIElement())
                return;

            if (Type == BuildingType.BuildingSite)
            {
                SoundManager.Instance.Play(Sound.Click, null);
                StartCoroutine(OpenBuildingPanelWithDelay());
            }
        }

        private IEnumerator OpenBuildingPanelWithDelay()
        {
            yield return new WaitForEndOfFrame();
            OpenBuildingPanel?.Invoke(this);
            Illuminate();
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

        private void SetDefaultMaterial()
        {
            _platform.material = GetBuildingAreaMaterial?.Invoke();
        }

        private void Illuminate() => _platform.material.color = _highlightColor;

        private void SetMaterial(Material material)
        {
            _platform.material = material;
            _initialColor = material.color;
        }

        private void TurnToRoad(Transform building)
        {
            var adjacents = _adjacentBuildings.Get4Sides();
            foreach (var adjacent in adjacents)
            {
                if (adjacent.Value != null && adjacent.Value.Type == BuildingType.Road)
                {
                    var angle = adjacent.Key switch
                    {
                        Direction.Left => 90f,
                        Direction.Top => 180f,
                        Direction.Right => 270f,
                        _ => 0f,
                    };
                    building.Rotate(new Vector3(0f, angle, 0f));
                    return;
                }
            }
        }
    }
}