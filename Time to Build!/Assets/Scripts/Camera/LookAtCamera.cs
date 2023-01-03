using UnityEngine;

namespace CameraEngine
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _camera;

        private void Awake()
        {
            _camera = Camera.main.transform;
        }

        private void Update()
        {
            transform.LookAt(_camera.position);
            //transform.Rotate(new Vector3(180, 180, 180));
        }
    }
}
