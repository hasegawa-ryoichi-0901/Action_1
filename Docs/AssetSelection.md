# アセット選定メモ

## 1. 前提

- 確認日: `2026-06-27`
- 対象エンジン: `Unity 6 / URP`
- 対象プラットフォーム: `Windows PC`
- 目的: 1年・1日1時間程度の開発で、短編ボス討伐ゲームを完成させる

元要件では `Unreal Engine 5.x` と `Fab` が前提になっているが、このリポジトリは Unity プロジェクトなので、ここでは Unity Asset Store を主な購入先として扱う。

価格は確認時点の表示価格であり、セール・税・地域表示・ライセンス種別で変わる。購入前に必ず各商品ページで再確認する。

## 2. 要件から見た選定基準

必須:

- Unityで使用可能
- 商用利用可能なライセンス
- Windows向けビルドで利用可能
- 見た目の統一感がある
- プレイヤー、敵、マップ、UI、音の最低限を短時間で揃えられる

強く推奨:

- アニメーション付き
- URPで動作確認しやすい
- ファイルサイズが大きすぎない
- 同一シリーズでPlayer、Enemy、Mapを揃えられる

避ける:

- 見た目だけ良く、調整工数が大きいもの
- 大規模オープンワールド向けで本作に過剰なもの
- ライセンス条件が曖昧なもの
- Steam公開用素材として二次利用範囲が不明なもの

## 3. 推奨構成

### 3.1 最有力: Synty統一構成

低ポリゴン寄りの見た目になるが、Player、Enemy、Mapのトーンを揃えやすい。短編MVPを完成させる目的では最も現実的。

| 用途 | 候補 | 価格目安 | 理由 | URL |
|---|---|---:|---|---|
| マップ | POLYGON - Dungeons Pack - Art by Synty | `$74.99` | 神殿・地下・祭壇系の小規模マップを組みやすい | https://assetstore.unity.com/packages/3d/environments/dungeons/polygon-dungeons-pack-art-by-synty-102677 |
| プレイヤー | POLYGON - Fantasy Characters Pack - Art by Synty | `$14.99` | 騎士・ファンタジー系のプレイヤー候補を低コストで確保できる | https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/polygon-fantasy-characters-pack-art-by-synty-97186 |
| 敵・ボス | POLYGON - Fantasy Rivals Pack - Art by Synty | `$49.99` | 敵・ボス候補を同一画風で揃えやすい | https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/polygon-fantasy-rivals-pack-art-by-synty-118399 |
| BGM | Ultimate Game Music Collection | `$45.00` | 曲数が多く、タイトル・探索・ボス戦の仮実装に使いやすい | https://assetstore.unity.com/packages/audio/music/ultimate-game-music-collection-37351 |
| SE | Universal Sound FX | `$49.95` | 戦闘、UI、汎用SEをまとめて確保しやすい | https://assetstore.unity.com/packages/audio/sound-fx/universal-sound-fx-17256 |
| UI | GUI Pro - Fantasy RPG | `$19.99` | HP、ボタン、RPG風フレームなどに流用しやすい | https://assetstore.unity.com/packages/2d/gui/gui-pro-fantasy-rpg-170168 |
| VFX | RPG VFX Bundle | `$24.00` + 依存パッケージ `$5.00` | ヒット、回復、ボス攻撃の演出に使える。URP互換情報がある | https://assetstore.unity.com/packages/vfx/particles/spells/rpg-vfx-bundle-133704 |

概算合計: `$283.91`

この構成を採る場合、最初に買うべき順番は以下。

1. `POLYGON - Dungeons Pack`
2. `POLYGON - Fantasy Characters Pack`
3. `POLYGON - Fantasy Rivals Pack`
4. `Universal Sound FX`
5. `GUI Pro - Fantasy RPG`
6. `RPG VFX Bundle`
7. `Ultimate Game Music Collection`

BGMは最初のプレイアブルでは仮音源でもよいので、購入優先度はやや下げてよい。

## 4. 低予算構成

まず動くMVPを作るなら、マップを完成済み寄りにして工数を削る。

