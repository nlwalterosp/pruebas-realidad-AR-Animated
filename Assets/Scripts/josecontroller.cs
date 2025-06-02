using UnityEngine;
using UnityEngine.UI;

public class josecontroller : MonoBehaviour
{
    private Toggle walkToggle;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        walkToggle = GameObject.Find("WalkToggle").GetComponent<Toggle>();
        
      walkToggle.onValueChanged.AddListener(OnWalkToggleChanged);
       
    }

    private void OnWalkToggleChanged(bool isOn)
    {
        animator.SetBool("Walk", isOn);
    }
}
