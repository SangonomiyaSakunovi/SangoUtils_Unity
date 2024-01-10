using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public struct TransformData
{
    public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }

    public override readonly bool Equals([NotNullWhen(true)] object obj) => obj is TransformData otherTrans && this.Equals(otherTrans);

    public readonly bool Equals(TransformData otherTrans) => Position.Equals(otherTrans.Position) && Rotation.Equals(otherTrans.Rotation) && Scale.Equals(otherTrans.Scale);

    public override readonly int GetHashCode() => (Position, Rotation, Scale).GetHashCode();

    public static bool operator ==(TransformData left, TransformData right) => left.Equals(right);

    public static bool operator !=(TransformData left, TransformData right) => !(left == right);
}
