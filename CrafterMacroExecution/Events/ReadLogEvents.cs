namespace CrafterMacroExecution
{
    public static class ReadLogEvents
    {

        public static void logline(string logInfo)
        {
            // キャラの名前を先に取得しておく
            string CharaName = this.キャラリストボックス.Text;

            // 18文字以下のログは読み捨てる
            // なぜならば、タイムスタンプ＋ログタイプのみのログだから
            if (logInfo.Length <= 18)
            {
                return;
            }

            // 装備が壊れた時のログを挿入する
            if (logInfo.Contains("の耐久度が10%以下になった。")
                 && this.checkBox7.Checked)
            {
                // ログ中断フラグを立てる
                this.RepairingFlg = true;
            }

            // 強化薬を使ったときの秒数を取得するイベント
            if (logInfo.Contains(CharaName + " gains the effect of 強化薬 from " + CharaName + " for ")
                && this.checkBox4.Checked)
            {
                // 格納する
                this.MedicineTime = int.Parse(logInfo.logLine.Substring(logInfo.logLine.Length - 15, 3));
            }

            // 飯を使ったときの秒数を取得するイベント
            if (logInfo.Contains(CharaName + " gains the effect of 食事 from " + CharaName + " for ")
                && this.checkBox3.Checked)
            {
                // 格納する
                this.FoodTime = int.Parse(logInfo.logLine.Substring(logInfo.logLine.Length - 16, 4));
            }

            // ろえな作成用蒐集品つけたときのイベントフラグ
            if (logInfo.Contains(CharaName + " gains the effect of 蒐集品製作 from " + CharaName + " for 9999.00 Seconds."))
            {
                this.roenaFlg = true;
            }

            // ろえな作成用蒐集品消えたときのイベントフラグ
            if (logInfo.Contains(CharaName + " loses the effect of 蒐集品製作 from " + CharaName + "."))
            {
                this.roenaFlg = false;
            }
        }
    }
}
