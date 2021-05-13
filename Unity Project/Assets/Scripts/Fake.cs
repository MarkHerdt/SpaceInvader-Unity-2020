using System.Collections;
using UnityEngine;

// Animates the fake image in the main menu
public class Fake : MonoBehaviour
{
    public AudioSource source;

    private void OnEnable()
    {
        StartCoroutine(ScaleFake());
    }

    IEnumerator ScaleFake()
    {
        transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(.5f);
        
        for (int i = 0; i < 100; i++)
        {
            transform.localScale += new Vector3(.01f, .01f, .01f);
            yield return new WaitForSeconds(.0001f);
            if (i == 50)
            {
                source.Play();
            }
        }
    }
}
