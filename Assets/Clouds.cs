using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed = 2f;              // Hastighet som molnen r�r sig med
    public float resetPositionX = 10f;     // X-koordinat d�r molnet ska teleporteras tillbaka
    public float startPositionX = -10f;    // Startpositionens X-koordinat

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Flytta molnen �t h�ger
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Om molnet har flyttat f�rbi resetPositionX
        if (transform.position.x >= resetPositionX)
        {
            // Flytta tillbaka till startpositionen
            Vector3 newPosition = transform.position;
            newPosition.x = startPositionX;
            transform.position = newPosition;
        }
    }
}
