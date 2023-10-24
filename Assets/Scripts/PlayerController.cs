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
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Slider healthBar;
    private GameObject bow;

    [SerializeField] private float timeBetweenShoots = 0.5f;
    private float shootTimer;

    private void Start()
    {
        shootTimer = timeBetweenShoots;
        bow = transform.Find("Bow").gameObject;
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        healthBar.value = characterHealth;

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
        Vector2 objectPosition = (Vector2) Camera.main.WorldToScreenPoint(transform.position);
        Vector2 rotation = mousePosition - objectPosition;

        bow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 45));
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
            if (shootTimer > timeBetweenShoots)
            {
                Vector2 projectileDirection = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f) - transform.position);
                StartCoroutine(Shoot(projectileDirection));
                shootTimer = 0f;
            }
    }
}
