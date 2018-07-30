using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// TODO: Сделать нормальный конструктор в Scoreboard, чтобы задавало все значения ( хотя хз )
public class FloatingScore : MonoBehaviour {
    static public float MOVEDUR = 2f;   // Время передвижения по дуге

    public List<Vector3> bezierPts;
    Transform UI;

    public float startTime = -1;   // Пока значение такое, движение не начинается

    // Свойства для быстрого доступа
    string _text;
    string text {
        get { return (gameObject.GetComponent<Text>().text); }
        set { gameObject.GetComponent<Text>().text = value; }
    }

    float _value;
    float value {
        get { return (float.Parse(text)); }
        set { text = value.ToString(); }
    }

    private void Update() {
        if (startTime == -1) return;    // Если время пока не задали, ничё не делаем
        if (Time.time < startTime) return;  // Если ещё не пришло время, ничё не делаем

        float u = (Time.time - startTime) / FloatingScore.MOVEDUR;
        u = Mathf.Clamp01(u); // Ограничиваем нулём и единицей

        transform.position = Utils.Bezier(u, bezierPts);

        if (u == 1) {
            // Переход оконченн
            Destroy(this.gameObject);   // Пока что
        }
    }
}
