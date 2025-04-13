using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController instance;

    public GameObject mrtOutsideBG;
    public GameObject waitingForMrtBG;
    public GameObject mrtInsideBG;


    private void Start()
    {
        // Let's say you want MRTOutside as the default visible one
        waitingForMrtBG.SetActive(true);
        mrtOutsideBG.SetActive(false);
        mrtInsideBG.SetActive(false);

    }

    private void Awake()
    {
        instance = this;
    }

    public void ChangeTo(string name)
    {
        Debug.Log($"i am inside the ChangeTo in background controller. bg name: {name}");
        mrtOutsideBG.SetActive(false);
        waitingForMrtBG.SetActive(false);
        mrtInsideBG.SetActive(false);
        Debug.Log("mrtOutsideBG name: " + mrtOutsideBG.name);
        Debug.Log("waitingForMrtBG name: " + waitingForMrtBG.name);
        Debug.Log($"shld be all set unactive alr");


        switch (name)
        {
            case "MRTOutside": mrtOutsideBG.SetActive(true); break;
            case "WaitingForMRT": waitingForMrtBG.SetActive(true); break;
            case "MRTInside": mrtInsideBG.SetActive(true); break;

        }
    }
}
