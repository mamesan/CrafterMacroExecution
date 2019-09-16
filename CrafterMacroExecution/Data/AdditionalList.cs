using System;

namespace CrafterMacroExecution.Data
{
    /// <summary>
    /// アディッショナル一覧を格納する
    /// </summary>
    public static class AdditionalList
    {
        // アディッショナル一覧
        public static string[] values = {
            "いったんindex下げる用",
            "ビエルゴの祝福",
            "工面算段",
            "工面算段II",
            "突貫作業",
            "ピース・バイ・ピース",
            "マニピュレーション",
            "イノベーション",
            "堅実作業",
            "堅実の心得",
            "倹約",
            "倹約II",
            "模範作業",
            "模範作業II",
            "秘訣",
            "コンファートゾーン",
            "ヘイスティタッチ",
            "ステディハンドII",
            "確信",
            "リクレイム"
        };

    }

    /// <summary>
    /// アディッショナル一覧を格納する拡張クラス
    /// </summary>
    public static class AdditionalListEdit
    {

        /// <summary>
        /// リストに存在しているValueかチェックをする
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConfirmCode(string value)
        {

            if (0 < Array.IndexOf(AdditionalList.values, value))
            {
                return true;
            }
            return false;
        }
    }
}