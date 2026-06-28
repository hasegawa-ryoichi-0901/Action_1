using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using ZLinq;
using ZLinq.Traversables;

public static class CommonUtils {
    public static T CopyProperties<T>(object src, T dst) {
        if (src == null || dst == null) return dst;
        var srcProperties = src.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .AsValueEnumerable()
            .Where(p => p.CanRead && p.CanWrite);
        var destProperties = dst.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .AsValueEnumerable()
            .Where(p => p.CanRead && p.CanWrite);
        var properties = srcProperties.Join(destProperties, p => new { p.Name, p.PropertyType },
            p => new { p.Name, p.PropertyType }, (p1, p2) => new { p1, p2 });
        foreach (var property in properties)
            property.p2.SetValue(dst, property.p1.GetValue(src));
        return dst;
    }

    public static bool IntersectRect(Vector2 v, RectInt b) {
        return v.x < b.xMax && v.x >= b.x && v.y < b.yMax && v.y >= b.y;
    }

    public static bool IntersectRectInt(RectInt a, RectInt b) {
        return a.x < b.xMax && a.xMax > b.x && a.y < b.yMax && a.yMax > b.y;
    }

    public static bool IntersectRectInt(Vector2Int v, RectInt b) {
        return v.x < b.xMax && v.x >= b.x && v.y < b.yMax && v.y >= b.y;
    }

    public static void Destroy(this GameObject obj) {
        if (!obj) return;
        GameObject.Destroy(obj);
    }

    public static void Destroy(this IEnumerable<GameObject> obj) {
        if (obj == null) return;
        foreach (var go in obj) {
            GameObject.Destroy(go);
        }
    }

    public static void Destroy(this ValueEnumerable<Children<TransformTraverser, Transform>, Transform> obj) {
        foreach (var tr in obj) {
            GameObject.Destroy(tr.gameObject);
        }
    }

    public static void Destroy(this ValueEnumerable<Children<GameObjectTraverser, GameObject>, GameObject> obj) {
        foreach (var go in obj) {
            GameObject.Destroy(go);
        }
    }
}
