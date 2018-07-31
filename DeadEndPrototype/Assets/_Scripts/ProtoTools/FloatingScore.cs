using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Чтобы они все двигались вместе с камерой, можно создавать в нужном ио список всех пи, которые относятся к нему.
// Потом обновлять их положение по сравнению с тем что было
public class FloatingScore : MonoBehaviour {
    // Ускорить
    static public float MOVEDUR = 1f;   // Время передвижения по дуге

    public List<Vector3> bezierPts;
    Transform UI;

    public float startTime = -1;   // Пока значение такое, движение не начинается

    // Свойства для быстрого доступа
    private string _text;
    public  string text {
        get { return (gameObject.GetComponent<Text>().text); }
        set { gameObject.GetComponent<Text>().text = value; }
    }

    private float _value;
    public  float value {
        get { return (float.Parse(text)); }
        set { text = value.ToString(); }
    }

    private void Update() {
        if (startTime == -1) return;    // Если время пока не задали, ничё не делаем
        if (Time.time < startTime) return;  // Если ещё не пришло время, ничё не делаем

        float u = (Time.time - startTime) / FloatingScore.MOVEDUR;
        u = Mathf.Clamp01(u); // Ограничиваем нулём и единицей

        // Работаем с позицией
        transform.position = Utils.Bezier(u, bezierPts);

        // Работаем с прозрачностю
        Color newColor = new Color(1, 1, 1, Utils.Bezier(u, new List<float> { 0,5,0}));
        gameObject.GetComponent<Text>().color = newColor;

        if (u == 1) {
            // Переход оконченн
            Destroy(this.gameObject);   // Пока что
        }
    }
}
