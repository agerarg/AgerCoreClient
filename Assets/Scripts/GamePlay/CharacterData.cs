using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterData : MonoBehaviour
{
    public TextMeshProUGUI playerNameTxt;
    public static CharacterData instance;
    public Json_Character basicData;
    private void Awake()
    {
        instance = this;

        basicData = JsonUtility.FromJson<Json_Character>(PlayerPrefs.GetString("character"));

        playerNameTxt.SetText(basicData.name);
    }


}
