# Auto Save

Unity Editor で編集中の Scene を自動保存する Azipa Tools のエディター拡張です。

公開後は、VCC / ALCOM で AzipaTools の VPM リポジトリから **Auto Save** を選択して導入できます。

- パッケージ ID: `com.azipaworks.auto-save`
- Unity: `2022.3`
- バージョン: `1.0.0`
- ライセンス: MIT

詳しい使い方は [パッケージ README](Packages/com.azipaworks.auto-save/README.md) を参照してください。

## リリース

GitHub Actions の `Build Release` を実行すると、`package.json` のバージョンを使って VPM 用 ZIP、UnityPackage、GitHub Release を生成します。

初回のみ、GitHub リポジトリの **Settings > Secrets and variables > Actions > Variables** に次の Repository variable を追加してください。

| Name | Value |
| --- | --- |
| `PACKAGE_NAME` | `com.azipaworks.auto-save` |

作成された GitHub リポジトリを AzipaTools の VPM リポジトリ定義へ登録すると、Release の `package.json` と ZIP がリスティングに取り込まれ、VCC / ALCOM からインストールできるようになります。
