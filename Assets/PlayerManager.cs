using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using TMPro;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CharacterController Controller;
    public CinemachineThirdPersonFollow cinemachine3Rd;
    public float speed;
    public GameObject GroundObj;
    public GameObject GroundContainer;
    public int Energy;
    public List<GameObject> Grounds;
    public TextMeshProUGUI EnergyText;
    public Camera camera;
    public float RotationSpeed = 500f;
    public float Jump;
    public float gravity = -9.81f;
    public float mouseSensitivity = 1;
    private Vector3 velocity;
    public CapsuleCollider PlayerHitbox;
    public Transform PlayerPosition;
    public Animator CameraAnimator;

    void Start()
    {
    }

    void Update()
    {
        EnergyText.text = Energy.ToString();
        switch (Energy)
        {
            case >= 25:
                EnergyText.color = Color.green;
                break;
            case >= 10:
                EnergyText.color = Color.yellow;
                break;
            case >= 0:
                EnergyText.color = Color.red;
                break;

        }
        for (int i = 0; i < 5; i++)
        {
            Vector3 richtung = Vector3.down;
            RaycastHit hit;

            Vector3 Pos = new Vector3((int)transform.position.x, 0, (int)transform.position.z);
            switch (i)
            {
                case 0:
                    Pos = Pos;
                    break;
                case 1:
                    Pos.x += 1;
                    break;
                case 2:
                    Pos.x -= 1;
                    break;
                case 3:
                    Pos.z += 1;
                    break;
                case 4:
                    Pos.z -= 1;
                    break;
            }
            {

            }
            bool positionTaken = Grounds.Any(ground => ground != null && ground.transform.position == Pos);
            if (Energy > 0)
            {
                if (!Physics.Raycast(Pos, richtung, out hit, 0.9f) && !positionTaken)
                {
                    GameObject GroundPrefab = Instantiate(GroundObj);
                    GroundPrefab.transform.SetParent(GroundContainer.transform);
                    Grounds.Add(GroundPrefab);
                    GroundPrefab.transform.position = Pos;
                    Energy -= 1;

                }
            }
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && Controller.isGrounded)
        {
            velocity.y += Jump;
        }

        if (Controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }


        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = Camera.main.transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 desiredMoveDirection = (forward * z + right * x).normalized;

        if (desiredMoveDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime
            );
        }
        Vector2 Scroll = Input.mouseScrollDelta;
        if (Scroll.y > 0)
        {
            cinemachine3Rd.CameraDistance += 0.25f;
            if (cinemachine3Rd.CameraDistance > 2.5f)
            {
                cinemachine3Rd.CameraDistance = 2.5f;
            }
        }
        if (Scroll.y < 0)
        {
            cinemachine3Rd.CameraDistance -= 0.25f;
            if (cinemachine3Rd.CameraDistance < -2)
            {
                cinemachine3Rd.CameraDistance = -2;
            }
        }
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            Vector3 currentEuler = transform.localEulerAngles;

            float newYRotation = currentEuler.y - mouseY;
            float newXRotation = 0;
            transform.localRotation = Quaternion.Euler(newXRotation, newYRotation, currentEuler.z);
            transform.Rotate(Vector3.up * mouseX);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        // === 3. CONTROLLER.MOVE AUSFÜHREN (NUR EINMAL) ===

        Vector3 horizontalVelocity = desiredMoveDirection * speed;

        Vector3 finalMovement = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);

        Controller.Move(finalMovement * Time.deltaTime);


    }
}
