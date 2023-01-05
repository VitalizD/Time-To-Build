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
        }

        private void OnDisable()
        {
            BuildingPanel.UpdateLotColors -= _buildingLot.UpdateColors;
        }
    }
}
