using KanKikuchi.AudioManager;
using R3;
using UnityEngine;

[RequireComponent(typeof(ToggleSwitcher))]
public class ToggleSwitcherSe : MonoBehaviour {
    [SerializeField] private SEPath.SEPathType _seType = SEPath.SEPathType.NONE;

    private void Awake() {
        var switcher = this.GetComponent<ToggleSwitcher>();
        switcher.OnSelectAsObservable().Subscribe(_ => this.Play());
    }

    private void Play() {
        if (!this.gameObject.activeSelf) {
            return;
        }
        SEManager.Instance.Play(this._seType);
    }
}