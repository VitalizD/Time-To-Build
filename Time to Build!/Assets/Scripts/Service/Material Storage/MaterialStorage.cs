using System.Collections.Generic;
using UnityEngine;
using System;

namespace Service.MaterialStorage
{
    public class MaterialStorage : MonoBehaviour
    {
        [SerializeField] private ZoneMaterial[] _zoneMaterials;
        [SerializeField] private Material _buildingArea;

        private Dictionary<ZoneType, Material> _zoneMaterialDictionary = new();

        public Material GetBuildingAreaMaterial() => _buildingArea;

        public Material GetZoneMaterial(ZoneType zoneType)
        {
            if (_zoneMaterialDictionary.ContainsKey(zoneType))
                return _zoneMaterialDictionary[zoneType];
            throw new Exception($"Материал {zoneType} отсутствует в хранилище");
        }

        private void Awake()
        {
            foreach (var material in _zoneMaterials)
                _zoneMaterialDictionary.Add(material.Type, material.Material);
        }
    }
}
