using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    public static Hero S;
    public static float HERO_HEIGHT = 4f;

    public float speed = 1f;
    public float rotEasing = 0.99f;

    public Vector3 tPos;

    public bool ________________;

    public float height;
    public bool onGround = false;

    public Sword[] swords;

    private void Awake() {
        S = this; 
        
        
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Attack();

        Move();
    }

    void Attack() {
        if (Input.GetMouseButtonUp(0)) {
            print("Hero attack");
            // Игрок нажал левую кнопку мыши
            // Значит мы бьём мечом ( который мы можем лутать или сменять
            
        }
    }

    void Move() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        tPos = transform.position;
        if (height != 0) tPos.y = height + Hero.HERO_HEIGHT / 2f;
        tPos.x += h * speed * Time.deltaTime;
        tPos.z += v * speed * Time.deltaTime;

        Vector3 direction = tPos - transform.position;

        transform.position = tPos;

        Vector3 NextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 
            Input.GetAxisRaw("Vertical"));
        //if (NextDir != Vector3.zero)
        //    transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(NextDir),
        //        transform.rotation, rotEasing);
        //transform.rotation = Quaternion.LookRotation(fPos, Vector3.up * 3);
        if (direction.magnitude != 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), 0.1f);
        }
    }

    private void FixedUpdate() {

    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            onGround = true;
            height = collision.gameObject.transform.position.y;
        }
    }
}
