using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    void Ray()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform the raycast and get an array of all hits
        RaycastHit[] hits = Physics.RaycastAll(ray);

        // Process the hits
        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            //Vector3 hitPoint = hit.point;

            ITargeteable target = hitObject.GetComponent<ITargeteable>();
            if(target!=null)
            {
                target.Active();
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray();
        }
    }
}
