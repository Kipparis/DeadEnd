using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IKillable {
    public GameObject healthBarPrefab;
    public HealthBar healthBar { get; set; }

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

    // Вводим переменные только для того, чтобы манекен можно было убить,
    // потом переводим все переменные в общий интерфейс
    private void Awake() {
        InitHealth();   // Создаём полоску и делаем её неактивной
    }

    private void OnTriggerEnter(Collider other) {
        if (Utils.FindTaggedParent(other.gameObject).tag == "Sword") {
            if (healthBar.inProcess) return;    // Если полоска хп уже пытается отобразиться, 

            ShowHealth();   // Обновляем таймер или запускаем новый

            Scoreboard.S.Init(transform.position, other.GetComponent<Sword>().dd.damage);
            health -= other.GetComponent<Sword>().dd.damage;
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
