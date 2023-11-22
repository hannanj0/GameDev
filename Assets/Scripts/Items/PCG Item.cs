using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGItem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Item;
    public int min_random;
    public int max_random;
    public float x_min;
    public float x_max;
    public float z_min;
    public float z_max;
    public float y;
    void Start()
    {
        GenerateObject();
    }

    public void GenerateObject()
    {
        int randomNumber = Random.Range(min_random, max_random + 1);
        for (int i = 0; i < randomNumber; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(x_min, x_max),
                y,
                Random.Range(z_min, z_max)
            );
            Instantiate(Item, randomPos, Quaternion.identity);
        }
    }
}
