using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : Character
{
    private Vector2 inputValue;
    private Vector2 mousePosition;
    [SerializeField] private int eliminationsCount;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Slider healthBar;
    private GameObject bow;
    private Camera playerCamera;

    [SerializeField] private float timeBetweenShoots = 0.5f;
    private float shootTimer;

    public int EliminationCount { get { return eliminationsCount; } set { eliminationsCount = value; } }
    private void Start()
    {
        shootTimer = timeBetweenShoots;
        bow = transform.Find("Bow").gameObject;
        playerCamera = gameObject.transform.Find("PlayerCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        healthBar.value = characterCurrentHealth;

        MoveCharacter(inputValue);
        if (inputValue == Vector2.zero)
        {
            _particleSystem.Stop();
            isMoving = false;
        } else
        {
            _particleSystem.Play();
            isMoving = true;
        }

        HandleUpdate();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        inputValue = context.ReadValue<Vector2>();
    }

    public void OnLookAt(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        Vector2 objectPosition = (Vector2) playerCamera.WorldToScreenPoint(transform.position);
        Vector2 rotation = mousePosition - objectPosition;

        bow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 45));
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
            if (shootTimer > timeBetweenShoots)
            {
                Vector2 mousePosition = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());

                Vector3 objectPosition = transform.position;
                Vector3 cameraPosition = playerCamera.transform.position;

                mousePosition = playerCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cameraPosition.z));

                Vector2 projectileDirection = mousePosition - new Vector2(objectPosition.x, objectPosition.y);
                StartCoroutine(Shoot(projectileDirection));
                shootTimer = 0f;
            }
    }

    public void Interact()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "Chest" && gameManager.IsChestOpen == false)
            {
                CharacterMaxHealth += 10;
                gameManager.IsChestOpen = true;
            }
        }
    }
}
