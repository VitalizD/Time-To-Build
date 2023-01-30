using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service.BuildingStorage
{
    public class BuildingStorage : MonoBehaviour
    {
        [SerializeField] private Building[] _allBuildings;
        [SerializeField] private BuildingType[] _availableBuildings;
        [SerializeField] private int _dayForLevelB;
        [SerializeField] private int _dayForLevelC;

        private readonly Dictionary<BuildingType, Building> _allBuildingsDictionary = new();
        private readonly Dictionary<BuildingLevel, List<BuildingType>> _availableBuildingsDictionary = new();
        private BuildingLevel _currentBuilgingLevel = BuildingLevel.A;

        public Building GetBuildingInfo(BuildingType buildingType)
        {
            if (_allBuildingsDictionary.ContainsKey(buildingType))
                return _allBuildingsDictionary[buildingType];
            throw new Exception($"Здание {buildingType} отсутствует в хранилище");
        }

        public Building GetNextBuilding()
        {
            while (!_availableBuildingsDictionary.ContainsKey(_currentBuilgingLevel) ||
                _availableBuildingsDictionary[_currentBuilgingLevel].Count == 0)
            {
                ++_currentBuilgingLevel;
                if (_currentBuilgingLevel >= BuildingLevel.None)
                    return null;
            }
            var list = _availableBuildingsDictionary[_currentBuilgingLevel];
            var buildingType = list[UnityEngine.Random.Range(0, list.Count)];
            list.RemoveAt(list.IndexOf(buildingType));
            return GetBuildingInfo(buildingType);
        }

        public void CheckBuildingLevel(int day)
        {
            if (day >= _dayForLevelC)
                _currentBuilgingLevel = BuildingLevel.C;
            else if (day >= _dayForLevelB)
                _currentBuilgingLevel = BuildingLevel.B;
        }

        private void Awake()
        {
            foreach (var building in _allBuildings)
                _allBuildingsDictionary.Add(building.Type, building);

            foreach (var building in _availableBuildings)
            {
                var level = _allBuildingsDictionary[building].Level;
                if (_availableBuildingsDictionary.ContainsKey(level))
                    _availableBuildingsDictionary[level].Add(building);
                else
                    _availableBuildingsDictionary.Add(level, new List<BuildingType> { building });
            }
        }
    }
}