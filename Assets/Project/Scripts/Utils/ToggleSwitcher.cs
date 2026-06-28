using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZLinq;

public class ToggleSwitcher : MonoBehaviour {
    [SerializeField] protected List<ToggleButton> toggles = new List<ToggleButton>();
    [SerializeField, Header("選択済みでもイベント呼ぶか")] protected bool distinctSelect = false;

    [SerializeField, Header("素早い切り替えをブロック")] private bool isSwitchClickBlock = true;
    [SerializeField] private float switchInterval = 0.3f;

    private Subject<(ToggleButton, int)> onSelectSubject = new Subject<(ToggleButton, int)>();
    public Observable<(ToggleButton, int)> OnSelectAsObservable() => this.onSelectSubject.AsObservable();

    private List<IDisposable> clickSubscriptions = new List<IDisposable>();

    public List<ToggleButton> Toggles => this.toggles;
    
    protected virtual void Awake() { this.SetToggles(this.toggles); }

    public virtual void SetToggles(List<ToggleButton> _regToggles) {
        this.clickSubscriptions.ForEach(_x => _x.Dispose());
        this.clickSubscriptions.Clear();
        this.toggles = _regToggles
            .AsValueEnumerable()
            .ToList();
        foreach (var toggle in this.toggles) {
            toggle.ToggleMode = ToggleButton.SingletToggleMode.VISIBLE;
            var obs = toggle.OnClickAsObservable();
            if (this.isSwitchClickBlock) {
                this.clickSubscriptions.Add(
                    obs.SubscribeClick(
                        _ => this.OnToggleClick(toggle),
                        TimeSpan.FromSeconds(this.switchInterval))
                       .AddTo(this.gameObject));
            } else {
                this.clickSubscriptions.Add(obs.Subscribe(_ => this.OnToggleClick(toggle)).AddTo(this.gameObject));
            }
        }
    }

    public virtual void ClearToggles() {
        this.SetToggles(new List<ToggleButton>());
    }
    
    public virtual void RegisterToggles(List<ToggleButton> _regToggles) {
        this.toggles.AddRange(_regToggles);
        this.SetToggles(this.toggles);
    }
    public virtual void RegisterToggle(ToggleButton _regToggle) {
        this.toggles.Add(_regToggle);
        this.SetToggles(this.toggles);
    }

    public void SetAllTogglesOff() {
        foreach (var toggle in this.toggles) {
            toggle.IsOn = false;
        }
    }

    public ToggleButton GetActiveToggle() { return this.toggles
        .AsValueEnumerable()
        .FirstOrDefault(_x => _x.IsOn); }

    public virtual void SelectToggle(ToggleButton _toggle) {
        if (_toggle == null) {
            return;
        }
        var i = 0;
        for (; i < this.toggles.Count; ++i) {
            var t = this.toggles[i];
            if (t == _toggle && (distinctSelect || !_toggle.IsOn)) {
                this.onSelectSubject.OnNext((_toggle, i));
            }

            t.IsOn = t == _toggle;
        }
    }
    protected virtual void OnToggleClick(ToggleButton _toggle) {
        this.SelectToggle(_toggle);
    }
}