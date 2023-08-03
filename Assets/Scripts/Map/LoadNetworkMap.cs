using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadNetworkMap : MonoBehaviour
{
    public string partialSceneName;
    private void OnEnable()
    {
        NetworkEvents.LoadMap += OnLoadMap;
        NetworkEvents.PortalOpen += OnPortalOpen;
    }
    private void OnDisable()
    {
        NetworkEvents.LoadMap -= OnLoadMap;
        NetworkEvents.PortalOpen -= OnPortalOpen;
    }
    public void OnPortalOpen(int newMap,Vector3 targetPosition)
    {
        SceneManager.UnloadSceneAsync(partialSceneName);
        NetworkClient.instance.SendDataToServer("CallDisconnected", new string[,] { });
        PointAndClickMovement.instance.Teleport(targetPosition);
        StartCoroutine(EnterNewArea(newMap));

    }
    IEnumerator EnterNewArea(int newMap)
    {
        yield return new WaitForSeconds(1);
        CharacterData.instance.basicData.mapLocation = newMap;
        LoadMap();
    }
    private void OnLoadMap(Json_MapLoad obj)
    {

       // Debug.Log("OnLoadMap: " + obj.name);
        partialSceneName = "map_" + obj.name;
        StartCoroutine(LoadSceneAsync());
     
    }
    private IEnumerator LoadSceneAsync()
    {
        // Begin loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(partialSceneName, LoadSceneMode.Additive);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            // Optionally, you can display a loading progress bar or other loading feedback here.
            yield return null;
        }
        NetworkEvents.NewMapLoaded?.Invoke();
    }

    void Start()
    {
        LoadMap();
    }

    void LoadMap()
    {
        NetworkEvents.MapFreshStart?.Invoke();
        int mapId = CharacterData.instance.basicData.mapLocation;

        NetworkClient.instance.SendDataToServer("CallEnterArea", new string[,] { { "area", mapId.ToString() } });
    }
   
}
