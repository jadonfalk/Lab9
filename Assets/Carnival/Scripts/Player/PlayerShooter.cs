using UnityEngine;
using UnityEngine.InputSystem;

namespace Carnival
{
    public sealed class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private BulletPool bulletPool;
        [SerializeField] private Transform firePoint;
        [SerializeField, Min(0f)] private float moveSpeed = 6f;
        [SerializeField, Min(0.02f)] private float fireInterval = 0.2f;
        [SerializeField, Min(0f)] private float horizontalLimit = 8f;
        private InputAction moveAction;
        private InputAction fireAction;
        private float nextFireTime;

        private void Awake()
        {
            // These are NEW Input System actions, created without an asset.
            moveAction = new InputAction("Move", InputActionType.Value);
            moveAction.AddCompositeBinding("1DAxis")
                .With("Negative", "<Keyboard>/a")
                .With("Positive", "<Keyboard>/d");
            moveAction.AddCompositeBinding("1DAxis")
                .With("Negative", "<Keyboard>/leftArrow")
                .With("Positive", "<Keyboard>/rightArrow");
            moveAction.AddBinding("<Gamepad>/leftStick/x");
            fireAction = new InputAction("Fire", InputActionType.Button);
            fireAction.AddBinding("<Keyboard>/space");
            fireAction.AddBinding("<Mouse>/leftButton");
            fireAction.AddBinding("<Gamepad>/buttonSouth");
        }

        private void OnEnable()
        {
            moveAction.Enable();
            fireAction.Enable();
        }

        private void Start()
        {
            if (bulletPool != null && firePoint != null) return;
            Debug.LogError("PlayerShooter needs Bullet Pool and Fire Point assigned.", this);
            enabled = false;
        }

        private void Update()
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x + moveAction.ReadValue<float>() *
                moveSpeed * Time.deltaTime, -horizontalLimit, horizontalLimit);
            transform.position = position;

            if (fireAction.IsPressed() && Time.time >= nextFireTime)
            {
                bulletPool.TryFire(firePoint.position);
                nextFireTime = Time.time + fireInterval;
            }
        }

        private void OnDisable()
        {
            moveAction?.Disable();
            fireAction?.Disable();
        }

        private void OnDestroy()
        {
            moveAction?.Dispose();
            fireAction?.Dispose();
        }
    }
}
