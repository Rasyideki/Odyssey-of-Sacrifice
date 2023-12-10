using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class interactInfo : MonoBehaviour
{
    public static interactInfo singleton;

    public Item itemku;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    public GameObject interactIcon;

    public bool PlayerinRange;
    // Start is called before the first frame update
    private void Awake()
    {
        singleton = this;
    }

    private void Update()
    {
        if (PlayerinRange && Input.GetButtonDown("Interaksi"))
        {
            Interact();
        }
        else if(!PlayerinRange)
        {
            unInteract();
        }
            
    }
    public void Interact()
    {
        dialogBox.SetActive(true);
        dialogText.text = itemku.itemDesc;
    }
    public void unInteract()
    {
        dialogBox.SetActive(false);
        dialogText.text = itemku.itemDesc;
    }
    public void ShowInteractIcon()
    {
        if (interactIcon)
        {
            interactIcon.SetActive(true); // Tampilkan ikon E klo pemain berada dekat chest
        }
    }

    public void HideInteractIcon()
    {
        if (interactIcon != null)
        {
            interactIcon.SetActive(false); // Sembunyikan ikon E klo pemain tidak lagi di dekat chest
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
        PlayerinRange = false;
    }
}
