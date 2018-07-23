using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    public static Hero S;

    public bool ________________;

    public float height;
    public bool onGround = false;

    Transform characterTrans;

    private void Awake() {
        S = this;

        characterTrans = transform.Find("CharacterTransform");
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = characterTrans.position;
        pos.y = height + 1;
        characterTrans.position = pos;
        transform.position = pos;
    }

    private void FixedUpdate() {

    }

    private void OnTriggerEnter(Collider other) {
        print("Trigger hero");
        if (other.tag == "Ground") {
            // Это земля
            height = other.gameObject.transform.position.y;
        }
    }
}
