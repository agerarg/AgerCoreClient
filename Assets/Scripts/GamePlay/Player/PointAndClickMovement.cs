using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClickMovement : MonoBehaviour
{
    public static PointAndClickMovement instance;

    public LayerMask groundLayerMask;
    public float speed = 5f;

    private Vector3 targetPosition;
    private Vector3 serverPositionUpdate;

    private bool isMovementDisable=false;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        NetworkEvents.PlayerMovement += OnPlayerMovement;
        NetworkEvents.PortalOpen += OnPortalOpen;
        NetworkEvents.NewMapLoaded += OnNewMapLoaded;
    }

    private void OnDisable()
    {
        NetworkEvents.PlayerMovement -= OnPlayerMovement;
        NetworkEvents.PortalOpen -= OnPortalOpen;
        NetworkEvents.NewMapLoaded -= OnNewMapLoaded;
    }

    private void OnNewMapLoaded()
    {
        isMovementDisable = false;
    }

    private void OnPortalOpen(int arg1, Vector3 targetPosition)
    {
        isMovementDisable = true;
        string X = targetPosition.x.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        string Y = targetPosition.z.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        NetworkClient.instance.SendDataToServer("CallMovement", new string[,] { { "X", X }, { "Y", Y } });
    }

    private void SetPosition()
    {
        transform.position = new Vector3(CharacterData.instance.basicData.mapPositionX, 0.5f, CharacterData.instance.basicData.mapPositionY);
    }
    private void Start()
    {
        SetPosition();
    }
    public void Teleport(Vector3 pos)
    {
        CharacterData.instance.basicData.mapPositionX = pos.x;
        CharacterData.instance.basicData.mapPositionY = pos.z;
        serverPositionUpdate = new Vector3(pos.x, 0.5f, pos.z);
        SetPosition();
    }
    private void OnPlayerMovement(int arg1, float arg2, float arg3)
    {
       if(arg1 == CharacterData.instance.basicData.id)
        {
            serverPositionUpdate = new Vector3(arg2, 0.5f, arg3);
        }
    }

    private void Update()
    {
        if (isMovementDisable)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }

        if (IsMoving())
        {
            Move();
        }
    }

    private void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Keep the same z-coordinate as the character

            string X = targetPosition.x.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            string Y = targetPosition.z.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

            NetworkClient.instance.SendDataToServer("CallMovement", new string[,] { { "X", X }, { "Y", Y } });

        }
    }

    private bool IsMoving()
    {
        return (transform.position - serverPositionUpdate).sqrMagnitude > 0.01f;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, serverPositionUpdate, speed * Time.deltaTime);
    }
}
