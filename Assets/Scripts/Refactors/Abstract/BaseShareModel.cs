using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseShareModel
{
    public ShareModel shareModel;
    
    public GameObject new_share; // new game object.
    
    public BaseShareModel(ShareModel sharemodel, Transform container, string loadPath)
    {
        this.shareModel = sharemodel;
        GameObject prefab = Resources.Load<GameObject>(loadPath);
        

        new_share = GameObject.Instantiate(prefab, container);
        
        /*ShareModel marketsharemodel*/shareModel = MethodsFind.FindShareModel(shareModel.shareName);
        
        /*marketsharemodel*/shareModel.SetPricee += SetPrice;
        //marketsharemodel.SetLastPrices+=
        
        TradePopUpClickListener();
        DetailPopUpClickListener();
    }

    protected void OpenTradePopUp()
    {
        TradeScreen.Open(this);
    }
    protected void OpenShareDetail()
    {
        DetailScreen.Open(this);
    }
    void TradePopUpClickListener()
    {
        new_share.transform.GetChild(new_share.transform.childCount - 1).GetComponent<Button>().onClick.AddListener(OpenTradePopUp);
    }
    void DetailPopUpClickListener()
    {
        new_share.transform.Find("ShareName").GetComponent<Button>().onClick.AddListener(OpenShareDetail);
    }
    public void Buy(Account account,int amount)
    {
        int cost = shareModel.price * amount;
        // musteri olmadigindan hata veriyor.
        if (!account.BalanceIsEnough(cost))
        {
            DataWarningNotification.SetMessage("Yetersiz Bakiye!",2);
            return;
        }
        
        MyShareModel accountShare;
        accountShare = account.HaveMyShare(shareModel);

        if (accountShare==null)
        {
            accountShare = new MyShareModel(shareModel,amount, account);

            account.AddMyShare(accountShare);

            
            HistoryFunction.AddHistory(AppMessages.TradeMessage(account.Name,shareModel.shareName,shareModel.price,amount, "alindi"));

            DataAchievementNotifications.SetNotify(BGIcons.BUY,$"{shareModel.shareName} hissesinden {amount} tane {account.Name} {(account is CustomerAccount ? "tarafindan" : "")} alindi.", 3);
            return;
        }
        accountShare.SetAmount(amount,account);

        HistoryFunction.AddHistory(AppMessages.TradeMessage(account.Name,shareModel.shareName,shareModel.price,amount,"alindi"));

        DataAchievementNotifications.SetNotify(BGIcons.BUY,$"{shareModel.shareName} hissesinden {amount} tane alindi.", 3); // kombo

        
    }

    public void Sell(Account account , int amount)
    {
        MyShareModel accountShare;
        int cost,fullamount;
        accountShare = account.HaveMyShare(shareModel);

        if (accountShare == null)
        {
            DataWarningNotification.SetMessage("Bu hisseye sahip degilsin satis gerceklestiremezsin !", 3);
            return;
        }

        fullamount = accountShare.myshare.amount;

        if (amount >= fullamount) amount = fullamount;

        cost = shareModel.price * amount;



        accountShare.SetAmount(-amount,account);
        
        account.SetBalance(cost);

        HistoryFunction.AddHistory(AppMessages.TradeMessage(account.Name, shareModel.shareName, shareModel.price, amount, "satildi"));

        DataAchievementNotifications.SetNotify(BGIcons.SELL, $"{shareModel.shareName} hissesinden {amount} tane {account.Name} {(account is CustomerAccount ? "tarafindan" : "")} satildi.", 3);

    }

    //kombo amount fonksiyonu yaz

    public abstract void SetPrice(int price);
}

