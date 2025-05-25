using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private Camera userCamera;
    [SerializeField] private GameObject avisoSeleccion;
    private GameObject targetObject;

    void Start()
    {
        avisoSeleccion.SetActive(false);
    }

    void Update()
    {
        Vector2 pointerPosition = Mouse.current != null ? 
            Mouse.current.position.ReadValue() : 
            Touchscreen.current != null ? 
                Touchscreen.current.primaryTouch.position.ReadValue() : 
                Vector2.zero;
                
        Ray ray = userCamera.ScreenPointToRay(pointerPosition);
        RaycastHit hitObject;

        if (Mouse.current != null && Mouse.current.leftButton.isPressed || 
            Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Debug.Log("Pantalla tocada");

            if (Physics.Raycast(ray, out hitObject))
            {
                Debug.Log("Raycast tocó un objeto: " + hitObject.collider.gameObject.name);
                if (hitObject.collider.CompareTag("Adam") ||
                    hitObject.collider.gameObject == targetObject ||
                    (targetObject != null && hitObject.collider.transform.IsChildOf(targetObject.transform) && hitObject.collider.isTrigger))
                {
                    Debug.Log("Objeto tocado: " + hitObject.collider.gameObject.name);
                    avisoSeleccion.SetActive(true);
                }
            }
        }
    }

    public void SetTargetObject(GameObject target)
    {
        targetObject = target;
        avisoSeleccion.SetActive(false);
        Debug.Log("Objeto objetivo asignado: " + target.name);
    }
}