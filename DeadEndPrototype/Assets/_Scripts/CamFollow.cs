using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    public GameObject poi;
    public Vector3 distToPoi;

    float camEasing = 0.5f;

    private void Awake() {
        poi = GameObject.Find("HeroBase");

        // Считаем разницу с инициированной камеры, 
        // чтобы она всегда оставалась на таком расстоянии
        distToPoi = poi.transform.position - Camera.main.transform.position;
    }

    private void Update() {
        Vector3 diff = (poi.transform.position - Camera.main.transform.position);
        Vector3 cameraDiffPos = diff - distToPoi;
        if (cameraDiffPos != Vector3.zero) {
            // Если объект куда то сдвинулся
            //GetComponent<Camera>().transform.position += cameraDiffPos;  // Шикарно работает
            // Смягчаем передвижение камеры
            Camera.main.transform.position = Vector3.Lerp(
                GetComponent<Camera>().transform.position,
                GetComponent<Camera>().transform.position + cameraDiffPos,
                camEasing);
        }
    }
}
