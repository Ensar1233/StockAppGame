using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{

    private Animator animator;

    [SerializeField] GameObject image;
    private void Awake()
    {
        animator = GetComponent<Animator>();

        PopUpBackground.Open = Open;
        PopUpBackground.Close = Close;
    }


    public void Open()
    {
        animator.SetTrigger("Open");
    }
    public void Close()
    {
        animator.SetTrigger("Close");
        image.SetActive(false);
    }    
    public void OpenEvent()
    {
        
        PopUpBackground.OpenEvent?.Invoke();
        image.SetActive(true);
    }
}


public struct PopUpBackground
{
    public static System.Action OpenEvent;

    public static System.Action Open;
    public static System.Action Close;
}
