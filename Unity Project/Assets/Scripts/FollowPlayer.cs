using UnityEngine;

// Makes the animation when the player receives a buff follow the player movement
public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    Player xPositionPlayer;

    private void OnEnable()
    {
        player = GameObject.Find("Player");
        xPositionPlayer = player.GetComponent<Player>();
    }

    private void Update()
    {
        if (player == null)
        {
            // Destroys the animation object, when the player dies
            Destroy(gameObject);
        }
        if (player != null)
        {
            transform.position = new Vector2(xPositionPlayer.transform.position.x, transform.position.y);
        }
    }
}
