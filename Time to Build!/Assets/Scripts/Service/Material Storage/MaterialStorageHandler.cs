using Gameplay.Buildings;
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
        }

        private void OnDisable()
        {
            BuildingArea.GetZoneMaterial -= _materialStorage.GetZoneMaterial;
            BuildingArea.GetBuildingAreaMaterial -= _materialStorage.GetBuildingAreaMaterial;
        }
    }
}
