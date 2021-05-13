using UnityEngine;
using UnityEngine.UI;

public class SceneManager_Death : MonoBehaviour
{
    public GameObject cursor;

    GameController gamecontroller;

    public Button start; // Reference for start button
    public Button exit; // Reference for exit button
    public InputField input;

    GameObject logo;
    GameObject gameOver;
    public Text controls;
    public Text stats;
    public Text first;
    public Text second;
    public Text third;

    private void Awake()
    {
        gamecontroller = GameObject.FindObjectOfType<GameController>();
        if (gamecontroller != null)
        {
            // Subscribes to the "GameControllers" "OnGameStateChange" method
            gamecontroller.OnGameStateChange += OnGameStateChange;
        }

        logo = GameObject.Find("GameOverLogo");
        gameOver = GameObject.Find("GameOver");
        controls = controls.GetComponent<Text>();
        stats = stats.GetComponent<Text>();
        input = input.GetComponent<InputField>();
        first = first.GetComponent<Text>();
        second = second.GetComponent<Text>();
        third = third.GetComponent<Text>();
    }

    private void Start()
    {
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
        first.text = "1st " + (GameController.name1 != null ? "<color=#007A8E>" + GameController.name1 + "</color>" + "\n" + "       <b>" + GameController.score1.ToString("#,##0").Replace(",", ".") + "</b>" : "");
        second.text = "2nd " + (GameController.name2 != null ? "<color=#007A8E>" + GameController.name2 + "</color>" + "\n" + "       <b>" + GameController.score2.ToString("#,##0").Replace(",", ".") + "</b>" : "");
        third.text = "3rd " + (GameController.name3 != null ? "<color=#007A8E>" + GameController.name3 + "</color>" + "\n" + "       <b>" + GameController.score3.ToString("#,##0").Replace(",", ".") + "</b>" : "");
    }

    private void OnGameStateChange(bool playing)
    {
        // When "GameController.playig" is "true", disable this gameobject
        if (playing)
        {
            gameObject.SetActive(false);
        }
        // When "GameController.playig" is "false", enable this gameobject
        else
        {
            if (gameObject.activeSelf == true)
            {
                MainMenu();
                gameObject.SetActive(true);
            }
            else
            {
                cursor.SetActive(true);
                gameObject.SetActive(true);
            }
        }
            
    }

    private void OnEnable()
    {
        GameController.minutes = (int)GameController.timer / 60;
        GameController.seconds = GameController.timer - (60 * GameController.minutes);
        // When the respective button is clicked, call its method
        start.onClick.AddListener(() => buttonStart());
        exit.onClick.AddListener(() => buttonExit());
        stats.text = string.Format(" \n Time Survived <size=30><color=red>{0}m{1}s</color></size> \n Score \n <size=30><color=red>{2}</color></size>", GameController.minutes, GameController.seconds.ToString("F0"), (UI.globalScore * (int)GameController.timeStamp).ToString("#,##0").Replace(",", "."));
    }

    // Method for the buttons
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
        Application.Quit();
    }
    //-----------------------

    /// <summary>
    /// Method to open the main menu
    /// </summary>
    public void MainMenu()
    {
        cursor.SetActive(true);
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            logo.gameObject.SetActive(false);
            stats.gameObject.SetActive(false);
            input.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            logo.gameObject.SetActive(true);
            stats.gameObject.SetActive(true);
            input.gameObject.SetActive(true);
        }       
    }        
}
