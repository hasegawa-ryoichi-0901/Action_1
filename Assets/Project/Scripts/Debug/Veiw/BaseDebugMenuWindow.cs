using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IDebugMenuWindow {
    public UniTask SetupAsync();
}

[Serializable]
public abstract class BaseDebugMenuWindow : MonoBehaviour, IDebugMenuWindow {
    public async UniTask SetupAsync() {

    }
}
