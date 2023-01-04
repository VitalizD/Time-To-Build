using UI.BuildingPanel;
using UnityEngine;

namespace CameraEngine
{
    [RequireComponent(typeof(ScreenRaycaster))]
    public class CameraHandler : MonoBehaviour
    {
        private ScreenRaycaster _screenRaycaster;

        private void Awake()
        {
            _screenRaycaster = GetComponent<ScreenRaycaster>();
        }

        private void OnEnable()
        {
            BuildingPanel.GetScreenRaycastResults += _screenRaycaster.GetScreenRaycastResults;
            CameraMoving.GetScreenRaycastResults += _screenRaycaster.GetScreenRaycastResults;
        }

        private void OnDisable()
        {
            BuildingPanel.GetScreenRaycastResults -= _screenRaycaster.GetScreenRaycastResults;
            CameraMoving.GetScreenRaycastResults -= _screenRaycaster.GetScreenRaycastResults;
        }
    }
}