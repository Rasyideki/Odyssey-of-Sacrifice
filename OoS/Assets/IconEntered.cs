using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class IconEntered : MonoBehaviour
{


    public GameObject interactIcon;
    public bool PlayerinRange;
    public void ShowInteractIcon()
    {
        if (interactIcon)
        {
            interactIcon.SetActive(true); // Tampilkan ikon E saat pemain berada di dekat peti
        }
    }

    public void HideInteractIcon()
    {
        if (interactIcon != null)
        {
            interactIcon.SetActive(false); // Sembunyikan ikon E saat pemain tidak lagi berada di dekat peti
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerinRange = true;
            
                interactIcon.SetActive(true);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactIcon.SetActive(false);

    }
}
