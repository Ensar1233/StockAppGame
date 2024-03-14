using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleMenu : MonoBehaviour
{
    [SerializeField] List<Button> _toggles;
    [SerializeField] List<Transform> _screens;

    [SerializeField] Color selectedcolor;

    private void Awake()
    {
                                
        for (int i = 0; i < _toggles.Count; i++)
        {
            int currentindex = i;
            _toggles[i].onClick.AddListener(() => ToggleClick(currentindex));
        }

        ToggleClick(0);

    }
    void ToggleClick(int index)
    {
        
        if (TempToggle.Toggle != null) BGSmoothColor.SelectedButton(_toggles[index], TempToggle.Toggle, selectedcolor, Color.black);
        else BGChangeColor.SelectButton(_toggles[index], selectedcolor);

        if (_screens[index].gameObject.activeInHierarchy) return;

        _screens[index].gameObject.SetActive(true);
        if (TempToggle.Screen != null) TempToggle.Screen.gameObject.SetActive(false);

        TempToggle.Toggle = _toggles[index];
        TempToggle.Screen = _screens[index];
        
    }


}


public struct TempToggle
{
    public static Button Toggle;
    public static Transform Screen;
}