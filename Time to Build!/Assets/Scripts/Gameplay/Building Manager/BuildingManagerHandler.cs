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
            BuildingArea.AddToBuildingManager += _buildingManager.AddBuilding;
            BuildingArea.AddToBuildingManagerWithEachProperty += _buildingManager.AddBuildingWithEachProperty;
            RewardsCalculator.GetBuildingsCountByZone += _buildingManager.GetBuildingCountByZone;
            RewardsCalculator.GetRewardsForBuildingsWithEachProperty += _buildingManager.GetRewardsForBuildingsWithEachProperty;
            BuildingLot.HighlightBuildingsByZone += _buildingManager.HighlightBuildingsByZone;
            BuildingLot.RemoveighlightingBuildingsByZone += _buildingManager.RemoveHighlightingBuildingsByZone;
        }

        private void OnDisable()
        {
            BuildingArea.AddToBuildingManager -= _buildingManager.AddBuilding;
            BuildingArea.AddToBuildingManagerWithEachProperty -= _buildingManager.AddBuildingWithEachProperty;
            RewardsCalculator.GetBuildingsCountByZone -= _buildingManager.GetBuildingCountByZone;
            RewardsCalculator.GetRewardsForBuildingsWithEachProperty -= _buildingManager.GetRewardsForBuildingsWithEachProperty;
            BuildingLot.HighlightBuildingsByZone -= _buildingManager.HighlightBuildingsByZone;
            BuildingLot.RemoveighlightingBuildingsByZone -= _buildingManager.RemoveHighlightingBuildingsByZone;
        }
    }
}