using Game.Input;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed;

        private IInputController _inputController;

        private void Start()
        {
            ChangeInputController(InputInstanceHolder.Instance);
            InputInstanceHolder.OnUpdateInputInstance += ChangeInputController;
        }

        private void OnDestroy()
        {
            InputInstanceHolder.OnUpdateInputInstance -= ChangeInputController;
        }

        private void Update()
        {
            ApplyMovement();
            ApplyRotation();
        }

        private void ChangeInputController(IInputController inputController)
        {
            _inputController = inputController;
        }

        private void ApplyMovement()
        {
            _characterController.Move(_inputController.Direction * _moveSpeed);
        }
        
        private void ApplyRotation()
        {
            if (_inputController.Horizontal != 0 || _inputController.Vertical != 0)
            {
                _player.rotation = Quaternion.LookRotation(_inputController.Direction);
            }
        }
    }
}
