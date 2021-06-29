using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public static MaterialController Instance;
    [SerializeField]
    Material tuongTrongSuot;
    [SerializeField]
    Material tuongDefault;
    [SerializeField]
    List<MeshRenderer> listWallInFrontOfPlayer;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void AddWall(MeshRenderer wall)
    {
        if (listWallInFrontOfPlayer.Contains(wall)) return;
        wall.material = tuongTrongSuot;
        listWallInFrontOfPlayer.Add(wall);
    }
    public void RemoveWalls()
    {
        for (int i = listWallInFrontOfPlayer.Count - 1; i >= 0; i--)
        {
            if (listWallInFrontOfPlayer[i] != null)
            {
                listWallInFrontOfPlayer[i].material = tuongDefault;
            }
            listWallInFrontOfPlayer.RemoveAt(i);
        }
    }
}
