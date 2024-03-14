using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public abstract class Account
{
    public List<MyShareModel> _mysharemodels = new List<MyShareModel>();
    public List<Instruction> _myinstructions = new List<Instruction>();

    public AccountModel accountmodel;

    public Vector3 mysharescale = new Vector3(1, 1, 1);
    public Transform ContainerAccount { get; set; }

    public int Balance { get { return accountmodel.balance; } set { accountmodel.balance = value; } }

    public string Name { get; set; }

    public Account(string name)
    {
        Name = name;
    }

    public MyShareModel HaveMyShare(ShareModel model)
    {

        foreach (MyShareModel myshare in _mysharemodels)
        {
            if (myshare.shareModel.shareName == model.shareName)
            {
                return myshare;

            }
        }

        return null;
    }

    public bool BalanceIsEnough(int cost)
    {
        if (cost <= Balance) 
        {
            SetBalance(-cost);
            return true;
        }
        return false;
    }

    public void AddMyShare(MyShareModel myShare)
    {
        _mysharemodels.Add(myShare);
    }
    public void RemoveMyShare(MyShareModel myshare)
    {
        _mysharemodels.Remove(myshare);

        accountmodel._myshare.Remove(FindMyShare(myshare.shareModel.shareName));

        myshare = null;
    }
    protected void LoadData()
    {
        List<MyShare> _myshare = accountmodel._myshare;


        MyShareModel mysharemodel;
        for(int i = 0; i < _myshare.Count; i++)
        {
            mysharemodel = new MyShareModel(_myshare[i].sharemodel,_myshare[i].amount, this);

            _mysharemodels.Add(mysharemodel);
        }

    }

    public MyShare FindMyShare(string name)
    {
        List<MyShare> _myshare = accountmodel._myshare;

        for(int i = 0; i < _myshare.Count; i++)
        {
            if (_myshare[i].sharemodel.shareName == name)
            {
                return _myshare[i];
            }
        }
        return null;
    }

    public abstract void SetBalance(int cost);

}
