using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Newtonsoft.Json;

public class MyAccount : Account
{
    [JsonIgnore]
    private List<FavoriteShareModel> _myfavoritemodels = new List<FavoriteShareModel>();
    private List<CustomerAccount> _mycustomers = new List<CustomerAccount>();
    //private List<Instruction> _myinstructions = new List<Instruction>(); 

    public static List<ItemUpComingPayment> _itemsupcomingpayments = new List<ItemUpComingPayment>();

    private TextMeshProUGUI tmpbalance, tmpcreditdept, tmppaymentdate;
    
    private Transform transform;


    public PlayerAccountModel playermodel;

    public MyAccountBoard Board { get; set; }


    public MyAccount(PlayerAccountModel playermodel,Transform transform,MyAccountBoard board) : base("")
    {
        this.accountmodel = playermodel;
        this.playermodel = playermodel;
        this.transform = transform;

        Board = board;

        ContainerAccount = Container.MYSHARE;

        ComponentAssigment();
        ComponentAssingVariables();
        LoadData();

        DataPlayerMethods.SetBalance = SetBalance;
        DataPlayerMethods.SetCreditDept = SetCreditDept;
        DataPlayerMethods.RemoveCustomer = RemoveMyCustomers;

    }
    
    void ComponentAssigment()
    {
        tmpbalance = transform.Find("Balance").GetComponent<TextMeshProUGUI>();
        tmpcreditdept = transform.Find("CreditDept").GetComponent<TextMeshProUGUI>();
        tmppaymentdate = transform.Find("PaymentDueDate").GetComponent<TextMeshProUGUI>();

    }
    void ComponentAssingVariables()
    {
        tmpbalance.text = playermodel.balance + " TL";
        tmpcreditdept.text = playermodel.creditdata.creditdept+ " TL";
        tmppaymentdate.text = "00/00/0000";

        Board.SetTMPBalance(Balance);

    }


    public FavoriteShareModel FindFavoriteShareModel(string name)
    {
        List<FavoriteShareModel> _favoriteshares = _myfavoritemodels;

        for(int i = 0; i < _favoriteshares.Count; i++)
        {
            if(_favoriteshares[i].shareModel.shareName == name)
            {
                return _favoriteshares[i];
            }            

        }
        return null;
    }
    public bool HaveFavoriteShare(string name)
    {
        foreach(FavoriteShareModel favoriteshare in _myfavoritemodels)
        {
            if (favoriteshare.shareModel.shareName==name)
            {
                return true;
            }
        }
        return false;
    }

    public CustomerAccount FindCustomerAccount(string name)
    {
        foreach(CustomerAccount customerAccount in _mycustomers)
        {
            if (customerAccount.customermodel.name.Equals(name))
            {
                return customerAccount;
            }
        }
        return null;
    }

    public CustomerModel FindCustomerModel(string name)
    {
        List<CustomerModel> customers = playermodel._mycustomermodels;

        for(int i = 0; i < customers.Count; i++)
        {
            if (customers[i].name == name)
            {
                return customers[i];
            }        
            
        }
        return null;
    }

    public FavoriteShare FindFavorite(string name)
    {
        List<FavoriteShare> favorites = playermodel._favoriteshares;
        
        for(int i = 0; i < favorites.Count; i++)
        {
            if(favorites[i].sharemodel.shareName == name)
            {
                return favorites[i];
            }
        }
        return null;
    }

    public override void SetBalance(int cost)
    {
        Balance += cost;
        //playermodel.balance = Balance;

        //TODO: Balance 0 altiysa oyun bitecek.

        tmpbalance.text = Balance + " TL";
        ScreenTarget.targetscreen.SetTarget(Balance);
        Board.SetTMPBalance(Balance);

    }

    public void TakeCredit(int creditdeptamount)
    {
        SetCreditDept(creditdeptamount, SetCredit.WRITEOFDEPT);
    }
    public void SetCreditDept(int amount,SetCredit credit)
    {
        playermodel.creditdata.creditdept+= amount;

        tmpcreditdept.text = playermodel.creditdata.creditdept+ " TL";


        if (credit == SetCredit.WRITEOFDEPT) 
        {
            SetBalance(amount);
            return;
        }

        SetBalance(amount);

        if (playermodel.creditdata.creditdept <= 0)
        {
            playermodel.creditdata.usecredit = false;
            playermodel.creditdata.index = 0;
            CreditAllButton.AllCreditButtonInteractable(true);

            PlayerReference.CreditControl.Close();
            PlayerReference.CreditControl = null;
 
            DataAchievementNotifications.SetNotify(BGIcons.CREDITCARD,"Kredi borcunuz bitti.", 3);//
        }
    }
    public void AddFavorite(FavoriteShareModel model) 
    {
        _myfavoritemodels.Add(model);
    }
    public void RemoveFavorite(FavoriteShareModel model) 
    {
        _myfavoritemodels.Remove(model);
    }


    public void AddMyCustomers(CustomerAccount account,ReasonForAdding foradding) 
    {
        _mycustomers.Add(account);
        
        if(foradding==ReasonForAdding.ADDING) playermodel._mycustomermodels.Add(account.customermodel);

        TradeScreen.DropDownCustomer.AddItem(account);
    }
    public void RemoveMyCustomers(CustomerAccount account) 
    {
        account.accountmodel.balance = account.customermodel.balance;

        _mycustomers.Remove(account);
        playermodel._mycustomermodels.Remove(account.customermodel);
        account = null;
        
        
    }
    
}

public enum ReasonForAdding
{
    LOADING,ADDING

}

public enum SetCredit
{
    PAYDEPT,WRITEOFDEPT
}


public struct DataPlayerMethods
{
    public static Action<int> SetBalance;
    public static Action<int, SetCredit> SetCreditDept;

    public static Action<CustomerAccount> RemoveCustomer;

}


public class ItemUpComingPayment
{

    private GameObject new_item;
    
    public ItemUpComingPayment(string message)
    {
        GameObject prefab;
        prefab = Resources.Load<GameObject>("Screen/MyAccount/UpComingPayment");

        new_item = GameObject.Instantiate(prefab, BGScreenMyAccount.ContainerUpComingPayment);

        ComponentAssigment(message);
    }

    void ComponentAssigment(string message)
    {
        new_item.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void Delete()
    {
        GameObject.Destroy(new_item);
        MyAccount._itemsupcomingpayments.Remove(this);
    }

}

