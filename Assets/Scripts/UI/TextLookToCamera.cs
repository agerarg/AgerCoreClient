using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLookToCamera : MonoBehaviour
{
    Transform mainCameraTransform;
    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }
    void Update()
    {
        // Ensure the mainCameraTransform reference is set
        if (mainCameraTransform == null)
        {
            Debug.LogWarning("Main Camera Transform reference is not set.");
            return;
        }

        // Calculate the rotation from the world space camera to face the main camera
        Quaternion lookRotation = Quaternion.LookRotation(mainCameraTransform.position - transform.position, Vector3.up);

        // Apply the calculated rotation to the world space camera
        transform.rotation = lookRotation;
    }
}
