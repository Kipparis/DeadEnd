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
    public int damage;
    public string type;
}

public class Sword : MonoBehaviour {
    public Hero owner;

    // Определяет позицию оружия
    private WeaponState _state = WeaponState.idle;
    public WeaponState state {
        get { return (_state); }
        set {
            switch (value) {
                case WeaponState.waiting:
                    break;
                case WeaponState.idle:
                    transform.localPosition = new Vector3(0.87f, 0.3f, 0);
                    transform.localRotation = Quaternion.Euler(0, 90, -48);
                    break;
                case WeaponState.attack:
                    transform.localPosition = new Vector3(0, 0.25f, 1);
                    transform.localRotation = Quaternion.Euler(0, -90, 0);
                    StartCoroutine(Attack());
                    break;
                case WeaponState.block:
                    transform.localPosition = new Vector3(-0.6f, 0.25f, 0.8f);
                    transform.localRotation = Quaternion.Euler(90, 0, 0);
                    Block();
                    break;
                default:
                    break;
            }
        }
    }

    public DamageDef dd = new DamageDef() {
        damage = 1,
        type = "Fire"
    };

    public string name;

    // У каждого эффекта будет пометка - влияет на персонажа или на врага 
    // или на кого вообще
    public List<string> specials;   // Пока что строки

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

    }

    IEnumerator Attack() {
        yield return new WaitForSeconds(0.4f);
        state = WeaponState.idle;
    }
}
