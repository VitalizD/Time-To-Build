using UI.BuildingPanel;
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
        }

        private void OnDisable()
        {
            BuildingPanel.RemoveBuildingAreaSelection -= _buildingArea.RemoveIllumination;
        }
    }
}
