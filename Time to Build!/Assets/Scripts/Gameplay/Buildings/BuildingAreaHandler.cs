using UI.BuildingPanel;
using UI.PopupWindows;
using UnityEngine;

namespace Gameplay.Buildings
{
    [RequireComponent(typeof(BuildingArea))]
    public class BuildingAreaHandler : MonoBehaviour
    {
        private BuildingArea _buildingArea;

        private void Awake()
        {
            _buildingArea = GetComponent<BuildingArea>();
        }

        private void OnEnable()
        {
            BuildingPanel.RemoveBuildingAreaSelection += _buildingArea.RemoveIllumination;
            PopupWindow.SetActiveCycle += _buildingArea.SetActiveTimer;
        }

        private void OnDisable()
        {
            BuildingPanel.RemoveBuildingAreaSelection -= _buildingArea.RemoveIllumination;
            PopupWindow.SetActiveCycle -= _buildingArea.SetActiveTimer;
        }
    }
}
