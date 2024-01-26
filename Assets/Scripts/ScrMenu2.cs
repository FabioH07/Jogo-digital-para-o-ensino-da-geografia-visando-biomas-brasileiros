using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrMenu2 : MonoBehaviour
{
    public GameObject pagina1;
    public GameObject pagina2;
    int pagina;
    void Start()
    {
        pagina1.SetActive(true);
    }
    public void paginaPlus()
    {
        pagina1.SetActive(false);
        pagina2.SetActive(true);

    }
    public void paginaMinus()
    {
        pagina1.SetActive(true);
        pagina2.SetActive(false);
    }
}
