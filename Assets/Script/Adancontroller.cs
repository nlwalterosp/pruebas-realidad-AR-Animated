using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public float speed = 0.5f;
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

        if (direction.magnitude > 0.1f)
        {
            // Movimiento del personaje
            rb.linearVelocity = direction.normalized * speed;

            // Rotaci�n suave hacia la direcci�n del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}
