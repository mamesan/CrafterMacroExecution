namespace CrafterMacroExecution.Bean
{
    /// <summary>
    /// テンプレマクロ情報を格納するクラス(インターフェイス)
    /// </summary>
    public interface IPlayMacroInfoBean
    {
        /// <summary>
        /// Noを格納する
        /// </summary>
        string No { get; set; }

        /// <summary>
        /// 待機時間を格納する
        /// </summary>
        string Wait { get; set; }

        /// <summary>
        /// マクロを格納する
        /// </summary>
        string Text { get; set; }

    }

    /// <summary>
    /// マクロ情報を格納するクラス
    /// </summary>
    public class PlayMacroInfoBean : IPlayMacroInfoBean
    {
        private string no;
        private string wait;
        private string text;

        /// <summary>
        /// No定義
        /// </summary>
        public string No
        {
            get { return this.no; }
            set { this.no = value; }
        }

        /// <summary>
        /// 待機時間定義
        /// </summary>
        public string Wait
        {
            get { return this.wait; }
            set { this.wait = value; }
        }

        /// <summary>
        /// マクロ定義
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
    }
}

