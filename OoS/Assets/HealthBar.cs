using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playerDamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if(player == null)
        {
            Debug.Log(" Tidak ditemukan Tag Player pada Scene ini, pastikan Player menggunakan tag 'Player'");
        }
        playerDamageable = player.GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("Tidak ditemukan Tag Player pada Scene ini, pastikan Player menggunakan tag 'Player'");
        }
        else
        {
            playerDamageable = player.GetComponent<Damageable>();

            // Ambil nilai kesehatan dari PlayerPrefs jika tersedia
            int savedHealth = PlayerPrefs.GetInt("PlayerHealth", playerDamageable.MaxHealth);
            int savedMaxHealth = PlayerPrefs.GetInt("PlayerMaxHealth", playerDamageable.MaxHealth);

            // Atur nilai kesehatan pemain dengan nilai yang disimpan
            playerDamageable.Health = savedHealth;
            playerDamageable.MaxHealth = savedMaxHealth;

            UpdateHealthUI(savedHealth, savedMaxHealth);
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // Mengatur nilai slider pada health bar
        healthSlider.value = CalculateSliderPercentage(currentHealth, maxHealth);

        // Mengatur teks yang menampilkan nilai kesehatan saat ini dan nilai maksimum kesehatan
        healthBarText.text = " HP " + currentHealth + " / " + maxHealth;
    }

    private void OnEnable()
    {
        playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }
    private void OnDisable()
    {
        playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxhealth)
    {
        return currentHealth / maxhealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = " HP " + newHealth + " / " + maxHealth;
    }
}
