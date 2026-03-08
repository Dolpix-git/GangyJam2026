using UnityEngine;

public class ChildFollowAndRotate : MonoBehaviour
    {
        [SerializeField] private Transform _child;

        [SerializeField] private float _positionLerpSpeed = 5f;
        [SerializeField] private float _rotationLerpSpeed = 5f;

        [SerializeField] private float _maxRotationAngle = 10f;
        [SerializeField] private float _rotationMultiplier = 1f;
        private Quaternion _initialChildRotation;

        private Vector3 _previousWorldPosition;
        private RectTransform _rectTransform;

        public Transform Child => _child;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _previousWorldPosition = _rectTransform.TransformPoint(_rectTransform.rect.center);
            _initialChildRotation = _child.localRotation;

            var childRect = _child.GetComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(childRect.sizeDelta.x, childRect.sizeDelta.y);
        }

        private void LateUpdate()
        {
            var targetWorldPos = _rectTransform.TransformPoint(_rectTransform.rect.center);
            var newWorldPos = Vector3.Lerp(_previousWorldPosition, targetWorldPos, _positionLerpSpeed * Time.deltaTime);
            _child.localPosition = transform.InverseTransformPoint(newWorldPos);

            var movement = newWorldPos - _previousWorldPosition;
            _previousWorldPosition = newWorldPos;

            var angle = 0f;
            if (movement.x != 0)
            {
                angle = -movement.x * _rotationMultiplier;
                angle = Mathf.Clamp(angle, -_maxRotationAngle, _maxRotationAngle);
            }

            var targetRotation = _initialChildRotation * Quaternion.Euler(0f, 0f, angle);
            _child.localRotation = Quaternion.Lerp(
                _child.localRotation,
                targetRotation,
                _rotationLerpSpeed * Time.deltaTime
            );
        }

        private void OnDestroy()
        {
            Destroy(_child.gameObject);
        }
    }
