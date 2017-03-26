# Private

# 001. Prismを使ったMVVM  
Prismを使うとこんな良いことが！  
・DataContextを使用せずにViewとViewModelの紐づけをしてくれる  
・プレーンなMVVMではINotifyPropertyChangedを継承してごにょごにょしてやらないといけなかったが、Prismで用意されているBindableBaseクラスを継承するだけでViewMode → Viewの変更通知を可能にする  
・ViewModel → Modelへ変更通知を行う際、ViewModelのプロパティにsetpropertyメソッドを使うだけで良い  
・コマンドの設定する場合はPrismで用意されているDelegeteCommandを使用することで楽に設定ができる。  
・InteractionRequestクラスで簡単にMessengerパターンを実装することが可能  
# 002.ReactivePropertyを使ったMVVM  
001のプロジェクトに追加で手を入れています。  
ReactivePropertyを使うとこんな良いことが！  
・Prismを使っても基本的にはプロパティとフィールドに同名の変数が必要だったが、ReactivePropertyではプロパティを宣言しさえしておけばそれでおｋ  
・加えて、Prismの時の様に、プロパティの中でSetPropertyメソッドを呼ぶ必要もなく、プレーンなプロパティのままでおｋ  
・変更通知を行うプロパティの型をReactiveProperty<T>にすれば、PrismではViewModelに継承していたBindableBaseクラスさえも不要になる  
・ViewModel ⇒ Model間の変更通知はFromObjectメソッドを使用することで可能  
・Model ⇒ ViewModel間の変更通知はToReactivePropertyメソッドを使用することで可能  
・ViewModel ⇔ Model間の変更通知はToReactivePropertyAsSynchronizedメソッドを使用することで可能  
・コマンドはReactiveCommandを使う事でCanExecuteも簡単に制御可能  
# 003. Messengerパターン+Behaviorを使ったMVVM  
002のプロジェクトに追加で手を入れています。  
Messengerパターンを使うとこんな良いことが！  
・ダイアログの表示は本来、View側で処理すべきだがそれをどう解決するか？ということで編み出されたパターン  
・Viewの責務をきちんとViewに持たせる事ができる  
・View と ViewModelの両方から参照できるDialogViewを作成  
・Viewからの変更通知をViewModelへ通知。ViewModelからInteractionRequestを使用してDialogViewを起動  
・DialogViewの結果(はいorいいえとか、003では選択したフォルダパス)をViewModelへ通知。ViewModelはその結果を受け取ってViewへ通知できる  
