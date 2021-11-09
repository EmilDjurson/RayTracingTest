using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject SettingsGroup;
    private Rigidbody rb;

    public bool moveCamera;
    public Transform cameraTransform;

    public float movementSpeed = 100;
    public float sensitivity = 10;

    private float pitch;
    private float yaw;

    public float moveForward;
    public float moveSide;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(SettingsGroup.activeInHierarchy == false)
        {
            this.transform.position = new Vector3(this.transform.position.x, Mathf.Clamp(this.transform.position.y, 1, float.MaxValue), this.transform.position.z);

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                moveCamera = !moveCamera;
            }

            if (moveCamera == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                moveForward = Input.GetAxis("Vertical") * movementSpeed;
                moveSide = Input.GetAxis("Horizontal") * movementSpeed * 50;

                yaw += sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime;
                pitch -= sensitivity * Input.GetAxis("Mouse Y") * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -90, 90);

                transform.eulerAngles = new Vector3(pitch, yaw, 0);
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                moveForward = Mathf.MoveTowards(moveForward, 0, 10000 * Time.deltaTime);
                moveSide = Mathf.MoveTowards(moveSide, 0, 10000 * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = (transform.forward * moveForward) + (transform.right * moveSide) * Time.deltaTime;
    }
}
