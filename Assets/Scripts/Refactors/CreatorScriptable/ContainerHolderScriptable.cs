using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Container 
{
    public static Transform FAVORITESHARE;
    public static Transform MYSHARE;
    public static Transform MARKETSHARE;
    public static Transform CUSTOMERS;
    public static Transform MYCUSTOMER;

    public static Transform INSTRUCTIONS;
    public static Transform CREDITS;
    public static Transform HISTORY;
    public static Transform NEWS;
    public static void SetContainers(Transform marketContainer, Transform myShareContainer, Transform favoriteContainer
        ,Transform customerContainer,Transform mycustomercontainer,Transform instructionscontainer,Transform creditcontainer
        ,Transform historycontainer,Transform newscriptable)
    {
        MARKETSHARE = marketContainer;
        MYSHARE = myShareContainer;
        FAVORITESHARE = favoriteContainer;
        CUSTOMERS = customerContainer;
        MYCUSTOMER = mycustomercontainer;
        INSTRUCTIONS = instructionscontainer;
        CREDITS = creditcontainer;
        HISTORY = historycontainer;
        NEWS = newscriptable;
    }


}



