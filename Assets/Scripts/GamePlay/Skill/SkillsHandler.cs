using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsHandler : MonoBehaviour
{
    public FireBall fireBallPrefab;


    private void OnEnable()
    {
        NetworkEvents.CastSkill += OnCastSkill;
    }
    private void OnDisable()
    {
        NetworkEvents.CastSkill -= OnCastSkill;
    }

    private void OnCastSkill(Json_Skill obj)
    {
        MobMovement mm = MobHandler.instance.AreaMobs[obj.toMob];
        if (mm != null)
        {
            if (obj.fromId == CharacterData.instance.basicData.id)
            {
                FireBall fb = Instantiate(fireBallPrefab, PointAndClickMovement.instance.transform.position, Quaternion.identity, transform);
                fb.Setup(obj, mm.transform);
            }
            else
            {
                OtherPlayerMovement opm = OtherPlayersSpowner.instance.ConnectedPlayers[obj.fromId];
                if (opm != null)
                {
                    FireBall fb = Instantiate(fireBallPrefab, opm.transform.position, Quaternion.identity, transform);
                    fb.Setup(obj, mm.transform);
                }
            }
        }
    }
}
