using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// View を保持する SAM コンテキストが公開すべき情報。
/// </summary>
/// <remarks>
/// 使用例: <c>var prefab = context.ViewPrefab;</c>
/// </remarks>
public interface ISamViewContext<TView> : ISamContext where TView : MonoBehaviour {
    /// <summary>
    /// Resources からロードする際のプレハブパス。
    /// 使用例: <c>Debug.Log(context.ViewPrefabPath);</c>
    /// </summary>
    public abstract string ViewPrefabPath { get; }
    /// <summary>
    /// ロード済みプレハブへの参照。
    /// 使用例: <c>var prefab = context.ViewPrefab;</c>
    /// </summary>
    public abstract GameObject ViewPrefab { get; }
    /// <summary>
    /// 実際にシーン上へ生成された View オブジェクト。
    /// 使用例: <c>context.ViewObject.SetActive(false);</c>
    /// </summary>
    public abstract GameObject ViewObject { get; }
    /// <summary>
    /// 型安全にアクセス可能な View インスタンス。
    /// 使用例: <c>var view = context.View;</c>
    /// </summary>
    public abstract TView View { get; }
}
