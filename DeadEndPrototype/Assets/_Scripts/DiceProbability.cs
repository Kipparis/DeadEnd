using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceProbability : MonoBehaviour {

    public int numDice = 2;
    public int numSides = 6;
    public bool checkToCalculate = false;
    // ^ когда отмечаем, начинается подсчёт
    public int maxIterations = 10000;
    // ^ максимальное число повторений для одного кадра
    public float width = 16;
    public float height = 9;

    public bool _____________;

    public int[] dice;  // Массив значений каждого кубика
    public int[] rolls; // Массив выпавших значение

    private void Awake() {
        // Задаём основную камеру чтобы корректно отображать краф
        Camera cam = Camera.main;
        //cam.backgroundColor = Color.black;
        cam.orthographic = true;
        cam.orthographicSize = 5;
        cam.transform.position = new Vector3(8, 4.5f, -10);
    }

    private void Update() {
        if (checkToCalculate) {
            StartCoroutine(CalculateRolls());
            checkToCalculate = false;
        }
    }

    private void OnDrawGizmos() {
        float minVal = numDice;
        float maxVal = numDice * numSides;

        // Если массивы пусты, возвращаем
        if (rolls == null || rolls.Length == 0 || rolls.Length != maxVal + 1) return;

        // Рисуем массив выпадов
        float maxRolls = Mathf.Max(rolls);
        float heightMult = 1f / maxRolls;
        float widthMult = 1f / (maxVal - minVal);

        Gizmos.color = Color.white;
        Vector3 v0, v1 = Vector3.zero;
        for (int i = numDice; i < maxVal; i++) {
            v0 = v1;
            v1.x = ((float)i - numDice) * width * widthMult;
            v1.y = ((float)rolls[i]) * height * heightMult;
            if (i != numDice) {
                Gizmos.DrawLine(v0, v1);
            }
        }
    }

    public IEnumerator CalculateRolls() {
        yield return null;
    }
}
