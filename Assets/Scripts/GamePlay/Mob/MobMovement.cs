using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour, ITargeteable, IDamageable
{
   public Json_Mob data;
    public bool isMoving = false;
    public Vector3 serverPositionUpdate;
    public float speed = 5;
    public GameObject targetFrame;
    private void Start()
    {
        targetFrame.SetActive(false);
    }
    private void OnEnable()
    {
        NetworkEvents.Targeted += OnTarget;
    }
    private void OnDisable()
    {
        NetworkEvents.Targeted -= OnTarget;
    }

    private void OnTarget(GameObject obj)
    {
        if(obj == gameObject)
            targetFrame.SetActive(true);
        else
            targetFrame.SetActive(false);
    }
    public void Active()
    {
        NetworkEvents.Targeted?.Invoke(gameObject);
    }

    public void Setup(Json_Mob data)
    {
        this.data = data;
        serverPositionUpdate = new Vector3(data.x, 0.5f, data.y);
    }
    private bool IsMoving()
    {
        return (transform.position - serverPositionUpdate).sqrMagnitude > 0.01f;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, serverPositionUpdate, speed * Time.deltaTime);
    }
    private void Update()
    {
        IsMoving();
        Move();
    }

    public void Damage(int fromId, float dmg)
    {
        NetworkClient.instance.SendDataToServer("CallMobDamage", new string[,] { { "mobId", data.uniqueId.ToString() }, { "damage", dmg.ToString() } });
    }
}
