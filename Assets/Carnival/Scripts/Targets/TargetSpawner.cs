using UnityEngine;

namespace Carnival
{
    public sealed class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private Target targetPrefab;
        [SerializeField, Min(0.1f)] private float respawnDelay = 1f;
        [SerializeField, Min(0.1f)] private float horizontalLimit = 7.5f;
        [SerializeField] private float[] laneHeights = { 0.5f, 2f, 3.5f };
        private Target[] targets;
        private float[] respawnAt;
        private TargetDirector director;

        private void Start()
        {
            if (targetPrefab == null || laneHeights == null || laneHeights.Length == 0)
            {
                Debug.LogError("TargetSpawner needs a Target prefab and at least one lane.", this);
                enabled = false;
                return;
            }
            director = new TargetDirector(new TargetBuilder(targetPrefab));
            targets = new Target[laneHeights.Length];
            respawnAt = new float[laneHeights.Length];
            for (int i = 0; i < targets.Length; i++) Spawn(i);
        }

        private void Update()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] != null) continue;
                if (respawnAt[i] < 0f) respawnAt[i] = Time.time + respawnDelay;
                if (Time.time >= respawnAt[i]) Spawn(i);
            }
        }

        private void Spawn(int lane)
        {
            Vector3 position = new Vector3(Random.Range(-6f, 6f), laneHeights[lane], 0f);
            targets[lane] = director.Create((TargetType)(lane % 3), position, transform,
                horizontalLimit, lane % 2 == 0 ? 1 : -1);
            respawnAt[lane] = -1f;
        }
    }
}
