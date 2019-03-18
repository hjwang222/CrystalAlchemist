using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Kamera Variablen")]
    public Transform target; //Player
    public float smothing;
    private Vector2 maxPosition;
    private Vector2 minPosition;
    private Camera cam;
    private float cameraHeight;
    private float cameraWidth;


    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        cameraHeight = 2f * cam.orthographicSize / 2;
        cameraWidth = cam.orthographicSize * cam.aspect;

        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    public void updateCameraBounds(Transform room)
    {        
        Vector2 pos = room.localPosition;
        Vector2 scale = room.localScale;
        Vector2 min = new Vector2(pos.x - (scale.x / 2), pos.y - (scale.y / 2));
        Vector2 max = new Vector2(pos.x + (scale.x / 2), pos.y + (scale.y / 2));
        this.minPosition = min;
        this.maxPosition = max;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(target.position.x, minPosition.x + cameraWidth, maxPosition.x - cameraWidth);
            targetPosition.y = Mathf.Clamp(target.position.y, minPosition.y + cameraHeight, maxPosition.y - cameraHeight);

            //Bewegt die Kamera zwischen Spieler und Kamera mit dem Multiplier smothing
            transform.position = Vector3.Lerp(transform.position, targetPosition, smothing);
        }
    }
}
