using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour {
    [SerializeField] private GameObject billboard;
    [SerializeField] private GameObject billboard2;
    public float radius = 1.5f;

    void Update() {
        if (Input.GetButtonDown("Fire3")) { //left shift
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hitCollider in hitColliders) {
                Debug.Log(hitCollider.transform.position);
                Vector3 direction = hitCollider.transform.position - transform.position;
                
                if (Vector3.Dot(transform.forward, direction) > .5f) {
                    hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
                }
            }
            billboard.SendMessage("Operate");
        }
        if (Input.GetButtonDown("Fire2")) { //left alt
            billboard2.SendMessage("Operate");
        }
    }
}
