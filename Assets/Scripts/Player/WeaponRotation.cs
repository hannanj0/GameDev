using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;  // Import the new input system

public class WeaponRotation : MonoBehaviour
{
    public Vector3 targetRotation;
    public float rotationSpeed = 250.0f;
    public bool isAttacking;

    private Quaternion initialRotation;
    private Quaternion finalRotation;
    private float lerpTime = 0f;

    private PlayerControls controls;  // Declare controls

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        isAttacking = false;
        initialRotation = transform.localRotation;
        finalRotation = Quaternion.Euler(targetRotation);
    }

    private void OnAttack()  // OnAttack method
    {
        if (!isAttacking)
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
        //isAttacking = false;
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
