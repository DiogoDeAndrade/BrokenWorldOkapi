using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSoundFilter : MonoBehaviour
{
    TimeScaler2d        timeScaler;
    AudioLowPassFilter  lowPassFilter;

    void Start()
    {
        timeScaler = GetComponent<TimeScaler2d>();
        lowPassFilter = Camera.main.GetComponent<AudioLowPassFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((lowPassFilter) && (timeScaler))
        {
            if (timeScaler.timeScale < 1.0f)
            {
                lowPassFilter.enabled = true;
            }
            else
            {
                lowPassFilter.enabled = false;
            }
        }
    }
}
