using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service.BuildingStorage
{
    public class BuildingStorage : MonoBehaviour
    {
        [SerializeField] private Building[] _buildings;

        private Dictionary<BuildingType, Building> _buildingDictionary = new();

        public Building GetBuilding(BuildingType buildingType)
        {
            if (_buildingDictionary.ContainsKey(buildingType))
                return _buildingDictionary[buildingType];
            throw new Exception($"Здание {buildingType} отсутствует в хранилище");
        }

        private void Awake()
        {
            foreach (var building in _buildings)
                _buildingDictionary.Add(building.Type, building);
        }
    }
}