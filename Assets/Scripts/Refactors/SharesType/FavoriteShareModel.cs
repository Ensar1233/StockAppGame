using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class FavoriteShareModel : BaseShareModel
{
    private TextMeshProUGUI tmpPrice;
    private TextMeshProUGUI tmpCurrentPrice;

    public FavoriteShare favoriteshare;
    public FavoriteShareModel(ShareModel sharemodel, Transform container)
        : base(sharemodel, container, "ShareType/FavoriteShare")
    {
        MyAccount account = Accounts.PlayerAccount;

        favoriteshare = account.FindFavorite(shareModel.shareName);

        if (favoriteshare == null)
        {
            favoriteshare = new FavoriteShare();
            favoriteshare.sharemodel = shareModel;
            favoriteshare.firstprice = shareModel.price;

            account.playermodel._favoriteshares.Add(favoriteshare);

        }
        else MethodsFind.FindMarketShareModel(shareModel.shareName).ChangeFavoriteColor(Color.yellow);

        Accounts.PlayerAccount.AddFavorite(this);//update
        
        ComponentAssigment();
        ComponentVariables();
    }


    /*Initilized*/
    void ComponentAssigment()
    {
        Transform transform = new_share.transform;

        tmpPrice = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        tmpCurrentPrice = transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        transform.GetChild(5).GetComponent<Button>().onClick.AddListener(Remove);

    }
    void ComponentVariables()
    {
        Transform transform = new_share.transform;

        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = shareModel.shareName;

        tmpPrice.text =favoriteshare.firstprice + "TL";
        tmpCurrentPrice.text = shareModel.price + "TL";
    }
    public override void SetPrice(int price)
    {

        tmpCurrentPrice.text = price + "TL";
        shareModel.price = price;
    }

    public void Remove() => Disable();
    
    void Disable()
    {
        MethodsFind.FindShareModel(shareModel.shareName).SetPricee -= SetPrice;
        MethodsFind.FindMarketShareModel(shareModel.shareName).ChangeFavoriteColor(Color.white);


        Accounts.PlayerAccount.RemoveFavorite(this);
        Accounts.PlayerAccount.playermodel._favoriteshares.Remove(favoriteshare);

        GameObject.Destroy(new_share);

    }

}
