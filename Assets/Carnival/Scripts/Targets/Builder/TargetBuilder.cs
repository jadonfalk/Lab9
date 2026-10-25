using System;
using UnityEngine;

namespace Carnival
{
    // Plain C# builder, not a component. Accumulates settings before building.
    public sealed class TargetBuilder : ITargetBuilder
    {
        private readonly Target prefab;
        private float speed;
        private float size;
        private int points;
        private Color color;

        public TargetBuilder(Target targetPrefab)
        {
            if (targetPrefab == null) throw new ArgumentNullException(nameof(targetPrefab));
            prefab = targetPrefab;
            Reset();
        }

        public void Reset()
        {
            speed = 1f;
            size = 1f;
            points = 10;
            color = Color.white;
        }

        public void SetSpeed(float value) => speed = Mathf.Max(0f, value);
        public void SetSize(float value) => size = Mathf.Max(0.1f, value);
        public void SetPoints(int value) => points = Mathf.Max(0, value);
        public void SetColor(Color value) => color = value;

        public Target Build(Vector3 position, Transform parent, float boundary, int direction)
        {
            Target target = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, parent);
            target.Configure(speed, size, points, color, Mathf.Max(0.1f, boundary), direction);
            Reset();
            return target;
        }
    }
}
