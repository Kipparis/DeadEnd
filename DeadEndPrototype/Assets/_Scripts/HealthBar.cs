﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    // Чтобы не приравнивать каждый раз это поле, нужно автоматически 
    // вызывать это поле и сделать так чтоб оно само определяло его позицию
    public GameObject owner;    // Использовать полоску жизни могут только поцы
    // которые наследуют интерфейс IKillable, так что потом заменим тип на интерфейс, и от него
    // будет выбирать хпшку
    GameObject healthStrip;

    public List<Vector3> bezierPts; // Точки для перехода в позицию повыше
    public List<Quaternion> bezierRots; // Точки для перехода в поворот

    bool isMoving, isRotating = false;  // От них зависит 

    public float startTime = -1f;
    public float moveDur = 1.5f;

    public bool visible = false;    // Становится видимой только если стоит на
    // своём месте

    private void Awake() {
        healthStrip = GameObject.Find("Health");

        bezierPts = new List<Vector3>();
        bezierRots = new List<Quaternion>();
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

    private void Update() {
        // TODO: Сделать смягчение
        if (visible) { // Если полоска на своём месте, её можно крутить
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
            Camera.main.transform.rotation * Vector3.up);
        }   // Поворачивает бар только если вся анимация законченна
        if (startTime != -1) { // Заданно какое то начальное время
            float u = (Time.time - startTime) / moveDur;
            u = Mathf.Clamp01(u);

            if (bezierRots.Count == 0) bezierRots.Add(transform.localRotation);
            if (bezierPts.Count == 0) bezierPts.Add(transform.localPosition);

            transform.localPosition = Utils.Bezier(u, bezierPts);
            transform.localRotation = Utils.Bezier(u, bezierRots);

            if (u == 0) {
                transform.localPosition = bezierPts[0];
                transform.localRotation = bezierRots[0];
            }

            if (u == 1) {
                transform.localPosition = bezierPts[bezierPts.Count - 1];
                transform.localRotation = bezierRots[bezierRots.Count - 1];
                startTime = -1; // Чтобы больше не заходить в этот цикл
                // Очищаем списки чтоб не мешалось
                bezierPts.Clear();
                bezierRots.Clear();
            }
        }
    }

    // т.к. мы знаем время переходов, создаём эвэйт фор секондс,
    // чтобы заново задать время старта только уже для поворота

    public IEnumerator Show() {
        // Делаем видимым
        this.gameObject.SetActive(true);
        // Задаём начальную позицию и поворот

        transform.position = owner.transform.position;
        transform.localRotation = Quaternion.Euler(0, 0, 90);

        // Сначала переходим в позицию повыше
        bezierPts.Add(transform.localPosition);  // Первая точка
        bezierPts.Add(transform.localPosition + Vector3.up * 1.3f);

        startTime = Time.time;

        yield return new WaitForSeconds(moveDur + 0.1f);    // +0.1f прочто чтобы убедиться

        // Меняем поворот
        bezierRots.Add(transform.localRotation); // Текущий поворот
        Vector3 fRot = transform.localRotation.ToEuler();
        fRot.z = 0;
        bezierRots.Add(Quaternion.Euler(fRot));

        startTime = Time.time;

        yield return new WaitForSeconds(moveDur + 0.1f);    // +0.1f прочто чтобы убедиться

        visible = true;
    }
}
