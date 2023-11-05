using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// The WeaponRotation script controls the rotation of hand-held weapons (currently a sword).
/// The weapon can rotate from its initial position to a target position and back.
/// </summary>
public class WeaponRotation : MonoBehaviour
{
    public PauseMenu pauseMenu;

    private bool isAttacking; // Check whether the player is attacking.
    public float rotationSpeed = 250.0f; // The speed the weapon will rotate by.
    public Vector3 targetRotation; // The weapon will rotate to this target rotation.

    private float currentRotation = 0f; // Used to ensure smoothness in rotations. Interpolation from initial rotation to final, vice versa.

    private Quaternion initialRotation; // An initial rotation for the weapon.
    private Quaternion finalRotation; // A final rotation for the weapon

    public bool IsAttacking() { return isAttacking; }

    /// <summary>
    /// Initialise variables. No attacking when the game behins. Set initial and target rotation to rotate to.
    /// </summary>
    private void Start()
    {
        isAttacking = false;
        initialRotation = transform.localRotation;
        finalRotation = Quaternion.Euler(targetRotation);
    }

    /// <summary>
    /// When the user attacks (and previously was not attacking), begin the attack and Coroutine rotation.
    /// </summary>
    public void BeginAttack()
    {
        if (!isAttacking && Time.timeScale == 1.0f && !pauseMenu.GameIsPaused())
        {
            isAttacking = true;
            StartCoroutine(RotateToTargetRotation());
        }
    }

    /// <summary>
    /// Coroutine to smoothly rotate weapon to a target rotation and back using a normalised time tracker and Slerp.
    /// </summary>
    private IEnumerator RotateToTargetRotation()
    {
        // Rotate to target location which is when normalised currentRotation reaches 1 (final rotation).
        currentRotation = 0f;
        float rotationAngle = Quaternion.Angle(transform.localRotation, finalRotation);

        while (currentRotation < 1)
        {
            currentRotation += Time.deltaTime * rotationSpeed / rotationAngle;
            transform.localRotation = Quaternion.Slerp(initialRotation, finalRotation, currentRotation);
            yield return null;
        }

        // Make sure the final rotation is exactly reached. Pause rotation for 0.1 seconds.
        transform.localRotation = finalRotation;
        yield return new WaitForSeconds(0.1f);

        // Rotate back to the initial rotation. Reset currentRotation to interpolate between start and end point again.
        currentRotation = 0f;

        while (currentRotation < 1)
        {
            currentRotation += Time.deltaTime * rotationSpeed / rotationAngle;
            transform.localRotation = Quaternion.Slerp(finalRotation, initialRotation, currentRotation);
            yield return null;
        }

        // Make sure the initial rotation is exactly reached. Stop attacking.
        transform.localRotation = initialRotation;
        isAttacking = false;
    }
}