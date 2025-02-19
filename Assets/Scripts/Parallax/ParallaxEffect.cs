using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxSpeed;
    private float xPosition;

    private void Start()
    {
        cam = Camera.main.GameObject();
        xPosition = transform.position.x;
    }

    private void Update()
    {
        float distanceToMove = parallaxSpeed * cam.transform.position.x;
        transform.position = new(xPosition + distanceToMove, transform.position.y);
    }
}
