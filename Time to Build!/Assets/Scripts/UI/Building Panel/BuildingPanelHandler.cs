using Gameplay.Buildings;
using UnityEngine;

namespace UI.BuildingPanel
{
    [RequireComponent(typeof(BuildingPanel))]
    public class BuildingPanelHandler : MonoBehaviour
    {
        private BuildingPanel _buildingPanel;

        private void Awake()
        {
            _buildingPanel = GetComponent<BuildingPanel>();
        }

        private void OnEnable()
        {
            BuildingArea.OpenBuildingPanel += _buildingPanel.Show;
            BuildingArea.SelectedArea += BuildingSiteSelected;
        }

        private void OnDisable()
        {
            BuildingArea.OpenBuildingPanel -= _buildingPanel.Show;
            BuildingArea.SelectedArea -= BuildingSiteSelected;
        }

        private bool BuildingSiteSelected() => _buildingPanel.BuildingSiteSelected;
    }
}
