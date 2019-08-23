namespace CrafterMacroExecution.Bean
{
    /// <summary>
    /// スキル情報を格納するクラス(インターフェイス)
    /// </summary>
    public interface ISkilInfoBean
    {
        /// <summary>
        /// スキル名を格納する
        /// </summary>
        string SkillName { get; set; }

        /// <summary>
        /// スキルタイプを格納する
        /// </summary>
        string SkillTypeV { get; set; }

        /// <summary>
        /// 必要CPをを格納する
        /// </summary>
        string CPV { get; set; }

        /// <summary>
        /// 作業工数を格納する
        /// </summary>
        string CraftSmanshipV { get; set; }

        /// <summary>
        /// 品質工程を格納する
        /// </summary>
        string CraftControlV { get; set; }

        /// <summary>
        /// 成功率
        /// </summary>
        string SuccessRateV { get; set; }

        /// <summary>
        /// タブType
        /// </summary>
        string TabType { get; set; }
    }

    /// <summary>
    /// スキル情報を格納するクラス
    /// </summary>
    public class SkilInfoBean : ISkilInfoBean
    {
        private string skillName;
        private string skillTypeV;
        private string cPV;
        private string craftSmanshipV;
        private string craftControlV;
        private string successRateV;
        private string tabType;

        /// <summary>
        /// スキル名定義
        /// </summary>
        public string SkillName
        {
            get { return this.skillName; }
            set { this.skillName = value; }
        }

        /// <summary>
        /// スキルタイプ定義
        /// </summary>
        public string SkillTypeV
        {
            get { return this.skillTypeV; }
            set { this.skillTypeV = value; }
        }

        /// <summary>
        /// 必要CP定義
        /// </summary>
        public string CPV
        {
            get { return this.cPV; }
            set { this.cPV = value; }
        }

        /// <summary>
        /// 作業工数定義
        /// </summary>
        public string CraftSmanshipV
        {
            get { return this.craftSmanshipV; }
            set { this.craftSmanshipV = value; }
        }

        /// <summary>
        /// 品質工程定義
        /// </summary>
        public string CraftControlV
        {
            get { return this.craftControlV; }
            set { this.craftControlV = value; }
        }

        /// <summary>
        /// 成功率定義
        /// </summary>
        public string SuccessRateV
        {
            get { return this.successRateV; }
            set { this.successRateV = value; }
        }

        /// <summary>
        /// タブType
        /// </summary>
        public string TabType
        {
            get { return this.tabType; }
            set { this.tabType = value; }
        }
    }
}

