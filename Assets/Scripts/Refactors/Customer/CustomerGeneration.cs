using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomerGeneration 
{

    int[,] minmax =
    {
        {500000,700000},
        {100000,200000},
        {300000,500000},
        {700000,1000000},
        {300000,2000000},
        {2000000,3000000},
        {100000000,150000000},
        {50000,100000},
        {100000,300000},

    };
    int[,] rewpunish =
    {
        {50000,5000 },
        {250000,100000},
        {300000,100000 },
        {5000000,2000000},
        {600000,300000 },

    };
    int womanspritecount = 4;
    int manspritecount = 6;



    int[] months = { 6, 12, 3, 24, 36 };

    private Customer[] customers =
    {
        new Customer{name="Ahmet K.",gender=CustomerGender.MALE},
        new Customer{name="Ali A.",gender=CustomerGender.MALE},
        new Customer{name="Yelda B.",gender=CustomerGender.FEMALE},
        new Customer{name="Mert M.",gender=CustomerGender.MALE},
        new Customer{name="Sakir K.",gender=CustomerGender.MALE},
        new Customer{name="Gizem D.",gender=CustomerGender.FEMALE},
        new Customer{name="Didem N.",gender=CustomerGender.FEMALE},
        new Customer{name="Mehmet L.",gender=CustomerGender.MALE},
        new Customer{name="Sarper O.",gender=CustomerGender.MALE},

    };

    public void CustomerGenerateValue(CustomerModel customermodel)
    {
        Customer customer;
        int customerrand, minmaxrand, rewpunishrand,monthsrand;
                
        customerrand = Random.Range(0, customers.Length-1);
        minmaxrand = Random.Range(0, (minmax.Length/2)-1);
        rewpunishrand = Random.Range(0, (rewpunish.Length/2)-1);
        monthsrand = Random.Range(0, months.Length-1);

        customer = customers[customerrand];

        customermodel.name = customer.name;
        customermodel.spritepath = SpritePath(customer.gender);


        customermodel.beginmoney = minmax[minmaxrand, 0];
        customermodel.targetmoney= minmax[minmaxrand, 1];

        customermodel.prizeamount = rewpunish[rewpunishrand, 0];
        customermodel.punisamount = rewpunish[rewpunishrand, 1];

        customermodel.months = months[monthsrand];

    }

    string SpritePath(CustomerGender gender)
    {
        int womanrand, manrand;
        string path;
        if(gender == CustomerGender.MALE)
        {
            manrand = Random.Range(1, manspritecount);
            path = $"man{manrand}";
        }
        else
        {
            womanrand = Random.Range(1,womanspritecount);
            path = $"woman{womanrand}";

        }

        return path;
    }

}

    
public class Customer
{
    public string name;
    public CustomerGender gender;
}

public struct BGGenerate
{
    private static CustomerGeneration instance = new CustomerGeneration();

    public static UnityAction<CustomerModel> CustomerGenerateValue = instance.CustomerGenerateValue;
}
public enum CustomerGender
{
    MALE,FEMALE

}
