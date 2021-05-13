using UnityEngine;

public class PowerUpRotation : MonoBehaviour
{
	public float rotationSpeed = 99.0f;

	void Update ()
	{
		transform.Rotate(new Vector3(0f, 0f, 1f) * Time.deltaTime * rotationSpeed);
	}
}
