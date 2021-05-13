using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cursor;

    public static float timer;
    public static float timeStamp;
    public static int minutes;
    public static float seconds;
    public static string name1;
    public static int score1;
    public static string name2;
    public static int score2;
    public static string name3;
    public static int score3;

    public Player player; // Reference of player

    bool playing = true; // State of the game
    public bool Playing { get { return playing; } }

    public delegate void gameStateChange(bool playing);
    public event gameStateChange OnGameStateChange;

    public SceneManager_Death sceneManager;

    public AudioSource source;
    public AudioClip playerDeath;
    public AudioClip openMenu;

    public GameObject particle;

    private void Awake()
    {
        name1 = PlayerPrefs.GetString("Name1st");
        score1 = PlayerPrefs.GetInt("Score1st");
        name2 = PlayerPrefs.GetString("Name2nd");
        score2 = PlayerPrefs.GetInt("Score2nd");
        name3 = PlayerPrefs.GetString("Name3rd");
        score3 = PlayerPrefs.GetInt("Score3rd");

        sceneManager = sceneManager.GetComponent<SceneManager_Death>();

        // Subscribes to the "OnLifeChange" Method of "Player"
        player.OnLifeChange += OnLifeChange;

        //PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        Cursor.visible = false;
        cursor.SetActive(false);
        // Broadcast "playing" to everyone that's subscribed on start of game
        OnGameStateChange?.Invoke(playing);
    }

    private void Update()
    {
        if (sceneManager.gameObject.activeSelf == false)
        {
            cursor.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && playing == true)
        {
            source.clip = openMenu;
            source.Play();
            sceneManager.MainMenu();
        }
    }

    private void OnLifeChange(int life)
    {
        // If the player is dead, set the game state to false
        if (life <= 0)
        {
            Instantiate(particle, player.transform.position, Quaternion.identity);
            source.clip = playerDeath;
            source.Play();
            Destroy(player.gameObject);
            timeStamp = Time.time;
            playing = false;
            // Broadcast "playing" to everyone that's subscribed after the player died
            OnGameStateChange?.Invoke(playing);
        }
    }

    /// <summary>
    /// Plays an audioclip, for objects that get destroyed before they can play theirs
    /// </summary>
    /// <param name="clipName"></param>
    public void PlaySound(AudioClip clipName)
    {
        AudioSource.PlayClipAtPoint(clipName, transform.position, 1);
    }
}
