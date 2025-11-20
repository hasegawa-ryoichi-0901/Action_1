using UnityEngine;
using R3;
using Cysharp.Threading.Tasks;


public class test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected async UniTask Test()
    {
        await UniTask.Delay(1);
    }
}
