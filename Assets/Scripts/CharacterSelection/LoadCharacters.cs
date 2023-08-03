using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacters : MonoBehaviour
{
    public Transform charPanel;
    public CharacterSelectionItem characteritem;
    private void OnEnable()
    {
        NetworkEvents.characterList += OnCharacterList;
    }
    private void OnDisable()
    {
        NetworkEvents.characterList -= OnCharacterList;
    }

    private void OnCharacterList(Json_CharacterList data)
    {
        CharacterSelectionItem newCharacterOption = Instantiate(characteritem, charPanel);
        newCharacterOption.Setup(data.id, data.name, "Level: " + data.level);
    }

    void Start()
    {
        foreach(Transform t in charPanel)
        {
            Destroy(t.gameObject);
        }
        NetworkClient.instance.SendDataToServer("CallCharactersList", new string[,] { });
    }

   
}
