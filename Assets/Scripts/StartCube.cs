using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(Random.Range(0.0f, 100.0f) % 1.0f, 1.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
