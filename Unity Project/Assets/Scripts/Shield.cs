using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletMonster")
        {
            Destroy(collision.gameObject);
        }
    }
}
