# Auto Save

Unity Editor で編集中の Scene を自動保存する Azipa Tools のエディター拡張です。

Scene に変更を加えたあと、設定した待機時間が経過すると、アクティブな Scene を自動的に保存します。

## 動作環境

- Unity `2022.3`
- VRChat Avatar / World プロジェクト

VRChat SDK 固有の API は使用していないため、Avatar と World のどちらでも利用できます。

## インストール

### VCC / ALCOM（推奨）

AzipaTools の VPM リポジトリに本パッケージが公開された後、リポジトリを VCC または ALCOM に追加し、対象プロジェクトのパッケージ管理画面から **Auto Save** を追加してください。

- パッケージ ID: `com.azipaworks.auto-save`
- 表示名: `Auto Save`

旧版の `Assets/Azipa Works/tools/AutoSave` が存在する場合は、VPM インストール時に移行対象として処理されます。

### GitHub Release の ZIP

Release に添付された `com.azipaworks.auto-save-<version>.zip` を展開せず、VPM 対応クライアントから追加できます。

## 起動

Unity メニューから `Tools > Azipa Tools > Auto Save` を開きます。

## 主な機能

- Scene の変更を検知して自動保存
- 自動保存の ON / OFF 切り替え
- 保存までの待機時間を `10秒`、`30秒`、`60秒` から選択
- ミリ秒単位のカスタム待機時間を設定可能
- 再生中、再生モードへの切り替え中、コンパイル中、アセット更新中は保存を保留
- 設定内容を Unity Editor の `EditorPrefs` に保存

## 使い方

### Auto Save

自動保存機能を切り替えます。初期設定は `ON` です。

### Save Delay

Scene の最後の変更から自動保存までの待機時間を設定します。

| 設定 | 待機時間 |
| --- | ---: |
| `10s` | 10秒 |
| `30s` | 30秒 |
| `60s` | 60秒 |
| `Custom` | 任意のミリ秒 |

`Custom` の最小値は `500 ms`、初期値は `5000 ms` です。待機中に Scene が再び変更された場合は、その時点から待機時間が再計測されます。

## 自動保存の対象

次の条件をすべて満たす Scene が保存対象です。

- 現在アクティブである
- 読み込み済みで変更がある
- 一度以上保存されており、ファイルパスが存在する
- Preview Scene ではない

新規作成直後で保存先が決まっていない Scene は自動保存されません。先に Unity の保存機能で Scene ファイルを作成してください。

複数の Scene を開いている場合でも、自動保存されるのはアクティブな Scene のみです。

## 設定の保存

設定は `EditorPrefs` に保存されます。Scene やプロジェクトのアセットには書き込まれず、使用している Unity Editor 環境ごとに保持されます。

## アンインストール

VCC または ALCOM のパッケージ管理画面から **Auto Save** を削除してください。

## 変更履歴

[CHANGELOG.md](CHANGELOG.md) を参照してください。
