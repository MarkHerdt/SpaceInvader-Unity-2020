using System.Collections;
using UnityEngine;

public class SpawnPowerUp : MonoBehaviour
{
    private Vector3 pos1 = new Vector3(-4, 6, 0);
    private Vector3 pos2 = new Vector3(4, 6, 0);
    private float speed = 1;

    public GameObject health; // Health prefab
    public GameObject shield; // Shield prefab
    public GameObject bullet; // Bullet prefab

    public AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(SpawnPowerUpTimer());
    }
    private void Update()
    {
        // Moves the object between the specified positions
        transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed, 1));
    }

    /// <summary>
    /// Has a chance to spawn a power up every new second
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnPowerUpTimer()
    {
        yield return new WaitForSeconds(Random.Range(2.5f, 5));
        while (true)
        {
            source.Play();
            if (Random.value <= 0.33f)
            {
                Instantiate(health, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(90, 270)));
            }
            else if (Random.value <= 0.66)
            {
                Instantiate(shield, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(90, 270)));
            }
            else if (Random.value <= 0.99)
            {
                Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(90, 270)));
            }
            yield return new WaitForSeconds(Random.Range(17.5f, 20));
        }
    }
}
