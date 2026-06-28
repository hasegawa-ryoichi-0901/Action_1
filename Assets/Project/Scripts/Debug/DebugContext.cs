using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.UI;
using R3;

public class DebugContext : SingletonMonoBehaviour<DebugContext> {
    [SerializeField] private Button _btnToggleDisable;
    [SerializeField] private DebugView _view;

    protected override async UniTask Awake() {
        bool isActive = true;
        _btnToggleDisable
            .OnClickAsObservable()
            .Subscribe(_ => {
                isActive = !isActive;
                _view.gameObject.SetActive(isActive);
            });
        _view.gameObject.SetActive(isActive);
        await _view.SetupAsync();
    }
}
