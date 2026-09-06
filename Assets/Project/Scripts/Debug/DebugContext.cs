using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.UI;
using R3;
using UnityEngine.InputSystem;
using System;

public class DebugContext : SingletonMonoBehaviour<DebugContext> {
    [SerializeField] private Button _btnToggleDisable;
    [SerializeField] private DebugView _view;
    [SerializeField] private DebugMenu _debugMenu;
    protected override async UniTask Awake() {
        bool isActive = true;
        _btnToggleDisable
            .OnClickAsObservable()
            .Subscribe(_ => {
                isActive = !isActive;
                _view.gameObject.SetActive(isActive);
            });
        _view.gameObject.SetActive(isActive);
        OnPressedKeyObservable(Key.M).Subscribe(
            pressedKey => {
                Debug.Log(Keyboard.current[pressedKey].displayName);
                isActive = !isActive;
                _view.gameObject.SetActive(isActive);
            });
        var controller = new DebugViewController(_debugMenu);
        await controller.InitializeAsync();
        await _view.SetupAsync();
    }

    private static Observable<Key> OnPressedKeyObservable(Key key) {
        return Observable.Create<Key>(async (observer, token) => {
            try {
                while (true) {
                    await UniTask.WaitUntil(() => Keyboard.current[key].wasPressedThisFrame, cancellationToken: token);

                    observer.OnNext(key);

                    await UniTask.Yield(token);
                }
            }
            catch (Exception e) {
                observer.OnCompleted(e);
            }
        });
    }
}
