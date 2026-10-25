using UnityEngine;

namespace Carnival
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float speed = 12f;
        [SerializeField, Min(0.1f)] private float lifetime = 2f;
        [SerializeField] private float topBoundary = 5.5f;
        private Rigidbody2D body;
        private float age;
        public BulletPool Owner { get; private set; }
        public bool IsInUse { get; private set; }

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            body.gravityScale = 0f;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<Collider2D>().isTrigger = true;
        }

        public void Initialize(BulletPool owner) => Owner = owner;

        public void Launch(Vector3 position)
        {
            age = 0f;
            IsInUse = true;
            transform.position = position;
            gameObject.SetActive(true);
            body.position = position;
        }

        private void FixedUpdate()
        {
            if (!IsInUse) return;
            age += Time.fixedDeltaTime;
            if (age >= lifetime || body.position.y >= topBoundary)
            {
                Owner.Return(this);
                return;
            }
            body.MovePosition(body.position + Vector2.up * speed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsInUse) return;
            Target target = other.GetComponentInParent<Target>();
            if (target != null && target.TryHit()) Owner.Return(this);
        }

        public void Deactivate()
        {
            IsInUse = false;
            gameObject.SetActive(false);
        }
    }
}
