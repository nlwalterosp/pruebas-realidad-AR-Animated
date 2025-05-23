using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private Camera userCamera; // Referencia a la cámara que se asignará desde el inspector
    private GameObject targetObject; // Objeto objetivo arrastrando desde el inspector
    
    [SerializeField] private ARRaycastManager arRaycastManager; // Referencia al ARRaycastManager
    [SerializeField] private bool useARRaycast = true; // Opción para alternar entre raycast normal y AR raycast

    private GameObject avisoSeleccion;
    
    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>(); // Lista para almacenar hits de AR raycast
    
    void Start()
    {
        if (userCamera == null)
        {
            Debug.LogError("Debes asignar una cámara en el inspector.");
        }
        
        if (targetObject == null)
        {
            Debug.LogError("Debes asignar un objeto objetivo en el inspector.");
        }
        
        if (useARRaycast && arRaycastManager == null)
        {
            arRaycastManager = FindObjectOfType<ARRaycastManager>();
            if (arRaycastManager == null)
            {
                Debug.LogError("No se encontró ARRaycastManager. Usando raycast normal.");
                useARRaycast = false;
            }
        }
        
        targetObject = GameObject.Find("MDL_Adam");
        avisoSeleccion = GameObject.Find("AvisoSeleccion");
        avisoSeleccion.SetActive(false); 
    }

    void Update()
    {
        targetObject = GameObject.Find("MDL_Adam");
        avisoSeleccion = GameObject.Find("AvisoSeleccion");
        avisoSeleccion.SetActive(false); 
        // Detectar toques en pantalla para Android
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Obtener posición del toque
            Vector2 touchPosition = Input.GetTouch(0).position;
            
            if (useARRaycast && arRaycastManager != null)
            {
                // Usar AR Raycast para detectar tanto superficies reales como objetos virtuales
                if (arRaycastManager.Raycast(touchPosition, s_Hits, TrackableType.AllTypes))
                {
                    // Verificar todos los hits en busca del objeto objetivo
                    bool foundTarget = false;
                    
                    // Primero intentamos un AR Raycast
                    foreach (var hit in s_Hits)
                    {
                        // Realizar un segundo raycast físico desde el punto de impacto AR
                        Ray ray = new Ray(hit.pose.position, hit.pose.rotation * Vector3.forward);
                        RaycastHit physicsHit;
                        
                        if (Physics.Raycast(ray, out physicsHit, 5f))
                        {
                            if (physicsHit.collider.isTrigger && (physicsHit.collider.gameObject == targetObject || physicsHit.collider.transform.IsChildOf(targetObject.transform)))
                            {
                                Debug.Log("AR Raycast hit " + targetObject.name);
                                avisoSeleccion.SetActive(true);
                                foundTarget = true;
                                break;
                            }
                        }
                    }
                    
                    // Si no encontramos el objetivo, intentamos un raycast directo desde la cámara
                    if (!foundTarget)
                    {
                        PerformStandardRaycast(touchPosition);
                    }
                }
                else
                {
                    // Si el AR Raycast no golpea nada, intentamos un raycast normal
                    PerformStandardRaycast(touchPosition);
                }
            }
            else
            {
                // Usar raycast estándar si no estamos usando AR Raycast
                PerformStandardRaycast(touchPosition);
            }
        }
    }
    
    private void PerformStandardRaycast(Vector2 touchPosition)
    {
        // Lanzar raycast desde la cámara del usuario
        Ray ray = userCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            // Verificar si golpeó al objeto objetivo mediante referencia directa
            if (hit.collider.gameObject == targetObject)
            {
                Debug.Log("Raycast hit " + targetObject.name);
                avisoSeleccion.SetActive(true);
            }
        }
    }
}
