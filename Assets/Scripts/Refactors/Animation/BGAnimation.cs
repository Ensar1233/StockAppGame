using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct BGSmoothColor
{
    
    public static void SelectedButton(Button selected,Button disabled,Color disabledcolor)
    {
        var selectedcolorblock = selected.colors;
        var disabledcolorblock = disabled.colors;

        selectedcolorblock.normalColor = Color.white;
        disabledcolorblock.normalColor = disabledcolor;

        selected.colors = selectedcolorblock;
        disabled.colors = disabledcolorblock;

    }
    public static void SelectedButton(Button selected, Button disabled, Color selectedcolor,Color disabledcolor)
    {
        var selectedcolorblock = selected.colors;
        var disabledcolorblock = disabled.colors;

        selectedcolorblock.normalColor = selectedcolor;
        disabledcolorblock.normalColor = disabledcolor;

        selected.colors = selectedcolorblock;
        disabled.colors = disabledcolorblock;

    }

}


public struct BGChangeColor
{
    public static void SelectButton(Button selected)
    {
        var selectedcolorblock = selected.colors;

        selectedcolorblock.normalColor = Color.cyan;

        selected.colors = selectedcolorblock;

    }

    public static void SelectButton(Button selected,Color selectcolor)
    {
        var selectedcolorblock = selected.colors;

        selectedcolorblock.normalColor = selectcolor;

        selected.colors = selectedcolorblock;

    }


    public static void DisableButton(Button disabled)
    {
        var disabledcolorblock = disabled.colors;

        disabledcolorblock.disabledColor = Color.cyan;

        disabled.colors = disabledcolorblock;

    }

    public static void SelectedColor(Button btn,Color color)
    {
        var selectedcolorblock = btn.colors;

        selectedcolorblock.selectedColor = color;

        btn.colors = selectedcolorblock;
    }
    
}