using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public List<string> bossesKilled = new List<string>();

    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public float currentHunger = 100f;
    public float maxHunger = 100f;
    public float attackDamage = 40f;

    public float spawnPositionX = 300f;
    public float spawnPositionY = 4f;
    public float spawnPositionZ = 315f;

}
