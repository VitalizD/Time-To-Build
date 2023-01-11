using Gameplay.Buildings;
using UI.InformationWindow;
using UnityEngine;

namespace Service.MaterialStorage
{
    [RequireComponent(typeof (MaterialStorage))]
    public class MaterialStorageHandler : MonoBehaviour
    {
        private MaterialStorage _materialStorage;

        private void Awake()
        {
            _materialStorage = GetComponent<MaterialStorage>();
        }

        private void OnEnable()
        {
            BuildingArea.GetZoneMaterial += _materialStorage.GetZoneMaterial;
            BuildingArea.GetBuildingAreaMaterial += _materialStorage.GetBuildingAreaMaterial;
            BuildingArea.GetRoadMaterial += _materialStorage.GetRoadMaterial;
            InfoWindow.GetZoneColor += _materialStorage.GetZoneColor;
            InfoWindow.GetResourceIcon += _materialStorage.GetResourceIcon;
            InfoWindow.GetCategoryIcon += _materialStorage.GetCategoryIcon;
            Translation.GetZoneColor += _materialStorage.GetZoneColor;
        }

        private void OnDisable()
        {
            BuildingArea.GetZoneMaterial -= _materialStorage.GetZoneMaterial;
            BuildingArea.GetBuildingAreaMaterial -= _materialStorage.GetBuildingAreaMaterial;
            BuildingArea.GetRoadMaterial -= _materialStorage.GetRoadMaterial;
            InfoWindow.GetZoneColor -= _materialStorage.GetZoneColor;
            InfoWindow.GetResourceIcon -= _materialStorage.GetResourceIcon;
            InfoWindow.GetCategoryIcon -= _materialStorage.GetCategoryIcon;
            Translation.GetZoneColor -= _materialStorage.GetZoneColor;
        }
    }
}
