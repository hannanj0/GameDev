using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    public Vector3 endRotation;
    public Vector3 startRotation;
    
    public float rotationSpeed = 120.0f;
    public Transform weaponObject;

    private Vector3 targetRotation;
    private bool isStartingSwing = false;
    private bool isFinishingSwing = false;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartingSwing)
        {
            StartWeaponSwing();
        }
        if (isFinishingSwing)
        {
            FinishWeaponSwing();
        }
    }

    void OnAttack()
    {
        isStartingSwing = true;
    }

    void StartWeaponSwing()
    {
        float step = rotationSpeed * Time.deltaTime;
        weaponObject.localRotation = Quaternion.RotateTowards(weaponObject.localRotation, Quaternion.Euler(endRotation), step);

        // Check if we've reached the end rotation
        if (weaponObject.localRotation.eulerAngles == endRotation)
        {
            isStartingSwing=false;
            isFinishingSwing = true;
        }
    }

    void FinishWeaponSwing()
    {
        float step = rotationSpeed * Time.deltaTime;
        weaponObject.localRotation = Quaternion.RotateTowards(weaponObject.localRotation, Quaternion.Euler(startRotation), step);

        // Check if we've reached the end rotation
        if (weaponObject.localRotation.eulerAngles == startRotation)
        {
            isFinishingSwing = false;
        }
    }
}