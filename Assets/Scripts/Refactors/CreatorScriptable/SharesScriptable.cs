using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Shares", menuName = "ScriptableObjects/Shares")]
public class SharesScriptable : ScriptableObject,ISetupLoad
{
    [HideInInspector] public List<MarketShareModel> _marketsharemodels;

    public List<ShareModel> shareModels; 

    private float sharesupdatetime;
    private float trendupdatetime;

    private TextMeshProUGUI LOG;
   
    TrendController trendController;

    public static System.Action PriceUpdate;

    NewsFeed newsfeed;

    DataAllSharesModel loadedObject;

    public void Load()
    {
        LOG = GameObject.Find("Canvas").transform.Find("LOG").GetComponent<TextMeshProUGUI>();

        MobileLOG.LOG = LOG;

        LoadShares();
        _marketsharemodels = new List<MarketShareModel>();
        sharesupdatetime= 0;
        trendupdatetime = 0;

        CreateMarketShares();
        LoadNews();//warning

        MobileLOG.Log("Haberler den sonra");
        GameSave.Save += Save;
        Setup.update += Update;

        MobileLOG.LOG.text += " ShareScriptableTamamlandi";

    }

    void Save()
    {
        PlayerPrefs.SetString("Shares2", JsonUtility.ToJson(loadedObject));
        PlayerPrefs.SetString("News", JsonUtility.ToJson(newsfeed));
    }
    void CreateMarketShares()
    {
        MarketShareModel model;

        for(int i = 0; i < shareModels.Count; i++)
        {
            shareModels[i].Awake();

            model = new MarketShareModel(shareModels[i], Container.MARKETSHARE);

            _marketsharemodels.Add(model);
        }

        MethodsFind.FindMarketShare = FindMarketShareModel;
        MethodsFind.FindMarketShareModel = FindMarketShare;

    }
    void LoadShares()
    {
        string jsondata = PlayerPrefs.GetString("Shares2");

        loadedObject = JsonUtility.FromJson<DataAllSharesModel>(jsondata);

        Debug.Log(jsondata);

        if (loadedObject==null || loadedObject._shares.Count<=0)
        {
            loadedObject = new DataAllSharesModel();
            loadedObject._shares = shareModels;
            
        }
        else shareModels = loadedObject._shares;

        MobileLOG.Log("Shares Dosyasi: " + loadedObject.ToString());

        MethodsFind.FindShareModel = loadedObject.FindShareModel;
        BGSavedLists._sharemodels = shareModels;
    }

    void LoadNews()
    {
        MobileLOG.Log("haberler gelecek.");

        
        string jsondata = PlayerPrefs.GetString("News");


        if (jsondata != "") newsfeed = JsonUtility.FromJson<NewsFeed>(jsondata);

        else newsfeed._news = new List<News>();
        

        List<News> _news = newsfeed._news;

        for (int i = 0; i < _news.Count; i++)
        {
            NewsItem item = new NewsItem(_news[i].news, _news[i].date);

            NewsManager.AddNews(item, ref newsfeed);
        }
        MobileLOG.Log("Haberler yuklendi...");

        NewsManager.Clear = ClearNews;
    }
    
    void ClearNews()
    {
        newsfeed._news.Clear();
    }
    public void Update()
    {
        

        TrendUpdate();
        SharesUpdate();

    }
    
    void SharesUpdate()
    {
        if (SharesRepeatTime(2))
        {
            MarketShareModel model;

            for (int i = 0; i < _marketsharemodels.Count; i++)
            {
                model = _marketsharemodels[i];
                model.shareModel.UpdatePrice(trendController.UpdatePrice(model._trendkeylist.CurrentMember()));
                model.shareModel.UpdateTrend(trendController.TrendName(model._trendkeylist.NextMember()));

            }

            RandomNews();
        }

    }

    void TrendUpdate()
    {
        MarketShareModel model;
        if (TrendRepeatTime(12))
        {
            for (int i = 0; i < _marketsharemodels.Count; i++)
            {
                model = _marketsharemodels[i];
                model._trendkeylist.Next();
                model.shareModel.trendindex = model._trendkeylist.currentCount;
            }
        }

    }
    
    BaseShareModel FindMarketShareModel(string name)
    {
        for(int i = 0; i < _marketsharemodels.Count; i++)
        {
            if (name == _marketsharemodels[i].shareModel.shareName)
            {
                return _marketsharemodels[i];
            }

        }
        return null;
    }
    MarketShareModel FindMarketShare(string name)
    {
        for (int i = 0; i < _marketsharemodels.Count; i++)
        {
            if (name == _marketsharemodels[i].shareModel.shareName)
            {
                return _marketsharemodels[i];
            }

        }
        return null;
    }

    void RandomNews()
    {
        int index = Random.Range(0, _marketsharemodels.Count - 1);
        MarketShareModel model = _marketsharemodels[index];
                
        string news = model.shareModel.TrendNews(model._trendkeylist.NextMember());

        NewsItem item = new NewsItem(news, Board.FullDate);

        News news1 = new News();
        news1.news = news;
        news1.date = Board.FullDate;

        
        NewsManager.AddNews(item,ref newsfeed);
        newsfeed._news.Add(news1);

    }

    /*Time*/ // gecici
    bool SharesRepeatTime(float time)
    {
        if(Time.time >= sharesupdatetime)
        {
            sharesupdatetime = Time.time + time;
            return true;
        }
        return false;
    }

    bool TrendRepeatTime(float time)
    {
        if (Time.time >= trendupdatetime)
        {
            trendupdatetime = Time.time + time;
            return true;
        }
        return false;

    }
    /*Time*/

}

[System.Serializable]
public class DataAllSharesModel
{
    public List<ShareModel> _shares;

    public ShareModel FindShareModel(string sharename)
    {
        for (int i = 0; i < _shares.Count; i++)
        {
            if (_shares[i].shareName.Equals(sharename))
            {
                return _shares[i];
            }

        }
        return null;
    }


}

public struct MethodsFind
{
    public static System.Func<string, ShareModel> FindShareModel;
    public static System.Func<string, BaseShareModel> FindMarketShare;
    public static System.Func<string, MarketShareModel> FindMarketShareModel;

    public static System.Func<string,AccountModel, InstructionModel> FindInstructionModel;
}

public struct BGSavedLists
{
    public static List<ShareModel> _sharemodels;
    public static List<CustomerModel> _customers;
    public static List<string> _histories;
}

public struct MobileLOG
{
    public static TextMeshProUGUI LOG;

    public static void Log(string message)
    {
        LOG.text += message + "\n";

    }
}