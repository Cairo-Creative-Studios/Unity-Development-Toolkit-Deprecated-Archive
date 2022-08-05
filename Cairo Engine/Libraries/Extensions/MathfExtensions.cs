//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;

public static class MathfExtensions
{
    public static Vector3 Lerp(this Vector3 a, Vector3 b, float alpha)
    {
        return new Vector3(Mathf.Lerp(a.x, b.x, alpha), Mathf.Lerp(a.y, b.y, alpha), Mathf.Lerp(a.z, b.z, alpha));
    }

    public static Vector3 LerpAngle(this Vector3 a, Vector3 b, float alpha)
    {
        return new Vector3(Mathf.LerpAngle(a.x, b.x, alpha), Mathf.LerpAngle(a.y, b.y, alpha), Mathf.LerpAngle(a.z, b.z, alpha));
    }

    public static Vector2 Lerp(this Vector2 a, Vector2 b, float alpha)
    {
        return new Vector2(Mathf.Lerp(a.x, b.x, alpha), Mathf.Lerp(a.y, b.y, alpha));
    }
}
