using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credit
{

    private GameObject new_credit;
    private CreditModel creditmodel;
    public Credit(CreditModel creditmodel,Transform container)
    {
        this.creditmodel = creditmodel;
        Create(container);

        ComponentAssignVariables();
    }
    void Create(Transform container)
    {
        GameObject prefab;

        prefab = Resources.Load<GameObject>("Credits/Credit");

        new_credit = GameObject.Instantiate(prefab, container);
    }
   
    void ComponentAssignVariables()
    {
        Transform transform = new_credit.transform;
        int count = creditmodel.interestrates.Length;
        Button btn;

        transform.Find("TMPCreditBalance").GetComponent<TextMeshProUGUI>().text = creditmodel.amount +" TL";

        for(int i = 0; i < count; i++)
        {
            btn = transform.Find($"BTN{i+1}").GetComponent<Button>();
            new SelecTableCredit(btn, creditmodel.interestrates[i],i+1);

            CreditAllButton._credits.Add(btn);
        }


    }

}

//TODO: onceki buttonun renk referansini tutabilir.
public class SelecTableCredit
{
    private Button button;
    private int interestamount;
    private int year;
    public SelecTableCredit(Button btn,int interestamount,int year)
    {
        button = btn;
        this.interestamount = interestamount;
        this.year = year;

        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = interestamount + " TL";

        button.onClick.AddListener(Click);
    }
    void Click()
    {

        InitilizedCreditValue();
        SelectionAnim();

    }


    void InitilizedCreditValue()
    {

        TempCreditValue.creditdept = interestamount;
        TempCreditValue.yearpaid = year;
        TempCreditValue.monthlydept = interestamount / (year * 12);
    }

    void SelectionAnim()
    {

        if (TempCreditTab.Tab != null) BGSmoothColor.SelectedButton(button, TempCreditTab.Tab, Color.black);
        else
        {
            var colorblock = button.colors;
            colorblock.normalColor = Color.white;
            button.colors = colorblock;

        }
        TempCreditTab.Tab = button;
    }


}


public struct CreditAllButton
{
    public static List<Button> _credits = new List<Button>();

    public static int index(Button btn)
    {
        for(int i = 0; i < _credits.Count; i++)
        {
            if(_credits[i] == btn)
            {
                return i;
            }
        }

        return 0;
    }

    public static Button FindButton(int index)
    {
        for (int i = 0; i < _credits.Count; i++)
        {
            if (i == index)
            {
                return _credits[i];
            }
        }
        return null;
    }
    public static void AllCreditButtonInteractable(bool interactable)
    {
        List<Button> _credits = CreditAllButton._credits;

        for (int i = 0; i < _credits.Count; i++)
        {
            _credits[i].interactable = interactable;
        }

    }


}

public struct TempCreditTab
{
    public static Button Tab;

}

public struct TempCreditValue
{
    public static int creditdept;
    public static int yearpaid;
    public static int monthlydept;
    public static int index;

}
