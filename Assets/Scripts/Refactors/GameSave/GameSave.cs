using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;

[CreateAssetMenu(fileName = "GameSave", menuName = "ScriptableObjects/GameSave")]
public class GameSave : ScriptableObject ,ISetupLoad
{
    
    public static Action Save;

    public static Action ASaveEverything;

    public static UnityAction ASaveClick;
    public void Load()
    {
        Button btnsave = GameObject.FindGameObjectWithTag("BTNSave").GetComponent<Button>();

        btnsave.onClick.AddListener(SaveGameClick);
        ASaveEverything = SaveEverything;
        
    }

    public void SaveGameClick()
    {
        SaveEverything();

        ASaveClick?.Invoke();

        DataAchievementNotifications.SetNotify(BGIcons.SAVEGAME,"Oyun kaydedildi...", 2);
    }
    
    public void SaveEverything()
    {
        Save?.Invoke();
    }


}


public struct RecordedData
{
    
    public static void MySaveData(object gamedata, string filename)
    {
        string jsondata = JsonUtility.ToJson(gamedata,true);

        File.WriteAllText(Application.persistentDataPath + "/" + filename, jsondata);

    }
    public static T MyLoadData<T>(string filename)
    {
        T t = default(T);

        if (File.Exists(Application.persistentDataPath + "/" + filename))
        {
            string jsonData = File.ReadAllText(Application.persistentDataPath + "/" + filename);
            t = JsonUtility.FromJson<T>(jsonData);

        }
        return t;


    }
}