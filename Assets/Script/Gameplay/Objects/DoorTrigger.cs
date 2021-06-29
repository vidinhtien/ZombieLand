using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    Door door;
    [SerializeField]
    List<int> listHashCode = new List<int>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("zombie"))
        {
            if (!listHashCode.Contains(other.GetHashCode()))
            {
                if (listHashCode.Count == 0)
                {
                    door?.OpenDoor();
                }
                listHashCode.Add(other.GetHashCode());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("zombie"))
        {
            listHashCode.Remove(other.GetHashCode());
            if (listHashCode.Count == 0)
            {
                door?.Close();
            }
        }
    }
}
