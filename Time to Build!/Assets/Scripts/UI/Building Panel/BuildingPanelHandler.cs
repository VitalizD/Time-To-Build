using Gameplay.Buildings;
using Gameplay.Cycle;
using UnityEngine;

namespace UI.BuildingPanel
{
    [RequireComponent(typeof(BuildingPanel))]
    public class BuildingPanelHandler : MonoBehaviour
    {
        [SerializeField] private Market.Market _market;

        private BuildingPanel _buildingPanel;

        private void Awake()
        {
            _buildingPanel = GetComponent<BuildingPanel>();
        }

        private void Start()
        {
            _market.Replenish(0);
        }

        private void OnEnable()
        {
            BuildingArea.OpenBuildingPanel += _buildingPanel.Show;
            BuildingArea.AreaIsSelected += BuildingSiteSelected;
            BuildingLot.StartBuilding += _buildingPanel.BuildOnSelectedArea;
            BuildingLot.HighlightAdjacents += _buildingPanel.HighlightAdjacentsInSelectedArea;
            BuildingLot.RemoveHighlightingAdjacents += _buildingPanel.RemoveHighlightingAdjacentsInSelectedArea;
            BuildingLot.AddLotToMarket += _market.AddOne;
            DayCycle.NewDay += _market.Replenish;
        }

        private void OnDisable()
        {
            BuildingArea.OpenBuildingPanel -= _buildingPanel.Show;
            BuildingArea.AreaIsSelected -= BuildingSiteSelected;
            BuildingLot.StartBuilding -= _buildingPanel.BuildOnSelectedArea;
            BuildingLot.HighlightAdjacents -= _buildingPanel.HighlightAdjacentsInSelectedArea;
            BuildingLot.RemoveHighlightingAdjacents -= _buildingPanel.RemoveHighlightingAdjacentsInSelectedArea;
            BuildingLot.AddLotToMarket -= _market.AddOne;
            DayCycle.NewDay -= _market.Replenish;
        }

        private bool BuildingSiteSelected() => _buildingPanel.BuildingSiteSelected;
    }
}
