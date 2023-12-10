using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Button playButton; // Referensi ke tombol Play Game diinspektor Unity
    public Button quitButton; // Referensi ke tombol Quit diinspektor Unity
    public Button settingButton; // Referensi ke tombol Setting diinspektor Unity
    FadeinOut fade;

    private Dictionary<Button, Vector3> originalScales = new Dictionary<Button, Vector3>();
    public float hoverScale = 1.2f; // Skala tambahan saat kursor di atas tombol

    void Start()
    {
        // Simpan skala asli dari tombol-tombol
        StoreOriginalScale(playButton);
        StoreOriginalScale(quitButton);
        StoreOriginalScale(settingButton);

        // Tambahkan event listener untuk menangani hover pada tombol-tombol
        AddHoverEffect(playButton);
        AddHoverEffect(quitButton);
        AddHoverEffect(settingButton);

        fade = FindObjectOfType<FadeinOut>();
    }

    public IEnumerator ChangeScene()
    {
        fade.Fadein();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void StoreOriginalScale(Button button)
    {
        if (button != null)
        {
            originalScales[button] = button.transform.localScale;
        }
    }

    void AddHoverEffect(Button button)
    {
        if (button != null)
        {
            button.gameObject.AddComponent<EventTrigger>();
            EventTrigger trigger = button.GetComponent<EventTrigger>();

            // Event saat kursor memasuki tombol
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnPointerEnter(button); });
            trigger.triggers.Add(entryEnter);

            // Event saat kursor keluar dari tombol
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnPointerExit(button); });
            trigger.triggers.Add(entryExit);
        }
    }

    public void PlayGame()
    {
        StartCoroutine(ChangeScene());
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnPointerEnter(Button button)
    {
        if (button != null)
        {
            // Mengubah skala tombol saat kursor masuk
            button.transform.localScale = originalScales[button] * hoverScale;
        }
    }

    public void OnPointerExit(Button button)
    {
        if (button != null)
        {
            // Mengembalikan skala tombol saat kursor keluar
            button.transform.localScale = originalScales[button];
        }
    }
}
