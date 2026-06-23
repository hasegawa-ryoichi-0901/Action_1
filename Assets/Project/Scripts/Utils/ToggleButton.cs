using System;
using R3;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.UI;
using UnityEditor;
#endif

/// <summary>
/// interactableのOn/OffでonButton, offButton表示切り替えるやつ
/// isToggleTargetGraphic onでtargetGraphic時にtargetGraphicも変える
/// </summary>
public class ToggleButton : Button {
    public enum SingletToggleMode {
        VISIBLE,
        INTERACTABLE,
    }

    private ReactiveProperty<bool> _isOn = new ReactiveProperty<bool>();
    public Observable<bool> IsOnAsObservable() => this._isOn
        .AsObservable()
        .DistinctUntilChanged();
    
    public bool IsOn {
        get => this._isOn.Value;
        set {
            this._isOn.Value = value;
            if (this.ToggleMode == SingletToggleMode.VISIBLE) {
                this.DoStateTransition(this.currentSelectionState, false);
            }
        }
    }

    public bool IsAutoToggle = true;
    private IDisposable _isAutoToggleDisposable = null;

    [SerializeField] public SingletToggleMode ToggleMode;
    [SerializeField] public bool isToggleTargetGraphic;
    [SerializeField] public Image onButton;
    [SerializeField] public Image offButton;
    [SerializeField] public GameObject onObject;
    [SerializeField] public GameObject offObject;
    [SerializeField] public float AutoToggleDelay = 0.2f;

    protected override void Awake() {
        base.Awake();
        this.OnIsAutoToggleChanged();
    }
    
    /// <summary>
    /// Transition the Selectable to the entered state.
    /// </summary>
    /// <param name="state">State to transition to</param>
    /// <param name="instant">Should the transition occur instantly.</param>
    protected override void DoStateTransition(SelectionState state, bool instant) {
        var isEnabled = this.ToggleMode == SingletToggleMode.INTERACTABLE
            ? (state != SelectionState.Disabled)
            : this.IsOn;
        if (this.isToggleTargetGraphic) {
            this.targetGraphic = isEnabled ? this.onButton : this.offButton;
        }

        base.DoStateTransition(state, instant);

        if (this.onObject != null) {
            this.onObject.SetActive(isEnabled);
        }
        else {
            if (this.onButton != null) {
                this.onButton.gameObject.SetActive(isEnabled);
            }
        }

        if (this.offObject != null) {
            this.offObject.SetActive(!isEnabled);
        }
        else {
            if (this.offButton != null) {
                this.offButton.gameObject.SetActive(!isEnabled);
            }

        }
    }
    
    public void OnIsAutoToggleChanged()
    {
        this._isAutoToggleDisposable?.Dispose();
        if (this.IsAutoToggle) {
            this._isAutoToggleDisposable = this.OnClickAsObservable()
                .SubscribeClick(_ => this.IsOn = !this.IsOn, TimeSpan.FromSeconds(this.AutoToggleDelay))
                .AddTo(this);
        }
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(ToggleButton), true)]
public class ToggleButtonEditor : ButtonEditor {
    
    SerializedProperty isAutoToggle;

    protected override void OnEnable()
    {
        base.OnEnable();
        isAutoToggle = serializedObject.FindProperty("IsAutoToggle");
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();

        var component = (ToggleButton)target;
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(isAutoToggle);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();

            // プロパティが変更されたときの処理
            component.OnIsAutoToggleChanged(); // 任意のメソッド呼び出し

            // オブジェクトをDirtyにする（保存対象）
            EditorUtility.SetDirty(component);
        }
        
        component.IsOn =
            EditorGUILayout.Toggle("IsOn", component.IsOn);
        component.ToggleMode =
            (ToggleButton.SingletToggleMode)EditorGUILayout.EnumPopup("SingletToggleMode", component.ToggleMode);
        component.isToggleTargetGraphic =
            EditorGUILayout.Toggle("IsToggleTargetGraphic", component.isToggleTargetGraphic);
        component.onButton =
            EditorGUILayout.ObjectField("OnButton", component.onButton, typeof(Image), true, null) as Image;
        component.offButton =
            EditorGUILayout.ObjectField("OffButton", component.offButton, typeof(Image), true, null) as Image;
        component.onObject =
            EditorGUILayout.ObjectField("OnObject", component.onObject, typeof(GameObject), true, null) as GameObject;
        component.offObject =
            EditorGUILayout.ObjectField("OffObject", component.offObject, typeof(GameObject), true, null) as GameObject;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif