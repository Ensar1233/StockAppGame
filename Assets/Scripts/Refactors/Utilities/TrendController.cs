using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TrendController
{

    int shrUnitPrice;

    public int UpdatePrice(string type)
    {
        int randomNum;
        switch (type)
        {

            case "A":
                randomNum = Random.Range(1, 11);

                if (randomNum <= 6)
                {
                    //fix
                }
                else if (randomNum == 7 || randomNum == 8)
                {
                    shrUnitPrice = 10;
                }
                else if (randomNum == 9 || randomNum == 10)
                {
                    shrUnitPrice = -5;
                }
                break;

            case "B":
                randomNum = Random.Range(0, 2);
                if (randomNum == 0)
                {
                    shrUnitPrice = Random.Range(5, 21);
                }
                else if (randomNum == 1)
                {
                    shrUnitPrice -= Random.Range(5, 21);
                }
                break;
            case "C":
                shrUnitPrice = -Random.Range(5, 15);
                break;
            case "D":
                shrUnitPrice = Random.Range(5, 15);
                break;
        }
        TrendKey = type;
        return shrUnitPrice;
    }

    public string TrendName(string key)
    {
        string trendname = "";
        switch (key)
        {
            case "A":
                trendname = "<color=#FAC50F>Istikrarli</color>"; 
                break;
            case "B":
                trendname = "<color=#B2B2B2>Dengesiz</color>"; 
                break;
            case "C":
                trendname = "<color=red>Dusus</color>";
                break;
            case "D":
                trendname = "<color=#64EE0F>Yukselis</color>"; 
                break;
        }
        return trendname;

    }

    public string TrendKey { get; set; }

}

