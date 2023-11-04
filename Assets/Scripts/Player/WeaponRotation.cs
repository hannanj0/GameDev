using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// The WeaponRotation script controls the rotation of hand-held weapons (currently a sword).
/// The weapon can rotate from its initial position to a target position and back.
/// </summary>
public class WeaponRotation : MonoBehaviour
{
    public Vector3 targetRotation; // The weapon will rotate to this target rotation.
    public float rotationSpeed = 250.0f; // The speed the weapon will rotate by.
    public bool isAttacking; // Check whether the player is attacking.

    private Quaternion initialRotation; // An initial rotation for the weapon.
    private Quaternion finalRotation; // A final rotation for the weapon
    private float normalisedAttackingTime = 0f; // Used to ensure smoothness in rotations. Interpolation time.

    private void Start()
    {
        isAttacking = false;
        initialRotation = transform.localRotation;
        finalRotation = Quaternion.Euler(targetRotation);
    }

    public void BeginAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(RotateToTargetRotation());
        }
    }

    /// <summary>
    /// Coroutine to smoothly rotate weapon to a target rotation and back.
    /// </summary>
    private IEnumerator RotateToTargetRotation()
    {
        float rotationDuration = Quaternion.Angle(transform.localRotation, finalRotation);
        normalisedAttackingTime = 0f;

        while (normalisedAttackingTime < 1)
        {
            normalisedAttackingTime += Time.deltaTime * rotationSpeed / rotationDuration;
            transform.localRotation = Quaternion.Slerp(initialRotation, finalRotation, normalisedAttackingTime);
            yield return null;
        }

        // Make sure the final rotation is exactly reached, pause for 0.1 seconds.
        transform.localRotation = finalRotation;
        yield return new WaitForSeconds(0.1f);

        // Rotate back to the initial rotation
        normalisedAttackingTime = 0f;

        while (normalisedAttackingTime < 1)
        {
            normalisedAttackingTime += Time.deltaTime * rotationSpeed / rotationDuration;
            transform.localRotation = Quaternion.Slerp(finalRotation, initialRotation, normalisedAttackingTime);
            yield return null;
        }

        // Make sure the initial rotation is exactly reached, stop attacking.
        transform.localRotation = initialRotation;
        isAttacking = false;
    }
}
