using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] int heartValue = 1;
    [SerializeField] AudioClip heartPickpSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(heartPickpSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddtoLives(heartValue);
        Destroy(gameObject);
    }
}
