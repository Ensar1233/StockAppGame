using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CustomerAccount : Account
{
    private GameObject new_customer;

    public CustomerModel customermodel;

    /*Customer Variables*/
    private Image profilephoto;

    private TextMeshProUGUI tmpname, tmpdescription, tmptotal,tmpremainingday;

    private Button btnaccept;

    public Transform scrollshares;
    /*Customer Variables*/

    public CustomerAccount(CustomerModel customermodel, Transform container) : base(customermodel.name)
    {
        this.accountmodel = customermodel;

        this.customermodel = customermodel;

        mysharescale = new Vector3(0.85f, 0.85f, 0);

        CreateCustomer(container);
        ComponentAssigment();

        ComponentAssignVariables(customermodel);

        LoadData();

    }
    void CreateCustomer(Transform container)
    {
        GameObject prefab;
        prefab = Resources.Load<GameObject>("Customers/Customer");
        new_customer = GameObject.Instantiate(prefab, container);
    }

    void ComponentAssigment()
    {
        Transform transform = new_customer.transform;

        profilephoto = transform.Find("CustomerPP").GetComponent<Image>();

        tmpname = transform.Find("CustomerName").GetComponent<TextMeshProUGUI>();
        tmpdescription = transform.Find("CustomerDescription").GetComponent<TextMeshProUGUI>();
        tmptotal = transform.Find("TMP_TotalMoney").GetComponent<TextMeshProUGUI>();
        tmpremainingday = transform.Find("TMP_RemainingDay").GetComponent<TextMeshProUGUI>();

        btnaccept = transform.Find("BTN_Accept").GetComponent<Button>();
        scrollshares = transform.GetChild(transform.childCount - 1);

        ContainerAccount = scrollshares.GetChild(0);
    }
    public void ComponentAssignVariables(CustomerModel customermodel)
    {

        profilephoto.sprite = Resources.Load<Sprite>($"CustomerPhoto/{customermodel.spritepath}");
        customermodel.profilephoto = profilephoto.sprite;

        tmpname.text = customermodel.name;
        tmpdescription.text = $"{customermodel.beginmoney} TL param var. {customermodel.months} ayda {customermodel.targetmoney} TL yapar misin? \n" +
            $"�d�l : <color=green>{customermodel.prizeamount} TL</color> Ceza : <color=red>{customermodel.punisamount} TL</color>";
        
        Balance = customermodel.beginmoney;

        tmptotal.text = "Total para: " + Balance;

        btnaccept.onClick.AddListener(ClickAcceptButton);

        btnaccept.interactable = !customermodel.agreement;

    }


    public void ConvertContractCustomer()
    {

        scrollshares.gameObject.SetActive(true);
        btnaccept.gameObject.SetActive(false);
        tmptotal.gameObject.SetActive(true);
        tmpremainingday.gameObject.SetActive(true);


        MyAccountBoard.DateUpdate += DateUpdate;

    }

    public void ClickAcceptButton()
    {

        if (customermodel.agreement) return;

        CustomerAccount account;
        CustomerModel customer = CopyCustomer(customermodel);

        account = new CustomerAccount(customer/*customermodel*/, Container.MYCUSTOMER);
        account.ConvertContractCustomer();
        account.customermodel.remainingday = customermodel.months * 30;


        Accounts.PlayerAccount.AddMyCustomers(account,ReasonForAdding.ADDING);//update

        btnaccept.interactable = false;
        customermodel.agreement = true;

        DataAchievementNotifications.SetNotify(BGIcons.DEAL, $"{customermodel.name} ile anlasmaya varildi iyi sanslar.", 3);
        HistoryFunction.AddHistory($"{customermodel.name} ile {customermodel.months} aylik anlasmaya varildi.");
    }

    public void DateUpdate()
    {
        SetRemainingDayControl();
        if (customermodel == null) return;
        SetTMPRemainingDay();

        customermodel.remainingday--;

    }

    void SetRemainingDayControl()
    {
        if (customermodel.remainingday <= 0)
        {
            int punishamount = customermodel.punisamount;

            DealStatus(BGIcons.NEGATIVE, -punishamount, $"{customermodel.name} ile anlasilan suren bitti.ceza {punishamount}");
            return;
        }

    }

    void SetTMPRemainingDay()
    {
        tmpremainingday.text = $"Kalan g�n: {customermodel.remainingday}";
    }
    public override void SetBalance(int cost)
    {
        
        Balance += cost;
        tmptotal.text = Balance + " TL";

        BalanceStatus();
    }
    
    void BalanceStatus()
    {
        if (Balance >= customermodel.targetmoney)
        {
            int rewardamount = customermodel.prizeamount;
            DealStatus(BGIcons.POSITIVE,rewardamount, $"{customermodel.name} basarindan dolayi tebrik ediyor.�d�l : {rewardamount}");
        } // basarili

        else if (Balance <= 0)
        {
            int punishamount = customermodel.punisamount;

            DealStatus(BGIcons.NEGATIVE,-punishamount, $"Musterini batirdin.Ceza {punishamount}");
        } // basarisiz.

    }
    void DealStatus(Sprite icon,int amount, string notify)
    {

        DataPlayerMethods.SetBalance(amount);
        DataAchievementNotifications.SetNotify(icon,notify, 3); // notifaction

        Destroy();
    }
            
    public void BTNSetActive(bool active)
    {
        btnaccept.interactable = !active;
    }

    CustomerModel CopyCustomer(CustomerModel origincustomer)
    {
        CustomerModel customer = new CustomerModel();

        //customer.balance = origincustomer.balance;
        customer.name = origincustomer.name;
        customer.beginmoney = origincustomer.beginmoney;
        customer.targetmoney = origincustomer.targetmoney;
        customer.spritepath = origincustomer.spritepath;
        customer.months = origincustomer.months;
        customer.prizeamount = origincustomer.prizeamount;
        customer.punisamount = origincustomer.punisamount;

        return customer;
    }

    void Destroy()
    {
        NewCustomerModel();//

        MyAccountBoard.DateUpdate -= DateUpdate;
        DataPlayerMethods.RemoveCustomer(this);

        DischargeCustomerModel();

        customermodel = null;
        GameObject.Destroy(new_customer);

    }
    
    void NewCustomerModel()
    {
        CustomerModel customermodel = Customers.FindCustomerModel(this.customermodel.name);

        CustomerAccount customeraccount = Customers.FindCustomerAccount(this.customermodel.name);
        TradeScreen.DropDownCustomer.RemoveItem(customeraccount);

        BGGenerate.CustomerGenerateValue(customermodel);
        customermodel.agreement = false;
        
        customeraccount.ComponentAssignVariables(customermodel);
    }

    void DischargeCustomerModel()
    {

      
        for(int i = 0; i < customermodel._instructions.Count; i++)
        {
            customermodel._instructions.RemoveAt(i);
        }

        Accounts.PlayerAccount.playermodel._mycustomermodels.Remove(customermodel);

        customermodel.agreement = false;
    }

}