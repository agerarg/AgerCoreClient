using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoadScreen : MonoBehaviour
{

    public GameObject loadingObj;
    void Start()
    {
        loadingObj.SetActive(true);
    }

    private void OnEnable()
    {
        NetworkEvents.NewMapLoaded += OnNewMapLoaded;
        NetworkEvents.PortalOpen += OnPortalOpen;
    }
    private void OnDisable()
    {
        NetworkEvents.NewMapLoaded -= OnNewMapLoaded;
        NetworkEvents.PortalOpen -= OnPortalOpen;
    }

    private void OnPortalOpen(int obj,Vector3 pos)
    {
        loadingObj.SetActive(true);
    }

    private void OnNewMapLoaded()
    {
        loadingObj.SetActive(false);
    }
}
