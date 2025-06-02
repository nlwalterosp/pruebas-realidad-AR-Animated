using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private Camera userCamera;
    private GameObject targetObject;
    private bool adamMode = true;
    [SerializeField] private LayerMask layerMask;
    private PlayerController playerController;
    public bool raycastBlocked = false;

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
            if (adamMode && !raycastBlocked)
            {
                Raycast(ray, out hitObject, 0);
            }
            if (!adamMode && !raycastBlocked)
            {
                Raycast(ray, out hitObject, 1);
            }
        }
    }

    public void Raycast(Ray ray, out RaycastHit hitObject, int a)
    {
        switch (a)
        {
            default:
                hitObject = new RaycastHit();
                return;
            case 0:
                if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                {
                    if (hitObject.collider.CompareTag("Adam") ||
                        hitObject.collider.gameObject == targetObject ||
                        (targetObject != null && hitObject.collider.transform.IsChildOf(targetObject.transform) && hitObject.collider.isTrigger))
                    {
                        Debug.Log("Objeto tocado: " + hitObject.collider.gameObject.name);
                        adamMode = false;
                    }
                }
                return;
            case 1:
                if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                {
                    if (hitObject.collider.CompareTag("Terrain"))
                    {
                        Debug.Log("Objeto tocado: " + hitObject.collider.gameObject.name + "moviendo a Adam a: " + hitObject.point);
                        raycastBlocked = true;
                        playerController.SetDestination(hitObject.point);
                        adamMode = true;
                    }
                }
                return;
        }
    }

    public void SetTargetObject(GameObject target)
    {
        targetObject = target;
        playerController = targetObject.GetComponent<PlayerController>();
    }
}
