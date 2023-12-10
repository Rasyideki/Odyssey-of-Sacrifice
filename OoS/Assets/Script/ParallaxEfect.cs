using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEfect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // posisi awal untuk parallax game objek
    Vector2 startingPosition;

    //memulai value Z dari parallax game objek
    float startingZ;

    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zdistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => (cam.transform.position.z + (zdistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(zdistanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //ketika target bergerak, gerak parallax objek sama dengan waktu
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        //posisi x.y berubah berdasarkan target bergerak
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);

    }
}
