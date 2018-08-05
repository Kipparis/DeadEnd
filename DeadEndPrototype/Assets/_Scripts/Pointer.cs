using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PointerState {
    ready,
    unready
}

// TODO: Написать функцию, которая сама складывает все rect'ы и задаёт свой собственный

public class Pointer : MonoBehaviour {

    public PointerState state = PointerState.unready;   // Когда всё перемещение законченно
    // Делаем его рейди, и там уже отображаем все значения

    public GameObject poi;  // Предмент, на который мы указываем

    Text _name;
    Text _damage;

    new public string name {
        get { return (_name.text); }
        set { _name.text = value; }
    }

    public string damage {  // Можно тут же задавать цвет
        get { return (_damage.text); }
        set { _damage.text = value; }
    }

	// Use this for initialization
	void Awake () {
        _name = transform.Find("Name").GetComponent<Text>();
        _damage = transform.Find("Damage").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        // Обновляем позицию
        transform.position = Camera.main.WorldToScreenPoint(poi.transform.position +
            Vector3.up);
	}
}
