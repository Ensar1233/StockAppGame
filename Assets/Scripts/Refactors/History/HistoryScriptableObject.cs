using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "History", menuName = "ScriptableObjects/History")]
public class HistoryScriptableObject : ScriptableObject , ISetupLoad
{
    DataHistory datahistory;

    History history;
    public void Load()
    {
        LoadData();
        MobileLOG.Log("Load History...");
        GameSave.Save += SaveData;

    }

    void SaveData()
    {
        PlayerPrefs.SetString("History", JsonUtility.ToJson(datahistory));
    }
    void LoadData()
    {
        DataHistory datahistory;
        string jsondata;
        
        jsondata = PlayerPrefs.GetString("History");
        datahistory = JsonUtility.FromJson<DataHistory>(jsondata);

        
        if (datahistory != null) this.datahistory = datahistory;                    
        else this.datahistory = new DataHistory();

        history = new History(this.datahistory);

        BGSavedLists._histories = this.datahistory._histories;

        LoadCreateHistory();
    }

    void LoadCreateHistory()
    {
        for (int i = 0; i < datahistory._histories.Count; i++)
        {
            history.CreateItem(datahistory._histories[i]);
        }

    }

}



public class History
{

    
    private GameObject prefab;

    private Transform container;
    private DataHistory datahistory;

    public History(DataHistory datahistory)
    {
        this.datahistory = datahistory;

        prefab = Resources.Load<GameObject>("History/HistoryItem");
        container = Container.HISTORY;

        HistoryFunction.AddHistory = Add;
    }

    public void Add(string message)
    {
        CreateItem(message);

        datahistory._histories.Add(message);

        if (container.childCount < 9) return;

        GameObject.Destroy(container.GetChild(container.childCount - 1).gameObject);
        datahistory._histories.RemoveAt(0);

    }
    public void CreateItem(string text)
    {
        GameObject new_item = GameObject.Instantiate(prefab, Container.HISTORY);

        new_item.transform.SetAsFirstSibling();

        new_item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

    }


}

[System.Serializable]
public class DataHistory
{
    public List<string> _histories = new List<string>();

}

public struct HistoryFunction
{
    public static System.Action<string> AddHistory;
}
