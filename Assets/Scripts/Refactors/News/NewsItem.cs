using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NewsItem
{

    public GameObject Item { get; set; }
    public NewsItem(string news,string date)
    {
        GameObject prefab;

        prefab = Resources.Load<GameObject>("News/NewsItem");

        Item = GameObject.Instantiate(prefab, Container.NEWS);

        Item.transform.Find("TMPNews").GetComponent<TextMeshProUGUI>().text = news;
        Item.transform.Find("TMPNewsDate").GetComponent<TextMeshProUGUI>().text = date;

        Item.transform.SetAsFirstSibling();


    }

}
