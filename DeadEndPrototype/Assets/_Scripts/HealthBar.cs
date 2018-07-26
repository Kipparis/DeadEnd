using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    // Чтобы не приравнивать каждый раз это поле, нужно автоматически 
    // вызывать это поле и сделать так чтоб оно само определяло его позицию
    public GameObject owner;
    GameObject healthStrip;

    private void Awake() {
        healthStrip = GameObject.Find("Health");
    }

    public int maxHealth;

    public int health {
        get { return (_health); }
        set {
            _health = value;
            if (_health < 0) {

                return;
            }
            healthStrip.transform.localScale = new Vector3(
                _health / maxHealth, 1, 1);
        }
    }
    private int _health;

    // TODO: Сделать метод для появления полоски

    private void Update() {
        // TODO: Сделать смягчение
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
            Camera.main.transform.rotation * Vector3.up);
    }
}
