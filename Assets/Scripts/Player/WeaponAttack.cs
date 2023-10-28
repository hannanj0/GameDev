using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponAttack : MonoBehaviour
{
    private bool damageDealt;

    public PlayerState playerState;
    public WeaponRotation weaponRotation;


    // Start is called before the first frame update
    void Start()
    {
        damageDealt = false;
    }

    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        GameObject swordPivot = GameObject.Find("SwordPivot");
        playerState = player.GetComponent<PlayerState>();
        weaponRotation = swordPivot.GetComponent<WeaponRotation>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("Weapon") && other.gameObject.CompareTag("Enemy") && weaponRotation.isAttacking && !damageDealt)
        {
            EnemyState enemy = other.gameObject.GetComponent<EnemyState>();
            enemy.health -= playerState.attackDamage;
            damageDealt = true;

            if (enemy.health <= 0)
            {
                if (enemy.isBoss)
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    SceneManager.LoadScene("WinGame");
                }
                other.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.gameObject.CompareTag("Weapon") && other.gameObject.CompareTag("Enemy") && damageDealt)
        {
            damageDealt = false;
        }
    }
}