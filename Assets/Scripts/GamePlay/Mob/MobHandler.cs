using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHandler : MonoBehaviour
{
    public static MobHandler instance;
    public MobMovement mobMovementPrefab;
    public Dictionary<int, MobMovement> AreaMobs = new();
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        NetworkEvents.DeadMob += OnDeadMob;
        NetworkEvents.NewMob += OnNewMob;
        NetworkEvents.MapFreshStart += OnFreshStart;
    }
    private void OnDisable()
    {
        NetworkEvents.DeadMob -= OnDeadMob;
        NetworkEvents.NewMob -= OnNewMob;
        NetworkEvents.MapFreshStart -= OnFreshStart;
    }

    private void OnDeadMob(int obj)
    {
        MobMovement mm = AreaMobs[obj];

        AreaMobs.Remove(obj);

        Destroy(mm.gameObject);
    }

    private void OnFreshStart()
    {
        AreaMobs.Clear();
       foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    private void OnNewMob(Json_Mob obj)
    {
       if(AreaMobs.ContainsKey(obj.uniqueId))
        {
            AreaMobs[obj.uniqueId].Setup(obj);
        }
       else
        {
            MobMovement newMob = Instantiate(mobMovementPrefab, new Vector3(obj.x, 0.5f, obj.y), Quaternion.identity, transform);
            newMob.Setup(obj);
            AreaMobs.Add(obj.uniqueId, newMob);
        }
    }
}
