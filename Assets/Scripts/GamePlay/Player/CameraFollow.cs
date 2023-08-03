using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float minAngle = 5f; // Minimum angle allowed (looking down towards the floor)
    public float maxAngle = 85f; // Maximum angle allowed (looking up towards the sky)

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float rotationSpeed = 2f;
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 10f;
    public float zoomSpeed = 2f;

    private float currentZoomDistance = 5f;
    private float rotationX = 0f;
    private float lastZoomInput;

    private Quaternion initialRotation;
    private Vector3 initialOffset;
    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned in the inspector.");
            return;
        }

        // Store the initial rotation and offset relative to the target
        initialRotation = Quaternion.Euler(transform.eulerAngles);
        initialOffset = transform.position - target.position;
    }
    private void LateUpdate()
    {
        if (target == null) return;

        // Rotation with the mouse
        if (Input.GetMouseButton(1))
        {
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed;

            rotationX -= verticalInput;
            rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle); // Limit the vertical rotation angle

            Vector3 rotation = new Vector3(rotationX, transform.eulerAngles.y + horizontalInput, 0f);
            transform.rotation = Quaternion.Euler(rotation);

            // Apply the rotation to the offset to maintain the relative position to the target
            //offset = Quaternion.Euler(rotation) * Vector3.back * currentZoomDistance;
            offset = Quaternion.Euler(rotation) * initialOffset;
        }
        // Zoom in/out with the mouse scroll wheel
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (lastZoomInput != zoomInput)
        {
            lastZoomInput = zoomInput;
            currentZoomDistance -= zoomInput * zoomSpeed;
            currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
        }
        // Smoothly update the camera position and rotation
        Vector3 desiredPosition = target.position + offset * currentZoomDistance;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
