using Gameplay.Buildings;
using UI.BuildingPanel;
using UI.InformationWindow;
using UnityEngine;

namespace Service.BuildingStorage
{
    [RequireComponent(typeof(BuildingStorage))]
    public class BuildingStorageHandler : MonoBehaviour
    {
        private BuildingStorage _buildingStorage;

        private void Awake()
        {
            _buildingStorage = GetComponent<BuildingStorage>();
        }

        private void OnEnable()
        {
            BuildingArea.GetBuilding += _buildingStorage.GetBuilding;
            BuildingLot.GetBuilding += _buildingStorage.GetBuilding;
            InfoWindow.GetBuilding += _buildingStorage.GetBuilding;
        }

        private void OnDisable()
        {
            BuildingArea.GetBuilding -= _buildingStorage.GetBuilding;
            BuildingLot.GetBuilding -= _buildingStorage.GetBuilding;
            InfoWindow.GetBuilding -= _buildingStorage.GetBuilding;
        }
    }
}
