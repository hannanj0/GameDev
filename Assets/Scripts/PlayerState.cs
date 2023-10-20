using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public static PlayerState Instance { get; set; }

    // Player Health
    public float currentHealth;
    public float maxHealth;


    // Player Hunger
    public float currentHunger;
    public float maxHunger;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    // awake looks and checks that it is the only instance in the game, if not, it will destroy it

    private void Awake() 
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
    }

    // Update is called once per frame
    void Update()
    {
        // each frame of distance travelled, it will increase by vector3 distance
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        // when distance travlled reaches 5, user will have travelled a certain distance in order to lose hunger
        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentHunger -= 1;
        }


        // if user presses N key, we will take -10 damage
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }
}
