using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    const string IDCHACRACTER = "CHARACTER_ID";

    [SerializeField]
    List<BaseCharacter> listCharacter;
    [SerializeField]
    List<Animator> listCharacterHome;
    [SerializeField]
    int _currentCharacterId;
    public int currentCharacterId { get { return _currentCharacterId; } private set { _currentCharacterId = value; } }

    private void Start()
    {
        if (FirstOpenController.instance.IsOpenFirst)
        {
            _currentCharacterId = 0;
        }
    }

    public static void FirstInit()
    {
        PlayerPrefs.SetInt(IDCHACRACTER, 0);
    }
    public BaseCharacter GetCharacter(int id)
    {
        if (id < 0 || id >= listCharacter.Count) return null;
        return listCharacter[id];
    }
    public void SelectCharacter(int id)
    {
        if (id < 0 || id >= listCharacter.Count) return;
        PlayerPrefs.SetInt(IDCHACRACTER, id);
        for (int i = 0; i < listCharacter.Count; i++)
        {
            listCharacter[i].gameObject.SetActive(false);
        }
        listCharacter[id].gameObject.SetActive(true);
    }
}
