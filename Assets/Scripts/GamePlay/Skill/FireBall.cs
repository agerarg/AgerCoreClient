using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Json_Skill data;

    public Transform target;

    public float speed = 20;


    public void Setup(Json_Skill data,Transform target)
    {
        this.data = data;
        this.target = target;
    }
    void Update()
    {
        if(target!=null)
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Mob"))
        {
            MobMovement mm = other.gameObject.GetComponent<MobMovement>();
            if(mm != null)
            {
                if(mm.data.uniqueId == data.toMob)
                {
                    mm.Damage(data.fromId, data.damage);
                    Destroy(gameObject);
                }
            }

        }
    }
}
