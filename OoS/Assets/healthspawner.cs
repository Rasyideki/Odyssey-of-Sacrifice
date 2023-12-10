using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthspawner : MonoBehaviour
{
    public GameObject petir;
    public float spawnTime;
    public float Xposmin, Xposmax; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPipeCoroutine());
    }

    IEnumerator SpawnPipeCoroutine()
    {
        yield return new WaitForSeconds(spawnTime);    
        Instantiate(petir, transform.position + Vector3.right * Random.Range(Xposmin, Xposmax), Quaternion.identity);
        StartCoroutine(SpawnPipeCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
