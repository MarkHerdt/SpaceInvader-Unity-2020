using UnityEngine;
using UnityEngine.UI;


public class SaveScore : MonoBehaviour
{
    public InputField input;
    public Text first;
    public Text second;
    public Text third;

    private void Awake()
    {
        input = input.GetComponent<InputField>();
        first = first.GetComponent<Text>();
        second = second.GetComponent<Text>();
        third = third.GetComponent<Text>();
    }

    public void Save()
    {
        if (input.text != "")
        {
            if (input.text.Length > 7)
            {
                input.text = input.text.Substring(0, 6);
                input.text += "...";
            }

            if ((UI.globalScore * (int)GameController.timeStamp) >= GameController.score1)
            {
                if (GameController.name1 != null)
                {
                    if (GameController.name2 != null)
                    {
                        GameController.name3 = GameController.name2;
                        PlayerPrefs.SetString("Name3rd", GameController.name3);
                        GameController.score3 = GameController.score2;
                        PlayerPrefs.SetInt("Score3rd", GameController.score3);
                    }
                    GameController.name2 = GameController.name1;
                    PlayerPrefs.SetString("Name2nd", GameController.name2);
                    GameController.score2 = GameController.score1;
                    PlayerPrefs.SetInt("Score2nd", GameController.score2);
                }
                GameController.name1 = input.text;
                PlayerPrefs.SetString("Name1st", GameController.name1);
                GameController.score1 = (UI.globalScore * (int)GameController.timeStamp);
                PlayerPrefs.SetInt("Score1st", GameController.score1);
                PrintScoreList();
            }
            else if ((UI.globalScore * (int)GameController.timeStamp) >= GameController.score2)
            {
                if (GameController.name2 != null)
                {
                    GameController.name3 = GameController.name2;
                    PlayerPrefs.SetString("Name3rd", GameController.name3);
                    GameController.score3 = GameController.score2;
                    PlayerPrefs.SetInt("Score3rd", GameController.score3);
                }
                GameController.name2 = input.text;
                PlayerPrefs.SetString("Name2nd", GameController.name2);
                GameController.score2 = (UI.globalScore * (int)GameController.timeStamp);
                PlayerPrefs.SetInt("Score2nd", GameController.score2);
                PrintScoreList();
            }
            else if ((UI.globalScore * (int)GameController.timeStamp) >= GameController.score3)
            {
                GameController.name3 = input.text;
                PlayerPrefs.SetString("Name3rd", GameController.name3);
                GameController.score3 = (UI.globalScore * (int)GameController.timeStamp);
                PlayerPrefs.SetInt("Score3rd", GameController.score3);
                PrintScoreList();
            }
        }
    }
    public void PrintScoreList()
    {
        first.text = "1st " + "<color=#007A8E>" + GameController.name1 + "</color>" + "\n" + "       <b>" + GameController.score1.ToString("#,##0").Replace(",", ".") + "</b>";
        second.text = "2nd " + "<color=#007A8E>" + GameController.name2 + "</color>" + "\n" + "       <b>" + GameController.score2.ToString("#,##0").Replace(",", ".") + "</b>";
        third.text = "3rd " + "<color=#007A8E>" + GameController.name3 + "</color>" + "\n" + "       <b>" + GameController.score3.ToString("#,##0").Replace(",", ".") + "</b>";
    }
}
