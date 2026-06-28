using Cysharp.Threading.Tasks;
using UnityEngine;

public class TitleView : MonoBehaviour {
    protected ITitleAction _context;
    public async UniTask SetupAsync(ITitleAction context) {
        _context = context;
    }
}
