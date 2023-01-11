using Gameplay.Cycle;
using UI.Counters;
using UnityEngine;

namespace UI.BuildingPanel
{
    [RequireComponent(typeof(BuildingLot))]
    public class BuildingLotHandler : MonoBehaviour
    {
        private BuildingLot _buildingLot;

        private void Awake()
        {
            _buildingLot = GetComponent<BuildingLot>();
        }

        private void OnEnable()
        {
            BuildingPanel.UpdateLotColors += _buildingLot.UpdateColors;
            UICounter.UpdateBuildingLotColors += _buildingLot.UpdateColors;
            BuildingPanelTab.UpdateBuildingLotColors += _buildingLot.UpdateColors;
            DayCycle.NewDay += _buildingLot.UpdateDaysForRefill;
        }

        private void OnDisable()
        {
            BuildingPanel.UpdateLotColors -= _buildingLot.UpdateColors;
            UICounter.UpdateBuildingLotColors -= _buildingLot.UpdateColors;
            BuildingPanelTab.UpdateBuildingLotColors -= _buildingLot.UpdateColors;
            DayCycle.NewDay -= _buildingLot.UpdateDaysForRefill;
        }
    }
}
