using UI.BuildingPanel;
using UnityEngine;

namespace UI.InformationWindow
{
    [RequireComponent(typeof(InfoWindow))]
    public class InfoWindowHandler : MonoBehaviour
    {
        private InfoWindow _infoWindow;

        private void Awake()
        {
            _infoWindow = GetComponent<InfoWindow>();
        }

        private void OnEnable()
        {
            BuildingLot.ShowInfoWindow += _infoWindow.Show;
            BuildingLot.HideInfoWindow += _infoWindow.Hide;
        }

        private void OnDisable()
        {
            BuildingLot.ShowInfoWindow -= _infoWindow.Show;
            BuildingLot.HideInfoWindow -= _infoWindow.Hide;
        }
    }
}