using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CameraEngine
{
    public class ScreenRaycaster : MonoBehaviour
    {
        public GameObject[] GetScreenRaycastResults()
        {
            var pointer = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            return raycastResults.Select(element => element.gameObject).ToArray();
        }
    }
}
