using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OnLoadCharacter : MonoBehaviour
{
    private void OnEnable()
    {
        NetworkEvents.LoadCharacter += OnLoadCharacterTrigger;
        NetworkEvents.CharacterEnter += OnCharacterEnter;
    }
    private void OnDisable()
    {
        NetworkEvents.LoadCharacter -= OnLoadCharacterTrigger;
        NetworkEvents.CharacterEnter -= OnCharacterEnter;
    }
    
    // Player has click in some character, we send to the server we want to load all data from that character!
    private void OnLoadCharacterTrigger(int id)
    {
        NetworkClient.instance.SendDataToServer("CallCharacterEnter", new string[,] { { "characterId", id.ToString() } });

    }
    // We recive all data from character, now we change scene to the game map
    private void OnCharacterEnter(Json_Character obj)
    {
        PlayerPrefs.SetString("character", JsonUtility.ToJson(obj));

        SceneManager.LoadScene("MapScene");
    }

}
