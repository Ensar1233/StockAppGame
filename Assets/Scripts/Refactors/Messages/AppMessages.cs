using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AppMessages
{
    public static string TradeMessage(string accountname,string name,int price,int amount,string tradetype)
    {
        int cost = price * amount;



        return $"{Board.FullDate}:<color=blue>{price}</color> TL birim fiyattan" +
                $" <color=#FA0FDA>{amount}</color> adet toplam <color=orange>{cost}</color> TL tutarinda <color=yellow>{name}</color> hissesi" +
                $" {ByWhom(accountname)} {tradetype}.";
    }

    public static string ByWhom(string name)
    {


        if (name == "") return "";
        else return $"{name} icin";
    }

}