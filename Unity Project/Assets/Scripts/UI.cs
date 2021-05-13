using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    GameController gameController;

    public static int globalScore = 0;

    public Player player;
    public Text life;
    public Text score;
    public Text time;

    public SpawnBullet_Player aoe;
    public SpawnBullet_Player barrage;
    GameObject aoeObject;
    Light aoeLight;
    Light[] barrageLight;
    Coroutine replenish;

    private void Awake()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        if (gameController != null)
        {
            // Calls the "OnGameStateChange" mehtod, when the game state is changed
            gameController.OnGameStateChange += OnGameStateChange;
        }
        // Subscribes to the "OnLifeChange" method of "Player"
        player.OnLifeChange += OnLifeChange;

        life = life.GetComponent<Text>();
        score = score.GetComponent<Text>();
        time = time.GetComponent<Text>();

        aoe.AoeActivated += AoeCooldown;
        aoeObject = GameObject.Find("Cooldown_AOE");
        aoeLight = aoeObject.GetComponentInChildren<Light>();
        barrage.BulletShot += BulletShot;
        barrage.BulletReplenished += BulletReplenished;
        barrageLight = GameObject.Find("Cooldown_Barrage").GetComponentsInChildren<Light>();
    }

    private void Start()
    {
        life.text = "" + player.Life;
    }

    private void Update()
    {
        GameController.minutes = (int)GameController.timer / 60;
        GameController.seconds = GameController.timer - (60 * GameController.minutes);

        // Substrtacts "TIme.time" with the last set "timestamp", to get the time for each round and not the total time that has passed
        GameController.timer = Time.time - GameController.timeStamp;
        score.text = "" + globalScore.ToString("#,##0").Replace(",", ".");
        if (GameController.timer >= 60)
        {
            time.text = "" + GameController.minutes + "m" + GameController.seconds.ToString("F2");
        }
        else
            time.text = "" + GameController.timer.ToString("F2");
    }

    private void OnGameStateChange(bool playing)
    {
        // When "GameController.playig" is "true", enable this gameobject
        if (playing)
        {
            gameObject.SetActive(true);
        }
        // When "GameController.playig" is "false", disable this gameobject
        else
            gameObject.SetActive(false);
    }

    private void OnLifeChange(int life)
    {
        // Display new "life" value when life has changed
        this.life.text = "" + life;
    }

    private void AoeCooldown()
    {
        StartCoroutine(CooldownAOE());
    }
    /// <summary>
    /// Displays when the cooldown is ready again
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownAOE()
    {
        float multiplier = 10;

        aoeLight.range = 0;
        aoeLight.intensity = .5f;
        for (int i = 0; i < aoe.AoeCooldown * multiplier; i++)
        {
            aoeLight.range += .2f / multiplier;
            yield return new WaitForSeconds(1 / multiplier);
        }
        for (int i = 0; i < 5; i++)
        {
            aoeLight.intensity += .1f;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void BulletShot()
    {
        //Debug.Log("Bullet Shot " + barrage.barrageAmount);
        if (replenish != null)
        {
            StopCoroutine(replenish);
        }
        for (int i = barrageLight.Length - 1; i >= 0; i--)
        {
            if (barrageLight[i].range != 0)
            {
                barrageLight[i].intensity = .5f;
                if (barrageLight[i].range >= .625f)
                {
                    barrageLight[i].range = .3125f;
                }
                else
                    barrageLight[i].range = 0;
                break;
            }
        }
    }
    private void BulletReplenished()
    {
        for (int i = 0; i <= barrageLight.Length - 1; i++)
        {
            if (barrageLight[i].intensity != 1)
            {
                replenish = StartCoroutine(Replenish(i));
                break;
            }
        }
        //Debug.Log("Bullet Replenished " + barrage.barrageAmount);
    }
    IEnumerator Replenish(int index)
    {
        for (int i = 0; i < 100; i++)
        {
            barrageLight[index].range += .003125f;
            yield return new WaitForSeconds(.01f);
            if (barrageLight[index].range >= .625f)
            {
                break;
            }
        }
        barrageLight[index].intensity = 1;
    }
}
