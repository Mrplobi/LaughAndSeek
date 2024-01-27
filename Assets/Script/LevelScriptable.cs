using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScript", menuName = "Scriptables/Level")]
public class LevelScriptable : ScriptableObject
{
    public List<string> levelSpawnNames = new List<string>();

    public List<GameObject> allowedClowns = new List<GameObject>();
}