| 用途 | 候補 | 価格目安 | 備考 | URL |
|---|---|---:|---|---|
| マップ | POLYGON - Fantasy Dungeon Map - Art by Synty | `$9.99` | 自由度は下がるが、短編1マップには向く | https://assetstore.unity.com/packages/3d/environments/dungeons/polygon-fantasy-dungeon-map-art-by-synty-143026 |
| プレイヤー | POLYGON - Fantasy Characters Pack - Art by Synty | `$14.99` | 低コスト |
| 敵・ボス | POLYGON - Fantasy Rivals Pack - Art by Synty | `$49.99` | 敵とボス候補 |
| SE | Universal Sound FX | `$49.95` | 戦闘とUIの最低限 |
| UI | GUI Pro - Fantasy RPG | `$19.99` | 最低限のUI素材 |
| VFX | 70 Fantasy Spells Effects Pack | `$1.50` | 安いが古いUnity版基準のため検証必須 | https://assetstore.unity.com/packages/vfx/particles/spells/70-fantasy-spells-effects-pack-112526 |

概算合計: `$146.41`

低予算構成の注意点:

- マップを大きく改造する余地が少ない可能性がある
- `70 Fantasy Spells Effects Pack` は古いUnity版基準なので、Unity 6 / URPで表示検証が必要
- BGMは後回しにし、まず既存の仮BGMまたは無音で進める

## 5. 高めの品質を狙う追加候補

| 用途 | 候補 | 価格目安 | 採用判断 |
|---|---|---:|---|
| プレイヤーカスタム | POLYGON - Modular Fantasy Hero Characters Pack - Art by Synty | `$149.99` | 見た目の自由度は上がるが、MVPには高価 |
| ボス | Unka the Dragon | `$59.99` | 大型ボス候補。神殿の番人として使うには見た目調整が必要 |
| BGM | Total Music Collection | `$50.00` | BGM候補を増やしたい場合 |

## 6. まだ不足するもの

アセット購入だけでは不足するものは以下。

- プレイヤー用の攻撃、回避、被弾、死亡モーション
- 敵A、敵B、ボスの攻撃モーション調整
- 武器モデル
- 回復アイテムや強化素材の小物
- タイトル背景またはキービジュアル
- Steamストア用カプセル画像
- フォント
- 入力アイコン
- マウスカーソル
- クレジット表記とライセンス管理表

## 7. 追加で用意すべき無料・低コスト系

| 用途 | 候補 | 理由 |
|---|---|---|
| カメラ | Unity Cinemachine | 三人称カメラとロックオン補助に使える |
| アニメーション仮素材 | Mixamo | Humanoidリグの仮モーション検証に使いやすい |
| フォント | Google Fonts / SIL系ライセンスフォント | UIに使う場合は商用利用と再配布条件を確認 |
| Steam素材 | ゲーム内スクリーンショットから制作 | 購入アセットのサムネイルや販売画像は使わない |

## 8. 購入前チェックリスト

購入前に必ず確認する。

- Unity 6 または Unity 2022 LTS以降でインポートできるか
- URPでマテリアルが破綻しないか
- アニメーションがHumanoidかGenericか
- 必要な攻撃、回避、被弾、死亡モーションがあるか
- ボスとして成立するサイズ、攻撃モーション、予備動作があるか
- 商用利用できるライセンスか
- Steamストア素材への使用可否に問題がないか
- セール価格ではなく通常価格でも予算内か

## 9. 推奨判断

最初に買うなら、以下の3つだけでよい。

1. `POLYGON - Dungeons Pack - Art by Synty`
2. `POLYGON - Fantasy Characters Pack - Art by Synty`
3. `POLYGON - Fantasy Rivals Pack - Art by Synty`

この3つで、Player、Enemy、Boss、Mapの見た目検証を先に行う。音、UI、VFXはゲームループが動いてから購入する。

## 10. 参照URL

- Unity Asset Store - 3D Dungeons: https://assetstore.unity.com/3d/environments/dungeons
- Unity Asset Store - Fantasy Characters: https://assetstore.unity.com/3d/characters/humanoids/fantasy
- Unity Asset Store - Creatures: https://assetstore.unity.com/3d/characters/creatures
- Unity Asset Store - Music: https://assetstore.unity.com/audio/music
- Unity Asset Store - Sound FX: https://assetstore.unity.com/audio/sound-fx
- Unity Asset Store - 2D GUI: https://assetstore.unity.com/2d/gui
- Unity Asset Store - Spells VFX: https://assetstore.unity.com/vfx/particles/spells
