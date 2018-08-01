using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IKillable {
    public float distanceForDialogue = 3f;

    public bool _________________________;

    public GameObject healthBarPrefab;
    public HealthBar healthBar { get; set; }

    public bool readyForDialogue = false;

    public Dictionary<string, float> resist { get; set; }

    public float maxHealth;
    public float health {
        get { return (healthBar.health); }
        set {
            healthBar.health = value;
            if (healthBar.health <= 0) {
                Die();
            }
        }
    }

    public int secondsToShowHealthBar;

    private void Update() {
        // Проверяем расстояние до героя
        Vector3 direction = Hero.S.transform.position - transform.position;
        float distance = direction.magnitude;

        // Если расстояние ближе какого то значения,
        readyForDialogue = (distance < distanceForDialogue);
        // Герой может нажать на кнопку и начать разговор

        // Если герой ближе этого расстояния, враг смотрит на него ( ну или как нибудь реагирует )
        if (readyForDialogue) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 0.2f);

        // TODO: пока что один враг, но нужно создавать список врагов и смотреть кто из них ближе
    }

    // Вводим переменные только для того, чтобы манекен можно было убить,
    // потом переводим все переменные в общий интерфейс
    private void Awake() {
        InitHealth();   // Создаём полоску и делаем её неактивной
    }

    public void InitHealth() {
        GameObject go = Instantiate(healthBarPrefab) as GameObject;
        go.transform.parent = transform;
        healthBar = go.GetComponent<HealthBar>();
        healthBar.owner = this.gameObject;

        // При создании всё на максимальках
        healthBar.maxHealth = maxHealth;
        healthBar.health = maxHealth;

        healthBar.gameObject.SetActive(false);
    }
    
    void ShowHealth() {
        if (healthBar.visible) {    // Если хпшки уже на своём месте, обновляем таймер
            // Если мы уже видим его, мы отменяем прошлый показ, и начинаем новый
            // Используем CancelInvoke and Invoke()
            CancelInvoke("UnshowHealth");   // Типо обновляем таймер
            Invoke("UnshowHealth", secondsToShowHealthBar);
            return;
        }
        StartCoroutine(healthBar.Show());

        // Сделать подсчёт общего времени как в полоске хп, так и тут, в зависимости от всяких
        // moveDuration и других времён
        Invoke("UnshowHealth", secondsToShowHealthBar);
    }

    public void UnshowHealth() {
        StartCoroutine(healthBar.Unshow());
    }

    // Добавить подсчёт в зависимости от резиста
    private void OnTriggerEnter(Collider other) {
        if (Utils.FindTaggedParent(other.gameObject).tag == "Sword") {
            if (!healthBar.inProcess) ShowHealth();   // Если полоска хп уже пытается отобразиться, 

            Sword otherSword = other.GetComponent<Sword>();

            float damage = otherSword.dd.damage;
            if (otherSword.state != WeaponState.attack) damage = damage / 4f;    // Если мы задели врага в состоянии блока, то урон поменьше
            health -= damage;

            Scoreboard.S.Init(transform.position, damage);
        }
    }

    void Die() {
        // Делаем нового
        DeadEnd.S.Die(this);
        // Удаляем этого
        Destroy(gameObject);
        // TODO: Сделать так чтоб хуйнюшка распадалась на составные части и затем исчезала
    }
}
