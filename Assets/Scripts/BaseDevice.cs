using UnityEngine;
using System.Collections;

public class BaseDevice : MonoBehaviour {
    public float radius = 3.5f;

    void OnMouseDown() {
        Transform player = GameObject.FindWithTag("Player").transform;
        if (Vector3.Distance(player.position, transform.position) < radius) {
            Vector3 direction = transform.position - player.position;
            if (Vector3.Dot(player.forward, direction) > 0.5f) {
                Operate();//如果玩家在附近并面对它
            }
        }
    }

    public virtual void Operate() {
        // behavior of the specific device
    }
}
