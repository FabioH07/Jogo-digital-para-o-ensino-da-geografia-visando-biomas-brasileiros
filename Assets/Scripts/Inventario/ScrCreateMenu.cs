using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrCreateMenu : MonoBehaviour
{
    public ScrBotaReference _element;
    private List<ScrBota> _inventory;
    void Start()
    {
        _inventory = new List<ScrBota>();
        _inventory = FindObjectOfType<ScrInventario>().inventory;
        InstantiateElements();
    }
    private void InstantiateElements()
    {
        for(int i = 0; i < _inventory.Count; i++)
        {
            if (IsReapeated(i)) continue;
             

            (Instantiate(_element, transform) as ScrBotaReference).SetValues(
                _inventory[i]);
        }
    }
    bool IsReapeated(int i)
    {
        if(i==0) return false;
         
        return _inventory[i].ID == _inventory[i - 1].ID;
    }
}
