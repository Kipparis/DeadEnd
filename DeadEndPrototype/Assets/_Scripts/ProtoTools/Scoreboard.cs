using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {
    public static Scoreboard S;

    public GameObject floatingScorePrefab;

    public bool ___________________;
    // Для того чтобы сразу задавать родителя для новых хуйнюшек
    Transform UI;

    private void Awake() {
        S = this;

        UI = GameObject.Find("UI").transform;
    }

    // Пока что инит с одной точкой, потом можно исп. params
    public void Init(Vector3 startPos, string str) {
        Vector3 pos = Vector3.zero;

        // Создаём флоэтин скор
        GameObject go = Instantiate(floatingScorePrefab) as GameObject;
        go.transform.SetParent(UI);

        FloatingScore fs = go.GetComponent<FloatingScore>();
        fs.text = str;

        // Парсим туда бизеровы точки
        // Пока только одну
        fs.bezierPts = new List<Vector3>();

        fs.bezierPts.Add(Camera.main.WorldToScreenPoint(startPos));

        // Добавляем точку на верхней границе в центре
        pos.x = Screen.width / 2f;
        pos.y = Screen.height;
        fs.bezierPts.Add(pos);

        // Третью точку выбираем немного правее от начальной позиции
        fs.bezierPts.Add(Camera.main.WorldToScreenPoint(startPos + Vector3.right * 2));
        fs.startTime = Time.time;
    }

    public void Init(Vector3 startPos, float value) {
        Init(startPos, value.ToString());
    }
}
