using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TaxControl
{
    public int remainingdays;

    public float taxamount = 0.1f;

    public TaxControl()
    {
                
        if(remainingdays<=0) SetRemainingDays();
        
        MyAccountBoard.DateUpdate += DateUpdate;

        GameSave.Save += Save;
        GameData.OnReset += OnReset;
    }

    void SetRemainingDays()
    {

        DateTime taxdatetime,newdatetime;
        TimeSpan subs;

        newdatetime = CurrentDateTime;

        taxdatetime = newdatetime;

        taxdatetime = taxdatetime.AddMonths(1);

        subs = taxdatetime.Subtract(newdatetime);

        remainingdays = subs.Days;

    }
     
    

    public void DateUpdate()
    {
        Debug.Log(remainingdays);
        if (remainingdays <= 0)
        {
            int tax = System.Convert.ToInt32(Accounts.PlayerAccount.Balance * taxamount);
            Debug.Log(taxamount);
            DataPlayerMethods.SetBalance(-tax);

            SetRemainingDays();

            DataAchievementNotifications.SetNotify(BGIcons.BUY,$"{tax} TL Gelir vergisi alindi.", 3);
            HistoryFunction.AddHistory($"<color=red>{tax} TL</color> gelir vergisi alindi.");
        }

        remainingdays--;        
    }
    
    private DateTime CurrentDateTime
    {
        get
        {
            string[] array;

            int day, month, year;

            array = Board.FullDate.Split('.');

            day = int.Parse(array[0]);
            month = int.Parse(array[1]);
            year = int.Parse(array[2]);

            return new DateTime(year, month, day);

        }
    }

    void Save()
    {
        PlayerPrefs.SetString("Tax", JsonUtility.ToJson(this));
    }


    void OnReset()
    {
        MyAccountBoard.DateUpdate -= DateUpdate;
        GameData.OnReset -= OnReset;
    }
}

