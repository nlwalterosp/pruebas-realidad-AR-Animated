using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public float speed = 0.2f;
    public float rotationSpeed = 10f;
    private Vector3 destination;
    private bool movingToDestination = false;

    private Rigidbody rb;
    private RaycastController raycastController;

    private void Start()
    {
        // Asignar autom�ticamente el Rigidbody del GameObject
        rb = GetComponent<Rigidbody>();
        raycastController = FindObjectOfType<RaycastController>(); 
    }

    private void FixedUpdate()
    {
        if (movingToDestination)
        {
            // Mover hacia el destino establecido
            Vector3 direction = (destination - transform.position).normalized;
            rb.velocity = direction * speed;
            
            // Rotar hacia la dirección del movimiento
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    rotationSpeed * Time.deltaTime
                );
            }
            
            // Comprobar si hemos llegado al destino
            if (Vector3.Distance(transform.position, destination) < 0.1f)
            {
                movingToDestination = false;
                rb.velocity = Vector3.zero;
                raycastController.raycastBlocked = false; // Desbloquear el raycast al llegar al destino
            }
        }
        else
        {
            // Movimiento por joystick
            float xVal = fixedJoystick.Horizontal;
            float yVal = fixedJoystick.Vertical;

            Vector3 direction = new Vector3(xVal, 0, yVal);

            rb.velocity = direction * speed;

            if (direction.magnitude > 0.1f)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(xVal, yVal) * Mathf.Rad2Deg, transform.eulerAngles.z);
            }
        }
    }
    
    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
        movingToDestination = true;
        raycastController.raycastBlocked = true; // Bloquear raycast mientras se mueve
    }
}
