using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable {
    int health { get; set; }
    Dictionary<string, float> resist { get; set; }

    void ShowHealth();
}
