using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CustomDropDown
{

    protected TMP_Dropdown dropdown;

    public CustomDropDown(TMP_Dropdown dropdown)
    {
        this.dropdown = dropdown;

        this.dropdown.onValueChanged.AddListener(OnChangedValue);
    }

    public virtual void OnChangedValue(int index) { }

}


public class CustomerDropDown : CustomDropDown
{
    private TextMeshProUGUI tmplabel;
    private Image pp;
    List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();

    public CustomerDropDown(TMP_Dropdown dropdown) : base(dropdown)
    {
        tmplabel = dropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        pp = dropdown.transform.GetChild(2).GetComponent<Image>();
    }

    public CustomerAccount SelectedCustomer { get; set; }

    public override void OnChangedValue(int index)
    {
        SelectedCustomer = Accounts.PlayerAccount.FindCustomerAccount(dropdown.options[index].text);
    }

    public void AddItem(CustomerAccount account)
    {
        TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(account.customermodel.name,account.customermodel.profilephoto);
        

        dropdown.options.Add(optionData);
        optionDatas.Add(optionData);

        if (dropdown.options.Count > 1) return;

        FirstAddItem();
    }

    public void RemoveItem(CustomerAccount account)
    {

        dropdown.options.Remove(FindOptionData(account.customermodel.name));
    }
    TMP_Dropdown.OptionData FindOptionData(string text)
    {
        for(int i = 0; i < optionDatas.Count; i++)
        {
            if(optionDatas[i].text == text)
            {
                return optionDatas[i];
            }
        }
        return null;
    }

    public void SetLabel(string label)
    {
        tmplabel.text = label;
    }
    public void SetSprite(Sprite sprite)
    {
        pp.sprite = sprite;   
    }
    void FirstAddItem()
    {
        SelectedCustomer = Accounts.PlayerAccount.FindCustomerAccount(dropdown.options[0].text);
        //SetLabel(SelectedCustomer.Name);
        //SetSprite(SelectedCustomer.customermodel.profilephoto);
    }

}


public class InstructionsDropDown : CustomDropDown
{


    public InstructionType instructiontype;
    public InstructionsDropDown(TMP_Dropdown dropdown) : base(dropdown)
    {
    }

    public override void OnChangedValue(int index)
    {
        if (index == 0) instructiontype = InstructionType.OVER;
        else instructiontype = InstructionType.UNDER;

    }

}

