using UnityEngine;

public class RotateAround : MonoBehaviour
{
    private Vector3 target = new Vector3(0, -14, 14);
    private Vector3 axis = new Vector3(0, 1, 0);
    // Speed the object move with
    public float speed = 5;

    private void Update()
    {
        transform.RotateAround(target, axis, speed * Time.deltaTime);
    }
}
