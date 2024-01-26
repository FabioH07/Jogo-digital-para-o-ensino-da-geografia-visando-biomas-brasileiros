using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
public class ScrMouseSensitive : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event System.Action<string, string, float, float, float> MouseON;
    public static event System.Action MouseOFF;
    private ScrBotaReference reference;
    void Start()
    {
        reference = GetComponent<ScrBotaReference>();
        ChangeColor(Color.gray);
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeColor(Color.white);
        MouseON?.Invoke(reference._bota.name, reference._bota.description, reference._bota.speed, reference._bota.resistence,reference._bota.jump);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeColor(Color.gray);
        MouseOFF?.Invoke();
    }
    private void ChangeColor(Color _color) =>
        reference.icon.color = _color;
}
