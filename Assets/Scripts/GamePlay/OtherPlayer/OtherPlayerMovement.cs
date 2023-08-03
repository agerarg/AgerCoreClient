using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OtherPlayerMovement : MonoBehaviour
{
    public TextMeshProUGUI playerNameTxt;
    public int playerId=0;
    public float speed = 5f;
    public Json_Player data;
    private Vector3 serverPositionUpdate;
    private void OnEnable()
    {
        NetworkEvents.PlayerMovement += OnPlayerMovement;
        NetworkEvents.MapFreshStart += OnFreshStart;
    }

    private void OnDisable()
    {
        NetworkEvents.PlayerMovement -= OnPlayerMovement;
        NetworkEvents.MapFreshStart -= OnFreshStart;
    }
    public void Remove()
    {
        Destroy(gameObject);
    }
    private void OnFreshStart()
    {
        Destroy(gameObject);
    }



    public void Setup(Json_Player data)
    {
        playerId = data.id;
        this.data = data;
        playerNameTxt.SetText(data.name);
        serverPositionUpdate = new Vector3(data.positionX, transform.position.y, data.positionY);
    }

    private void OnPlayerMovement(int arg1, float arg2, float arg3)
    {
        if (arg1 == playerId)
        {
            serverPositionUpdate = new Vector3(arg2, transform.position.y, arg3);
        }
    }

    private void Update()
    {
        if (IsMoving())
        {
            Move();
        }
    }

    private bool IsMoving()
    {
        return (transform.position - serverPositionUpdate).sqrMagnitude > 0.01f;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, serverPositionUpdate, speed * Time.deltaTime);
    }
}
