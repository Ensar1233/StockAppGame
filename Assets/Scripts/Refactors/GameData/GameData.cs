using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject , ISetupLoad
{

    public static UnityAction OnReset;

    [Header("Player")]

    public int balance;

    [Header("Shares")]
    public int[] shareprice;

    [Header("GameSettings")]

    public string date;
    public float speed;

    public void Load()
    {
        Transform screen = GameObject.Find("Canvas").transform.Find("SettingsScreen").Find("Screen");

        screen.Find("Reset").GetComponent<Button>().onClick.AddListener(Reset);

    }
    
    public void Reset()
    {
        ResetDate();
        ResetPlayer();
        ResetShares();
        ResetNews();
        ResetTax();
        ResetTargets();
        ResetCustomer();
        ResetHistory();
        

        OnReset?.Invoke();

        GameSave.ASaveEverything?.Invoke();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        
    }


    void ResetPlayer()
    {
        PlayerAccountModel playermodel = Accounts.PlayerAccount.playermodel;

        playermodel._instructions.Clear();
        playermodel._myshare.Clear();
        playermodel._favoriteshares.Clear();
        playermodel._mycustomermodels.Clear();

        playermodel.balance = balance;

    }
    void ResetShares()
    {
        List<ShareModel> _sharemodels = BGSavedLists._sharemodels;

        ShareModel sharemodel;
        for(int i = 0; i < _sharemodels.Count; i++)
        {
            sharemodel = _sharemodels[i];
            sharemodel.price = shareprice[i];
            sharemodel.lasprices.Clear();
            sharemodel.trendindex = 0;
        }

    }
  
    void ResetNews()
    {
        NewsManager.ClearNews();
    }
    void ResetTax()
    {
        Payments.TAX.remainingdays = 31;
        Payments.TAX.taxamount = 0.1f;
    }

    void ResetTargets()
    {
        TargetScreen targetscreen = ScreenTarget.targetscreen;

        targetscreen.targetmodel.Reset();

        for (int i = 0; i < 4; i++)
        {
            targetscreen.SetFailed(i);
        }
    }

    void ResetCustomer()
    {
        List<CustomerModel> _customermodels = BGSavedLists._customers;
        
        for(int i = 0; i < _customermodels.Count; i++)
        {
            _customermodels[i].agreement = false;
        }

    }
    void ResetHistory()
    {
        BGSavedLists._histories.Clear();
    }

    void ResetDate()
    {
        BGUniqueClasses.datemodel.date = date;
        BGUniqueClasses.datemodel.speed = BGGameSetting.speed;
        Board.FullDate = date;
    }
}



