using UnityEngine;
using UnityEngine.UI;

public class ObjectActions : MonoBehaviour {

    [SerializeField] GameObject pickableObject;
    [SerializeField] Text objectText;
    [SerializeField] Transform playerHand;

    private Rigidbody objectGravity;
    private bool isColiding;

    private void Start() {
        objectGravity = pickableObject.GetComponent<Rigidbody>();
        objectGravity.useGravity = true;

        objectText.text = "";
        objectText.enabled = false;
    }

    private void OnMouseEnter() {
        objectText.text = pickableObject.name;
        objectText.enabled = true;
    }

    private void OnMouseExit() {
        objectText.text = "";
        objectText.enabled = false;
    }

    private void OnMouseDown() {
        objectGravity.useGravity = false;
        objectGravity.isKinematic = true;
        pickableObject.transform.position = playerHand.transform.position;
        pickableObject.transform.rotation = playerHand.transform.rotation;
        gameObject.transform.position = pickableObject.transform.position;
        gameObject.transform.rotation = pickableObject.transform.rotation;
        pickableObject.transform.parent = playerHand.transform;
    }

    private void OnMouseUp() {
        objectGravity.isKinematic = false;
        objectGravity.useGravity = true;
        pickableObject.transform.parent = GameObject.Find("Pickable Objects").transform;
        pickableObject.transform.position = playerHand.transform.position;
        gameObject.transform.position = pickableObject.transform.position;
    }

}