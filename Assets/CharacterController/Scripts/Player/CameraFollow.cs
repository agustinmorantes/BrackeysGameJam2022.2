using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float smoothSpeed = 0.125f;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 endPosition = target.position;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, endPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }
}
