using UnityEngine;
using System.Collections;

public class WeaponRotation : MonoBehaviour
{
    public Vector3 targetRotation;
    public float rotationSpeed = 250.0f; // Adjust the rotation speed as needed
    public bool isAttacking;

    private Quaternion initialRotation;
    private Quaternion finalRotation;
    private float lerpTime = 0f;

    private void Start()
    {
        isAttacking = false;
        initialRotation = transform.localRotation;
        finalRotation = Quaternion.Euler(targetRotation);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(RotateToTargetRotation());
        }
    }

    private IEnumerator RotateToTargetRotation()
    {
        float journeyLength = Quaternion.Angle(transform.localRotation, finalRotation);
        lerpTime = 0f;

        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * rotationSpeed / journeyLength;
            transform.localRotation = Quaternion.Slerp(initialRotation, finalRotation, lerpTime);
            yield return null;
        }

        // Ensure we reach the exact target rotation
        transform.localRotation = finalRotation;

        // Wait for a brief moment (you can adjust this duration)
        yield return new WaitForSeconds(0.1f);

        // Rotate back to the initial rotation
        lerpTime = 0f;

        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * rotationSpeed / journeyLength;
            transform.localRotation = Quaternion.Slerp(finalRotation, initialRotation, lerpTime);
            yield return null;
        }

        // Ensure we reach the exact initial rotation
        transform.localRotation = initialRotation;
        isAttacking = false;
    }
}
