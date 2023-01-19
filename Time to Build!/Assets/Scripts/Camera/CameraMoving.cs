using System;
using UI;
using UnityEngine;

namespace CameraEngine
{
    [RequireComponent(typeof(CameraBounds))]
    public class CameraMoving : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _minY = 20f;
        [SerializeField] private float _maxY = 60f;
        [SerializeField] private float _minRotateX;
        [SerializeField] private float _maxRotateX;
        [SerializeField] private float _scrollSpeed = 1000f;
        [SerializeField] private float _scrollSmoothness = 1f;
        [SerializeField] private Transform _camera;

        private CameraBounds _bounds;
        private float _currentY = 50f;
        private float _reachedY;

        public static event Func<GameObject[]> GetScreenRaycastResults;

        private void Awake()
        {
            _bounds = GetComponent<CameraBounds>();
            _currentY = transform.position.y;
            _reachedY = _currentY;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                Move();
            if (Input.GetKey(KeyCode.E))
                Rotate(-Vector3.up);
            if (Input.GetKey(KeyCode.Q))
                Rotate(Vector3.up);

            SetPosition();

            var hits = GetScreenRaycastResults?.Invoke();
            foreach (var obj in hits)
            {
                if (obj.GetComponent<UIElement>() != null)
                    return;
            }

            Scroll();
        }

        private void Scroll()
        {
            var wheelValue = Input.GetAxis("Mouse ScrollWheel");
            if (wheelValue != 0f)
            {
                _reachedY -= wheelValue * Time.deltaTime * _scrollSpeed;
                _reachedY = Mathf.Clamp(_reachedY, _minY, _maxY);
            }

            _currentY = Mathf.Lerp(_currentY, _reachedY, Time.deltaTime * _scrollSmoothness);
            _camera.eulerAngles = new Vector3(Mathf.Lerp(_minRotateX, _maxRotateX, (_currentY - _minY) / (_maxY - _minY)), _camera.eulerAngles.y, 0f);
        }

        private void SetPosition()
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, _bounds.LeftLimit, _bounds.RightLimit),
                _currentY,
                Mathf.Clamp(transform.position.z, _bounds.BottomLimit, _bounds.TopLimit));
        }

        private void Move()
        {
            if (Input.GetKey(KeyCode.W))
                transform.position += _moveSpeed * Time.fixedDeltaTime * transform.forward;
            if (Input.GetKey(KeyCode.S))
                transform.position -= _moveSpeed * Time.fixedDeltaTime * transform.forward;
            if (Input.GetKey(KeyCode.A))
                transform.position -= _moveSpeed * Time.fixedDeltaTime * transform.right;
            if (Input.GetKey(KeyCode.D))
                transform.position += _moveSpeed * Time.fixedDeltaTime * transform.right;
        }

        private void Rotate(Vector3 dir)
        {
            var expectedAngle = transform.eulerAngles + dir * _rotateSpeed * Time.fixedDeltaTime;
            var toAngle = new Vector3(
                transform.eulerAngles.x,
                expectedAngle.y,
                transform.eulerAngles.z);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, toAngle, _rotateSpeed * Time.fixedDeltaTime);
        }
    }
}