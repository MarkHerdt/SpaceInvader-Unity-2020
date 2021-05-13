using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip hover;
    public AudioClip click;

    public void OnHover()
    {
        source.PlayOneShot(hover);
    }
    public void OnClick()
    {
        source.PlayOneShot(click);
    }
}
