using AsyncFSM;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// View を伴う SAM コンテキストが提供すべき操作群。
/// </summary>
/// <remarks>
/// 使用例: <c>await action.PrepareViewAsync();</c>
/// </remarks>
public interface ISamViewAction : ISamAction {
    /// <summary>
    /// 現在バインドされている View をダウンキャストして取得する。
    /// 使用例: <c>var view = action.GetView<MyView>();</c>
    /// </summary>
    TView GetView<TView>() where TView : MonoBehaviour;

    /// <summary>
    /// View プレハブのロードや差し替え前後処理を行う。
    /// 使用例: <c>await action.PrepareViewAsync();</c>
    /// </summary>
    UniTask PrepareViewAsync();

    /// <summary>
    /// View プレハブを非同期ロードする。
    /// 使用例: <c>var prefab = await action.LoadViewPrefabAsync();</c>
    /// </summary>
    UniTask<GameObject> LoadViewPrefabAsync();
    /// <summary>
    /// ロード済みプレハブから実際の View を生成する。
    /// 使用例: <c>action.InstantiateViewObject();</c>
    /// </summary>
    void InstantiateViewObject();

    /// <summary>
    /// View を配置する親 Transform を取得する。
    /// 使用例: <c>var parent = action.GetViewParent<Transform>();</c>
    /// </summary>
    TTransform GetViewParent<TTransform>() where TTransform : Transform;

    /// <summary>
    /// インスタンス生成後に追加セットアップを行う。
    /// 使用例: <c>await action.OnPostInstantiateAsync();</c>
    /// </summary>
    UniTask OnPostInstantiateAsync();

    /// <summary>
    /// View を表示状態に切り替える。
    /// 使用例: <c>await action.ShowAsync();</c>
    /// </summary>
    UniTask ShowAsync();
    /// <summary>
    /// View を非表示にする。
    /// 使用例: <c>await action.HideAsync();</c>
    /// </summary>
    UniTask HideAsync();

    /// <summary>
    /// View・モデル・リソースを破棄する。
    /// 使用例: <c>await action.UnloadAsync();</c>
    /// </summary>
    UniTask UnloadAsync();
    /// <summary>
    /// View コンテキストが操作するステートマシンへの参照。
    /// 使用例: <c>action.StateMachine.RunUpdate = false;</c>
    /// </summary>
    StateMachine StateMachine { get; }
}
