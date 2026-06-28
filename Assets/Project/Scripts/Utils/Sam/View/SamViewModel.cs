using System;

using UnityEngine;

public abstract class SamViewModel<TView> : SamBaseModel where TView : MonoBehaviour {
    public abstract string ViewPrefabPath { get; }
    [SerializeField] public GameObject ViewPrefab;
    [SerializeField] public GameObject ViewObject;
    [SerializeField] public TView View;
}
