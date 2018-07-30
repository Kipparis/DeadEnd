using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Ввести систему отображения урона
// для начала просто отплывающие значения, а уже потом стакающиеся

public class DeadEnd : MonoBehaviour {

    public static DeadEnd S;
    public static bool DEBUG = true;

    private void Awake() {
        S = this;
    }

    public void HeroCollWithGround() {
        if (DEBUG) print("HeroCollWithGround");
    }
}
