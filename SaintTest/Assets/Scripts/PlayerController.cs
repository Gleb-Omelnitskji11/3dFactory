using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _moveSpeed;
    
    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical) * _moveSpeed;

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            _player.rotation = Quaternion.LookRotation(_rigidBody.velocity);
        }
    }
}
