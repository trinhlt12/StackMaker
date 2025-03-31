namespace _GAME.Scripts.Camera
{
    using System;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }

        [SerializeField] private Transform target;
        [SerializeField] private float distance = 10f;
        [SerializeField] private float height = 10f;
        [SerializeField] private float rotationSpeed = 0.2f;

        private float _currentAngle = 0f;
        private float _defaultAngle = 0f;
        private bool _isReturningToDefaultAngle = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void LateUpdate()
        {
            if(this.target == null) return;

            var offset = Quaternion.Euler(0, this._currentAngle, 0) * Vector3.back * this.distance;
            offset.y = this.height;

            var desiredPosition = this.target.position + offset;
            this.transform.position = Vector3.Lerp(this.transform.position, desiredPosition, Time.deltaTime * 5);
            transform.LookAt(target);

            if (this._isReturningToDefaultAngle)
            {
                this._currentAngle = Mathf.Lerp(this._currentAngle, this._defaultAngle, Time.deltaTime * 5f);
                if (Mathf.Abs(this._currentAngle - this._defaultAngle) < 0.1f)
                {
                    this._currentAngle              = this._defaultAngle;
                    this._isReturningToDefaultAngle = false;
                }
            }

        }

        public void RotateCamera(float deltaX)
        {
            /*
            if(this._isReturningToDefaultAngle) return;
            */
            this._currentAngle += deltaX * this.rotationSpeed;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void ReturnToDefaultAngle()
        {
            this._isReturningToDefaultAngle = true;
        }
    }
}