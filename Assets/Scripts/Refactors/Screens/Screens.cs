using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Screens : MonoBehaviour
{
    [SerializeField] List<BaseScreen> _basescreen;
    private void Awake()
    {
        for(int i = 0; i < _basescreen.Count; i++)
        {
            _basescreen[i].Load();
        }

        FindMainScreenItem();
    }
    
    void FindMainScreenItem()
    {
        Transform screen = GameObject.Find("Canvas").transform.Find("MainScreen").Find("Screen");

        Button btnbank = screen.Find("BTNBank").GetComponent<Button>();

        MainScreen.btnbank = btnbank;
    }
}

[Serializable]
public class BaseScreen
{
    [SerializeField] string name;
    [SerializeField] Transform Base;

    [SerializeField] List<TranstionDirection> _directions;
    public void Load()
    {
        LoadDirections();
    }
    
    void LoadDirections()
    {
        for (int i = 0; i < _directions.Count; i++)
        {
            _directions[i].Load(Base);
        }
    }
   
}

[Serializable]
public class TranstionDirection
{
    private Transform Base;
    [SerializeField] Transform Target;

    [SerializeField] Button Event;

    public void Load(Transform Base)
    {
        this.Base = Base;
        Event.onClick.AddListener(TransitionTrigger);
    }

    void TransitionTrigger()
    {
        Base.gameObject.SetActive(false);
        Target.gameObject.SetActive(true);
    }

}

public struct MainScreen
{
    public static Button btnbank;
}



