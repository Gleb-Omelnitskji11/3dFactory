using UnityEngine;

public class JoystickInputController : IInputController
{
    private FixedJoystick _joystick;

    public float Horizontal => _joystick.Horizontal;
    public float Vertical => _joystick.Vertical;
    public Vector2 Direction => _joystick.Direction;

    public void Setup(FixedJoystick joystick)
    {
        _joystick = joystick;
    }
}