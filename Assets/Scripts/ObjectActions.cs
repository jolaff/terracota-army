using UnityEngine;
using UnityEngine.UI;

public class ObjectActions : MonoBehaviour {

    [SerializeField] private float throwForce;
    [SerializeField] Text objectText;
    [SerializeField] Transform playerHand;
    [SerializeField] private bool isPickable;

    private Vector3 objectPos;
    private float distance;
    private Rigidbody objectRB;
    private bool isHolding = false;

    private void Start() {
        objectRB = gameObject.GetComponent<Rigidbody>();
    }

    // Shows up object name when mouse over it.
    private void OnMouseEnter() {
        objectText.text = gameObject.name;
        objectText.enabled = true;
    }

    // Clear the text.
    private void OnMouseExit() {
        objectText.text = "";
        objectText.enabled = false;
    }

    private void Update() {

        // Checks for the distance so the player only pick up objects that are near.
        distance = Vector3.Distance(gameObject.transform.position, playerHand.transform.position);
        if (distance >= 1.0f) isHolding = false;

        if (isHolding == true) {
            ObjectIsPicked();

            if (Input.GetMouseButtonDown(1)) {
                ThrowObject();
            }

        } else
        {
            ObjectIsDropped();
        }
    }

    private void OnMouseUpAsButton() {
        if (isPickable) {
            if (!isHolding) {
                CanPickObject();
            } else isHolding = false;
        }
    }

    private void CanPickObject() {
        if (distance <= 1.0f) {
            isHolding = true;
            objectRB.useGravity = false;
            objectRB.detectCollisions = true;
        }
    }

    private void ObjectIsPicked() {
        objectRB.velocity = Vector3.zero;
        objectRB.angularVelocity = Vector3.zero;
        gameObject.transform.SetParent(playerHand.transform);
    }

    private void ObjectIsDropped()
    {
        objectPos = gameObject.transform.position;
        gameObject.transform.SetParent(null);
        objectRB.useGravity = true;
        gameObject.transform.position = objectPos;
    }

    private void ThrowObject() {
        objectRB.AddForce(playerHand.transform.forward * (throwForce / objectRB.mass));
        isHolding = false;
    }

}