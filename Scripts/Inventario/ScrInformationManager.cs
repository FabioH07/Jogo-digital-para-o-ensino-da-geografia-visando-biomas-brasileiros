using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrInformationManager : MonoBehaviour
{
    public GameObject descriptionArea;
    public Text nameText;
    public Text descriptionText;
    public Text speedText;
    public Text jumpText;
    public Text resistenceText;
    void Start()
    {
        ScrMouseSensitive.MouseON += ShowInformations;
        ScrMouseSensitive.MouseOFF += ResetInformations;
    }
    private void OnDestroy()
    {
        ScrMouseSensitive.MouseON -= ShowInformations;
        ScrMouseSensitive.MouseOFF -= ResetInformations;
    }
    public void ShowInformations(string name,string description,float speed,float resistence, float jump)
    {
        descriptionArea.SetActive(true);
        nameText.text = name;
        descriptionText.text = description;
        speedText.text = speed.ToString();
        jumpText.text = jump.ToString();
        resistenceText.text = resistence.ToString();
    }
    private void ResetInformations()
    {
        descriptionArea.SetActive(false);
        nameText.text = string.Empty ;
        descriptionText.text = string.Empty;
    }
}
