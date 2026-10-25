using System.Collections.Generic;
using UnityEngine;

namespace Carnival
{
    // Object Pool: all bullets are created once, then borrowed and returned.
    public sealed class BulletPool : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField, Min(1)] private int poolSize = 20;
        private readonly Queue<Bullet> available = new Queue<Bullet>();
        public int AvailableCount => available.Count;

        private void Awake()
        {
            if (bulletPrefab == null)
            {
                Debug.LogError("BulletPool needs a Bullet prefab.", this);
                enabled = false;
                return;
            }

            for (int i = 0; i < Mathf.Max(1, poolSize); i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, transform);
                bullet.name = "Pooled Bullet " + (i + 1);
                bullet.Initialize(this);
                bullet.gameObject.SetActive(false);
                available.Enqueue(bullet);
            }
        }

        public bool TryFire(Vector3 position)
        {
            if (available.Count == 0) return false;
            Bullet bullet = available.Dequeue();
            bullet.Launch(position);
            return true;
        }

        public void Return(Bullet bullet)
        {
            // Reject duplicate returns and bullets from another pool.
            if (bullet == null || bullet.Owner != this || !bullet.IsInUse) return;
            bullet.Deactivate();
            available.Enqueue(bullet);
        }
    }
}
