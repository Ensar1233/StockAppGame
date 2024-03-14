using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
public class DetailShare
{
    private BaseShareModel share;

    private TextMeshProUGUI tmplastprice,tmpdescription,tmpsharename;

    private Transform detailshare;
    public DetailShare()
    {
        detailshare= GameObject.Find("Canvas").transform.Find("DetailShare");

        tmpsharename = detailshare.Find("ShareName").GetComponent<TextMeshProUGUI>();
        tmpdescription = detailshare.Find("Description").GetComponent<TextMeshProUGUI>();
        tmplastprice = detailshare.Find("TMP_LastPrices").GetComponent<TextMeshProUGUI>();
        detailshare.Find("Close").GetComponent<Button>().onClick.AddListener(Close);

        DetailScreen.Open = Open;        
    }
    public void Open(BaseShareModel share)
    {
        this.share = share;

        tmpsharename.text = share.shareModel.shareName;
        tmpdescription.text = share.shareModel.description;

        SetTMPLastPrices();

        PopUpBackground.OpenEvent = OpenAnim;
        PopUpBackground.Open();
    }    

    void SetTMPLastPrices()
    {
        List<int> lastprices = share.shareModel.lasprices;

        string lastpricetext = "";

        for (int i = lastprices.Count - 1; i>=0; i--)
        {
            lastpricetext += $"{lastprices[i]} \n";
        }

        tmplastprice.text = lastpricetext;
    }

    void OpenAnim()
    {
        detailshare.gameObject.SetActive(true);
    }

    void Close()
    {
        detailshare.gameObject.SetActive(false);
        PopUpBackground.Close();
        Time.timeScale = 1;
    }
}


public struct DetailScreen
{
    public static UnityAction<BaseShareModel> Open;

}
