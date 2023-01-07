using Service.BuildingStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.InformationWindow;
using UnityEngine;

namespace Gameplay.Buildings
{
    [RequireComponent(typeof(AdjacentBuildings))]
    public class RewardsCalculator : MonoBehaviour
    {
        private AdjacentBuildings _adjacentBuildings;
        private Dictionary<ResourceType, int> _gettedRewards = new();

        public static event Func<Dictionary<ResourceType, int>> GetRewardsForBuildingsWithEachProperty;
        public static event Action<ResourceType, int> AddResource;
        public static event Func<BuildingType, Building> GetBuilding;
        public static event Func<ZoneType, int> GetBuildingsCountByZone;

        public static Dictionary<ResourceType, int> UnionRewards(IEnumerable<Dictionary<ResourceType, int>> rewardList)
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

        public void GetAllRewards(Building buildingInfo)
        {
            var rewards = new List<Dictionary<ResourceType, int>>();
            rewards.Add(GetRewardsInThis(buildingInfo));
            var adjacents = _adjacentBuildings.Get8Sides().Values
                .Where(building => building != null && building.Type != BuildingType.BuildingSite);
            foreach (var adjacent in adjacents)
                rewards.Add(adjacent.GetRewardsInThis());
            rewards.Add(GetRewardsForBuildingsWithEachProperty?.Invoke());
            foreach (var reward in UnionRewards(rewards))
                AddResource?.Invoke(reward.Key, reward.Value);
        }

        public Dictionary<ResourceType, int> GetRewardsInThis(Building buildingInfo)
        {
            var rewardList = new List<Dictionary<ResourceType, int>>();
            rewardList.Add(GetInstantRewards(buildingInfo.InstantBonuses));

            foreach (var property in buildingInfo.Properties)
            {
                switch (property.Type)
                {
                    case PropertyType.Adjacents: rewardList.Add(GetPropertyAdjacentRewards(property)); break;
                    case PropertyType.Each: rewardList.Add(GetPropertyEachRewards(property)); break;
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

        private void Awake()
        {
            _adjacentBuildings = GetComponent<AdjacentBuildings>();
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

        private Dictionary<ResourceType, int> GetPropertyEachRewards(Property property)
        {
            var rewards = new Dictionary<ResourceType, int>();
            foreach (var zone in property.Zones)
            {
                foreach (var bonus in property.Bonuses)
                {
                    var buildingsCount = GetBuildingsCountByZone(zone);
                    if (rewards.ContainsKey(bonus.Resource))
                        rewards[bonus.Resource] += buildingsCount;
                    else
                        rewards.Add(bonus.Resource, buildingsCount * bonus.Value);
                }
            }
            return rewards;
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
    }
}
