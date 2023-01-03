using UnityEngine;

namespace CameraEngine
{
    public class CameraBounds : MonoBehaviour
    {
        [SerializeField] private float topLimit;
        [SerializeField] private float bottomLimit;
        [SerializeField] private float leftLimit;
        [SerializeField] private float rightLimit;

        public float TopLimit { get => topLimit; }

        public float BottomLimit { get => bottomLimit; }

        public float LeftLimit { get => leftLimit; }

        public float RightLimit { get => rightLimit; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var y = transform.position.y;
            Gizmos.DrawLine(new Vector3(leftLimit, y, topLimit), new Vector3(rightLimit, y, topLimit));
            Gizmos.DrawLine(new Vector3(leftLimit, y, bottomLimit), new Vector3(rightLimit, y, bottomLimit));
            Gizmos.DrawLine(new Vector3(leftLimit, y, topLimit), new Vector3(leftLimit, y, bottomLimit));
            Gizmos.DrawLine(new Vector3(rightLimit, y, topLimit), new Vector3(rightLimit, y, bottomLimit));
        }
    }
}
