using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager_Main : MonoBehaviour
{
    public Button start; // Reference for start button
    public Button exit; // Reference for exit button

    public GameObject cursor;
    public GameObject logo;
    public GameObject uiMain;
    public GameObject uiControls;
    Text controls;

    public Light aoeCD;
    public GameObject cdBarrage;
    Light[] barrageCD;

    public AudioSource source;

    private void Awake()
    {
        controls = uiControls.GetComponentInChildren<Text>();
        controls.text = "Menu: <b><color=#007A8E>ESC</color></b> \n " +
                        "Left: <b><color=#007A8E>A</color></b> \n" +
                        "Right: <b><color=#007A8E>D</color></b> \n" +
                        "Shoot: <b><color=#007A8E>Space</color></b> \n" +
                        "Charged Shot: \n" +
                        "<b><color=#007A8E>Hold Space</color></b> \n" +
                        "AOE: <b><color=#007A8E>2xSpace</color></b> \n" +
                        "Bullet-Barrage: <b><color=#007A8E>S</color></b> \n" +
                        "Buy-Health: <b><color=#007A8E>Enter</color></b> \n" +
                        "<i>(250 Coins)</i>";
        barrageCD = cdBarrage.GetComponentsInChildren<Light>();
    }

    private void Start()
    {
        Cursor.visible = false;
        InvokeRepeating("AOECooldown", 0, 12);
        InvokeRepeating("BulletReplenished", 0, 13);
    }

    private void OnEnable()
    {
        // When the respective button is clicked, call its method
        start.onClick.AddListener(() => buttonStart());
        exit.onClick.AddListener(() => buttonExit());
    }

    private void Update()
    {
        // Opens/closes the controls-menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            source.Play();
            if (uiControls.activeSelf == false)
            {
                cursor.SetActive(false);
                logo.SetActive(false);
                uiMain.SetActive(false);
                uiControls.SetActive(true);
            }
            else
            {
                cursor.SetActive(true);
                logo.SetActive(true);
                uiMain.SetActive(true);
                uiControls.SetActive(false);
            }
        }
    }

    // Method for the bottons
    private void buttonStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        UI.globalScore = 0;
        // Saves the current time, when the "start" button is clicked
        GameController.timeStamp = Time.time;
        GameController.timer = 0;
    }
    private void buttonExit()
    {
        Debug.Log("sdfsdf");
        Application.Quit();
    }
    //-----------------------

    void AOECooldown()
    {
        StartCoroutine(CooldownAOE());
    }
    IEnumerator CooldownAOE()
    {
        float multiplier = 10;

        aoeCD.range = 0;
        aoeCD.intensity = .5f;
        for (int i = 0; i < 100; i++)
        {
            aoeCD.range += .2f / multiplier;
            yield return new WaitForSeconds(1 / multiplier);
        }
        for (int i = 0; i < 5; i++)
        {
            aoeCD.intensity += .1f;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void BulletReplenished()
    {
        for (int i = 0; i < barrageCD.Length; i++)
        {
            barrageCD[i].range = 0;
            barrageCD[i].intensity = .5f;
        }
        StartCoroutine(Replenish());
    }
    IEnumerator Replenish()
    {
        for (int i = 0; i < barrageCD.Length; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                barrageCD[i].range += .00625f;
                yield return new WaitForSeconds(.02f);
                if (barrageCD[i].range >= .625f)
                {
                    break;
                }
            }
            barrageCD[i].intensity = 1;
        }
    }


}

