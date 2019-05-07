using UnityEngine;
public class WarriorProp : MonoBehaviour{
	void Start(){
		transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.color = new Color(1, 0, 0); //One liners rock
	}
}
