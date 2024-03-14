using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

[Serializable]
public class ShareModel
{
    public string shareName;
    public int price;
    public string description;
    
    public string[] trendkeys;
    public List<int> lasprices = new List<int>();

    public List<NewsModel> _newsmodels;

    public int trendindex;

    public Action<int,string> SetPrice;
    public Action<int> SetPricee;
    public Action<string> SetTrend;
 

    public void Awake()
    {

    }
    
    public void UpdatePrice(int price)
    {
        this.price += price;
        SetPricee?.Invoke(this.price);

    }
    public void UpdateTrend(string trendname)
    {
        SetTrend?.Invoke(trendname);
    }

    public string TrendNews(string trendkey)
    {

        for(int i = 0; i < _newsmodels.Count; i++)
        {
            if(_newsmodels[i].trendkey == trendkey)
            {
                return _newsmodels[i].RandomNews();
            }
        }


        return "";
    }
    public void AddLastPrice(int price)
    {
        if (lasprices.Count >= 10) lasprices.RemoveAt(0);

        lasprices.Add(price);
    }

}


[System.Serializable]
public class NewsModel
{
    public string trendkey;

    public List<string> news;
    
    public string RandomNews()
    {
        int index = UnityEngine.Random.Range(0, news.Count - 1);

        return news[index];
    }
}

[Serializable]
public class News
{
    public string news;
    public string date; 

}

public struct NewsManager
{
    private static List<NewsItem> _newsmodels = new List<NewsItem>();

    public static UnityAction Clear;

    public static void AddNews(NewsItem model,ref NewsFeed newsfeed)
    {
        _newsmodels.Add(model);

        if (_newsmodels.Count < 8) return;

        GameObject.Destroy(_newsmodels[0].Item);
        _newsmodels.RemoveAt(0);
        newsfeed._news.RemoveAt(0);
    }
    
    public static void ClearNews()
    {
        _newsmodels.Clear();
        Clear?.Invoke();
    }        
}

[Serializable]
public struct NewsFeed 
{
    public List<News> _news;

}

