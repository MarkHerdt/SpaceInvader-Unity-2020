using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float x = 0;
    public float y = 0.0624f;
    public float z = 0;

    void Update()
    {
        transform.Rotate(new Vector3(x, y, z));
    }
}
