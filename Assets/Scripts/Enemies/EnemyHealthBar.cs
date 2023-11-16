using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 barOffset; // The offset of the health bar
    [SerializeField] private float visibilityDistance = 10f; // Sets the distance for the health bar to be visible

    private void Start()
    {
        SetHealthBarVisibility(false);
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (slider != null)
        {
            slider.value = currentValue / maxValue;
            SetHealthBarVisibility(true);
        }
    }

    void Update()
    {
        if (camera != null && target != null)
        {
            float distanceToPlayer = Vector3.Distance(target.position, camera.transform.position);

            if (distanceToPlayer <= visibilityDistance)
            {
                transform.rotation = camera.transform.rotation;
                transform.position = target.position + barOffset;
            }
            else
            {
                SetHealthBarVisibility(false);
            }
        }
    }

    private void SetHealthBarVisibility(bool visible)
    {
        if (slider != null)
        {
            slider.gameObject.SetActive(visible);
        }
    }
}
