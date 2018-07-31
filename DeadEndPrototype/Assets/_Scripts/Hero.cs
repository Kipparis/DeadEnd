using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Сделать интерфейс IKillable который определяет хп и разные резисты.
// также с этим интерфейсом отображается хп бар над 

// TODO: Сделать класс alive, в нём будет функция он хит энтер которая спаунит показ урона, и показ хпшки, так же основывая на
// резистах убирает из текущего хп.

public class Hero : MonoBehaviour {

    public static Hero S;
    public static float HERO_HEIGHT = 4f;

    public float speed = 1f;
    public float rotEasing = 0.99f;

    public Vector3 tPos;

    public float maxHealth;
    public GameObject healthBarPrefab;

    public GameObject floatingScorePrefab;

    public bool ________________;

    public float height;
    public bool onGround = false;

    public Sword sword;   // Максимальное кол-во экипируемых мечей
    public HealthBar healthBar;

    GameObject poi; // У игрока есть точка к которой он может привязать свой взгляд, крч как в экшнах на консолях

    Transform UI;   // Холст для отображения UI

    private void Awake() {
        S = this;

        // TODO: Заменить на нормальный поиск и экипировку оружия
        sword = new Sword();
        Sword tSword = GameObject.Find("Sword").GetComponent<Sword>();
        if (tSword != null) {
            sword = tSword;
        }

        UI = GameObject.Find("UI").GetComponent<Transform>();

        InitHealth();
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
                UnshowHealthBar();  // Если хп уже было видно, скрываем
            } else {
                print("Showing HealsBar");
                ShowHealthBar();    // Если их не видно, показываем
            }
        }

        // Проверяем функционирование пои
        if (Input.GetKeyUp(KeyCode.L)) {
            SwitchBetweenEnemies();  
        } 
    }

    void SwitchBetweenEnemies() {
        // Если пои всё ещё равун нулю, выбираем первого врага
        if (poi == null) {
            poi = DeadEnd.S.enemies[0].gameObject;
            return;
        }
        int ndx = DeadEnd.S.enemies.IndexOf(poi.GetComponent<Enemy>());
        if (ndx == DeadEnd.S.enemies.Count - 1) {
            poi = null; // Если это последний моб в списке, просто снимаем список
            return;
        }
        ndx++;
        poi = DeadEnd.S.enemies[ndx].gameObject;
    }

    void UnshowHealthBar() {
        StartCoroutine(healthBar.Unshow());
    }

    void ShowHealthBar() {
        StartCoroutine(healthBar.Show());
    }

    // Функция создаёт объект HealthBar, и даёт ему понять что делать
    void InitHealth() {
        GameObject go = Instantiate(healthBarPrefab) as GameObject;
        go.transform.parent = transform;
        healthBar = go.GetComponent<HealthBar>();
        healthBar.owner = this.gameObject;

        // При создании всё на максимальках
        healthBar.maxHealth = maxHealth;
        healthBar.health = maxHealth;

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

        if (poi != null) {
            // Если есть точка интереса, вращение делаем именно к этой точке
            direction = poi.transform.position - transform.position;
        }

        if (direction.magnitude != 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), 0.1f);
        }
        transform.position = tPos;
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

    float health {
        get { return (healthBar.health); }
        set { healthBar.health = value; }
    }
}
