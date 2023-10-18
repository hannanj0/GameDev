using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
 


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.eulerAngles += dragSpeed * new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }
    }


}