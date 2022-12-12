using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New DBCharacter", menuName = "Database Character")]
public class DBCharacter : ScriptableObject
{
    public character[] characters;

    public int CountCharacter() => characters.Length;

    public character GetCharacter(int idx) => characters[idx]; 
}
