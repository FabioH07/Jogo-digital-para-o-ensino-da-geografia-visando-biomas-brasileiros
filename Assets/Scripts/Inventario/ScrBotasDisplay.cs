using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrBotasDisplay : MonoBehaviour
{
    public ScrBota bota;
    public Text nameText;
    public Text descriptionText;

    public Image artworkImage;
    public Text speed;
    public Text resistence;
    public Text jump;
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = bota.name;
        descriptionText.text = bota.description;
        artworkImage.sprite = bota.artWork;
        speed.text = bota.speed.ToString();
        resistence.text = bota.resistence.ToString();
        jump.text = bota.jump.ToString();  


    }
    
}
