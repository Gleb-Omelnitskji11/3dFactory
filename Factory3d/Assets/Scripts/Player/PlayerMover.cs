using Game.Input;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _moveSpeed;

        private IInputController _inputController;

        private void Start()
        {
            _inputController = InputInstance.Instance;
        }

        private void FixedUpdate()
        {
            _rigidBody.velocity = new Vector3(_inputController.Horizontal, 0, _inputController.Vertical) * _moveSpeed;

            if (_inputController.Horizontal != 0 || _inputController.Vertical != 0)
            {
                _player.rotation = Quaternion.LookRotation(_rigidBody.velocity);
            }
        }
    }
}
