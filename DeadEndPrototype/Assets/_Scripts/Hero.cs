using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Сделать интерфейс IKillable который определяет хп и разные резисты.
// также с этим интерфейсом отображается хп бар над 

// TODO: Сделать класс alive, в нём будет функция он хит энтер которая спаунит показ урона, и показ хпшки, так же основывая на
// резистах убирает из текущего хп.

// TODO: Написать функции Loot Equip, и класс инвентаря для того чтобы можно было что то делать

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

    // TODO: Добавить свойство, что когда добавляешь сюда итем, сразу задаётся родитель и стостояние
    // для меча
    public Sword sword;   // Максимальное кол-во экипируемых мечей
    public HealthBar healthBar;

    public GameObject poi; // У игрока есть точка к которой он может привязать свой взгляд, крч как в экшнах на консолях

    public bool isTalking = false;

    private void Awake() {
        S = this;

        // TODO: Заменить на нормальный поиск и экипировку оружия
        sword = new Sword();
        Sword tSword = GameObject.Find("Sword").GetComponent<Sword>();
        if (tSword != null) {
            sword = tSword;
        }

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

        // TODO: Сделать отображение кто
        // Проверяем функционирование пои
        if (Input.GetKeyUp(KeyCode.L)) {
            SwitchBetweenEnemies();
        }

        // TODO: При старте диалога камера перемещается между игроком и объектом
        // Перенести подсчёт на что нибудь, кроме пои, т.к. это всё таки для врагов фигнюшка
        if (Input.GetKeyUp(KeyCode.Space)) {
            if (poi != null && poi.GetComponent<Enemy>() != null && poi.GetComponent<Enemy>().readyForDialogue) {
                // Если наша цель - враг, с которым есть доступная дистанция, можно разговаривать
                if (isTalking) {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    return;
                } 
                poi.GetComponent<DialogueTrigger>().TriggerDialogue();
                isTalking = true;
            } 
        }

        // Если нажимаем на P рядом с предметом, то он лутается
        if (Input.GetKeyUp(KeyCode.P)) {
            
        }
    }


    void SwitchBetweenEnemies() {
        // Если пои всё ещё равун нулю, выбираем первого врага
        if (poi == null) {
            poi = DeadEnd.S.enemies[0].gameObject;
            SelectEnemy.S.poi = poi;
            return;
        }
        int ndx = DeadEnd.S.enemies.IndexOf(poi.GetComponent<Enemy>());
        if (ndx == DeadEnd.S.enemies.Count - 1) {
            poi = null; // Если это последний моб в списке, просто снимаем список
            SelectEnemy.S.poi = poi;
            return;
        }
        ndx++;
        poi = DeadEnd.S.enemies[ndx].gameObject;

        // TODO: Сделать выделение ио
        SelectEnemy.S.poi = poi;
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
        //if (height != 0) tPos.y = height + Hero.HERO_HEIGHT / 2f;
        tPos.x += h * speed * Time.deltaTime;
        tPos.z += v * speed * Time.deltaTime;

        Vector3 direction = tPos - transform.position;

        // TODO: Сделать какое то обозначение врага которого мы выбрали
        // TODO: Сделать выбор врагов только которые находятся в поле вида камеры
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

    private void OnTriggerEnter(Collider other) {
        if (Utils.FindTaggedParent(other.gameObject).tag == "Sword") {
            //Debug.Log("Triggered with sword");

            // Сначала просто лутаем, но это для начала
            // Задаём родителя ( потому что мы его лутанули )
            GameObject go = other.gameObject;
            go.transform.SetParent(transform);

            // Задаём состояние и другое
            sword = other.GetComponent<Sword>();

            sword.state = WeaponState.idle;
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
