using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using TMPro;
[CreateAssetMenu(fileName = "MyAccount", menuName = "ScriptableObjects/MyAccount")]
public class AccountScriptable : ScriptableObject, ISetupLoad
{
    [SerializeField] PlayerAccountModel playermodel;
    [SerializeField] DateModel datemodel;

    [SerializeField] Transform transform;

    Board board;

    TextMeshProUGUI LOG;

    public void Load()
    {
        transform = GameObject.Find("MyAccount").transform.Find("Screen");
        //LOG = GameObject.FindGameObjectWithTag("LOG").GetComponent<TextMeshProUGUI>();

        LoadPlayer();

        GameSave.Save += Save;

        MobileLOG.Log("Player yuklendi...");
    }

    void Save()
    {
        SaveDate();
        SavePlayer();
    }
    void SavePlayer()
    {
        PlayerPrefs.SetString("Player", JsonUtility.ToJson(playermodel));
    }
    void LoadPlayer()
    {
        LoadDate();
        MobileLOG.LOG.text += "AccountBoard öncesi ";
        MyAccountBoard myboard = new MyAccountBoard(datemodel);


        string jsondata = PlayerPrefs.GetString("Player");
        PlayerAccountModel playermodel = JsonUtility.FromJson<PlayerAccountModel>(jsondata);


        if (playermodel != null) this.playermodel = playermodel;


        Accounts.PlayerAccount = new MyAccount(this.playermodel, transform, myboard);

        LoadMyCustomer(); 
        LoadMyFavorite();
        LoadMyInstructions();

        MobileLOG.LOG.text += "Player: " + this.playermodel.ToString();
        
        MethodsFind.FindInstructionModel = FindInstructionModel;
    }

    void SaveDate()
    {
        datemodel.date = Board.FullDate;
        
        // burada settings den degistirdigimiz ayara aticaz. //code

        PlayerPrefs.SetString("Date", JsonUtility.ToJson(datemodel));

        MobileLOG.Log($"FullDate: {datemodel.date} kaydedildi..."); 
    }


    void LoadDate()
    {
        DateModel datemodel;
        string jsondata;

        jsondata = PlayerPrefs.GetString("Date");

        datemodel = JsonUtility.FromJson<DateModel>(jsondata);
        
        if (datemodel != null)
        {
            this.datemodel = datemodel;
        }

        BGGameSetting.speed = this.datemodel.speed;
        BGUniqueClasses.datemodel = this.datemodel;

        MobileLOG.Log($"Date: {this.datemodel.ToString()} {jsondata}" );
    }

    void LoadMyCustomer()
    {
        List<CustomerModel> _mycustomermodel = playermodel._mycustomermodels;

        for(int i = 0; i < _mycustomermodel.Count; i++)
        {
            CustomerAccount customer = new CustomerAccount(_mycustomermodel[i], Container.MYCUSTOMER);
            customer.ConvertContractCustomer();
            LoadMyCustomerInstructions(customer);

            Accounts.PlayerAccount.AddMyCustomers(customer,ReasonForAdding.LOADING);
        }
    }
    
    void LoadMyCustomerInstructions(Account account)
    {

        List<InstructionModel> _instructions = account.accountmodel._instructions ;

        InstructionModel model;
        Instruction instruction;
        BaseShareModel bshrmodel;

        for(int i = 0; i < _instructions.Count; i++)
        {
            model = _instructions[i];
            bshrmodel = MethodsFind.FindMarketShare(model.sharename);

            instruction = new Instruction(model.id, model.instructionprice, model.amount, bshrmodel, model.text, (InstructionType)model.instructiontype,
                (InstructionTrade)model.instructiontrade, account);

            account._myinstructions.Add(instruction);
        }
    }
    void LoadMyFavorite()
    {
        List<FavoriteShare> _favoriteshare = playermodel._favoriteshares;
        for(int i = 0; i < _favoriteshare.Count; i++)
        {
            FavoriteShareModel model = new FavoriteShareModel(_favoriteshare[i].sharemodel, Container.FAVORITESHARE);
        }
    }
    void LoadMyInstructions()
    {


        List<InstructionModel> _instructions = playermodel._instructions;
        InstructionModel model;
        Instruction instruction;
        BaseShareModel bshrmodel;
        for(int i = 0; i < _instructions.Count; i++)
        {
            model = _instructions[i];
            bshrmodel = MethodsFind.FindMarketShare(model.sharename);

            instruction = new Instruction(model.id,model.instructionprice, model.amount, bshrmodel, model.text, (InstructionType)model.instructiontype,
                (InstructionTrade)model.instructiontrade, Accounts.PlayerAccount);

            Accounts.PlayerAccount._myinstructions.Add(instruction);
        }

    }
    private InstructionModel FindInstructionModel(string id,AccountModel account)
    {
        List<InstructionModel> _instructions = account._instructions;

        for (int i = 0; i < _instructions.Count; i++)
        {
            if(_instructions[i].id == id)
            {
                return _instructions[i];
            }
        }
        return null;
    }

}


[System.Serializable]
public class PlayerAccountModel : AccountModel
{

    public int upcomingpayment;

    public float timeleftpay;

    public List<CustomerModel> _mycustomermodels = new List<CustomerModel>();

    public List<FavoriteShare> _favoriteshares = new List<FavoriteShare>();

    public CreditData creditdata;
    
}

public class AccountModel
{
    public int balance;

    public List<MyShare> _myshare = new List<MyShare>();

    public List<InstructionModel> _instructions = new List<InstructionModel>();
}

[Serializable]
public class MyShare
{
    public ShareModel sharemodel;
    public int firstprice;

    public int amount;
}
[Serializable]
public class FavoriteShare
{
    public ShareModel sharemodel;
    public int firstprice;

}


[System.Serializable]
public class DateModel
{
    public float speed;
    public string date;
}



public struct Accounts
{
    public static MyAccount PlayerAccount { get; set; }
}

public struct PlayerReference
{
    public static CreditDeptControl CreditControl;
}

public struct BGUniqueClasses
{
    public static DateModel datemodel;

}


public struct Board 
{
    public static string FullDate;

    public static float taxamount;
}

public struct BGScreenMyAccount
{
    private static TextMeshProUGUI tmpupcomingpaycredit = FindHierarchy("PaymentDueDate").GetComponent<TextMeshProUGUI>();

    public static Transform ContainerUpComingPayment = FindHierarchy("Scroll").Find("Container");

    private static Transform FindHierarchy(string name)
    {

        return GameObject.Find("Canvas").transform.Find("MyAccount")
            .Find("Screen")
            .Find(name);
    }

    public static void SetTMPUpcomingPaymentCredit(string date)
    {
        tmpupcomingpaycredit.text = date;
    }

}

public struct BGGameSetting
{
    public static float speed;
}