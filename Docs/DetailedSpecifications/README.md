# 詳細仕様一覧

このディレクトリは、[../DetailedSpecification.md](../DetailedSpecification.md) をシステム単位に分割した詳細仕様群です。

## 一覧

- [ブート・シーン遷移仕様](01_BootAndSceneFlow.md)
- [プレイヤー入力仕様](02_PlayerInput.md)
- [プレイヤー移動・カメラ仕様](03_PlayerMovementAndCamera.md)
- [プレイヤー戦闘仕様](04_PlayerCombat.md)
- [敵仕様](05_EnemySpecifications.md)
- [ボス仕様](06_BossSpecification.md)
- [マップ・進行制御仕様](07_MapAndProgression.md)
- [成長・セーブ仕様](08_ProgressionAndSave.md)
- [UI仕様](09_UI.md)
- [音・演出仕様](10_AudioAndVFX.md)
- [受け入れ基準・デバッグ仕様](11_AcceptanceCriteria.md)

## 運用方針

- 仕様変更時は、統合版と分割版の両方で差分がないように更新する
- 実装判断は分割版を優先して参照してよい
- スコープ外の案は `Docs/Backlog.md` に移す
