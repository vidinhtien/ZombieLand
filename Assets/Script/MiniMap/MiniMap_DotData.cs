using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MiniMap Dot Data", menuName = "Manager/MiniMap DotData")]
[System.Serializable]
public class MiniMap_DotData : ScriptableObject
{
    public string tag_Name;
    public Sprite icon;
    public Color color = Color.white;
    public Vector2 default_Size = Vector2.one;
}
