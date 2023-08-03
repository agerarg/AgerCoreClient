using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int portalTo;

    public Vector3 spownPosition;

    private bool isTriggered=false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            NetworkEvents.PortalOpen?.Invoke(portalTo, spownPosition);
        }
    }


}
