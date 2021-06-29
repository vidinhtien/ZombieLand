using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    Image healthSprite;
    [SerializeField]
    GameObject healthBar;
    [SerializeField]
    int _maxHealthPoint;
    public int MaxHealthPoint { get { return _maxHealthPoint; }set { _maxHealthPoint = value; } }
    [SerializeField]
    int _currentHealthPoint;
    public int CurrentHealthPoint { get { return _currentHealthPoint; } }
    public bool IsDead()
    {
        return _currentHealthPoint <= 0;
    }
    public void Damage(int damage)
    {
        _currentHealthPoint = _currentHealthPoint > damage ? _currentHealthPoint - damage : 0;
        ShowHealth();
    }
    public void Setup()
    {
        healthBar.gameObject.SetActive(true);
        _currentHealthPoint = _maxHealthPoint;
        ShowHealth();
    }
    float point;
    Vector3 pos;
    public void ShowHealth()
    {
        healthBar.gameObject.SetActive(true);
        point = (float) _currentHealthPoint / _maxHealthPoint;
        healthSprite.fillAmount = point;
        if (IsDead())
        {
            Invoke("HideHealth", 1f);
        }
    }
    public void HideHealth()
    {
        healthBar.gameObject.SetActive(false);
    }
    public void Heal(int hp)
    {
        _currentHealthPoint = Mathf.Min(_currentHealthPoint + hp, _maxHealthPoint);
        ShowHealth(); 
    }

}
