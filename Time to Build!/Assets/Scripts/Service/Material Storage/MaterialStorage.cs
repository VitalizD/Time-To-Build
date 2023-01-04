using System.Collections.Generic;
using UnityEngine;
using System;

namespace Service.MaterialStorage
{
    public class MaterialStorage : MonoBehaviour
    {
        [SerializeField] private ZoneMaterial[] _zoneMaterials;
        [SerializeField] private Material _buildingArea;
        [SerializeField] private Material _road;

        private Dictionary<ZoneType, ZoneMaterial> _zoneMaterialDictionary = new();

        public Material GetBuildingAreaMaterial() => _buildingArea;

        public Material GetRoadMaterial() => _road;

        public Material GetZoneMaterial(ZoneType zoneType)
        {
            if (_zoneMaterialDictionary.ContainsKey(zoneType))
                return _zoneMaterialDictionary[zoneType].Material;
            throw new Exception($"ћатериал зоны {zoneType} отсутствует в хранилище");
        }

        public Color GetZoneColor(ZoneType zoneType)
        {
            if (_zoneMaterialDictionary.ContainsKey(zoneType))
                return _zoneMaterialDictionary[zoneType].Color;
            throw new Exception($"÷вет зоны {zoneType} отсутствует в хранилище");
        }

        private void Awake()
        {
            foreach (var material in _zoneMaterials)
                _zoneMaterialDictionary.Add(material.Type, material);
        }
    }
}
