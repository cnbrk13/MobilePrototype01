using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    [Tooltip("A multiplier to the input. Describes the maximum speed in degrees / second. To flip vertical rotation, set Y to a negative value")]
    [SerializeField] private Vector2 sensitivity;

    [Tooltip("The maximum angle from the horizon the player can rotate, in degrees")]
    [SerializeField] private float maxVerticalAngleFromHorizon, maxHorizontalAngleFromHorizon;

    [Tooltip("If set to true, works only when mouse down (or with touch)")]
    [SerializeField] private bool turnWhenMouseDown = false;

    private Vector2 rotation; // The current rotation, in degrees
    private bool disabled = false;

    private float ClampVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxVerticalAngleFromHorizon, maxVerticalAngleFromHorizon);
    }

    private float ClampHorizontalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxHorizontalAngleFromHorizon, maxHorizontalAngleFromHorizon);
    }

    private Vector2 GetInput()
    {
        // Get the input vector. This can be changed to work with the new input system or even touch controls
        Vector2 input = new(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        return input;
    }

    private void FixedUpdate()
    {
        if (disabled)
        {
            return;
        }

        if (turnWhenMouseDown && !Input.GetKey(KeyCode.Mouse0))
        {
            return;
        }

        // The wanted velocity is the current input scaled by the sensitivity
        // This is also the maximum velocity
        Vector2 wantedVelocity = GetInput() * sensitivity;


        rotation += wantedVelocity * Time.deltaTime;
        rotation.y = ClampVerticalAngle(rotation.y);
        rotation.x = ClampHorizontalAngle(rotation.x);

        // Convert the rotation to euler angles
        transform.localEulerAngles = new Vector3(rotation.y, rotation.x, 0);
    }


    private void Start()
    {
#if (UNITY_EDITOR)
        sensitivity = new Vector2(250, -250);
#endif
    }

}
