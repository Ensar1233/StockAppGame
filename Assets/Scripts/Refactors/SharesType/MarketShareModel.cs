using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
public class MarketShareModel : BaseShareModel
{
    private TextMeshProUGUI tmpTrend;
    public TextMeshProUGUI tmpPrice;

    public UnityAction<Color> ChangeFavoriteColor;

    public CircularList<string> _trendkeylist;
    

    public FavoriteShareModel favoriteshare;

    private Button btnfavoriteicon;        

    private string[] trendkeys;

    public MarketShareModel(ShareModel sharemodel, Transform container)
        : base(sharemodel, container, "ShareType/MarketShare")
    {
        this.trendkeys = sharemodel.trendkeys;

                    
        _trendkeylist = new CircularList<string>(trendkeys); // her seferinde sure sifirlandigi icin 1 eksiginden baslatiliyor.
        _trendkeylist.currentCount = shareModel.trendindex-1;
        _trendkeylist.count = (shareModel.trendindex + 1) - 1;

        ComponentAssigment();
        ComponentAssigmentVariables();

        sharemodel.SetTrend += SetTrendName;

        ChangeFavoriteColor = ChangeFavoriteIconColor;

    }
    void ComponentAssigment()
    {
        Transform transform = new_share.transform;

        tmpTrend = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        tmpPrice = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        btnfavoriteicon = transform.GetChild(5).GetComponent<Button>();
        
    }

    void ComponentAssigmentVariables()
    {
        Transform transform = new_share.transform;

        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = shareModel.shareName;

        tmpPrice.text = shareModel.price + " TL";

        btnfavoriteicon.onClick.AddListener(AddFavorite);

    }
    /*Click Listener*/
    void AddFavorite()
    {
        MyAccount myAccount = Accounts.PlayerAccount;

        if (!myAccount.HaveFavoriteShare(shareModel.shareName))
        {

            favoriteshare = new FavoriteShareModel(shareModel, Container.FAVORITESHARE);
            ChangeFavoriteIconColor(Color.yellow);
            return;
        }
        favoriteshare = myAccount.FindFavoriteShareModel(shareModel.shareName);

        favoriteshare.Remove();
    }

    void ChangeFavoriteIconColor(Color color)
    {
        BGChangeColor.SelectedColor(btnfavoriteicon, color);
        BGChangeColor.SelectButton(btnfavoriteicon, color);

    }

    public override void SetPrice(int price)
    {
        tmpPrice.text = price + "TL";

        shareModel.AddLastPrice(price);
    }

    public void SetTrendName(string trendname)
    {
        tmpTrend.text = trendname;
    }
}




public class CircularList<T>
{
    private T[] t;
    public int count = 0;

    public int currentCount;

    public CircularList(T[] t)
    {
        this.t = t;
    }

    public void Next()
    {
        currentCount = count;

        if(count >= t.Length-1) count=0;
        else count++; 

    }

    public T CurrentMember()
    {

        return t[currentCount];
    }

    public T NextMember()
    {

        return t[count];
    }

}



