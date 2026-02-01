using UnityEngine;

namespace Game.Input
{
    public class JoystickInputController : MonoBehaviour, IInputController
    {
        [SerializeField] private FixedJoystick _joystick;

        public float Horizontal => _joystick.Horizontal;
        public float Vertical => _joystick.Vertical;
        public Vector3 Direction => new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        private void Awake()
        {
            InputInstanceHolder.UpdateInputInstance(this);
        }
    }
}
