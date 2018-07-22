using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
