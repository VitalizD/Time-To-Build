using Gameplay.Buildings;
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
            BuildingPanel.CursorOverUIElement += _screenRaycaster.CursorOverUIElement;
            CameraMoving.GetScreenRaycastResults += _screenRaycaster.GetScreenRaycastResults;
            BuildingArea.CursorOverUIElement += _screenRaycaster.CursorOverUIElement;
        }

        private void OnDisable()
        {
            BuildingPanel.CursorOverUIElement -= _screenRaycaster.CursorOverUIElement;
            CameraMoving.GetScreenRaycastResults -= _screenRaycaster.GetScreenRaycastResults;
            BuildingArea.CursorOverUIElement -= _screenRaycaster.CursorOverUIElement;
        }
    }
}