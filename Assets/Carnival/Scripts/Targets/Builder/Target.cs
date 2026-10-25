using System;
using UnityEngine;

namespace Carnival
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public sealed class Target : MonoBehaviour
    {
        // Observer subject: targets announce hits without knowing the score manager.
        public static event Action<int> Hit;
        private float speed;
        private int points;
        private float horizontalLimit;
        private int direction;
        private bool configured;
        private bool wasHit;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetEvents() => Hit = null;

        public void Configure(float moveSpeed, float size, int pointValue, Color color,
            float boundary, int startingDirection)
        {
            speed = moveSpeed;
            points = pointValue;
            horizontalLimit = boundary;
            direction = startingDirection < 0 ? -1 : 1;
            transform.localScale = Vector3.one * size;
            GetComponent<SpriteRenderer>().color = color;
            wasHit = false;
            configured = true;
        }

        private void Update()
        {
            if (!configured || wasHit) return;
            Vector3 position = transform.position;
            position.x += speed * direction * Time.deltaTime;
            if (position.x >= horizontalLimit)
            {
                position.x = horizontalLimit;
                direction = -1;
            }
            else if (position.x <= -horizontalLimit)
            {
                position.x = -horizontalLimit;
                direction = 1;
            }
            transform.position = position;
        }

        public bool TryHit()
        {
            if (!configured || wasHit) return false;
            wasHit = true; // Prevent two bullets from scoring the same target.
            GetComponent<Collider2D>().enabled = false;
            Hit?.Invoke(points);
            Destroy(gameObject); // Only bullets are required to be pooled.
            return true;
        }
    }
}
