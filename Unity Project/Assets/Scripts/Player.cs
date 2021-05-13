using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D player;
    SpriteRenderer spriteRenderer;
    Vector2 moveDirection;
    private float hInput;
    float speed = 5; // Speed the player moves with

    public AudioSource source;
    public AudioClip hit;
    public AudioClip buy;
    public AudioClip error;

    public GameObject bloodParticle;
    public GameObject uiParticle;

    public int price = 250;
    public Transform ui; // UI reference
    public Text scoreText; // Reference for score position
    public Text lifeText; // Reference for life position
    public Text textObj; // Text prefab
    Text textObjScore;
    Text textObjLifeUp;
    Text textObjLifeDown;
    bool shakeScore = false;
    public Transform scoreBubble;
    public Transform lifeBubble;

    // Life of player
    public int life = 3; 
    public int Life
    {
        get
        {
            return life;
        }
    }
    public event System.Action<int> OnLifeChange;

    private bool isHit = false; // Member for coroutine
    bool shield = false;
    public bool Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
        }
    }

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        scoreText = scoreText.GetComponent<Text>();
        lifeText = lifeText.GetComponent<Text>();
        ui = ui.GetComponent<Transform>();
        scoreBubble = scoreBubble.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        // Returns a value between "-1" - "1", depending on what button is pressed and saves it in "hInput"
        hInput = Input.GetAxisRaw("Horizontal");
        // Adds or substracts the current coordinats on the respective axis, depending on the value of "hInput" and saves them in "moveDirection"
        moveDirection = new Vector2(hInput, 0);
        // Moves the player to the coordinats saved in "moveDirection" with the specified speed, ".normalized" to set the vector to 1 
        player.velocity = moveDirection.normalized * speed;

        // If value of "hInput" is == 0, play "IsMovingIdle" animation
        GetComponent<Animator>().SetBool("IsIdle", (hInput == 0));
        // If value of "hInput" is < 0, play "IsMovingLeft" animation
        GetComponent<Animator>().SetBool("IsMovingLeft", (hInput < 0));
        // If value of "hInput" is > 0, play "IsMovingRight" animation
        GetComponent<Animator>().SetBool("IsMovingRight", (hInput > 0));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            BuyHealth();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletMonster" || collision.gameObject.tag == "Monster1" || collision.gameObject.tag == "Monster2")
        {
            // Player won't take damage for 0.75 seconds after being hit
            if (isHit == false && shield == false)
            {
                if (life - 1 != 0)
                {
                    Instantiate(bloodParticle, transform.position, Quaternion.identity);
                }
                source.clip = hit;
                source.Play();
                dmgTaken();
                StartCoroutine(BlinkAnimation());
            }
            // When player has 0 life, destroy player gameobject
            if (life <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Method to substract 1 life point from the player
    /// </summary>
    public void dmgTaken()
    {
        textObjLifeDown = textObj.GetComponent<Text>();
        textObjLifeDown.text = "<color=red>-1</color>";
        textObjLifeDown = Instantiate(textObj, lifeText.transform.position, Quaternion.identity);
        textObjLifeDown.transform.SetParent(ui);
        textObjLifeDown = null;

        life--;
        OnLifeChange?.Invoke(life);
    }

    /// <summary>
    /// Disables/enables the sprite renderer in an intervall, when the player is hit
    /// </summary>
    /// <returns></returns>
    IEnumerator BlinkAnimation()
    {
        for (float i = 0; i < 1.5f; i += .5f)
        {
            isHit = true;
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.125f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.125f);
        }
        isHit = false;
    }

    /// <summary>
    /// Spend score points to buy one health point
    /// </summary>
    private void BuyHealth()
    {
        if (UI.globalScore >= price)
        {
            source.clip = buy;
            source.Play();

            Instantiate(uiParticle, scoreBubble.transform.position, Quaternion.identity);

            UI.globalScore -= price;

            textObjScore = textObj.GetComponent<Text>();
            textObjScore.text = "<color=red>-250</color>";
            textObjScore = Instantiate(textObj, scoreText.transform.position, Quaternion.identity);
            textObjScore.transform.SetParent(ui);
            textObjScore = null;

            AddLife();
        }
        else
        {
            source.clip = error;
            source.Play();
            StartCoroutine(ShakeScore());
        }
    }

    /// <summary>
    /// Method to add one life point to the player
    /// </summary>
    public void AddLife()
    {
        Instantiate(uiParticle, lifeBubble.transform.position, Quaternion.identity);

        textObjLifeUp = textObj.GetComponent<Text>();
        textObjLifeUp.text = "<color=green>+1</color>";
        textObjLifeUp  = Instantiate(textObj, lifeText.transform.position, Quaternion.identity);
        textObjLifeUp.transform.SetParent(ui);
        textObjLifeUp = null;

        life++;
        OnLifeChange?.Invoke(life);
    }

    public void InvokeLife()
    {
        OnLifeChange?.Invoke(life);
    }

    IEnumerator ShakeScore()
    {
        if (shakeScore == false)
        {
            shakeScore = true;
            for (int i = 0; i < 10; i++)
            {
                scoreText.transform.position = new Vector2(scoreText.transform.position.x + .05f, scoreText.transform.position.y);
                yield return new WaitForSeconds(.01f);
                scoreText.transform.position = new Vector2(scoreText.transform.position.x - .05f, scoreText.transform.position.y);
                yield return new WaitForSeconds(.01f);
            }
            shakeScore = false;
        }
    }
}
