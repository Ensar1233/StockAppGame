using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class TradePopUp : MonoBehaviour
{
    private static TradePopUp instance;
    
    [Header("Text")]
    [SerializeField] TextMeshProUGUI tmpShareName;
    [SerializeField] TextMeshProUGUI tmpUnitPrice;
    [SerializeField] TextMeshProUGUI tmpDescription;
    [Header("Input")]
    [SerializeField] TMP_InputField inpShareNumber;
    [SerializeField] TMP_InputField inpOtoShareNumber;
    [SerializeField] TMP_InputField inp_instruction;
    [Header("Dropdown")]
    [SerializeField] TMP_Dropdown dropdowninstructions;
    [SerializeField] TMP_Dropdown dropdowncustomers;
    [Header("Image")]
    [SerializeField] Image img_customer, img_my;

    private bool ismytab;        

    private BaseShareModel bshr;

    private Animator anim;

    public CustomerDropDown DropDownCustomer{ get; set; }
    public InstructionsDropDown InstructionsDropDown { get; set; }

    public int InputShareAmount { get => int.Parse(inpShareNumber.text == "" ? "0" : inpShareNumber.text); }
    public int InstructionPrice { get => int.Parse(inp_instruction.text); }

    public int Cost { get => InputShareAmount * InstructionPrice; }

    private Account CurrentAccount
    {
        get
        {
            if (ismytab) return Accounts.PlayerAccount;

            return DropDownCustomer.SelectedCustomer;
        }
    }


    public static TradePopUp Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TradePopUp>();
            }
            return instance;
        }
    }


    private void Awake()
    {

        anim = GetComponent<Animator>();

        DropDownCustomer = new CustomerDropDown(dropdowncustomers);
        InstructionsDropDown = new InstructionsDropDown(dropdowninstructions);
    }

    public void Buy()
    {
        int amount = InputShareAmount ;

        if (amount<= 0) return;

        if (ismytab) bshr.Buy(Accounts.PlayerAccount, amount);//update
        else bshr.Buy(DropDownCustomer.SelectedCustomer, amount);

    }

    public void Sell()
    {
        int amount = InputShareAmount;
        if (amount<= 0) return;

        if (ismytab) bshr.Sell(Accounts.PlayerAccount, amount);//edit
        else bshr.Sell(DropDownCustomer.SelectedCustomer, amount);
    }

    public void OpenPopUp(BaseShareModel bshr)
    {
        tmpShareName.text = bshr.shareModel.shareName;
        tmpUnitPrice.text = bshr.shareModel.price + "TL";
        tmpDescription.text = bshr.shareModel.description;

        this.bshr = bshr;

        anim.SetTrigger("Open");

        ismytab = true;

        if(ismytab) dropdowncustomers.gameObject.SetActive(!ismytab);
    }


    public void Close()
    {
        anim.SetTrigger("Close");
    }


    public void SelectMyTab()
    {

        ismytab = true;

        dropdowncustomers.gameObject.SetActive(!ismytab);
    }

    public void SelectCustomerTab()
    {
        ismytab = false;

        dropdowncustomers.gameObject.SetActive(!ismytab);

    }

    public void InstructedTransaction(int index)
    {
        if (CurrentAccount.Balance < Cost) return;

        //Instruction instruction = new Instruction();


        //Instruction instruction;
        //InstrcutionTradeStatus tradestatus;

        //int instructionprice = InstructionPrice;
        //int shareamount = InputShareAmount;

        //if (index == 0) tradestatus = InstrcutionTradeStatus.BUY;            
        //else tradestatus = InstrcutionTradeStatus.SELL;
        
        

    }
    

}



