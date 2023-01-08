
using UI.BuildingPanel;
using UnityEngine;

namespace UI.InformationWindow
{
    public class InfoWindowPointInBuildingPanelHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            BuildingLot.GetInfoWindowSpawnPoint += GetPoint;
        }

        private void OnDisable()
        {
            BuildingLot.GetInfoWindowSpawnPoint -= GetPoint;
        }

        private Transform GetPoint() => transform;
    }
}
