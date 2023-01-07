using Service.BuildingStorage;
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
        [SerializeField] private Color _highlightColor;
        [Space]
        [SerializeField] private bool _empty = true;
        [SerializeField] private BuildingType _building;

        private Dictionary<ResourceType, int> _gettedRewards = new();
        private AdjacentBuildings _adjacentBuildings;
        private RoadAdapter _roadAdapter;
        private Vector2 _infoWindowPoint;
        private Timer _timer;
        private Color _initialColor;

        public BuildingType Type { get; private set; } = BuildingType.BuildingSite;

        public static event Func<BuildingType, Building> GetBuilding;
        public static event Func<ZoneType, Material> GetZoneMaterial;
        public static event Func<Material> GetBuildingAreaMaterial;
        public static event Func<Material> GetRoadMaterial;
        public static event Action<BuildingArea> OpenBuildingPanel;
        public static event Func<bool> AreaIsSelected;
        public static event Action<ResourceType, int> AddResource;
        public static event Func<Vector2> GetInfoWindowPoint;
        public static event Action<Vector2, BuildingType> ShowInfoWindow;
        public static event Action HideInfoWindow;

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
            _timer.Run(buildingTime, () => Build(buildingType));
        }

        public Dictionary<ResourceType, int> GetRewardsInThis()
        {
            return GetRewardsInThis(GetBuilding?.Invoke(Type));
        }

        public void HighlightAdjacents() => _highlightArea.SetActive(true);

        public void RemoveHighlightingAdjacents() => _highlightArea.SetActive(false);

        public void HighlightThis() => _highlightBuilding.SetActive(true);

        public void RemoveHighlightingThis() => _highlightBuilding.SetActive(false);

        private void Awake()
        {
            _adjacentBuildings = GetComponent<AdjacentBuildings>();
            _timer = GetComponent<Timer>();
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
            if (!AreaIsSelected() && Type == BuildingType.BuildingSite)
                Illuminate();

            if (Type != BuildingType.Road && Type != BuildingType.BuildingSite)
            {
                ShowInfoWindow?.Invoke(_infoWindowPoint, Type);
            }
        }

        private void OnMouseExit()
        {
            if (!AreaIsSelected() && Type == BuildingType.BuildingSite)
                RemoveIllumination();

            if (Type != BuildingType.Road && Type != BuildingType.BuildingSite)
                HideInfoWindow?.Invoke();
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
            Illuminate();
        }

        private void Build(BuildingType buildingType)
        {
            CheckRoad(buildingType, out bool isRoad);

            if (isRoad)
                return;

            var buildingInfo = GetBuilding?.Invoke(buildingType);
            var building = Instantiate(buildingInfo.Prefab, _buildingPoint.position, Quaternion.identity, transform);
            SetMaterial(GetZoneMaterial?.Invoke(buildingInfo.Zone));
            TurnToRoad(building.transform);
            GetAllRewards(buildingInfo);
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

        private void GetAllRewards(Building buildingInfo)
        {
            var rewards = new List<Dictionary<ResourceType, int>>();
            rewards.Add(GetRewardsInThis(buildingInfo));
            var adjacents = _adjacentBuildings.Get8Sides().Values
                .Where(building => building != null && building.Type != BuildingType.BuildingSite);
            foreach (var adjacent in adjacents)
                rewards.Add(adjacent.GetRewardsInThis());
            foreach (var reward in UnionRewards(rewards))
                AddResource?.Invoke(reward.Key, reward.Value);
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

        private Dictionary<ResourceType, int> GetRewardsInThis(Building buildingInfo)
        {
            var rewardList = new List<Dictionary<ResourceType, int>>();
            rewardList.Add(GetInstantRewards(buildingInfo.InstantBonuses));

            foreach (var property in buildingInfo.Properties)
            {
                switch (property.Type)
                {
                    case PropertyType.Adjacents: rewardList.Add(GetPropertyAdjacentRewards(property)); break;
                }
            }
            var commonRewards = UnionRewards(rewardList);
            var rewardsToGet = new Dictionary<ResourceType, int>();
            foreach (var resourceType in commonRewards.Keys)
            {
                if (_gettedRewards.ContainsKey(resourceType))
                    rewardsToGet.Add(resourceType, commonRewards[resourceType] - _gettedRewards[resourceType]);
                else
                    rewardsToGet.Add(resourceType, commonRewards[resourceType]);
            }
            _gettedRewards = new Dictionary<ResourceType, int>(commonRewards);
            return rewardsToGet;
        }

        private Dictionary<ResourceType, int> GetInstantRewards(BonusInfo[] bonuses)
        {
            return bonuses.ToDictionary(bonus => bonus.Resource, bonus => bonus.Value);
        }

        private Dictionary<ResourceType, int> GetPropertyAdjacentRewards(Property property)
        {
            var adjacents = _adjacentBuildings.Get8Sides()
                .Where(adjacent => adjacent.Value != null && adjacent.Value.Type != BuildingType.BuildingSite);
            var commonRewards = new Dictionary<ResourceType, int>();
            foreach (var adjacent in adjacents)
            {
                var rewards = GetPropertyBonus(property, GetBuilding(adjacent.Value.Type).Zone);
                foreach (var reward in rewards)
                {
                    if (commonRewards.ContainsKey(reward.Key))
                        commonRewards[reward.Key] += reward.Value;
                    else
                        commonRewards.Add(reward.Key, reward.Value);
                }
            }
            return commonRewards;
        }

        private Dictionary<ResourceType, int> GetPropertyBonus(Property property, ZoneType buildingZone)
        {
            var rewards = new Dictionary<ResourceType, int>();
            foreach (var zone in property.Zones)
            {
                if (zone == buildingZone)
                {
                    foreach (var bonus in property.Bonuses)
                    {
                        if (rewards.ContainsKey(bonus.Resource))
                            rewards[bonus.Resource] += bonus.Value;
                        else
                            rewards.Add(bonus.Resource, bonus.Value);
                    }
                }
            }
            return rewards;
        }

        private Dictionary<ResourceType, int> UnionRewards(IEnumerable<Dictionary<ResourceType, int>> rewardList)
        {
            var result = new Dictionary<ResourceType, int>();
            foreach (var rewards in rewardList)
            {
                foreach (var resource in rewards)
                {
                    if (result.ContainsKey(resource.Key))
                        result[resource.Key] += resource.Value;
                    else
                        result.Add(resource.Key, resource.Value);
                }
            }
            return result;
        }
    }
}