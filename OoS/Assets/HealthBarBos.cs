using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBos : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable bosDamageable;

    private void Awake()
    {
        GameObject Boss_DarkKnight = GameObject.FindGameObjectWithTag("Bos");

        if (Boss_DarkKnight == null)
        {
            Debug.Log(" Tidak ditemukan Tag Bos pada Scene ini!");
        }
        bosDamageable = Boss_DarkKnight.GetComponent<Damageable>();
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
        bosDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }
    private void OnDisable()
    {
        bosDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
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

