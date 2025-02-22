using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject /* buff,debuff, */hpChangeDisplay;
    float timeCounter;

    // Update is called once per frame
    void Update()
    {
        /* if (buff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f)
            {
                timeCounter = 0f;
                buff.SetActive(false);
            }
        }
        if (debuff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f)
            {
                timeCounter = 0f;
                debuff.SetActive(false);
            }
        } */
        if (hpChangeDisplay.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f)
            {
                timeCounter = 0f;
                hpChangeDisplay.SetActive(false);
            }
        }
    }
}
