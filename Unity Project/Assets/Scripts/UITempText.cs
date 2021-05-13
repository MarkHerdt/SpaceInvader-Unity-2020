using UnityEngine;

public class UITempText : MonoBehaviour
{
    Transform textTransform;

    private void Awake()
    {
        textTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        textTransform.transform.localScale = new Vector3(1, 1, 1);
        Destroy(gameObject, 2.5f);
    }

    void Update()
    {
        transform.position += new Vector3(0, .05f, 0);
    }
}
