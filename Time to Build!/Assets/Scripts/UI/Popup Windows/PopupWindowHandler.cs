using UI.BuildingPanel;
using UI.Counters.Population;
using UnityEngine;

namespace UI.PopupWindows
{
    [RequireComponent(typeof(PopupWindow))]
    public class PopupWindowHandler : MonoBehaviour
    {
        private PopupWindow _popupWindow;

        private void Awake()
        {
            _popupWindow = GetComponent<PopupWindow>();
        }

        private void OnEnable()
        {
            PopulationRequirement.ShowPopupWindow += _popupWindow.Show;
            BuildingLot.ShowPopupWindow += _popupWindow.Show;
        }

        private void OnDisable()
        {
            PopulationRequirement.ShowPopupWindow -= _popupWindow.Show;
            BuildingLot.ShowPopupWindow -= _popupWindow.Show;
        }
    }
}