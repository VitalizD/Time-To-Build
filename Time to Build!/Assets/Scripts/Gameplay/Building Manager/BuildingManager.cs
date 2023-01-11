using Gameplay.Buildings;
using Service.BuildingStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.BuildingManager
{
    public class BuildingManager : MonoBehaviour
    {
        private readonly Dictionary<ZoneType, List<BuildingArea>> _buildingsByZone = new();
        private readonly Dictionary<BuildingCategory, List<BuildingArea>> _buildingsByCategory = new();
        private readonly List<BuildingArea> _buildingsWithEachProperty = new();
        private readonly List<ZoneType> _lastZones = new();
        private readonly List<BuildingCategory> _lastCategories = new();

        public int GetBuildingCountByZone(ZoneType zoneType)
        {
            if (_buildingsByZone.ContainsKey(zoneType))
                return _buildingsByZone[zoneType].Count;
            return 0;
        }

        public int GetBuildingCountByCategory(BuildingCategory buildingCategory)
        {
            if (_buildingsByCategory.ContainsKey(buildingCategory))
                return _buildingsByCategory[buildingCategory].Count;
            return 0;
        }

        public void AddBuilding(ZoneType zoneType, BuildingCategory[] categories, BuildingArea buildingArea)
        {
            if (_buildingsByZone.ContainsKey(zoneType))
                _buildingsByZone[zoneType].Add(buildingArea);
            else
                _buildingsByZone.Add(zoneType, new List<BuildingArea> { buildingArea });

            foreach (var category in categories)
            {
                if (_buildingsByCategory.ContainsKey(category))
                    _buildingsByCategory[category].Add(buildingArea);
                else
                    _buildingsByCategory.Add(category, new List<BuildingArea> { buildingArea });
            }
        }

        public void AddBuildingWithEachProperty(BuildingArea buildingArea) => _buildingsWithEachProperty.Add(buildingArea);

        public Dictionary<ResourceType, int> GetRewardsForBuildingsWithEachProperty()
        {
            var rewards = new List<Dictionary<ResourceType, int>>();
            foreach (var building in _buildingsWithEachProperty)
                rewards.Add(building.GetRewardsInThis());
            return RewardsCalculator.UnionRewards(rewards);
        }

        public void HighlightBuildingsByZone(ZoneType[] zoneTypes)
        {
            foreach (var zone in zoneTypes)
            {
                if (!_buildingsByZone.ContainsKey(zone))
                    return;
                foreach (var building in _buildingsByZone[zone])
                    building.HighlightThis();
            }
            _lastZones.AddRange(zoneTypes);
            _lastZones.Distinct();
        }

        public void HighlightBuildingsByCategory(BuildingCategory[] buildingCategories)
        {
            foreach (var category in buildingCategories)
            {
                if (!_buildingsByCategory.ContainsKey(category))
                    return;
                foreach (var building in _buildingsByCategory[category])
                    building.HighlightThis();
            }
            _lastCategories.AddRange(buildingCategories);
            _lastCategories.Distinct();
        }

        public void RemoveHighlightingBuildings()
        {
            foreach (var zone in _lastZones)
            {
                if (!_buildingsByZone.ContainsKey(zone))
                    return;
                foreach (var building in _buildingsByZone[zone])
                    building.RemoveHighlightingThis();
            }
            _lastZones.Clear();

            foreach (var cateogry in _lastCategories)
            {
                if (!_buildingsByCategory.ContainsKey(cateogry))
                    return;
                foreach (var building in _buildingsByCategory[cateogry])
                    building.RemoveHighlightingThis();
            }
            _lastCategories.Clear();
        }
    }
}