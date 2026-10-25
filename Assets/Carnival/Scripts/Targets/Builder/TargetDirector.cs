using UnityEngine;

namespace Carnival
{
    public enum TargetType { Large, Medium, Small }

    // Director: premade recipes reuse the same builder steps.
    public sealed class TargetDirector
    {
        private readonly ITargetBuilder builder;
        public TargetDirector(ITargetBuilder targetBuilder) => builder = targetBuilder;

        public Target Create(TargetType type, Vector3 position, Transform parent,
            float boundary, int direction)
        {
            builder.Reset();
            switch (type)
            {
                case TargetType.Large:
                    builder.SetSpeed(1.5f);
                    builder.SetSize(1.2f);
                    builder.SetPoints(10);
                    builder.SetColor(new Color(0.3f, 1f, 0.5f));
                    break;
                case TargetType.Medium:
                    builder.SetSpeed(2.5f);
                    builder.SetSize(0.85f);
                    builder.SetPoints(20);
                    builder.SetColor(new Color(1f, 0.8f, 0.2f));
                    break;
                case TargetType.Small:
                    builder.SetSpeed(3.5f);
                    builder.SetSize(0.55f);
                    builder.SetPoints(30);
                    builder.SetColor(new Color(1f, 0.35f, 0.4f));
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(type));
            }
            Target result = builder.Build(position, parent, boundary, direction);
            result.name = type + " Target";
            return result;
        }
    }
}
