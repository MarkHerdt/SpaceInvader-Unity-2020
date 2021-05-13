using UnityEngine;

// Script for custom mouse pointer
public class FollowMouse : MonoBehaviour
{
    // Objects position
    Vector3 obj; 
    public float x;
    public float y;
    public float z;

    private void Update()
    {
        obj = Input.mousePosition;
        obj.x += x;
        obj.y += y;
        obj.z = z;

        // Sticks the object to the mouse position
        this.transform.position = Camera.main.ScreenToWorldPoint(obj);
    }
}
