using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    public string name;

    // У каждого эффекта будет пометка - влияет на персонажа или на врага 
    // или на кого вообще
    public List<string> specials;   // Пока что строки
}
