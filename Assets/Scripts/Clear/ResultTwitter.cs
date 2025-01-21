using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <see cref="https://qiita.com/kurisaka_konabe/items/d1142b86a43d2f537a88">unityroomで得点をツイートする実装例【初心者向け】</see>
///

public class ResultTwitter : MonoBehaviour {
    [SerializeField] private string gameId = "unityroom_game_id"; // unityroom上のゲームID
    [SerializeField] private string tweetPrefix = "洞窟から脱出できました！スコア";
    [SerializeField] private string tweetSuffix = "点を獲得できました";
    [SerializeField] private string hashTags = "#unityroom #LightTheWay";
    [SerializeField] private ScoreManager scoreManager;

    /// <summary>
    /// ツイートボタンから呼び出すメソッド
    /// </summary>
    public void Tweet() {
        // スコアの取得とツイートメッセージの生成
        string tweetMessage = GenerateTweetMessage();
        PostTweet(tweetMessage);
    }

    /// <summary>
    /// ツイートメッセージを生成
    /// </summary>
    /// <returns>生成されたツイートメッセージ</returns>
    private string GenerateTweetMessage() {
        if (scoreManager == null) {
            Debug.LogError("ScoreManagerが設定されていません。インスペクタで設定してください。");
            return string.Empty;
        }

        int score = scoreManager.GetScore();
        return $"{tweetPrefix}{score}{tweetSuffix}\n{hashTags}";
    }

    /// <summary>
    /// 実際にツイートを投稿
    /// </summary>
    /// <param name="tweetMessage">投稿するツイートメッセージ</param>
    private void PostTweet(string tweetMessage) {
        if (string.IsNullOrEmpty(tweetMessage)) {
            Debug.LogWarning("ツイートメッセージが空のため、ツイートをスキップしました。");
            return;
        }

        naichilab.UnityRoomTweet.Tweet(gameId, tweetMessage);
    }
}