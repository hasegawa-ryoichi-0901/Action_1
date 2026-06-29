using Cysharp.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Loaderで行うLifecycleの関数を格納
/// このクラスを継承したインターフェースを作成する
/// </summary>
public interface IResourcesLoader {
    public UniTask SetupAsync();
    public UniTask LoadAsync();
    public UniTask SpawnAsync();
    public UniTask DespawnAsync();
    public UniTask UnloadAsync();
}
