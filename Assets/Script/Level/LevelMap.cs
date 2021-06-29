using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LevelMap 
{
    void OnStartLevel();
    void OnFinishLevel();
    void OnQuitLevel();
    void OnKillZombie();
    void OnKillZombie(Vector3 pos);
    Vector3 GetClosetZombiePosition(Vector3 playerPos);
}
