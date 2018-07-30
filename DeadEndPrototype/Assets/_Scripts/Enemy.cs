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
        set { healthBar.health = value; }
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

    // Когда получаем урон, показываем полоску хп на 2 секунды
    private void OnCollisionEnter(Collision collision) {
        if (Utils.FindTaggedParent(collision.gameObject).tag == "Hero") {
            if (healthBar.inProcess) return;    // Если полоска хп уже пытается отобразиться, 
            ShowHealth();   // ничего не делаем
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (Utils.FindTaggedParent(other.gameObject).tag == "Sword") {
            if (healthBar.inProcess) return;    // Если полоска хп уже пытается отобразиться, 
            health -= other.GetComponent<Sword>().dd.damage;
            if (health <= 0) { // Кто то умер, мы убиваем и спауним нового
                Die();
            }
            ShowHealth();   // ничего не делаем
        }
    }

    void Die() {
        Destroy(gameObject);
        // TODO: Сделать так чтоб хуйнюшка распадалась на составные части и затем исчезала
    }
}
