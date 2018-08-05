using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Ввести систему отображения урона
// для начала просто отплывающие значения, а уже потом стакающиеся

// TODO: Создать список в DeadEnd, и по нему уже скролить

// TODO: Поменять именна сворд на веапон, либо перенести всё в супер класс веапон

// TODO: Сделать ПИ скалируемым, сделать считывание и парс диалогов из файла .di

// TODO: Вверху сделать простую табличку о характеристиках, и выплывающую таблицу об эквипе

public class DeadEnd : MonoBehaviour {

    public static DeadEnd S;
    public static bool DEBUG = true;

    public GameObject enemyPrefab;

    public bool _________________;

    public List<Enemy> enemies;

    private void Awake() {
        S = this;

        SpawnEnemy();
    }

    public void HeroCollWithGround() {
        if (DEBUG) print("HeroCollWithGround");
    }

    public void SpawnEnemy() {
        GameObject go = Instantiate(enemyPrefab) as GameObject;
        // Задаём позицию
        go.transform.position = new Vector3(10, 2, 0);

        Enemy enemy = go.GetComponent<Enemy>();
        if (enemies == null) enemies = new List<Enemy>();
        enemies.Add(enemy);
    }

    public void Die(Enemy enemy) {
        SpawnEnemy();

        enemies.Remove(enemy);
    }
}
