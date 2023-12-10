using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadeout : MonoBehaviour
{
    FadeinOut fade;
    void Start()
    {
        fade = FindAnyObjectByType<FadeinOut>();

        fade.FadeOut();
    }
}
