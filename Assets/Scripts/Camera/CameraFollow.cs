using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothDamp = 1f;
    [SerializeField] private Transform playerTransform;

    private Vector3 _velocity;

    void Update()
    {
        if (!playerTransform) return;

        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerTransform.position, ref _velocity, smoothDamp);
        targetPosition.z = -10;
        transform.position = targetPosition;
    }
}
