using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;            // Player ka Transform
    public float smoothSpeed = 5f;       // Camera ka smooth movement ka speed
    public Vector3 offset = new Vector3(0f, 5f, -5f);  // Offset between player and camera

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned to the TopDownFollowCamera script.");
            return;
        }

        // Calculate the desired position of the camera based on the player's position and offset
        Vector3 desiredPosition = player.position + offset;

        // Use Mathf.Lerp to smoothly interpolate between the current position and the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Look at the player
        //transform.LookAt(player.position);
    }
}
