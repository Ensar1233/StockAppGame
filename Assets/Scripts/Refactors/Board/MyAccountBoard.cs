using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class MyAccountBoard
{
    private TextMeshProUGUI tmpdate,tmpbalance;

    private DateModel datemodel;

    private float currentSpeed;
    private DateTime dateTime;

    public static Action DateUpdate;
    public static Action MonthUpdate;

    public MyAccountBoard(DateModel datemodel)
    {
        this.datemodel = datemodel;
        currentSpeed = datemodel.speed;

        MobileLOG.Log($"DateBoard: {datemodel.date}");

        ComponentAssignment();
        ComponentAssignVariables();

        LoadTax();
        Setup.update += Update;
        GameData.OnReset += OnReset;
    }
    void LoadTax()
    {
        string jsondata;
        TaxControl taxcontrol;

        jsondata = PlayerPrefs.GetString("Tax");

        taxcontrol = JsonUtility.FromJson<TaxControl>(jsondata);
        if (taxcontrol == null) taxcontrol = new TaxControl();

        Payments.TAX = taxcontrol;

        Debug.Log(jsondata);
        MobileLOG.Log("Tax: " + taxcontrol.ToString());
    }

    string[] Date
    {
        get
        {
       
            return datemodel.date.Split('.');
        }
    }

    void ComponentAssignment()
    {
        Transform board;
        board = GameObject.Find("Canvas").transform.Find("BoardMyAccount");

        tmpbalance = board.Find("BoardBalance").GetComponent<TextMeshProUGUI>();
        tmpdate = board.Find("BoardDate").GetComponent<TextMeshProUGUI>();
    }
    void ComponentAssignVariables()
    {
        tmpdate.text = datemodel.date;

        string[] date = Date;
        int day, month, year;

        day = int.Parse(date[0]);
        month = int.Parse(date[1]);
        year = int.Parse(date[2]);

        dateTime = new DateTime(year, month, day);

        Board.FullDate = dateTime.ToString("dd.MM.yyyy");

    }

    public void Update() // GameLoop
    {
        SetTMPUpdate();
    }

    public void SetTMPBalance(int balance)
    {
        tmpbalance.text = balance + "TL";

    }

    void SetTMPUpdate()
    {
        if (Time.time >= currentSpeed)
        {
            SetDate();

            currentSpeed = datemodel.speed + Time.time;
        }

    }
    void SetDate()
    {
        string datetime = dateTime.ToString("dd.MM.yyyy");

        tmpdate.text = datetime;
        Board.FullDate = datetime;

        
        DateUpdate?.Invoke();


        dateTime = dateTime.AddDays(1);

    }

    void OnReset()
    {
        Setup.update -= Update;
        GameData.OnReset -= OnReset;
    }
}

public struct Payments
{
    public static TaxControl TAX;
}
