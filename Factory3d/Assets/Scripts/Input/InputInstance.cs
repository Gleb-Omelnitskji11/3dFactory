using UnityEngine;

namespace Game.Input
{
    public class InputInstance : MonoBehaviour
    {
        [SerializeField] private FixedJoystick _joystick;
        public static IInputController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            JoystickInputController joystickInputController = new JoystickInputController();
            joystickInputController.Setup(_joystick);
            Instance = joystickInputController;
        }
    }
}
