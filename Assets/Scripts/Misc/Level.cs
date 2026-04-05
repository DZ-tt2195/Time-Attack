using UnityEngine;
using System.Collections.Generic;
public enum LevelType {Fixed, Endless, Shuffled}
[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    public List<Wave<Collection>> listOfWaves = new();
    public string levelName = nameof(AutoTranslate.Blank);
    public LevelType levelType;
}

[System.Serializable]
public class Wave<Collection>
{
    public List<Collection> enemies = new();
    public string tutorialKey = nameof(AutoTranslate.Blank);
}

[System.Serializable]
public class Collection
{
    public BaseEnemy toCreate;
    public Vector2 position;
}
