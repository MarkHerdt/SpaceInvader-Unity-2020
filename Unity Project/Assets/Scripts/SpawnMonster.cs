using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    public GameObject monster1Prefab;
    public GameObject monsteb2Prefab;

    float monster1Chance = 0.625f; // 62.5% Chance to spawn "Monster1"
    float monster2Chance = 0.85f; // 22.5% Chance to spawn "Monster2"
    float intervall = 3.75f; // Intervall the monster spawn with

    int i = 1; // Multiplier for time

    void Start()
    {
        InvokeRepeating("InstantiateMonster", 0, intervall);
    }

    void InstantiateMonster()
    {
        float spawnMonster = Random.value;

        if (spawnMonster <= monster1Chance)
        {
            Instantiate(monster1Prefab, transform.position, Quaternion.identity);
        }
        else if (spawnMonster <= monster2Chance)
        {
            Instantiate(monsteb2Prefab, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        // Reduces the spawn intervall by 0.5f for every minute that passes
        if (Time.time >= 60 * i && i < 10)
        {
            i++;
            if (intervall > 2.5f)
            {
                // Decreases intervall, teh monster spawn with
                intervall -= 0.25f;
            }
            // Increases chance to spawn a monster
            monster1Chance += 0.01f; // +10% after 10min
            monster2Chance += 0.01f; // +10% after 10min
            // Cancels the running "InstantiateMonster" invoke
            CancelInvoke("InstantiateMonster");
            // Invokes "InstantiateMonster" with the new "intervall"
            InvokeRepeating("InstantiateMonster", intervall, intervall);
        }
    }
}
