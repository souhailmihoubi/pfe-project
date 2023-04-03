using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float arrowSpeed = 5f; // adjust speed as needed

    private void Update()
    {
        // move the arrow forward in the direction the character is facing
        transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime);
    }
}
