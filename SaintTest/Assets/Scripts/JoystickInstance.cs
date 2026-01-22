using UnityEngine;

public class JoystickInstance : MonoBehaviour
{
    [SerializeField] private FixedJoystick _joystick;
    public static FixedJoystick Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = _joystick;
    }
}
