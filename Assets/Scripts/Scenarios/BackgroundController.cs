using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController instance;

    public GameObject mrtOutsideBG;
    public GameObject waitingForMrtBG;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeTo(string name)
    {
        mrtOutsideBG.SetActive(false);
        waitingForMrtBG.SetActive(false);

        switch (name)
        {
            case "MRTOutside": waitingForMrtBG.SetActive(true); break;
            case "WaitingForMRT": mrtOutsideBG.SetActive(true); break;
        }
    }
}
