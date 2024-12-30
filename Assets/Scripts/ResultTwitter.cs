using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultTwitter : MonoBehaviour {
    // ※ここの初期値ではなく、インスペクタ上の値を変えてください
    [SerializeField] private string gameID = "unityroom_game_id"; // unityroom上で投稿したゲームのID
    [SerializeField] private string tweetText1 = "洞窟から脱出できました！スコア";
    [SerializeField] private string tweetText2 = "点を獲得できました";
    [SerializeField] private string hashTags = "#unityroom #LightTheWay";

    // ※ スコアマネージャーというものが存在すると仮定します
    // 貴方の作成しているゲームの事情に合わせてください
    // インスペクタ上でスコアマネージャーコンポーネントを持つゲームオブジェクトをセット
    public ScoreManager scoreManager;

    // ツイートボタンから呼び出す公開メソッド
    public void Tweet() {
        int score = scoreManager.GetScore(); // ※ 何らかの方法でスコアの値を取得(貴方のゲーム事情に合わせてください)
        string tweetMessage = tweetText1 + score + tweetText2 + "\n" + hashTags;
        naichilab.UnityRoomTweet.Tweet(gameID, tweetMessage);
    }
}