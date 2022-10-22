using UnityEngine;
using UnityEngine.InputSystem;

namespace InputExamples
{
    [DisallowMultipleComponent]
    public class RotationMotor : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private bool _verticalInverted = false;
        [SerializeField] private bool _horizontalInverted = false;
        [SerializeField] private Transform _horizontalTransform;
        [SerializeField] private Transform _verticalTransform;
        [SerializeField, Min(0f)] private float _horizontalSpeed = 0.1f;
        [SerializeField, Min(0f)] private float _verticalSpeed = 0.1f;
        [SerializeField, Range(0f, 180f)] private float _verticalRangeUp = 40f;
        [SerializeField, Range(0f, 180f)] private float _verticalRangeDown = 40f;

        #endregion
        #region Properties Public

        // These can be accessed via external scripts referencing this one at runtime.

        public bool VerticalInverted
        {
            get => _verticalInverted;
            set => _verticalInverted = value;
        }

        public bool HorizontalInverted
        {
            get => _horizontalInverted;
            set => _horizontalInverted = value;
        }

        public Transform HorizontalTransform
        {
            get => _horizontalTransform;
            set => _horizontalTransform = value;
        }
        public Transform VerticalTransform
        {
            get => _verticalTransform;
            set => _verticalTransform = value;
        }

        public float HorizontalSpeed
        {
            get => _horizontalSpeed;
            set => _horizontalSpeed = value;
        }
        public float VerticalSpeed
        {
            get => _verticalSpeed;
            set => _verticalSpeed = value;
        }

        public float VerticalRangeUp
        {
            get => _verticalRangeUp;
            set => _verticalRangeUp = value;
        }

        public float VerticalRangeDown
        {
            get => _verticalRangeDown;
            set => _verticalRangeDown = value;
        }

        #endregion
        /// <summary>
        /// This can be called from the PlayerInput component movement event in the inspector.
        /// </summary>
        public void Rotate(InputAction.CallbackContext input)
        {
            var moveVector = input.ReadValue<Vector2>();

            if (HorizontalTransform != null)
                HandleHorizontal(HorizontalTransform, moveVector.x, HorizontalSpeed, _horizontalInverted);

            if (VerticalTransform != null)
                HandleVertical(VerticalTransform, moveVector.y, VerticalSpeed, VerticalRangeUp, VerticalRangeDown, _verticalInverted);
        }

        private void HandleHorizontal(Transform t, float val, float speed, bool inverted)
        {
            if (!inverted)
                val *= -1f;

            Vector3 rot = t.localEulerAngles;
            rot.y += speed * val;
            t.localEulerAngles = rot;
        }
        private void HandleVertical(Transform t, float val, float speed, float rangeUp, float rangeDown, bool inverted)
        {
            if (!inverted)
                val *= -1f;

            Vector3 rot = t.localEulerAngles;
            rot.x += speed * val;

            // We have to clamp based on the hemisphere since Euler angles work like a clock.
            if (rot.x > 180f)
            {
                // Upper hemisphere.
                // 360 - _verticalRange
                if (rot.x < 360 - rangeUp)
                    rot.x = 360 - rangeUp;
            }
            else
            {
                // Lower hemisphere.
                // 0 + _verticalRange
                if (rot.x > rangeDown)
                    rot.x = rangeDown;
            }

            t.localEulerAngles = rot;
        }
    }
}
