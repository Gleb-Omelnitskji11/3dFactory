using System;

namespace Game.Input
{
    public static class InputInstanceHolder
    {
        public static IInputController Instance { get; private set; }

        public static void UpdateInputInstance(IInputController inputController)
        {
            Instance = inputController;
            OnUpdateInputInstance?.Invoke(Instance);
        }
        
        public static event Action<IInputController> OnUpdateInputInstance;
    }
}