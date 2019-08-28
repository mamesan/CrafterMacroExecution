namespace CrafterMacroExecution.Bean
{
    /// <summary>
    /// マクロ情報を格納するクラス(インターフェイス)
    /// </summary>
    public interface IMacroInfoBean
    {
        /// <summary>
        /// マクロ名を格納する
        /// </summary>
        string MacroName { get; set; }

        /// <summary>
        /// 必要工数を格納する
        /// </summary>
        string NecessaryManHours { get; set; }

        /// <summary>
        /// 星数を格納する
        /// </summary>
        string StarCount { get; set; }

        /// <summary>
        /// 必要品質を格納する
        /// </summary>
        string CraftControlCount { get; set; }

        /// <summary>
        /// 作るもの
        /// </summary>
        string WhatMakes { get; set; }
    }

    /// <summary>
    /// マクロ情報を格納するクラス
    /// </summary>
    public class MacroInfoBean : IMacroInfoBean
    {
        private string macroName;
        private string necessaryManHours;
        private string starCount;
        private string craftControlCount;
        private string whatMakes;

        /// <summary>
        /// マクロ名定義
        /// </summary>
        public string MacroName
        {
            get { return this.macroName; }
            set { this.macroName = value; }
        }

        /// <summary>
        /// 作業精度定義
        /// </summary>
        public string NecessaryManHours
        {
            get { return this.necessaryManHours; }
            set { this.necessaryManHours = value; }
        }

        /// <summary>
        /// 星数定義
        /// </summary>
        public string StarCount
        {
            get { return this.starCount; }
            set { this.starCount = value; }
        }

        /// <summary>
        /// 必要品質定義
        /// </summary>
        public string CraftControlCount
        {
            get { return this.craftControlCount; }
            set { this.craftControlCount = value; }
        }

        /// <summary>
        /// 作るもの
        /// </summary>
        public string WhatMakes
        {
            get { return this.whatMakes; }
            set { this.whatMakes = value; }
        }
    }
}

