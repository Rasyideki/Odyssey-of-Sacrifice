using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petirspawner : MonoBehaviour
{
    public GameObject petirPrefab; // Prefab petir yang akan di-instantiate
    public float spawnTime;
    public float Xposmin, Xposmax;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPetirCoroutine());
    }

    IEnumerator SpawnPetirCoroutine()
    {
        yield return new WaitForSeconds(spawnTime);

        // Buat objek petir baru
        GameObject newPetir = Instantiate(petirPrefab, transform.position + Vector3.right * Random.Range(Xposmin, Xposmax), Quaternion.identity);

        // Dapatkan komponen AudioSource dari objek petir yang baru di-spawn
        AudioSource petirAudioSource = newPetir.GetComponentInChildren<AudioSource>();

        // Mainkan suara dari objek petir yang baru di-spawn
        if (petirAudioSource != null)
        {
            petirAudioSource.Play();
        }

        // Tetapkan pemutar suara petir yang di-spawn agar tidak di-destroy
        DontDestroyOnLoad(petirAudioSource.gameObject);

        StartCoroutine(DestroyPetirAfterDelay(newPetir, 2f));
        StartCoroutine(SpawnPetirCoroutine());
    }

    IEnumerator DestroyPetirAfterDelay(GameObject objToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hancurkan hanya komponen visual petir
        Destroy(objToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        // Tidak ada yang perlu di-update pada setiap frame dalam contoh ini
    }
}
