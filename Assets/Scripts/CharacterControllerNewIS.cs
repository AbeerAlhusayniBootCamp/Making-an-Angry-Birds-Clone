using UnityEngine;
using UnityEngine.InputSystem;

namespace InputExamples
{
    /// <summary>
    /// Moves a CharacterController attached to the same GameObject.
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(CharacterController))]

    public class CharacterControllerNewIS : MonoBehaviour
{
        #region Serialized Fields

        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _gravity = -5f;

        #endregion

        #region Fields Private

        private CharacterController _characterController;
        private Vector2 _moveVector;
        private Transform _transform;

        #endregion

        #region Properties Public
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        public float Gravity
        {
            get => _gravity;
            set => _gravity = value;
        }
        #endregion

        private void Awake()
        {
            _transform = transform;
            _characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            Vector3 moveBy = (_transform.forward * _moveVector.y) + (_transform.right * _moveVector.x);

            if (moveBy.magnitude > 1f)
                moveBy.Normalize();

            moveBy *= Speed;
            moveBy.y += Gravity;

            _characterController.Move(moveBy * Time.deltaTime);
        }
        public void Move(InputAction.CallbackContext input)
        {
            _moveVector = input.ReadValue<Vector2>();

            if (_moveVector.magnitude > 1f)
                _moveVector.Normalize();
        }
    }
}
