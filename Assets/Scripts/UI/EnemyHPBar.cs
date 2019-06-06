using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public Image greenbar;
    public Image redbar;

    [HideInInspector]
    public float percentHP = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
        if(redbar.fillAmount > percentHP)
        {
            redbar.fillAmount -= Time.deltaTime * (0.05f + (redbar.fillAmount - greenbar.fillAmount));
        }
    }

    public void UpdateBars(float percent)
    {
        percentHP = percent;
        greenbar.fillAmount = percentHP;
    }
}
