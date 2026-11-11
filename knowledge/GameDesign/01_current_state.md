# 01. 現行実装の棚卸し

Unity 6000.2.8f1 / URP 17 / Input System / UI Toolkit / Addressables / UniTask / Cinemachine 3 / AI Navigation / VFX Graph。
シーンは `SampleScene` 1つ＋UI用の加算シーン（`TetrisInventory`, `InteractUI`）。

## 1. いま動いている・形があるもの

| 分野 | 状態 | 中身 |
|---|---|---|
| 起動・更新 | 🟦 | `GameMain` が `ASystem` 7本を生成し、PreUpdate/Update/FixedUpdate/LateUpdate を配る。共有状態は `GameStatus`、通知は `GameEventBus`（`Action` の束） |
| 入力 | 🟦 | `InputSystem`：移動(WASD/左スティック)・攻撃・スプリント(押下中)・インタラクト(F)・インベントリ(I)。Player/UI/System の3マップ。Crouch・Jump・Previous・Next は定義のみ |
| エイム | 🟦 | カーソル→ワールド座標。SphereCast によるエイムアシスト、銃口高さの仮想平面フォールバック |
| プレイヤー | 🟦 | `PlayerController`：Rigidbody 移動、カーソル方向へ回転、右手（武器）の向き補正（近距離デッドゾーン・距離別の射角制限）、個別タイムスケール |
| アニメ | 🟦 | `PlayerAnimationHandler`：武器種別でベース Controller 切替＋武器固有 Override |
| 武器 | 🟦 | `AGunBase<T>`：セミ/バースト/フルオート、発射間隔、銃口のめり込みチェック、マズルフラッシュ。5銃種のクラスとプレハブとファクトリ |
| 弾 | 🟦 | `BulletService`：構造体リストでの弾シミュレーション（FixedUpdate で Raycast、Update で見た目補間）。当たっても**消えるだけ** |
| アイテム定義 | 🟦 | `ItemData` 系 SO（Gun/Melee/Ammo/Attachment/Magazine/Container/Enchant）、`ItemVisualData` |
| アイテム実体 | 🟦 | `AItemRuntimeBase`（GUID・スタック・マージ）、`GunRuntime`（マガジン/内蔵弾倉・ステータス）、アタッチメントの入れ子スロット |
| ステータス | 🟦 | 2系統が並存：①`Stats`/`StatsData`/`BuffManager`（5層合算）②`StatController`/`StatHandler*`/`StatModifier`（補正＋戦略＋キャッシュ） |
| インベントリ | 🟦 | モデル（`Inventory`/`Container`/`GridBlock`：配置・回転・空き探索）とビュー（UI Toolkit のコンポーネント群）。D&D の入力検出まで。**モデルとビューは未接続** |
| インタラクト | 🟦 | 近傍探索→最寄り表示→F で実行。`ILootable`（DeathBox/Crate）。コンテナ生成は未実装 |
| シーン | 🟦 | `SceneSystem`：Addressables 加算ロードのキュー処理、`ISceneEntryPoint` 経由の依存受け渡し |
| AI | 🟦 | HTN の骨格（Compound/Primitive/Method/Plan/Planner/Runner/前提条件）。オペレーター実装・敵キャラは未着手 |
| プール | 🟦 | `ObjectPool<T>` + `APooledObject` + `IObjectFactory<T>` |
| ツール | 🟦 | スプレッドシート→SO インポート（`ItemImportTool`）、`SubclassSelector` |

## 2. 読んで見つけた不具合（コードで確認済み）

設計の議論より先に直す価値があるもの。番号は優先順ではありません。

