using CrafterMacroExecution.Bean;

namespace CrafterMacroExecution
{
    public static class ReadLogEvents
    {

        public static void logline(string logInfo, CreateSettingBean createSettingBean)
        {
            // 18文字以下のログは読み捨てる
            // なぜならば、タイムスタンプ＋ログタイプのみのログだから
            if (logInfo.Length <= 18)
            {
                return;
            }

            // 装備が壊れた時のログを挿入する
            if (logInfo.Contains("の耐久度が10%以下になった。")
                 && createSettingBean.I修理チェック)
            {
                // ログ中断フラグを立てる
                this.RepairingFlg = true;
            }

            // 強化薬を使ったときの秒数を取得するイベント
            if (logInfo.Contains(createSettingBean.Iキャラ名 + " gains the effect of 強化薬 from " + createSettingBean.Iキャラ名 + " for ")
                && createSettingBean.I薬チェック)
            {
                // 格納する
                this.MedicineTime = int.Parse(logInfo.Substring(logInfo.Length - 15, 3));
            }

            // 飯を使ったときの秒数を取得するイベント
            if (logInfo.Contains(createSettingBean.Iキャラ名 + " gains the effect of 食事 from " + createSettingBean.Iキャラ名 + " for ")
                && createSettingBean.I飯チェック)
            {
                // 格納する
                this.FoodTime = int.Parse(logInfo.Substring(logInfo.Length - 16, 4));
            }

            // ろえな作成用蒐集品つけたときのイベントフラグ
            if (logInfo.Contains(createSettingBean.Iキャラ名 + " gains the effect of 蒐集品製作 from " + createSettingBean.Iキャラ名 + " for 9999.00 Seconds."))
            {
                this.roenaFlg = true;
            }

            // ろえな作成用蒐集品消えたときのイベントフラグ
            if (logInfo.Contains(createSettingBean.Iキャラ名 + " loses the effect of 蒐集品製作 from " + createSettingBean.Iキャラ名 + "."))
            {
                this.roenaFlg = false;
            }
        }
    }
}
