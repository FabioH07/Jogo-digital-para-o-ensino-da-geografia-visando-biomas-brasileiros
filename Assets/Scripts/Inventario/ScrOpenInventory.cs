using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrOpenInventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    public static bool isOpen = false;
    public void OpenInventory()
    {
        if(ScrDialogueManager.isActive == false)
        {
            if (!isOpen)
            {
                inventoryPanel.SetActive(true);
                isOpen = true;
            }
            else
            {
                inventoryPanel.SetActive(false);
                isOpen = false;
            }
        }
    }


    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        isOpen = false;
    }
    
}
