using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    public static Chest singleton;

    public GameObject keyObject;
    public GameObject fragObject;
    public GameObject interactIcon;
    public BoolVa _value;
    public intValue jmlh;
    public Item itemku;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    AudioSource openSource;

    public bool isOpen;

    public bool PlayerinRange;
   
    public bool isFragmentChest;
    public bool isKeyChest;


    Animator animator;

    private GoalManager goalManager;


    private void Awake()
    {
        singleton = this;
        openSource = GetComponent<AudioSource>();
    }
    private void Start()
    {

        isOpen = _value.val;
        animator = GetComponent<Animator>();    
        if(isOpen ) {
            animator.SetBool("Open", true);            
        }

        goalManager = GoalManager.singleton;
     
    }

    private void Update()
    {
        if (PlayerinRange && Input.GetButtonDown("Interaksi")) 
        {
            if(!isOpen)
            {
                if (openSource)
                    AudioSource.PlayClipAtPoint(openSource.clip, gameObject.transform.position, openSource.volume);
                Interact();
            }
            else
            {
                PlayerinRange = false;
            }
        }
    }

    public void Interact()
    {
        dialogBox.SetActive(true);
        dialogText.text = itemku.itemDesc;
        StartCoroutine(chatbx());
        

            isOpen = true;
            animator.SetBool("Open", true);
            _value.val = isOpen;
        if(isFragmentChest)
        {
            StartCoroutine(fragment());// Aktifkan objek kunci
            jmlh.Fragvalue++;
        }
        else if(isKeyChest)
        {
            StartCoroutine(kunci());
            keyObject.SetActive(true); // Aktifkan objek kunci
            jmlh.Keyvalue++;
        }
        
    }

    
    IEnumerator chatbx()
    {
        dialogBox.SetActive(true);
        yield return new WaitForSeconds(2f);
        dialogBox.SetActive(false);
    }
    IEnumerator fragment()
    {
        fragObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        fragObject.SetActive(false);
    }
    IEnumerator kunci()
    {
        keyObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        keyObject.SetActive(false);
    }

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
            if (!isOpen) 
            {
                interactIcon.SetActive(true) ;
            } 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {        
            interactIcon.SetActive(false);
        
    }
}