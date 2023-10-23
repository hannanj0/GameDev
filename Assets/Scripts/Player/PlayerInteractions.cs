using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    GameObject player;
    public MobEnemyState mobEnemyScript;
    public PlayerState playerState;

    private float enemyCollisionCooldown = 0.0f;
    private float enemyCollisionCooldownDuration = 2.0f;
    private bool offCooldown;

    // Start is called before the first frame update
    void Start()
    {
        offCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!offCooldown)
        {
            enemyCollisionCooldown += Time.deltaTime;
            if (enemyCollisionCooldown >= enemyCollisionCooldownDuration)
            {
                offCooldown = true;
                enemyCollisionCooldown = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MobEnemy") && offCooldown) {
            Debug.Log("ok");
            playerState.currentHealth -= mobEnemyScript.mobAttackPower;
            offCooldown = false;
            enemyCollisionCooldown = 0.0f;
            //if (collision.gameObject.TryGetComponent<OtherScript>(out OtherScript otherScript))
            //{
            //    // Access the "score" variable from the "OtherScript"
            //    int otherScriptScore = otherScript.score;
            //    Debug.Log("The score from the other script is: " + otherScriptScore);
            //}
        }
    }
}
