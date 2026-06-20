using AsyncFSM;

using Cysharp.Threading.Tasks;

using UnityEngine;

public interface ISamViewAction : ISamAction {
    UniTask<GameObject> LoadViewPrefabAsync();
    void InstantiateViewObject();
    RectTransform GetViewParent();
    UniTask OnPostInstantiateAsync();
    UniTask ShowAsync();
    UniTask HideAsync();
    StateMachine StateMachine { get; }
}
