using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "Customers", menuName = "ScriptableObjects/Customers")]
public class CustomersScriptable : ScriptableObject , ISetupLoad
{

    public List<CustomerModel> _customermodels = new List<CustomerModel>();

    private List<CustomerAccount> _customeraccounts = new List<CustomerAccount>();

    private Customers customers;
    public void Load()
    {
        LoadCustomerData();
        CreateCustomers();

        Customers.FindCustomerModel = FindCustomerModel;
        Customers.FindCustomerAccount = FindCustomerAccount;

        Setup.ApplicationQuit += AppQuit;
        GameSave.Save += Save;

        MobileLOG.Log("Load Customers...");
    }

    void CreateCustomers()
    {
        CustomerAccount customeraccount;
        CustomerModel savecustomermodel;
        CustomerModel customermodel;

        MyAccount playermodel = Accounts.PlayerAccount;


        for(int i = 0; i < _customermodels.Count; i++)
        {
            customermodel = _customermodels[i];
            customeraccount = new CustomerAccount(customermodel,Container.CUSTOMERS);
                    
            savecustomermodel = playermodel.FindCustomerModel(customermodel.name);
            if (savecustomermodel != null) customermodel.agreement = true;

            customeraccount.BTNSetActive(customermodel.agreement);
            _customeraccounts.Add(customeraccount);
        }


    }

    void LoadCustomerData()
    {
        string jsondata = PlayerPrefs.GetString("Customers");

        if (jsondata == "") customers._customermodels = _customermodels;
        else
        {
            customers = JsonUtility.FromJson<Customers>(jsondata);
            _customermodels = customers._customermodels;
            Debug.Log(jsondata);
        }

        BGSavedLists._customers = _customermodels;
    }
  
    void AppQuit()
    {

        for(int i = 0; i < _customermodels.Count; i++)
        {
            _customermodels[i].agreement = false;
            FreeInstructions(_customermodels[i]);
        }

    }
    
    void FreeInstructions(CustomerModel model)
    {
        List<InstructionModel> _instructions = model._instructions;
        for(int i = 0; i < model._instructions.Count; i++)
        {
            _instructions.RemoveAt(i);
        }

    }

    private CustomerModel FindCustomerModel(string name)
    {
        for(int i = 0; i < _customermodels.Count; i++)
        {
            if(_customermodels[i].name == name)
            {
                return _customermodels[i];
            }

        }
        return null;
    }
    CustomerAccount FindCustomerAccount(string name)
    {
        for (int i = 0; i < _customeraccounts.Count; i++)
        {
            Debug.Log(_customeraccounts[i].customermodel.name);
            if (_customeraccounts[i].customermodel.name== name)
            {
                return _customeraccounts[i];
            }

        }
        return null;
    }

    void Save()
    {
        PlayerPrefs.SetString("Customers", JsonUtility.ToJson(customers));

        //RecordedData.MySaveData(customers, "Customers.json");
    }
}

[Serializable]
public class CustomerModel : AccountModel
{

    public Sprite profilephoto;

    public string spritepath;
    public string name;
    public int punisamount,prizeamount;
    public int months;
    public int targetmoney,beginmoney;
    public int remainingday;
    

    public bool agreement;

}

[Serializable]
public struct Customers
{
    public List<CustomerModel> _customermodels;


    public static Func<string,CustomerModel> FindCustomerModel;
    public static Func<string, CustomerAccount> FindCustomerAccount;

}


