using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service.BuildingStorage
{
    public class BuildingStorage : MonoBehaviour
    {
        [SerializeField] private Building[] _allBuildings;
        [SerializeField] private BuildingType[] _availableBuildings;

        private readonly Dictionary<BuildingType, Building> _allBuildingsDictionary = new();
        private readonly Dictionary<BuildingLevel, List<BuildingType>> _availableBuildingsDictionary = new();
        private Dictionary<BuildingLevel, List<BuildingType>> _buildingsForCurrentGame;
        private BuildingLevel _currentBuilgingLevel = BuildingLevel.A;

        public Building GetBuildingInfo(BuildingType buildingType)
        {
            if (_allBuildingsDictionary.ContainsKey(buildingType))
                return _allBuildingsDictionary[buildingType];
            throw new Exception($"Здание {buildingType} отсутствует в хранилище");
        }

        public Building GetNextBuilding()
        {
            while (!_buildingsForCurrentGame.ContainsKey(_currentBuilgingLevel) || 
                _buildingsForCurrentGame[_currentBuilgingLevel].Count == 0)
            {
                ++_currentBuilgingLevel;
                if (_currentBuilgingLevel >= BuildingLevel.None)
                    return null;
            }
            var list = _buildingsForCurrentGame[_currentBuilgingLevel];
            var buildingType = list[UnityEngine.Random.Range(0, list.Count)];
            list.RemoveAt(list.IndexOf(buildingType));
            return GetBuildingInfo(buildingType);
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
            _buildingsForCurrentGame = new Dictionary<BuildingLevel, List<BuildingType>>(_availableBuildingsDictionary);
        }
    }
}