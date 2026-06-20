using System.Collections.Generic;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using KanKikuchi.AudioManager;

using UnityEngine;

using ZLinq;

public class ContextManager : SingletonMonoBehaviour<ContextManager> {
    [SerializeField] public RectTransform RootRectTrans;
    [SerializeField] public Canvas Canvas;

    public static Dictionary<ContextType, ISamContext> Contexts { get; private set; } = new();

    private async UniTask InitializeContextsAsync() {
        await this.UnloadAsync();
        // 初期に必要なContext
        var initialContexts = new ISamContext[] {
            //MainContext.Instance,
        };
        Contexts = initialContexts
            .AsValueEnumerable()
            .ToDictionary(k => k.ContextType, k => k);
    }

    public async UniTask SetupAsync() {
        await this.InitializeContextsAsync();
        // Setup
        await UniTask.WhenAll(Contexts.Values
            .Select(x => x.SetupAsync()));

        // Setup完了待ち
        var setupWaitTasks =
            Contexts.Select<KeyValuePair<ContextType, ISamContext>>(x => x.Value
                .OnSetupCompletedAsAsyncEnumerable()
                .FirstAwaitAsync(async t => t == x.Key));
        await UniTask.WhenAll(setupWaitTasks);
    }

    protected async UniTask UnloadAsync() {
        await UniTask.WhenAll(Contexts.Select(kv => kv.Value.DisposeAsync()));
        Contexts.Clear();
        foreach (var go in this.RootRectTrans.gameObject.Children()) {
            go.Destroy();
        }
    }

    public static async UniTask ReloadAsync() {
        await Instance.UnloadAsync();
    }


}
