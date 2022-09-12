using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickup : MonoBehaviour
{
    [SerializeField] int diamondValue = 100;
    [SerializeField] AudioClip diamondPickpSFX;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(diamondPickpSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddtoScore(diamondValue);
        Destroy(gameObject);
    }
}
