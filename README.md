# Auto Save

<p align="center">
  <img src="Packages/com.azipaworks.auto-save/Editor/auto_save_icon.png" width="128" alt="Auto Save icon">
</p>

<p align="center">
  <strong>Unity Sceneを、編集が止まったタイミングで自動保存</strong><br>
  作業を妨げず、保存忘れによる編集内容の消失を防ぐAzipa Toolsのエディター拡張です。
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2022.3-222c37?logo=unity&logoColor=white" alt="Unity 2022.3">
  <img src="https://img.shields.io/badge/VRChat-Avatar%20%2F%20World-2d9bf0" alt="VRChat Avatar and World">
  <img src="https://img.shields.io/badge/VPM-1.0.0-6f42c1" alt="VPM 1.0.0">
  <img src="https://img.shields.io/badge/License-MIT-green" alt="MIT License">
</p>

## 概要

Auto Saveは、Unity EditorでアクティブなSceneの変更を検知し、最後の変更から一定時間が経過したあとに自動保存するツールです。

変更のたびに即座に保存するのではなく、操作が止まってから保存するため、連続した編集を邪魔しません。再生中やコンパイル中など、安全に保存できない状態では処理を保留します。

## 主な機能

- アクティブなSceneの変更を検知して自動保存
- 自動保存のON / OFFをワンクリックで切り替え
- `10秒`、`30秒`、`60秒`のプリセットを用意
- `500 ms`以上のカスタム待機時間に対応
- Sceneが再度変更された場合は待機時間を自動的に再計測
- 再生中、コンパイル中、アセット更新中の保存を安全に保留
- 設定を`EditorPrefs`へ保存し、Unity Editor環境ごとに維持
- Avatar / Worldの両方のVRChatプロジェクトで利用可能

## 動作環境

- Unity `2022.3`
- VRChat AvatarまたはWorldプロジェクト

VRChat SDK固有のAPIには依存していないため、AvatarとWorldのどちらでも使用できます。

## インストール

### VCC / ALCOM（推奨）

AzipaToolsのVPMリポジトリに本パッケージが公開された後、VCCまたはALCOMへリポジトリを追加し、対象プロジェクトのパッケージ管理画面から **Auto Save** を追加してください。

| 項目 | 値 |
| --- | --- |
| パッケージID | `com.azipaworks.auto-save` |
| 表示名 | `Auto Save` |
| 対応Unity | `2022.3` |

### GitHub Release

Releaseに添付された`com.azipaworks.auto-save-<version>.zip`は、`package.json`が直下にあるVPM互換パッケージです。VPM対応クライアントへZIPを追加して導入できます。

## 起動方法

Unityメニューから次を選択します。

```text
Tools > Azipa Tools > Auto Save
```

## 設定

### Auto Save

自動保存機能のON / OFFを切り替えます。初期設定は`ON`です。

### Save Delay

Sceneの最後の変更から自動保存するまでの待機時間を指定します。

| 設定 | 待機時間 |
| --- | ---: |
| `10s` | 10秒 |
| `30s` | 30秒 |
| `60s` | 60秒 |
| `Custom` | 任意のミリ秒 |

`Custom`の初期値は`5000 ms`、設定可能な最小値は`500 ms`です。待機中にSceneが再び変更されると、その時点から待機時間を計測し直します。

## 自動保存の対象

次の条件をすべて満たすSceneが保存対象です。

- 現在アクティブである
- 読み込み済みで変更がある
- 一度以上保存されており、Sceneファイルのパスが存在する
- Preview Sceneではない

新規作成直後で保存先が決まっていないSceneは自動保存されません。最初の1回はUnity標準の保存機能でSceneファイルを作成してください。

複数のSceneを開いている場合、自動保存されるのはアクティブなSceneのみです。

## 保存を保留する状態

データ破損やUnityの処理との競合を避けるため、次の状態では自動保存を実行しません。

- Play Mode中
- Play Modeへの切り替え中
- スクリプトのコンパイル中
- Asset Databaseの更新中
- Auto Save自身が保存処理中

保留中もSceneの変更状態は維持され、安全に保存できる状態へ戻ってから処理されます。

## 設定データ

設定値はUnity Editorの`EditorPrefs`へ保存されます。Scene、Prefab、プロジェクト内のアセットは設定変更によって書き換えられません。

保存される設定は次のとおりです。

- Auto Saveの有効状態
- 選択中の待機時間プリセット
- Customの待機時間

## 旧版からの移行

従来版の次のフォルダーは、VPMインストール時に移行対象として認識されます。

```text
Assets/Azipa Works/tools/AutoSave
```

これにより、Assets版とVPM版が同時に読み込まれてスクリプトが重複することを防ぎます。

## アンインストール

VCCまたはALCOMのパッケージ管理画面から **Auto Save** を削除してください。`EditorPrefs`に保存された設定は、パッケージを削除してもUnity Editor環境に残ります。

## 開発者向けリリース手順

GitHub Actionsの`Build Release`は、`package.json`のバージョンを使用して次の成果物を生成します。

- VPM用ZIP
- UnityPackage
- `package.json`
- GitタグとGitHub Release

初回のみ、GitHubリポジトリの **Settings > Secrets and variables > Actions > Variables** に次のRepository variableを追加してください。

| Name | Value |
| --- | --- |
| `PACKAGE_NAME` | `com.azipaworks.auto-save` |

Release作成後、GitHubリポジトリをAzipaToolsのVPMリポジトリ定義へ登録すると、VCC / ALCOMから配信できます。

## 変更履歴

[CHANGELOG.md](CHANGELOG.md)を参照してください。

## ライセンス

このプロジェクトは[MIT License](LICENSE.txt)で提供されます。
