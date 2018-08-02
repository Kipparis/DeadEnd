using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEnemy : MonoBehaviour {
    // Если какое то другое место указанно, плавно перемещаемся
    // Если ничего, то как бы исчезаем

    // Задаём здесь пои, а эта фигнюшка сама находит её и крутиться на месте
    public static SelectEnemy S;

    public float rotateMult = 10f;

    public float timeBetweenUpdates = 0.01f;
    public bool isUpdatingPosition = false; // Изначально

    // Сделать так чтоб это свойство обращалась к Hero.S так как там происходит неправильный цикл
    // Соответственно Hero.S. обратно обращается это само собой
    public GameObject poi {
        get { return (Hero.S.poi); }
        set {
            Hero.S.poi = value;   // Приравниваем новый ио, оно меняет свою позицию
            if (Hero.S.poi == null) {
                isUpdatingPosition = false;
                this.transform.position = new Vector3(-100, -100, 0);
                return;
            }
            this.transform.position = Camera.main.WorldToScreenPoint(Hero.S.poi.transform.position);
            isUpdatingPosition = true;
        }
    }

    private void Awake() {
        S = this;
    }

    private void Start() {
        // Начинаем цикл проверки
        StartCoroutine(UpdatePosition());
    }

    private void Update() {
        this.transform.Rotate(Vector3.back * Time.deltaTime * rotateMult);
    }

    // Смотрим есть ли он на экране, если да то обновляем позицию указателя, каждые 10-100 мс
    IEnumerator UpdatePosition() {
        while (true) {
            // Если мы не обновляем позицию, ничего не делаем
            if (!isUpdatingPosition) {
                yield return new WaitForSeconds(timeBetweenUpdates);
                continue;   // Переходим к следующему циклу
            }
            // Если пои = нулю, тогда и обновлять нечего
            if (Hero.S.poi == null) {
                yield return new WaitForSeconds(timeBetweenUpdates);
                continue;   // Переходим к следующему циклу
            }

            // Если предмет вышел за экран, обнуляем пои, возвращаем
            Vector3 poiPos = Camera.main.WorldToScreenPoint(Hero.S.poi.transform.position);

            // Если хоть одно координата вышла за пределы, убираем нахуй
            if (poiPos.x > Screen.width || poiPos.x < 0 || poiPos.y > Screen.height || poiPos.y < 0){
                // Обозначил пределы экрана и поставил отрицание
                poi = null;
                yield return new WaitForSeconds(timeBetweenUpdates);
                continue;
            }

            // Все условия пройденны, можно обновлять положение
            this.transform.position = Camera.main.WorldToScreenPoint(Hero.S.poi.transform.position);
            //  ( копируем строчку из свойства )

            yield return new WaitForSeconds(timeBetweenUpdates);
        }
    }
}
