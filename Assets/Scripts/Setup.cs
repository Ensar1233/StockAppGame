using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.IO;
public class Setup : MonoBehaviour
{
    private static Setup instance;
    


    [Header("Containers")]
    /*Containers*/
    public Transform customerContainer;
    public Transform marketShareContainer2;
    public Transform favoriteShareContainer2;
    public Transform myShareContainer;
    public Transform mycustomercontainer;
    public Transform instructionscontainer;
    public Transform creditcontainer;
    public Transform historycontainer;
    public Transform newscontainer;
    /*Containers*/

    [Header("ScriptableObjects")]

    [SerializeField] List<ScriptableObject> _scriptableobjects;
    
    public static UnityAction update;

    public static UnityAction Load;
    public static UnityAction ApplicationQuit;

    [Header("OperatingSystem")]
    [SerializeField] BGOperatingSystem operatingsystem;

    private void Awake()
    {
        //BGSelectedOperatingSystem.operatingsystem = operatingsystem;

        Trade trade = new Trade();
        DetailShare detail = new DetailShare();

        SettingsScreen setting = new SettingsScreen();

        Container.SetContainers(marketShareContainer2, myShareContainer, favoriteShareContainer2,customerContainer,mycustomercontainer,
            instructionscontainer,creditcontainer,historycontainer,newscontainer);

        LoadScriptable();
        Debug.Log("Setup awake bitti...");
    }
    private void Start()
    {
        MobileLOG.Log("LoadNotification Oncesi");
        LoadNotification();
        MobileLOG.Log("Target Oncesi");
        LoadTargets();
        MobileLOG.Log("Target sonrasi");

    }

    public static Setup Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Setup>();
            return instance;
        }
            
    }
    private void Update()
    {

        update?.Invoke();
    }

    void LoadNotification()
    {
        AchievementNotifications achievement = new AchievementNotifications();
        ErrorNotification errorNotification = new ErrorNotification();

        DataAchievementNotifications.SetNotify = achievement.SetNotify;
        DataWarningNotification.SetMessage = errorNotification.SetMessage;

    }
    void LoadTargets()
    {
        TargetModel targetmodel;
        string jsondata;

        jsondata = PlayerPrefs.GetString("Target");
        targetmodel = JsonUtility.FromJson<TargetModel>(jsondata);

        if (targetmodel == null) targetmodel = new TargetModel(0, false, false);

        ScreenTarget.targetscreen = new TargetScreen(targetmodel);
    }
    void LoadScriptable()
    {
        for(int i = 0; i < _scriptableobjects.Count; i++)
        {
            if(_scriptableobjects[i] is ISetupLoad)
            {
                ISetupLoad load = (ISetupLoad) _scriptableobjects[i];

                load.Load();
            }
        }

    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        ApplicationQuit?.Invoke();
    }

}

public interface ISetupLoad
{
    public void Load();
}



public struct BGSelectedOperatingSystem
{
    public static BGOperatingSystem operatingsystem;

}

public enum BGOperatingSystem
{
    ANDROID,WINDOWS

}
