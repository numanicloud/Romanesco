
## EditorViewModel

`Editor` に対応するビューモデル。ビューモデルのルートでもある。

## Editor

モデルのルート。

## EditorState

エディターの状態を表現し、場面ごとに実行できるアクションと実行できないアクションを管理したり、アクションの内容を実装したりする。実際の実装は、 `ILoadService`, `ISaveService`, `IHistoryService` に委譲する。

`Empty` 状態だけはプロジェクト情報を持たないため、 `EditorState` は `Project` オブジェクトを持つとは限らない。

## CommandHistory

Undo/Redo を実際に実行するために必要。Stateたちに行き渡らなければならない。 `IPlugin` 実装オブジェクトに渡すと良いと思うが、何かに包んで渡すべきかもしれない。たとえば、 `EditorContext` 。

プロジェクトが新しく生成されたら、このオブジェクトは新しいオブジェクトに代わられることになる。プラグイン側で再購読が必要になるのではないか？なにか対策ができるか？ `OnProjectChanged` などのハンドラーを定義できるようにするか？それとも、 `CommandHistory` プロパティ自体を `ReactiveProperty` にしておくか？

少なくとも、プロジェクトが新しく生成される際にStateたちが一掃されることはドキュメントに書く必要がありそうだ。

## EditorContext

全てのプラグインに配布されるオブジェクト。モデルに配布するのが主な目的のため、ドメインモデルに関する情報だけを持つべきだが、モデルにだけ配布するべきか？それともビューモデルにも配布するべき？

たぶん、ビューモデル向けには `EditorViewModelContext` みたいのを作るべき。

## StateViewContext

ビューのルート。複数のルート `StateViewContext` を持っている。

# どこから手をつけるか

## 最低限必要

* EditorViewModel
* Editor
* EditorState
    * DirtyState
* Project
* StateRoot
* 仮のProjectを生成する機能
* RootViewContext

## Gen 2

* Projectを実際に操作する
* NewState
* CleanState
* EmptyState
* ILoadServiceの実装
* ISaveServiceの実装
* 仮のプロジェクト型を与える機能(IProjectSettingProviderの仮実装)

`IFieldState` の実装者が初期値をきちんとロードしないといけないのが気がかり。明示的にロードする必要性があることを分かりやすくしたい

Viewとの接続がうまくいかないと（Bindingマークアップが間違っていたりすると）、生のデータが更新されないが無言で続行されてしまう。この問題に気づきやすくすることはできないか？

## Gen 3

* Commonを Model, ViewModel, View に分離
* 名前空間を整理

## Gen 4

* IHistoryServiceの実装
* EditorContext
* CommandHistory

## Gen 5

* コマンドの実行可否に応じてボタン自体を押せないようにする

## Gen 6

* EditorProjectAttribute
* ProjectSettings

## Gen 7

* デフォルトのシリアライズ/デシリアライズ
* ユーザー定義のシリアライズ/デシリアライズ
* Export

## Gen 8

* DependencyManager

## Gen 9

* エラー処理の取り残し
* プロジェクトが不正な状態の場合の復旧機能