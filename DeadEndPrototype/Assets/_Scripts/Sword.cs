using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Позиции в котором находится оружие (в последующем перейдёт в такой же список
// только для стоки героя )
public enum WeaponState {
    waiting,
    idle,
    attack,
    block
}

[System.Serializable]
public class DamageDef {
    public float damage;
    public string type;

    public float attackStartTime;   // Время которое проходит "для замаха"
    public float attackDuration;    // Как долго проходит атака
}

public class Sword : MonoBehaviour {
    public Hero owner;

    // TODO: Сделать скалируемую настройку позиции для оружия
    // т.е. в зависимости от размеров модельки мы изменяем чёто там

    // Определяет позицию оружия
    [SerializeField]
    private WeaponState _state = WeaponState.idle;
    public WeaponState state {
        get { return (_state); }
        set {
            switch (value) {    // Переносим всё в функции, т.к. некоторые оружия
                case WeaponState.waiting:   // могут не иметь возможности атаковать или блокировать
                    break;
                case WeaponState.idle:
                    Idle();
                    break;
                case WeaponState.attack:
                    StartCoroutine(Attack());
                    break;
                case WeaponState.block:
                    Block();
                    break;
                default:
                    break;
            }
        }
    }

    public DamageDef dd = new DamageDef() {
        damage = 100,
        type = "Fire"
    };

    // У каждого эффекта будет пометка - влияет на персонажа или на врага 
    // или на кого вообще
    public List<string> specials;   // Пока что строки

    public GameObject lastGO;

    private void Awake() {
        GameObject go = Utils.FindTaggedParent(this.gameObject);
        if (go != null && go.tag == "Hero") {
            owner = go.GetComponent<Hero>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (owner != null) {
            owner.DamageSomething();
        }
    }

    //------ Следующие методы могут настраиваться для конкретного оружие ( типо
    // вызывание различных эффектов )

    void Block() {
        _state = WeaponState.block;
        transform.localPosition = new Vector3(-0.6f, 0.25f, 0.6f);
        transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    IEnumerator Attack() {
        // Зарержка перед атакой (типо замах)
        yield return new WaitForSeconds(dd.attackStartTime);
        _state = WeaponState.attack;
        transform.localPosition = new Vector3(0, 0.25f, 1);
        transform.localRotation = Quaternion.Euler(0, -90, 0);
        
        // Возвращаем через некоторое время в обратное положение
        yield return new WaitForSeconds(dd.attackDuration);
        state = WeaponState.idle;
    }

    void Idle() {
        _state = WeaponState.idle;
        //transform.localPosition = new Vector3(0.87f, 0.3f, 0);
        transform.localPosition = new Vector3(0.7f, 0.2f, 0);
        transform.localRotation = Quaternion.Euler(0, 90, -48);
    }
}
