using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayersSpowner : MonoBehaviour
{
    public static OtherPlayersSpowner instance;
    public OtherPlayerMovement otherPlayerPrefb;

    public Dictionary<int, OtherPlayerMovement> ConnectedPlayers = new();

    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        NetworkEvents.NewPlayer += OnNewPlayer;
        NetworkEvents.RemovePlayer += OnRemovePlayer;
        NetworkEvents.MapFreshStart += OnFreshStart;
    }

    private void OnDisable()
    {
        NetworkEvents.NewPlayer -= OnNewPlayer;
        NetworkEvents.RemovePlayer -= OnRemovePlayer;
        NetworkEvents.MapFreshStart -= OnFreshStart;
    }
    public void OnFreshStart()
    {
        ConnectedPlayers.Clear();

    }
    private void OnRemovePlayer(int id)
    {
        foreach (KeyValuePair<int, OtherPlayerMovement> p in ConnectedPlayers)
        {
            if (p.Value.data.id == id)
            {
                ConnectedPlayers.Remove(p.Key);
                p.Value.Remove();
                return;
            }
        }
    }

    private void OnNewPlayer(Json_Player newPlayer)
    {
        if (newPlayer.id != CharacterData.instance.basicData.id)
        {
            if(!ConnectedPlayers.ContainsKey(newPlayer.id))
            {
                Vector3 playerPosition = new(newPlayer.positionX, 0.5f, newPlayer.positionY);
                OtherPlayerMovement newOtherPlayer = Instantiate(otherPlayerPrefb, playerPosition, Quaternion.identity, transform);
                newOtherPlayer.Setup(newPlayer);
                ConnectedPlayers.Add(newPlayer.id,newOtherPlayer);
            }
        }
    }
}
