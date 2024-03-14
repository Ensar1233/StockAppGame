using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Trade 
{
    //TOGGLE BUTTONS SIL
    private Animator anim;
    
    private TextMeshProUGUI tmpShareName;
    private TextMeshProUGUI tmpUnitPrice;
    private TextMeshProUGUI tmpDescription;

    private TMP_InputField inpShareAmount;
    private TMP_InputField inp_instruction;

    private TMP_Dropdown dropdowninstructions;
    private TMP_Dropdown dropdowncustomers;

    private bool ismytab;

    private BaseShareModel share;

    private Transform tradeproccess;

    private Button tabcustomer, tabmy;

    public Trade()
    {
        ComponentAssigment();
        TradeScreen.Open = Open;
        //TradeScreen.DropDownCustomer;
    }

    void ComponentAssigment()
    {
        Transform instruciontrade;
        Transform canva = GameObject.FindGameObjectWithTag("Canva").transform;
        

        //tradeproccess = GameObject.FindGameObjectWithTag("TradeProccess").transform;
        tradeproccess = canva.Find("TradePopUp");

        anim = tradeproccess.GetComponent<Animator>();

        tmpShareName = tradeproccess.Find("TradeShareName").GetComponent<TextMeshProUGUI>();
        tmpUnitPrice = tradeproccess.Find("TradeUnitPrice").GetComponent<TextMeshProUGUI>();
        tmpDescription = tradeproccess.Find("TradeDescription").GetComponent<TextMeshProUGUI>();

        inpShareAmount= tradeproccess.Find("TradeInputShareAmount").GetComponent<TMP_InputField>();
        //
        instruciontrade = tradeproccess.Find("TradeInstructions");

        Transform instructionparent = instruciontrade.Find("InstructionSentence");

        dropdowninstructions = instructionparent.Find("DRP_LIST").GetComponent<TMP_Dropdown>();
        inp_instruction = instructionparent.Find("INP_UnitPrice").GetComponent<TMP_InputField>();

        dropdowncustomers = tradeproccess.Find("TradeDRPCustomers").GetComponent<TMP_Dropdown>();

        tabcustomer = tradeproccess.Find("TAB").transform.Find("Customer").GetComponent<Button>();
        tabmy = tradeproccess.Find("TAB").transform.Find("My").GetComponent<Button>();

        tabcustomer.onClick.AddListener(SelectCustomerTab);
        tabmy.onClick.AddListener(SelectMyTab);

        tradeproccess.Find("TradeBuy").GetComponent<Button>().onClick.AddListener(Buy);
        tradeproccess.Find("TradeSell").GetComponent<Button>().onClick.AddListener(Sell);

        tradeproccess.Find("TradeClose").GetComponent<Button>().onClick.AddListener(Close);

        instruciontrade.Find("BTN_InstructionSell").GetComponent<Button>().onClick.AddListener(InstructedSales);
        instruciontrade.Find("BTN_InstructionBuy").GetComponent<Button>().onClick.AddListener(InstructedPurchase);

        inpShareAmount.onValueChanged.AddListener(OnChangeValueSharAmount);

        TradeScreen.DropDownCustomer = new CustomerDropDown(dropdowncustomers);
        InstructionsDropDown = new InstructionsDropDown(dropdowninstructions);


    }
    public CustomerDropDown DropDownCustomer { get; set; }
    public InstructionsDropDown InstructionsDropDown { get; set; }

    public Account CurrentAccount
    {
        get
        {
            if (ismytab) return Accounts.PlayerAccount;
            else return TradeScreen.DropDownCustomer.SelectedCustomer;
        }
    }

    public int InputShareAmount { get => int.Parse(inpShareAmount.text == "" ? "0" : inpShareAmount.text); }
    public int InstructionPrice { get => int.Parse(inp_instruction.text); }
    public int Cost { get => InputShareAmount * InstructionPrice; }

    public void Buy()
    {
        int amount = InputShareAmount;
        if (amount <= 0) return;

        if (CurrentAccount == null)
        {
            DataWarningNotification.SetMessage($"Musteriniz bulunmamakta.", 3);
            return;
        }

        share.Buy(CurrentAccount, amount);
        
    }

    public void Sell()
    {
        int amount = InputShareAmount;
        if (amount <= 0) return;
        
        if(CurrentAccount==null)
        {
            DataWarningNotification.SetMessage($"Musteriniz bulunmamakta.",3);
            return;
        }

        share.Sell(CurrentAccount, amount);
    }


    public void Open(BaseShareModel bshr)
    {

        tmpShareName.text = bshr.shareModel.shareName;
        tmpUnitPrice.text = bshr.shareModel.price + "TL";
        
        tmpDescription.text = $"{bshr.shareModel.price} birim fiyattan alinan 0 adet {bshr.shareModel.shareName} hissesi bulunmaktadir.";


        share = bshr;

        PopUpBackground.OpenEvent = OpenAnim;
        PopUpBackground.Open();

        ismytab = true;

        if (ismytab) dropdowncustomers.gameObject.SetActive(!ismytab);

        SelectMyTab();
    }
    void OpenAnim()
    {
        tradeproccess.gameObject.SetActive(true);
    }

    

    public void Close()
    {
        tradeproccess.gameObject.SetActive(false);
        PopUpBackground.Close();
        Time.timeScale = 1;

    }
    void OnChangeValueSharAmount(string shareamount)
    {

        tmpDescription.text = $"{share.shareModel.price} birim fiyattan alinan {shareamount} adet {share.shareModel.shareName} hissesi bulunmaktadir.";
    }
    public void SelectMyTab()
    {
        SelectedTab(tabmy, tabcustomer, true);
    }

    public void SelectCustomerTab()
    {
        SelectedTab(tabcustomer, tabmy, false);
    }

    void SelectedTab(Button selected,Button disabled,bool ismytab)
    {
        Color disabledcolor;

        if (ColorUtility.TryParseHtmlString("#636363", out disabledcolor))
        {
            BGSmoothColor.SelectedButton(selected, disabled, disabledcolor);
        }
        this.ismytab = ismytab;

        dropdowncustomers.gameObject.SetActive(!ismytab);

    }

    void InstructedPurchase() // talimat ile satin alma
    {
        NewInstruction(InstructionTrade.BUY);
    }
    void InstructedSales() // talimat ile satis yapma
    {
        if (CurrentAccount.FindMyShare(share.shareModel.shareName)==null)
        {
            Debug.Log("Hisseye sahip degil.");
            DataWarningNotification.SetMessage($"Bu hisse sahip degilsin.Satis emri veremezsin",3);
            return;
        }
        Debug.Log("Hisseye sahip");

        NewInstruction(InstructionTrade.SELL);
    }


    void NewInstruction(InstructionTrade trade) // paramiz yetiyor ise talimat verebiliriz.
    {

        string id = EKRandom.ID();
        string instructiontype;
        if (trade == InstructionTrade.BUY) instructiontype = "alis";
        else instructiontype = "satis";
        
        string text = $"{InstructionPrice} birim fiyattan {InputShareAmount} adet {share.shareModel.shareName} hisse {instructiontype} talimati {CurrentAccount.Name} verilmistir.";

        Instruction instruction = new Instruction(id, InstructionPrice, InputShareAmount, share, text, InstructionsDropDown.instructiontype, trade,
            CurrentAccount);

        InstructionModel instructionmodel = new InstructionModel(id, InstructionPrice, InputShareAmount, share.shareModel.shareName
            , text, (int)InstructionsDropDown.instructiontype, (int)trade, CurrentAccount.Name);

        CurrentAccount.accountmodel._instructions.Add(instructionmodel);
        CurrentAccount._myinstructions.Add(instruction);

        DataAchievementNotifications.SetNotify(BGIcons.INSTRUCTION, $"{share.shareModel.shareName} hisse {instructiontype} talimati " +
            $"{CurrentAccount.Name} verilmistir.",3);

    }
}


public struct TradeScreen
{
    public static System.Action<BaseShareModel> Open;
    public static CustomerDropDown DropDownCustomer;
}


public struct EKRandom
{
    public static char[] chars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', '1', '2', '3', '4', '5', '6' };

    public static string ID()
    {
        string id = "";
        for(int i = 0; i < 15; i++)
        {
            int randindex = Random.Range(0, chars.Length - 1);
            id += chars[randindex];
        }

        return id;
    }

}