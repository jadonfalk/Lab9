using UnityEngine;

namespace Carnival
{
    // Builder contract: construction is split into explicit steps.
    public interface ITargetBuilder
    {
        void Reset();
        void SetSpeed(float speed);
        void SetSize(float size);
        void SetPoints(int points);
        void SetColor(Color color);
        Target Build(Vector3 position, Transform parent, float boundary, int direction);
    }
}
