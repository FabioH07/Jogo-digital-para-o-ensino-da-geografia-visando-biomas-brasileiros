using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Nova Bota",menuName = "scriptableObjects/Bota",order =2)]
public class ScrBota : ScriptableObject
{
    public int ID { get;private set; }  
    public new string name;
    public string description;
    public float speed;
    public float jump;
    public float resistence;

    public Sprite artWork;
    public int count { get {
            return
                FindObjectOfType<ScrInventario>().inventory.FindAll(
                    x => x.ID == this.ID).Count;

        }
    }

    private void OnEnable() =>
        ID = this.GetInstanceID();

}
