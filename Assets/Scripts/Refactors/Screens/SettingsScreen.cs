using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsScreen
{
    public SettingsScreen()
    {
        Transform screen = GameObject.Find("Canvas").transform.Find("SettingsScreen").Find("Screen");

        screen.Find("BTNSlow").GetComponent<Button>().onClick.AddListener(()=>OnClickSpeedButton(2));
        screen.Find("BTNMedium").GetComponent<Button>().onClick.AddListener(() => OnClickSpeedButton(4));
        screen.Find("BTNFast").GetComponent<Button>().onClick.AddListener(() => OnClickSpeedButton(8));

    }

    void OnClickSpeedButton(int speed)
    {
        BGGameSetting.speed = speed;
    }        


}
