using KanKikuchi.AudioManager;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSe : MonoBehaviour {
    [SerializeField] private SEPath.SEPathType _seType;

    private void Awake() {
        var button = this.GetComponent<Button>();
        button.OnClickAsObservable().Subscribe(this.Play).AddTo(this);
    }

    private void Play(Unit _) { SEManager.Instance.Play(this._seType); }
}