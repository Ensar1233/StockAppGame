using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class Instruction 
{

    private Transform new_instruction;
    private BaseShareModel basesharemodel;
    private Account account;

    private int targetprice;

    public string id;

    Action<int> action;

    public Instruction(string id,int targetprice,int amount,BaseShareModel basesharemodel,string text,InstructionType instructiontype,
        InstructionTrade instructiontrade,Account account)//Account
    {
        this.account = account;
        this.targetprice = targetprice;
        this.basesharemodel = basesharemodel;
        this.id = id;
        Create();

        new_instruction.Find("InstructionText").GetComponent<TextMeshProUGUI>().text = text;
        new_instruction.Find("BTN_InstructionCancel").GetComponent<Button>().onClick.AddListener(Cancel);

        this.basesharemodel.shareModel = MethodsFind.FindShareModel(basesharemodel.shareModel.shareName);

        SelectedMethod(amount, instructiontype,instructiontrade);
        this.basesharemodel.shareModel.SetPricee += SetPrice;
    }
   
    void Create()
    {
        GameObject prefab;

        prefab = Resources.Load<GameObject>("Instructions/Instruction");
        new_instruction = GameObject.Instantiate(prefab,Container.INSTRUCTIONS).transform;

        new_instruction.SetAsFirstSibling();
    }
    
    void SetPrice(int price)
    {
        action?.Invoke(price);
    }
    
    void Cancel()
    {
        Destroy();
    }

    void SelectedMethod(int amount, InstructionType instructiontype,InstructionTrade instructiontrade)
    {
        
    
        action = (x) =>//uzerindeyse
        {
            Debug.Log("Uzerindeyse: " + basesharemodel.shareModel.price+" Tradetype: "+instructiontrade.ToString());
            
            if (x >= targetprice)
            {
                if(instructiontrade == InstructionTrade.BUY)
                {
                    if (account.BalanceIsEnough(x * amount))
                    {
                        basesharemodel.Buy(account, amount);
                        Destroy();

                    }
                }
                else
                {
                    basesharemodel.Sell(account, amount);
                    Destroy();

                }

            }
        };

        if (instructiontype == InstructionType.OVER) return;

        action = (x) => // altindaysa
        {
            Debug.Log("altinda: " + basesharemodel.shareModel.shareName);
            if (x <= targetprice)
            {
                if (instructiontrade == InstructionTrade.BUY)
                {
                    if (account.BalanceIsEnough(x * amount))
                    {
                        basesharemodel.Buy(account, amount);
                        Destroy();
                    }

                }
                else 
                {
                    basesharemodel.Sell(account, amount);
                    Destroy();
                }


            }
        };

    }
    public void Destroy()
    {
        InstructionModel instructionmodel;
            
        instructionmodel = MethodsFind.FindInstructionModel(id,account.accountmodel);
        

        basesharemodel.shareModel.SetPricee -= SetPrice;

        
        account.accountmodel._instructions.Remove(instructionmodel);
        account._myinstructions.Remove(this);

        GameObject.Destroy(new_instruction.gameObject);

    }


}

public enum InstructionType { OVER,UNDER}

public enum InstructionTrade { BUY,SELL}


[Serializable]
public class InstructionModel
{
    public string id;
    public string accountname;
    public string sharename;
        
    public int instructionprice;

    public string text;

    public int instructiontype,instructiontrade;

    public int amount;

    public InstructionModel(string id,int instructionprice,int amount,string sharename,string text,int instructiontype,int instructiontrade,
       string accountname )
    {
        this.id = id;
        this.sharename = sharename;
        this.instructionprice = instructionprice;
        this.text = text;
        this.instructiontype = instructiontype;
        this.instructiontrade = instructiontrade;
        this.amount = amount;
        this.accountname = accountname;
    }
}
