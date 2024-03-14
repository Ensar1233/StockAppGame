using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Newtonsoft.Json;
public class MyShareModel : BaseShareModel
{
    
    [JsonIgnore]
    TextMeshProUGUI tmpprice;
    TextMeshProUGUI tmpamount;    
    TextMeshProUGUI tmpcurrentprice;
    

    public int firstprice;

    public MyShare myshare;

    public MyShareModel(ShareModel sharemodel,int amount,Account account) 
        : base(sharemodel, account.ContainerAccount, "ShareType/MyShare")
    {
       myshare = account.FindMyShare(sharemodel.shareName);
                        
        if(myshare==null)//warning
        {
            myshare = new MyShare();

            myshare.sharemodel = sharemodel;
            myshare.firstprice = sharemodel.price;
            myshare.amount = amount;

            account.accountmodel._myshare.Add(myshare);
        }
        
        new_share.transform.localScale = account.mysharescale;

        ComponentAssigment();
        ComponentVariablesAssigment();
    }

    void ComponentAssigment()
    {
        tmpprice = new_share.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tmpcurrentprice = new_share.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        tmpamount = new_share.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }
    void ComponentVariablesAssigment()
    {
        new_share.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = shareModel.shareName;

        tmpamount.text = myshare.amount.ToString();

        tmpprice.text = myshare.firstprice+ "TL";
        
        tmpcurrentprice.text =shareModel.price + "TL";
    }


    public override void SetPrice(int price)
    {
        tmpcurrentprice.text = price + "TL";
        shareModel.price = price;        
    }
    
    public void SetAmount(int amount,Account account)
    {

        myshare.amount += amount;

        if (myshare.amount <= 0) 
        {
            Destroy(account);

            return;
        }

        tmpamount.text = myshare.amount.ToString();
    }
    
    public void Destroy(Account account)
    {
        MethodsFind.FindShareModel(shareModel.shareName).SetPricee -= SetPrice;
        account.RemoveMyShare(this);
        GameObject.Destroy(new_share);
    }

}
