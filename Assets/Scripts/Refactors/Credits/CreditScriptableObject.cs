using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "Credits", menuName = "ScriptableObjects/Credits")]
public class CreditScriptableObject : ScriptableObject , ISetupLoad
{
    [SerializeField] List<CreditModel> _creditsmodel;


    /// <summary>
    /// Yillik kredi borcunun ödenmesi beklenecek.
    /// </summary>
    public void Load()
    {
        ComponentAssigment();
        LoadCreditsModel();
        LoadCreditData();

        MobileLOG.Log("Load Credits...");
    }

    void LoadCreditsModel()
    {
        for (int i = 0; i < _creditsmodel.Count; i++)
        {
            new Credit(_creditsmodel[i], Container.CREDITS);
        }
    }

    void LoadCreditData()
    {
        PlayerAccountModel playermodel = Accounts.PlayerAccount.playermodel;

        if (!playermodel.creditdata.usecredit) return;

        PlayerReference.CreditControl = new CreditDeptControl(playermodel);
        LoadCreditButtons();
    }
    void ComponentAssigment()
    {
        Transform screen = GameObject.Find("Canvas").transform.Find("Bank").GetChild(0);
        
        Button btntakecredit = screen.Find("BTNTakeCredit").GetComponent<Button>();
        Button btncreditoffpay = screen.Find("BTNPayOffDept").GetComponent<Button>();

        btntakecredit.onClick.AddListener(TakeCredit);
        btncreditoffpay.onClick.AddListener(CreditOffPay);
    }

    void TakeCredit()
    {
        PlayerAccountModel playermodel = Accounts.PlayerAccount.playermodel;

        if (playermodel.creditdata.usecredit)
        {
            DataWarningNotification.SetMessage("Kredi borcun varken kredi cekemezsin!", 2);

            return;
        }
        Accounts.PlayerAccount.TakeCredit(TempCreditValue.creditdept);


        PlayerReference.CreditControl = new CreditDeptControl(playermodel);

        CreditAllButton.AllCreditButtonInteractable(false);

        int index = CreditAllButton.index(TempCreditTab.Tab);


        BGChangeColor.DisableButton(TempCreditTab.Tab);

        playermodel.creditdata.index = index;
        playermodel.creditdata.usecredit = true;
    }

    void LoadCreditButtons()
    {
        PlayerAccountModel playermodel = Accounts.PlayerAccount.playermodel;

        Button btn = CreditAllButton.FindButton(playermodel.creditdata.index);
        CreditAllButton.AllCreditButtonInteractable(false);


        BGChangeColor.DisableButton(btn);

    }
    void CreditOffPay()
    {
        
        PlayerAccountModel playermodel = Accounts.PlayerAccount.playermodel;
        int balance,creditdept;

        balance = playermodel.balance;
        creditdept = playermodel.creditdata.creditdept;;

        if(balance>= creditdept)
        {
            PayOffDebt(creditdept);
            return;
        }
        DataWarningNotification.SetMessage("Kredi kapatmak icin yeterli bakiyeniz yok!",2);
    }


    void PayOffDebt(int creditdept)
    {
        DataPlayerMethods.SetCreditDept(-creditdept, SetCredit.PAYDEPT);
    }

}

[Serializable]
public class CreditData
{
    public bool usecredit;

    public int creditdept;
    public int monthlydept;
    public int monthlyremaining;
    public int yearremaining;
    public int index;
    public string duedate;

}


[Serializable]
public class CreditModel
{
    public int amount;

    public int[] interestrates = new int[3];
}


