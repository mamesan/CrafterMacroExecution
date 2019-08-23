namespace CrafterMacroExecution.Bean
{
    /// <summary>
    /// ちょこぼ情報を格納するクラス(インターフェイス)
    /// </summary>
    public interface IChocoboInfoBean
    {
        /// <summary>
        /// NMLの名前を格納する
        /// </summary>
        string XMLName { get; set; }

        /// <summary>
        /// 回数を格納する
        /// </summary>
        string Count { get; set; }

        /// <summary>
        /// 作るものを格納する
        /// </summary>
        string WhatMakes { get; set; }

        /// <summary>
        /// ディレイを格納する
        /// </summary>
        string Delay { get; set; }

        /// <summary>
        /// 職
        /// </summary>
        string Job { get; set; }

        /// <summary>
        /// キー
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// X座標
        /// </summary>
        string X { get; set; }

        /// <summary>
        /// Y座標
        /// </summary>
        string Y { get; set; }
    }

    /// <summary>
    /// ちょこぼ情報を格納するクラス
    /// </summary>
    public class ChocoboInfoBean : IChocoboInfoBean
    {
        private string xMLName;
        private string count;
        private string whatMakes;
        private string delay;
        private string job;
        private string key;
        private string x;
        private string y;

        /// <summary>
        /// CXML名P定義
        /// </summary>
        public string XMLName
        {
            get { return this.xMLName; }
            set { this.xMLName = value; }
        }

        /// <summary>
        /// 回数定義
        /// </summary>
        public string Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        /// <summary>
        /// 作るもの定義
        /// </summary>
        public string WhatMakes
        {
            get { return this.whatMakes; }
            set { this.whatMakes = value; }
        }

        /// <summary>
        /// ディレイの定義
        /// </summary>
        public string Delay
        {
            get { return this.delay; }
            set { this.delay = value; }
        }

        /// <summary>
        /// 職の定義
        /// </summary>
        public string Job
        {
            get { return this.job; }
            set { this.job = value; }
        }

        /// <summary>
        /// キーの定義
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Xの定義
        /// </summary>
        public string X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        /// <summary>
        /// Yの定義
        /// </summary>
        public string Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
    }
}

