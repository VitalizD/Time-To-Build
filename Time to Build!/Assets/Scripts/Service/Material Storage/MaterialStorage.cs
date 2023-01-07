using System.Collections.Generic;
using UnityEngine;
using System;

namespace Service.MaterialStorage
{
    public class MaterialStorage : MonoBehaviour
    {
        [SerializeField] private ZoneMaterial[] _zoneMaterials;
        [SerializeField] private ResourceIcon[] _resourceIcons;
        [SerializeField] private Material _buildingArea;
        [SerializeField] private Material _road;

        private readonly Dictionary<ZoneType, ZoneMaterial> _zoneMaterialDictionary = new();
        private readonly Dictionary<ResourceType, Sprite> _resourceSpriteDictionary = new();

        public Material GetBuildingAreaMaterial() => _buildingArea;

        public Material GetRoadMaterial() => _road;

        public Material GetZoneMaterial(ZoneType zoneType)
        {
            if (_zoneMaterialDictionary.ContainsKey(zoneType))
                return _zoneMaterialDictionary[zoneType].Material;
            if (zoneType == ZoneType.None)
                return null;
            throw new Exception($"�������� ���� {zoneType} ����������� � ���������");
        }

        public Color GetZoneColor(ZoneType zoneType)
        {
            if (_zoneMaterialDictionary.ContainsKey(zoneType))
                return _zoneMaterialDictionary[zoneType].Color;
            throw new Exception($"���� ���� {zoneType} ����������� � ���������");
        }

        public Sprite GetResourceIcon(ResourceType resourceType)
        {
            if (_resourceSpriteDictionary.ContainsKey(resourceType))
                return _resourceSpriteDictionary[resourceType];
            throw new Exception($"������ ������� {resourceType} ����������� � ���������");
        }

        private void Awake()
        {
            foreach (var material in _zoneMaterials)
                _zoneMaterialDictionary.Add(material.Type, material);

            foreach (var icon in _resourceIcons)
                _resourceSpriteDictionary.Add(icon.Type, icon.Icon);
        }
    }
}
