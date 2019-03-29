using UnityEngine;
public class WarriorProp : MonoBehaviour{
	void Start(){
		transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0); //One liners rock
	}
}
