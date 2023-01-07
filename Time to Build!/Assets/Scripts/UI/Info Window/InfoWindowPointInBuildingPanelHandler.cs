
using UI.BuildingPanel;
using UnityEngine;

namespace UI.InformationWindow
{
    public class InfoWindowPointInBuildingPanelHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            BuildingLot.GetInfoWindowSpawnPoint += GetPosition;
        }

        private void OnDisable()
        {
            BuildingLot.GetInfoWindowSpawnPoint -= GetPosition;
        }

        private Vector2 GetPosition() => transform.position;
    }
}
