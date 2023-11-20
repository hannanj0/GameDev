using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotation : MonoBehaviour
{
    public float rotationSpeed;

    void Start()
    {
        rotationSpeed = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
