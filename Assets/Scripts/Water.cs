using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.PlayerEnteredWater(transform);
        }
    }
}
