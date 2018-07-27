using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Сделать интерфейс IKillable который определяет хп и разные резисты.
// также с этим интерфейсом отображается хп бар над 

public class Hero : MonoBehaviour {

    public static Hero S;
    public static float HERO_HEIGHT = 4f;

    public float speed = 1f;
    public float rotEasing = 0.99f;

    public Vector3 tPos;

    public int maxHealth;
    public GameObject healthBarPrefab;

    public bool ________________;

    public float height;
    public bool onGround = false;

    public Sword sword;   // Максимальное кол-во экипируемых мечей
    public HealthBar healthBar;

    private void Awake() {
        S = this;

        // TODO: Заменить на нормальный поиск и экипировку оружия
        sword = new Sword();
        Sword tSword = GameObject.Find("Sword").GetComponent<Sword>();
        if (tSword != null) {
            sword = tSword;
        }

        InitHealthBar();
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // Движение сначала, т.к. стоим на месте в редких случаях
        Move(); // Двигаемся

        // Если блок активен, не можем ударить
        Block();    // Блокируем удар
        Attack();   // Атакуем

        // Проверяем инициализацию healthBar'a
        if (Input.GetKeyUp(KeyCode.H)) { 
            if (healthBar.visible) {
                UnshowРealthBar();  // Если хп уже было видно, скрываем
            } else {
                print("Showing HealsBar");
                ShowHealthBar();    // Если их не видно, показываем
            }
        }
    }

    void ShowHealthBar() {
        StartCoroutine(healthBar.Show());
    }

    // Функция создаёт объект HealthBar, и даёт ему понять что делать
    void InitHealthBar() {
        GameObject go = Instantiate(healthBarPrefab) as GameObject;
        go.transform.parent = transform;
        healthBar = go.GetComponent<HealthBar>();
        healthBar.owner = this.gameObject;
        healthBar.gameObject.SetActive(false);
    }

    void Block() {
        if (Input.GetKeyDown(KeyCode.J)) { // ПКМ
            print("Hero block with " + sword.name);
            // Блокируем
            sword.state = WeaponState.block;
        }
        if (Input.GetKeyUp(KeyCode.J)) {
            // Перестаём блокировать
            sword.state = WeaponState.idle;
        }
    }

    void Attack() {    
        if (Input.GetKeyDown(KeyCode.K)) { // ЛКМ
            // Значит мы бьём мечом ( который мы можем лутать или сменять
            print("Hero attack with " + sword.name);
            sword.state = WeaponState.attack;
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

        if (direction.magnitude != 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), 0.1f);
        }

        // Хпшечки всегда смотрят в камеру
        //healthBar.transform.LookAt(Camera.main.transform);
        

        //healthBar.transform.rotation = rot;

    }

    private void FixedUpdate() {

    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            onGround = true;
            height = collision.gameObject.transform.position.y;
        }
    }

    public void DamageSomething() {
        print("Sword triggered with something");
    }
}
