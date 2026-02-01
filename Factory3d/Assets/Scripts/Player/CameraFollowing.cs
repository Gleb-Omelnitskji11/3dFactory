using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    private Vector3 _offset;

    private void Awake()
    {
        _offset = transform.position - _target.position;
    }

    private void LateUpdate()
    {
        var targetPosition = _target.position + _offset;
        transform.position = targetPosition;
    }
}
