using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

public class ScrInventario : MonoBehaviour
{
    [SerializeField]
    ScrBota[] arrayInventory;

    public List<ScrBota> inventory { get; private set; }

    private void Awake()
    {
        inventory = new List<ScrBota>();
        inventory = arrayInventory.OrderBy(i => i.name).ToList();
    }
    public void Additem(ScrBota bota)
    {
        if (bota != null)
        {
            inventory.Add(bota);
        }
    }
    public void RemoveItem(ScrBota bota)
    {
        if (bota != null)
        {
            inventory.Remove(bota);
        }
    }
}
