using UnityEngine;

public class SpawnBullet_Monster : MonoBehaviour
{
    public GameObject bullet;
    GameObject player;

    public AudioSource source;
    public AudioClip bulletShot;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        if (player != null && bullet != null)
        {
            source.clip = bulletShot;
            // Spawns a bullet every 2.5-5sec, after 0.5sec when the monster has spawned 
            InvokeRepeating("InstantiateBullet", .5f, Random.Range(2.5f, 5));
        }
    }

    /// <summary>
    /// Spawns the monster bullets
    /// </summary>
    public void InstantiateBullet()
    {
        if (gameObject != null)
        {
            source.Play();
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}