| # | 場所 | 内容 |
|---|---|---|
| B1 | `InventoryView.AddContainer` | セル座標を `count % width`・`count / height` で計算している（ループ変数 `j` と `width` を使うべき）。全セルが同じ座標になる |
| B2 | `StatHandlerEffect.Tick` | 同じ処理が2回書かれており、持続時間が毎フレーム2倍減る |
| B3 | `StatModifier` | フィールドが `private` で `[SerializeField]` 無し。インスペクタ・インポートから値が入らず常に既定値 |
| B4 | `Stats.ApplyEffectToLayer` | 解除時に引数の `StatsData` 自体の符号を反転する。同じバフを再利用すると符号が壊れる |
| B5 | `BuffManager.ApplyBuff` | 既存バフを `RemoveEffect`（内部で Remove）した後もう一度 Remove |
| B6 | `HTNPlanRunner` | `_planner` が `readonly` のまま未代入。`Replanning` で必ず NullReference |
| B7 | `Method._subTasks` | `List<ITask>` はインターフェースなので Unity がシリアライズできない |
| B8 | `HTNPlanRunner` | 1秒ごとに実行中タスクを破棄して再計画する（行動が毎秒リセットされる） |
| B9 | `AGunBase.GetDestination` | ヒットしない場合の到達点が `dir * MaxRange`（始点を足していない）。Raycast も `dir` ではなく銃口 forward |
| B10 | `AGunBase.AttackStart` | バースト中の再入力ガードが無く、連打でバーストが重なる |
| B11 | `GunRuntime.CanConsume` | 名前は判定だが弾を消費する。内蔵弾倉側は `isConsume` を無視 |
| B12 | `GunRuntime.TryInternalReload` | 上限に `MaxStack`（スタック数）を使っている。`InternalCapacity` のはず |
| B13 | `GunRuntime.LoadAmmoData` | マガジン未装着で NullReference |
| B14 | `BulletService` | 命中しても `IDamagable.TakeDamage` を呼ばない。ダメージが一切入らない |
| B15 | `PlayerController.Move` | 速度に `fixedDeltaTime` を掛けてから `linearVelocity` に入れている（単位が合わない） |
| B16 | `InputSystem` | `_assistRadius` に値を入れておらず常に 0（アシストが効かない）。`Camera.main` を毎フレーム参照 |
| B17 | `InputSystem.OnDispose` | Interact の購読解除漏れ。Interact が Player と System の両マップにある |
| B18 | `InventorySystem.OpenLootContainer` | トグルイベント発火→フラグ設定→もう一度トグル、で開閉が二重に走る。`gameStat?.interactView.Hide()` は null 条件の位置が逆 |
| B19 | `ItemSystem.ProvideContainer` | コンテナを生成せず null を通知 |
| B20 | `WeaponSystem.CreateWeapon` | Melee のプールが無く KeyNotFound。武器 A ロード中に B へ切替えると A が後から上書き（TODO 記載あり） |
| B21 | `AWeaponBase.ReturnToPool` | 見た目を Release した後 `_visualInstance` を null にしない。次の `VisualSet` で二重解放 |
| B22 | `GameMain` | `OnDispose` を一度も呼ばない。`IsActiveForUpdate` を見ていない。`EquipmentSystem` 未登録 |
| B23 | `SceneSystem` | `OnRequestLoadScene` の購読解除漏れ。ロード済みシーンへ後から登録したコールバックは呼ばれない |
| B24 | `Container` | 重量の加算（`CurrentWeight`）と減算（`Weight * StackCount`）で式が違う |
| B25 | `BurstFire` / `InvokeMuzzleFlash` | `UniTask.Delay` が実時間。タイムスケール・ポーズ・個別スローが効かない |

## 3. 構造上の負債

| # | 内容 | 影響 |
|---|---|---|
| D1 | `GameStatus` が何でも入る共有袋（入力・カメラ・プレハブ・ビュー・サービス・テスト用データ） | 誰が何を書き換えるか追えない。テスト不能 |
| D2 | ステータス系が2系統＋`GunStatModifier`（enum キー）と `StatModifier`（文字列キー）も二重 | どれが正か分からない |
| D3 | `IItemRuntime` と `IItemRuntimeData`（旧）が並存。インベントリは旧側を参照 | アイテムとインベントリが繋がらない |
| D4 | 銃5種のクラス・プレハブ・ファクトリが実質同一（差はデータのみ）。`Shotgun` は基底をコピーしただけ | 銃種を増やすたびに3ファイル増える |
| D5 | 武器のロジックが MonoBehaviour（見た目）側にある | 敵が同じ銃を使う・テストする・インベントリ内で弾数を見る、がやりにくい |
| D6 | アイテムの能力が継承で表現されている | 「防具＋収納（アーマードリグ）」「回数付きの鍵」など複合が作れない |
| D7 | `ItemActionTag` と `ItemTraitTag` の中身が名前と逆（Action に可燃性、Trait に使用可能） | 混乱の元 |
| D8 | イベントが引数なし `Action`。プレイヤー専用の前提（誰が撃ったかが無い） | 敵・複数アクター対応不可 |
| D9 | 名前空間が `Game.Data`・`Game.UI`・`HTN`・無しで混在。asmdef 無し | 依存方向を強制できない。コンパイルも遅くなる |
| D10 | 綴り：`Survice`/`Abtract`/`Assult`/`Incriment`/`Craete`/`Excution`/`IDamagable` | 検索性 |
| D11 | `mono_crash.0.0.json`・`.idea` がコミットされている | リポジトリ衛生 |
| D12 | `Effect`（`AmmoEffect.cs`）という汎用すぎるクラス名。`ElementType` は None のみ | 拡張時に衝突 |

## 4. 活かすべき良い判断

- 定義（SO）と実体（Runtime）の分離、GUID 付き実体。
- 弾を構造体で一括シミュレーションし、見た目だけプールする構成（弾が多くても軽い）。
- `ISceneEntryPoint` による加算シーンからの依存受け渡し。
- インベントリのモデル／ビュー分離、グリッドブロック複数からなるコンテナ。
- アタッチメントの入れ子スロット（アタッチメントがさらにスロットを増やす）。
- AI に HTN を選んだこと（脱出型の「状況に応じた戦術行動」に合う）。
- スプレッドシート起点のデータパイプライン。
- プレイヤー個別のタイムスケール。
