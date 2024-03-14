using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ErrorNotification
{
    private TextMeshProUGUI tmpnotification;
    private Animator anim;

    private bool isopen;

    private float currentime;
    public ErrorNotification()
    {
        Transform achievement = GameObject.Find("Canvas").transform.Find("ErrorNotification");

        tmpnotification = achievement.Find("ErrorWindow").Find("TMPError").GetComponent<TextMeshProUGUI>();

        anim = achievement.GetComponent<Animator>();


        Setup.update += TimeUpdate;
        GameData.OnReset += OnReset;
    }


    public void SetMessage(string text,int time)
    {
        tmpnotification.text = text;

        if (!isopen) anim.SetTrigger("Open");

        isopen = true;

        currentime = Time.unscaledTime + time;

    }
    
    
    void TimeUpdate()
    {
        if (!isopen) return;

        if (Time.unscaledTime >= currentime)
        {
            anim.SetTrigger("Close");
            isopen = false;
        }
    }    

    public void OnReset()
    {
        Setup.update -= TimeUpdate;
        GameData.OnReset -= OnReset;
    }
}


public struct DataWarningNotification
{

    public static System.Action<string, int> SetMessage;

}
