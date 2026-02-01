using UnityEngine;

public interface IInputController
{
    public float Horizontal { get; }
    public float Vertical { get; }
    public Vector2 Direction { get; }
}