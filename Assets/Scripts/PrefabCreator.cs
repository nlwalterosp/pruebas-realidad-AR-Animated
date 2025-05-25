using System;
using System.Resources;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject scenarioPrefab;
    [SerializeField] private Vector3 scenarioOffsetPosition;
    [SerializeField] private Vector3 scenarioOffsetRotation;
    [SerializeField] private RaycastController raycastController;
    private GameObject scenario;
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
                scenario = Instantiate(scenarioPrefab, image.transform);
                scenario.transform.localPosition = scenarioOffsetPosition;
                scenario.transform.localRotation = Quaternion.Euler(scenarioOffsetRotation);
                TransferChildTarget(scenario);
            }
        }
    }

    private void TransferChildTarget(GameObject scenario)
    {
        Transform targetTransform = scenario.transform.Find("Escenario y personajes/MDL_Adam");

        if (targetTransform != null)
        {
            GameObject target = targetTransform.gameObject;
            raycastController.SetTargetObject(target);
        }
    }
}