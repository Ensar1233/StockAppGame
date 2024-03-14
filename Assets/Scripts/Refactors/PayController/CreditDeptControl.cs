using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class CreditDeptControl
{

    private CreditData creditdata;

    private ItemUpComingPayment item;
    public CreditDeptControl(PlayerAccountModel playermodel)
    {

        DateTime currentdatetime = CurrentDateTime;
        creditdata = playermodel.creditdata;
        SetUpcomingPayUI();

        if(creditdata.monthlyremaining<=10)
        {
            item = new ItemUpComingPayment($"Kredi Borcu: {creditdata.monthlydept} ");
            MyAccount._itemsupcomingpayments.Add(item);
        }

        MyAccountBoard.DateUpdate += DateUpdateMonthControl;
        MyAccountBoard.DateUpdate += DateUpdateYearControl;


        if (creditdata.usecredit) return;

        creditdata.monthlyremaining = DateTime.DaysInMonth(currentdatetime.Year,currentdatetime.Month);
        creditdata.yearremaining = TempCreditValue.yearpaid * 365;
        creditdata.monthlydept = TempCreditValue.monthlydept;
        creditdata.usecredit = true;

        SetUpcomingPay();
    }


    public void DateUpdateMonthControl()
    {
        Debug.Log("kalan gun: "+creditdata.monthlyremaining+" aylik odeme: "+creditdata.monthlydept);
        

        if (creditdata.monthlyremaining <= 0)
        {
            DateTime currentdatetime = CurrentDateTime;
            creditdata.monthlyremaining = DateTime.DaysInMonth(currentdatetime.Year, currentdatetime.Month);

            SetUpcomingPay();

            item.Delete();

            DataPlayerMethods.SetCreditDept(-creditdata.monthlydept, SetCredit.PAYDEPT);
            DataAchievementNotifications.SetNotify(BGIcons.CREDITPAY,$"{creditdata.monthlydept} kredi taksidin ödendi.", 3);
            HistoryFunction.AddHistory($"<color=red>{creditdata.monthlydept} TL</color> olan aylik kredi borcun ödendi.");    
            
            
        }
        if(creditdata.monthlyremaining==10)
        {
            item = new ItemUpComingPayment($"Kredi Borcu: {creditdata.monthlydept} ");
            MyAccount._itemsupcomingpayments.Add(item);
        }

        creditdata.monthlyremaining--;
    }
    void DateUpdateYearControl()
    {
        if (creditdata.yearremaining<= 0)
        {

            DataPlayerMethods.SetCreditDept(-creditdata.creditdept, SetCredit.PAYDEPT);
           

        }
        creditdata.yearremaining--;
    }
    void SetUpcomingPay()
    {
        creditdata.duedate = CurrentDateTime.AddDays(creditdata.monthlyremaining).ToString("dd.MM.yyyy");
        SetUpcomingPayUI();
    }
    void SetUpcomingPayUI()
    {
        BGScreenMyAccount.SetTMPUpcomingPaymentCredit(creditdata.duedate);

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

            Debug.Log($"Day: {day} Month: {month} Year: {year}");

            return new DateTime(year, month, day);

        }
    }

    public void Close()
    {
        MyAccountBoard.DateUpdate -= DateUpdateMonthControl;
        MyAccountBoard.DateUpdate -= DateUpdateYearControl;

    }
}
