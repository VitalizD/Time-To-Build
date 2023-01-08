using Gameplay.Buildings;
using UI.BuildingPanel;
using UI.BuildingPanel.Market;
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
            BuildingArea.GetBuilding += _buildingStorage.GetBuildingInfo;
            BuildingLot.GetBuilding += _buildingStorage.GetBuildingInfo;
            InfoWindow.GetBuilding += _buildingStorage.GetBuildingInfo;
            RewardsCalculator.GetBuilding += _buildingStorage.GetBuildingInfo;
            Market.GetNextBuildingInfo += _buildingStorage.GetNextBuilding;
        }

        private void OnDisable()
        {
            BuildingArea.GetBuilding -= _buildingStorage.GetBuildingInfo;
            BuildingLot.GetBuilding -= _buildingStorage.GetBuildingInfo;
            InfoWindow.GetBuilding -= _buildingStorage.GetBuildingInfo;
            RewardsCalculator.GetBuilding -= _buildingStorage.GetBuildingInfo;
            Market.GetNextBuildingInfo -= _buildingStorage.GetNextBuilding;
        }
    }
}
