using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillUsage : MonoBehaviour
{
    public Image cooldownImage;

    public bool isCoolingDown = false;

    public float cooldownTime = 2f;

    private GameObject target;
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
        target = obj;
    }


    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Alpha1) )
        {
            if (!isCoolingDown)
            {
                if (target != null)
                {
                    MobMovement mob = target.GetComponent<MobMovement>();
                    if (mob != null)
                    {
                        cooldownImage.fillAmount = 1;
                        isCoolingDown = true;
                        StartCoroutine(CooldownCoroutine(cooldownTime));
                        NetworkClient.instance.SendDataToServer("CallSkill", new string[,] { { "skillId", "1" }, { "mobTarget", mob.data.uniqueId.ToString() } });
                    }
                }
            }
        }

    }
    private IEnumerator CooldownCoroutine(float cooldownTime)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < cooldownTime)
        {
            cooldownImage.fillAmount = 1.0f - (elapsedTime / cooldownTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cooldownImage.fillAmount = 0.0f;
        isCoolingDown = false;
    }


}
