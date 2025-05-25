using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public float speed = 0.2f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;

    private void Start()
    {
        // Asignar autom�ticamente el Rigidbody del GameObject
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float xVal = fixedJoystick.Horizontal;
        float yVal = fixedJoystick.Vertical;

        Vector3 direction = new Vector3(xVal, 0, yVal);

        rb.velocity = direction * speed;

        if (xVal != 0 && yVal != 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(xVal, yVal) * Mathf.Rad2Deg, transform.eulerAngles.z);
        }
    }
}
