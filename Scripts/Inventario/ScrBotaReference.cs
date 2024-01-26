using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrBotaReference : MonoBehaviour
{
    public Image icon;
    public Text CountText;
    public ScrBota _bota { get;private set; }
    public void SetValues(ScrBota bota)
    {
        _bota = bota;
        icon.sprite = bota.artWork;
        UpdateCount();
    }

    private void UpdateCount()
    {
        CountText.text = "x" + _bota.count.ToString();
    }
}
