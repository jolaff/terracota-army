using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjects : MonoBehaviour
{

    [SerializeField] GameObject triggerObject;
    [SerializeField] string triggerName;

    ObjectActions objActions;
    Animator openDoorAnim;

    // Start is called before the first frame update
    void Start()
    {
        objActions = triggerObject.GetComponent<ObjectActions>();
        openDoorAnim = GetComponent<Animator>();
    }

    private void OnMouseUpAsButton()
    {
        if (triggerObject.name == triggerName && objActions.IsHolding)
        {
            openDoorAnim.SetTrigger("OpenDoor");
        }
    }
}
