using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TargetScreen
{

    //resetledikten sonra olan musterilerin butonlari pasifize edilecek.


    public TargetModel targetmodel;

    private Transform screen;
    private Sprite spritesucces, spritefailed;

    private Target[] arraytarget =
        {
        new Target(500000,()=> 
        {
            MainScreen.btnbank.interactable = true;            
        }),
        new Target(1000000,()=> 
        {
            Payments.TAX.taxamount +=0.02f; 
        }),
        new Target(3000000,()=> 
        {
            Payments.TAX.taxamount +=0.05f; 
        }),
        new Target(5000000,()=> 
        {
            Payments.TAX.taxamount +=0.1f;
        })
    };

    public TargetScreen(TargetModel targetmodel)
    {
        screen = GameObject.Find("Canvas").transform.Find("TargetScreen").Find("Screen");
        this.targetmodel = targetmodel;
        MainScreen.btnbank.interactable = targetmodel.isbankactive;
        LoadResourcesIcon();
        BeginConvertCheck();
        ConvertCheckTarget();
        
        GameSave.Save += Save;
        GameSave.ASaveClick += SaveGameClick;
    }


    void ConvertCheckTarget()
    {
        if (!targetmodel.isgamesave) return;

        for (int i = 0; i < targetmodel.index; i++)
        {
            SetSuccess(i);
        }

    }
    void BeginConvertCheck()
    {
        if (targetmodel.isgamesave) return;
        
        for(int i = 0; i < 4; i++)
        {
            SetTarget(Accounts.PlayerAccount.Balance);
        }

    }

    public void SetTarget(int balance)
    {
        int index = targetmodel.index;
        if (index >= 4) return;

        if (balance >= arraytarget[index].balance)
        {
            arraytarget[index].Result();
            SetSuccess(index);
            //Debug.Log("TargetModel: " + index);

            targetmodel.index++;
            DataAchievementNotifications.SetNotify(BGIcons.CONGI, "Tebrikler Seviye atladin.", 3);
        }
    }
    void LoadResourcesIcon()
    {
        spritesucces = Resources.Load<Sprite>("Icon/MarketScreen/ProcessButton");
        spritefailed = Resources.Load<Sprite>("Icon/MarketScreen/SellButton");

    }
    public void SetSuccess(int index)
    {
        screen.Find($"IMG_Level{index + 1}").Find("Check").gameObject.SetActive(true);
        screen.Find($"IMG_Level{index + 1}").GetComponent<Image>().sprite = spritesucces;

    }
    public void SetFailed(int index)
    {
        screen.Find($"IMG_Level{index + 1}").Find("Check").gameObject.SetActive(false);
        screen.Find($"IMG_Level{index + 1}").GetComponent<Image>().sprite = spritefailed;

    }


    void Save()
    {
        SaveFile();
    }
    void SaveGameClick()
    {
        targetmodel.isgamesave = true;
        if (targetmodel.index >= 1) targetmodel.isbankactive = true;

        //SaveFile();

    }

    void SaveFile()
    {
        PlayerPrefs.SetString("Target", JsonUtility.ToJson(targetmodel));
    }

}


public class TargetModel
{
    public int index;
    public bool isgamesave;
    public bool isbankactive;

    public TargetModel(int index,bool isgamesave,bool isbankactive)
    {
        this.index = index;
        this.isgamesave = isgamesave;
        this.isbankactive = isbankactive;
    }

    public void Reset()
    {
        index = 0;
        isgamesave = false;
        isbankactive = false;
    }

}
public class Target
{
    public int balance;
    private UnityAction result;
    public Target(int targetbalance,UnityAction result)
    {
        balance = targetbalance;
        this.result = result;
    }
    
    public void Result()
    {
        result?.Invoke();
    }
    
}


public struct ScreenTarget
{
    public static TargetScreen targetscreen;

}

