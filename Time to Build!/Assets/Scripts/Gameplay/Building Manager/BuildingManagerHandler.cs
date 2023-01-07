using Gameplay.Buildings;
using UI.BuildingPanel;
using UnityEngine;

namespace Gameplay.BuildingManager
{
    [RequireComponent(typeof(BuildingManager))]
    public class BuildingManagerHandler : MonoBehaviour
    {
        private BuildingManager _buildingManager;

        private void Awake()
        {
            _buildingManager = GetComponent<BuildingManager>();
        }

        private void OnEnable()
        {
            BuildingArea.GetBuildingsCountByZone += _buildingManager.GetBuildingCountByZone;
            BuildingArea.AddToBuildingManager += _buildingManager.AddBuilding;
            BuildingArea.AddToBuildingManagerWithEachProperty += _buildingManager.AddBuildingWithEachProperty;
            BuildingArea.GetRewardsForBuildingsWithEachProperty += _buildingManager.GetRewardsForBuildingsWithEachProperty;
            BuildingLot.HighlightBuildingsByZone += _buildingManager.HighlightBuildingsByZone;
            BuildingLot.RemoveighlightingBuildingsByZone += _buildingManager.RemoveHighlightingBuildingsByZone;
        }

        private void OnDisable()
        {
            BuildingArea.GetBuildingsCountByZone -= _buildingManager.GetBuildingCountByZone;
            BuildingArea.AddToBuildingManager -= _buildingManager.AddBuilding;
            BuildingArea.AddToBuildingManagerWithEachProperty -= _buildingManager.AddBuildingWithEachProperty;
            BuildingArea.GetRewardsForBuildingsWithEachProperty -= _buildingManager.GetRewardsForBuildingsWithEachProperty;
            BuildingLot.HighlightBuildingsByZone -= _buildingManager.HighlightBuildingsByZone;
            BuildingLot.RemoveighlightingBuildingsByZone -= _buildingManager.RemoveHighlightingBuildingsByZone;
        }
    }
}