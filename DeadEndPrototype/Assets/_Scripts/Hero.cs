using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    public static Hero S;

    public bool ________________;

    public float height;
    public bool onGround = false;

    private void Awake() {
        S = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void FixedUpdate() {
        Move();
    }

    void Move() {
        Vector3 pos = transform.position;
        if (onGround) {
            pos.y = height;
        }
    }

    void OnCollisionEnter(Collision collision) {
        print("Collision with mag");
        if (collision.gameObject.tag == "Ground") {
            // Если герой взаимодействовал с землёй
            // Значит персонаж уже на земле
            onGround = true;
            DeadEnd.S.HeroCollWithGround();
            height = collision.gameObject.transform.position.y;
        }
    }
}
