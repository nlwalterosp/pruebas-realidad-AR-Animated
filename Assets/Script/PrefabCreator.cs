using System;
using System.Resources;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject josePrefab;
    
    [SerializeField] private Vector3 joseonOffset;
    [SerializeField] private Vector3 joseOffsetrotation;


    private GameObject jose;
    
    private ARTrackedImageManager arTrackedImageManager;

    private void OnEnable()
    {
        arTrackedImageManager = gameObject.GetComponent<ARTrackedImageManager>();

        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {
            if (image.referenceImage.name == "Target conejo prueba")
            {
                jose = Instantiate(josePrefab, image.transform);
                jose.transform.position = joseonOffset;
                jose.transform.rotation = Quaternion.Euler(joseOffsetrotation);

            }

           
        }

    }
}