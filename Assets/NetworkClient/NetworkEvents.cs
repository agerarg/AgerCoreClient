using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NetworkEvents : MonoBehaviour
{
    public static NetworkEvents instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
        instance = this;

    }
    public static Action<bool, string> login;
    public static Action<Json_CharacterList> characterList;
    public static Action<int> LoadCharacter;
    public static Action<Json_Character> CharacterEnter;

    public static Action<int,Vector3> PortalOpen;
    public static Action<Json_MapLoad> LoadMap;
    public static Action NewMapLoaded;
    public static Action MapFreshStart;

    public static Action<int, float, float> PlayerMovement;

    public static Action<Json_Player> NewPlayer;
    public static Action<int> RemovePlayer;

    public static Action<Json_Mob> NewMob;
    public static Action<int> DeadMob;
    public static Action<GameObject> Targeted;

    public static Action<Json_Skill> CastSkill;

}
