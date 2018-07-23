using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclic : MonoBehaviour {

    public float theta = 0;
    public bool showCosX = false;
    public bool showSinY = false;

    public bool ______________;

    public Vector3 pos;
    public Color[] colors;

    private void Awake() {
        // Определяем всякие цвета
        colors = new Color[] {
            new Color(1,0,0),
            new Color(1,0.5f,0),
            new Color(1,1,0),
            new Color(0.5f,1,0),
            new Color(0,1,0),
            new Color(0,1,0.5f),
            new Color(0,1,1),
            new Color(0,0.5f,1),
            new Color(0,0,1),
            new Color(0.5f,0,1),
            new Color(1,0,1),
            new Color(1,0,0.5f),
            new Color(1,0,0),
        };
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Считаем радианы от времени
        float radians = Time.time * Mathf.PI;
        // Радианы переводим в градусы
        theta = Mathf.Round(radians * Mathf.Rad2Deg) % 360;
        // Обновляем позицию
        pos = Vector3.zero;
        // Считаем х и y в зависимости от синуса и косинуса
        pos.x = Mathf.Cos(radians);
        pos.y = Mathf.Sin(radians);

        // Используем синус и косинус если они отмечены в инспекторе
        Vector3 tPos = Vector3.zero;
        if (showCosX) tPos.x = pos.x;
        if (showSinY) tPos.z = pos.y;
        // Располагаем
        transform.position = tPos;
	}

    private void OnDrawGizmos() {
        if (!Application.isPlaying) return;

        // Выбираем цвет в зависимости от того какой круг
        float cIndexFloat = (theta / 180) % 1f * (colors.Length - 1);
        int cIndex = Mathf.FloorToInt(cIndexFloat);
        float cU = cIndexFloat % 1.0f;  // Получаем цифры после запятой
        Gizmos.color = Color.Lerp(colors[cIndex], colors[cIndex + 1], cU);
        // Показываем синус и косинус используя Gizmos
        Vector3 cosPos = new Vector3(pos.x,0, -1 - (theta / 360f));
        Gizmos.DrawSphere(cosPos, 0.05f);
        if (showCosX) Gizmos.DrawLine(cosPos, transform.position);

        Vector3 sinPos = new Vector3(1f + (theta / 360), 0, pos.y);
        Gizmos.DrawSphere(sinPos, 0.05f);
        if (showSinY) Gizmos.DrawLine(sinPos, transform.position);
    }
}
