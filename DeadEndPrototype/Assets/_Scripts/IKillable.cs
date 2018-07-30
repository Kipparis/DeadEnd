using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable {
    float health { get; set; }
    Dictionary<string, float> resist { get; set; }

    HealthBar healthBar { get; set; }

    void InitHealth();  // Задаёт где хп будут расположенны
}
