using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _moveSpeed;

    private FixedJoystick _joystick;

    private void Start()
    {
        _joystick = JoystickInstance.Instance;
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical) * _moveSpeed;

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            _player.rotation = Quaternion.LookRotation(_rigidBody.velocity);
        }
    }
}