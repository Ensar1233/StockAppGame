using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class AchievementNotifications
{

    private TextMeshProUGUI tmpnotification;
    private Image imgIcon;

    private Animator anim;

    private bool isopen;

    private float currentime;
    public AchievementNotifications()
    {
        Transform achievement = GameObject.Find("Canvas").transform.Find("AchievementNotifications");

        tmpnotification = achievement.GetChild(0).GetComponent<TextMeshProUGUI>();
        imgIcon = achievement.GetChild(1).GetComponent<Image>();

        anim = achievement.GetComponent<Animator>();

        Setup.update += TimeUpdate;
        GameData.OnReset += OnReset;

    }

    public void SetNotify(Sprite icon,string text,int repeatime) // gelistirelecek.
    {

        tmpnotification.text = $"{text}";
        if(icon!=null) imgIcon.sprite = icon;

        if (!isopen) anim.SetTrigger("Open");

        isopen = true;

        currentime = Time.unscaledTime + repeatime;

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









public struct DataAchievementNotifications
{
    public static System.Action<Sprite,string, int> SetNotify;

    public static UnityAction CloseAnimation;
}

public struct BGIcons
{


    public static Sprite BUY = Path("Buy");
    public static Sprite SELL = Path("Sell");
    public static Sprite DEAL = Path("deal");
    public static Sprite CREDITPAY = Path("creditpay");
    public static Sprite CREDITCARD = Path("credit-card");
    public static Sprite INSTRUCTION = Path("orientation");
    public static Sprite SAVEGAME = Path("savegame");
    public static Sprite POSITIVE = Path("positive");
    public static Sprite NEGATIVE = Path("negative");
    public static Sprite CONGI = Path("congi");

    private static Sprite Path(string iconfile)
    {

        return Resources.Load<Sprite>($"Notifications/{iconfile}");
    }

}
