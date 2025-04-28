using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed = 2f;              // Hastighet som molnen rör sig med
    public float resetPositionX = 10f;     // X-koordinat där molnet ska teleporteras tillbaka
    public float startPositionX = -10f;    // Startpositionens X-koordinat

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Flytta molnen åt höger
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Om molnet har flyttat förbi resetPositionX
        if (transform.position.x >= resetPositionX)
        {
            // Flytta tillbaka till startpositionen
            Vector3 newPosition = transform.position;
            newPosition.x = startPositionX;
            transform.position = newPosition;
        }
    }
}
