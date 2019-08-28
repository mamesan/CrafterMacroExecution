using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using CrafterMacroExecution.Bean;
using System;
using System.IO;
using Advanced_Combat_Tracker;
using System.Text;
using static CrafterMacroExecution.Utils.Const;

namespace CrafterMacroExecution.Utils
{
    /// <summary>
    /// ファイルコントロールを行うクラス
    /// </summary>
    public static class FileController
    {

        /// <summary>
        /// 全てのユーザコントロール情報を取得する
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static Control[] GetAllControls(Control top)
        {
            ArrayList buf = new ArrayList();
            foreach (Control c in top.Controls)
            {
                buf.Add(c);
                buf.AddRange(GetAllControls(c));
            }
            return (Control[])buf.ToArray(typeof(Control));
        }

        /// <summary>
        /// Configファイルの全てを取得する（CommandConfig.config）
        /// </summary>
        public static List<string> ReadAllXML(string filePath)
        {
            XmlDocument document = new XmlDocument();
            // ファイルから読み込む
            document.Load(filePath);

            // 配列に格納する
            List<string> list = new List<string>();
            // すべてのコマンドを回す
            foreach (XmlElement element in document.DocumentElement)
            {
                // マップに格納する
                list.Add(element.InnerText);
            }

            // マップを返却する
            return list;

        }

        /// <summary>
        /// テンプレートマクロを取得する
        /// </summary>
        /// <returns></returns>
        public static List<IPlayMacroInfoBean> GetTempMacroInfo(string filePath)
        {
            // 返却用インスタンスを生成する
            List<IPlayMacroInfoBean> list = new List<IPlayMacroInfoBean>();

            // 格納用インスタンスを生成
            IPlayMacroInfoBean playMacroInfoBean = new PlayMacroInfoBean();

            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, filePath);

            // XmlDocumentを取得する
            XmlDocument document = GetAllXmlDocumentInfo(settingsFile);

            foreach (XmlElement element in document.DocumentElement)
            {
                // マクロインフォを初期化する
                playMacroInfoBean = new PlayMacroInfoBean();

                // No
                playMacroInfoBean.No = element.GetAttribute(NO);
                // Wait
                playMacroInfoBean.Wait = element.GetAttribute(WAIT);
                // Text
                playMacroInfoBean.Text = element.InnerText;

                // リストに格納を行う
                list.Add(playMacroInfoBean);
            }

            return list;
        }

        /// <summary>
        /// アディッショナルリストを取得する
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<IPlayMacroInfoBean> GetAdditionalInfo(string filePath)
        {
            // 返却用インスタンスを生成する
            List<string> list = new List<string>();

            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, filePath);

            // XmlDocumentを取得する
            XmlDocument document = GetAllXmlDocumentInfo(settingsFile);

            foreach (XmlElement element in document.DocumentElement)
            {
                // リストに格納を行う
                list.Add(element.InnerText);
            }


            // 取得したマクロを、実行用に変換する
            List<IPlayMacroInfoBean> Adilist = new List<IPlayMacroInfoBean>();
            IPlayMacroInfoBean playMacroInfo = new PlayMacroInfoBean();

            playMacroInfo.Text = "/aaction clear";
            playMacroInfo.No = "アディ";
            playMacroInfo.Wait = "2";
            Adilist.Add(playMacroInfo);

            // アディッショナルリストをループする
            foreach (string tempStr in list)
            {
                playMacroInfo = new PlayMacroInfoBean();
                playMacroInfo.Text = "/aaction " + tempStr + " on";
                playMacroInfo.No = "アディ";
                playMacroInfo.Wait = "2";
                Adilist.Add(playMacroInfo);
            }

            return Adilist;
        }

        /// <summary>
        /// スキル情報一覧を取得する
        /// </summary>
        /// <returns></returns>
        public static List<ISkilInfoBean> GetSkilInfo(string filePath)
        {
            // 返却用インスタンスを生成する
            List<ISkilInfoBean> list = new List<ISkilInfoBean>();

            // 格納用インスタンスを生成
            ISkilInfoBean skilInfoBean = new SkilInfoBean();

            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, filePath);


            // XmlDocumentを取得する
            XmlDocument document = GetAllXmlDocumentInfo(settingsFile);

            foreach (XmlElement element in document.DocumentElement)
            {
                // 初期化する
                skilInfoBean = new SkilInfoBean();

                // スキル名
                skilInfoBean.SkillName = element.InnerText;
                // SkillType
                skilInfoBean.SkillTypeV = element.GetAttribute(SKILLTYPE_V);
                // CP
                skilInfoBean.CPV = element.GetAttribute(CP_V);
                // 作業
                skilInfoBean.CraftSmanshipV = element.GetAttribute(CRAFT_SMANSHIP_V);
                // 加工
                skilInfoBean.CraftControlV = element.GetAttribute(CRAFT_CONTROL_V);
                // 成功率
                skilInfoBean.SuccessRateV = element.GetAttribute(SUCCESS_RATE_V);
                // タブタイプ
                skilInfoBean.TabType = element.GetAttribute(TAB_TYPE_V);

                // リストに格納を行う
                list.Add(skilInfoBean);
            }

            return list;
        }

        /// <summary>
        /// お気に入り一覧を取得する
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFavorite(string filePath)
        {

            if (!File.Exists(ActGlobals.oFormActMain.AppDataFolder.FullName + "\\" + filePath))
            {
                return new List<string>();
            }

            // 返却用インスタンスを生成する
            List<string> list = new List<string>();

            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, filePath);

            // XmlDocumentを取得する
            XmlDocument document = GetAllXmlDocumentInfo(settingsFile);

            foreach (XmlElement element in document.DocumentElement)
            {
                // スキル名名
                list.Add(element.InnerText);
            }
            return list;
        }

        /// <summary>
        /// elementの情報を全て返却する
        /// </summary>
        /// <param name="settingsFile"></param>
        /// <returns></returns>
        private static XmlDocument GetAllXmlDocumentInfo(string settingsFile)
        {
            XmlDocument document = new XmlDocument();
            // ファイルから読み込む
            document.Load(settingsFile);

            return document;
        }

        /// <summary>
        /// 情報を、XMLに保存するメソッド
        /// </summary>
        /// <param name="key">キャラ名</param>
        /// <param name="cp">CP</param>
        /// <param name="CraftSmanship">作業精度</param>
        /// <param name="CraftControl">加工精度</param>
        /// <param name="filePath">ファイルパス</param>
        public static void SaveInfo(List<string> key, List<Dictionary<string, string>> dictionary, string filePath)
        {

            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, filePath);

            // ファイルの存在チェックを実施する
            // なければ新規作成
            if (!File.Exists(settingsFile))
            {
                // 新規XML作成
                CreateXML(settingsFile);
            }

            // 現在のxmlを取得する
            var xmlFile = XElement.Load(settingsFile);

            int i = 0;

            // XML書き込み処理
            if (dictionary.Count != 0)
            {
                foreach (Dictionary<string, string> temp in dictionary)
                {
                    XElement element = new XElement("element", key[i]);
                    foreach (KeyValuePair<string, string> pair in temp)
                    {
                        // キーを設定する
                        // 設定する
                        element.SetAttributeValue(pair.Key, pair.Value);

                    }
                    i++;
                    var person = element;
                    xmlFile.Add(person);
                }
            }
            else
            {
                foreach (string tempKey in key)
                {
                    XElement element = new XElement("element", tempKey);
                    var person = element;
                    xmlFile.Add(person);
                    i++;
                }
            }

            // 保存
            xmlFile.Save(settingsFile);

        }

        /// <summary>
        /// 情報を、XMLに保存するメソッド(List)
        /// </summary>
        /// <param name="key">キャラ名</param>
        /// <param name="cp">CP</param>
        /// <param name="CraftSmanship">作業精度</param>
        /// <param name="CraftControl">加工精度</param>
        /// <param name="filePath">ファイルパス</param>
        public static void SaveListInfo(string key, List<Dictionary<string, string>> dictionary, string filePath)
        {

            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, filePath);

            // ファイルの存在チェックを実施する
            // なければ新規作成
            if (!File.Exists(settingsFile))
            {
                // 新規XML作成
                CreateXML(settingsFile);
            }

            // 現在のxmlを取得する
            var xmlFile = XElement.Load(settingsFile);

            var element = new XElement("element", key);

            foreach (Dictionary<string, string> temp in dictionary)
            {
                // 設定する
                element.SetAttributeValue(temp.Keys.ToString(), temp.Values);
            }

            var person = element;

            xmlFile.Add(person);

            // 保存
            xmlFile.Save(settingsFile);

        }

        /// <summary>
        /// 新しくXMLを生成する
        /// </summary>
        /// <param name="settingsFile"></param>
        private static void CreateXML(string settingsFile)
        {
            FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.Indentation = 1;
            xWriter.IndentChar = '\t';
            xWriter.WriteStartDocument(true);
            xWriter.WriteStartElement("Config");    // <Config>
            xWriter.WriteEndElement();  // </Config>
            xWriter.WriteEndDocument(); // Tie up loose ends (shouldn't be any)
            xWriter.Flush();    // Flush the file buffer to disk
            xWriter.Close();
        }

        /// <summary>
        /// キャラクター情報を、XMLから読み込むメソッド
        /// </summary>
        /// <param name="key">キャラクター名</param>
        /// <returns></returns>
        public static ICharacterBean ReadCharacterInfo(string key)
        {

            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, FILE_PATH_CHARAINFO);

            // 返却用Beanクラスを生成する
            ICharacterBean characterBean = new CharacterBean();

            // ドキュメントの情報を取得する
            XmlElement element = GetXmlElementInfo(key, settingsFile);

            // 取得したドキュメントがNullでない場合
            if (element != null)
            {
                // CP
                characterBean.CP = element.GetAttribute(CP);
                // 作業精度
                characterBean.CraftSmanship = element.GetAttribute(CRAFT_SMANSHIP);
                // 加工精度
                characterBean.CraftControl = element.GetAttribute(CRAFT_CONTROL);
                // 木工師
                characterBean.Carpenter = element.GetAttribute(CARPENTER);
                // 鍛冶師
                characterBean.Blacksmith = element.GetAttribute(BLACKSMITH);
                // 甲冑師
                characterBean.Armorer = element.GetAttribute(ARMORER);
                // 彫金師
                characterBean.Goldsmith = element.GetAttribute(GOLDSMITH);
                // 革細工師
                characterBean.Leatherworker = element.GetAttribute(LEATHERWORKER);
                // 裁縫師
                characterBean.Weaver = element.GetAttribute(WEAVER);
                // 錬金術師
                characterBean.Alchemist = element.GetAttribute(ALCHEMIST);
                // 調理師
                characterBean.Culinarian = element.GetAttribute(CULINARIAN);
                // チョコボ師
                characterBean.ChocoboMeister = element.GetAttribute(CHOCOBO);
                // 例のあの人
                characterBean.AwayukiKusushi = element.GetAttribute(AWAYUKIKUSUSHI);
                // マイスター1
                characterBean.Meister1 = element.GetAttribute(MEISTER1);
                // マイスター2
                characterBean.Meister2 = element.GetAttribute(MEISTER2);
                // マイスター3
                characterBean.Meister3 = element.GetAttribute(MEISTER3);

                // 返却する
                return characterBean;

            }
            return null;
        }

        /// <summary>
        /// マクロ情報を、XMLから読み込むメソッド
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IMacroInfoBean ReadMacroInfo(string key)
        {
            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, FILE_PATH_MACROINFO);

            // 返却用Beanクラスを生成する
            IMacroInfoBean macroInfoBean = new MacroInfoBean();

            // ドキュメントの情報を取得する
            XmlElement element = GetXmlElementInfo(key, settingsFile);

            // 取得したドキュメントがNullでない場合
            if (element != null)
            {
                // 必要工数
                macroInfoBean.NecessaryManHours = element.GetAttribute(NECESSARY_MAN_HOURS);
                // 星数
                macroInfoBean.StarCount = element.GetAttribute(STAR_COUNT);
                // 必要品質
                macroInfoBean.CraftControlCount = element.GetAttribute(CRAFTCONTROL_COUNT);
                // 作るもの
                macroInfoBean.WhatMakes = element.GetAttribute(WHAT_MAKES);

                // 返却する
                return macroInfoBean;

            }
            return null;
        }

        /// <summary>
        /// XMLから情報を取得する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="settingsFile">ファイルパス</param>
        /// <returns></returns>
        private static XmlElement GetXmlElementInfo(string key, string settingsFile)
        {

            XmlDocument document = new XmlDocument();
            // ファイルの存在チェックを実施する
            if (File.Exists(settingsFile))
            {
                // ファイルから読み込む
                document.Load(settingsFile);

                foreach (XmlElement element in document.DocumentElement)
                {
                    // 要素の内容
                    string text = element.InnerText;

                    if (key.Equals(text))
                    {
                        // keyと一致したら、返却を行う
                        return element;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 指定されたXMLのキー項目を削除する
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filePath">ファイルパス</param>
        public static void RemoveXMLInfo(string key, string filePath)
        {

            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, filePath);

            // インスタンス生成
            XmlDocument xml = new XmlDocument();

            // 現在のxmlを取得する
            xml.Load(settingsFile);

            // ループ処理を実施する
            for (int i = 0; xml.ChildNodes[1].ChildNodes.Count > i; i++)
            {
                // 比較対象のテキストを取得する
                var targetKey = xml.ChildNodes[1].ChildNodes[i].InnerText.ToString();

                // Keyと一致しているか比較を行う
                if (targetKey.Equals(key))
                {
                    // 要素の削除を行う
                    xml.ChildNodes[1].RemoveChild(xml.ChildNodes[1].ChildNodes[i]);
                }
            }
            // 対象xmlを保存する
            xml.Save(settingsFile);
        }

        /// <summary>
        /// 指定されたXMLのキー項目を編集する(キャラクター情報)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cp"></param>
        /// <param name="CraftSmanship"></param>
        /// <param name="CraftControl"></param>
        /// <param name="Carpenter"></param>
        /// <param name="Blacksmith"></param>
        /// <param name="Armorer"></param>
        /// <param name="Goldsmith"></param>
        /// <param name="Leatherworker"></param>
        /// <param name="Weaver"></param>
        /// <param name="Alchemist"></param>
        /// <param name="Culinarian"></param>
        /// <param name="ChocoboMeister"></param>
        /// <param name="AwayukiKusushi"></param>
        /// <param name="Meister1"></param>
        /// <param name="Meister2"></param>
        /// <param name="Meister3"></param>
        public static void EditCharacterInfo(
            string key,
            string cp,
            string CraftSmanship,
            string CraftControl,
            string Carpenter,
            string Blacksmith,
            string Armorer,
            string Goldsmith,
            string Leatherworker,
            string Weaver,
            string Alchemist,
            string Culinarian,
            string ChocoboMeister,
            string AwayukiKusushi,
            string Meister1,
            string Meister2,
            string Meister3)
        {
            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, FILE_PATH_CHARAINFO);

            // インスタンス生成
            XmlDocument xml = new XmlDocument();
            // 現在のxmlを取得する
            xml.Load(settingsFile);

            // xml情報を取得する
            int i = getXmlNodeInfo(key, settingsFile, xml);
            // cp
            xml.ChildNodes[1].ChildNodes[i].Attributes[0].InnerText = cp;
            // 作業精度
            xml.ChildNodes[1].ChildNodes[i].Attributes[1].InnerText = CraftSmanship;
            // 加工精度
            xml.ChildNodes[1].ChildNodes[i].Attributes[2].InnerText = CraftControl;
            // 木工師
            xml.ChildNodes[1].ChildNodes[i].Attributes[3].InnerText = Carpenter;
            // 鍛冶師
            xml.ChildNodes[1].ChildNodes[i].Attributes[4].InnerText = Blacksmith;
            // 甲冑師
            xml.ChildNodes[1].ChildNodes[i].Attributes[5].InnerText = Armorer;
            // 彫金師
            xml.ChildNodes[1].ChildNodes[i].Attributes[6].InnerText = Goldsmith;
            // 革細工師
            xml.ChildNodes[1].ChildNodes[i].Attributes[7].InnerText = Leatherworker;
            // 裁縫師
            xml.ChildNodes[1].ChildNodes[i].Attributes[8].InnerText = Weaver;
            // 錬金術師
            xml.ChildNodes[1].ChildNodes[i].Attributes[9].InnerText = Alchemist;
            // 調理師
            xml.ChildNodes[1].ChildNodes[i].Attributes[10].InnerText = Culinarian;
            // チョコボ師
            xml.ChildNodes[1].ChildNodes[i].Attributes[11].InnerText = ChocoboMeister;
            // 例のあの人
            xml.ChildNodes[1].ChildNodes[i].Attributes[12].InnerText = AwayukiKusushi;
            // マイスター1
            xml.ChildNodes[1].ChildNodes[i].Attributes[13].InnerText = Meister1;
            // マイスター2
            xml.ChildNodes[1].ChildNodes[i].Attributes[14].InnerText = Meister2;
            // マイスター3
            xml.ChildNodes[1].ChildNodes[i].Attributes[15].InnerText = Meister3;

            // 対象xmlを保存する
            xml.Save(settingsFile);
        }

        /// <summary>
        /// 指定されたXMLのキー項目を編集する(マクロ情報)
        /// </summary>
        /// <param name="key">マクロ名</param>
        /// <param name="NecessaryManHours">必要工数</param>
        /// <param name="StarCount">星数</param>
        /// <param name="CraftControlCount">必要品質</param>
        /// <param name="filePath">ファイルパス</param>
        public static void EditMacroInfo(string key, string NecessaryManHours, string StarCount, string CraftControlCount, string WhatMakes)
        {
            // 設定ファイル保存先パス
            string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, FILE_PATH_MACROINFO);

            // インスタンス生成
            XmlDocument xml = new XmlDocument();
            // 現在のxmlを取得する
            xml.Load(settingsFile);

            // xml情報を取得する
            int i = getXmlNodeInfo(key, settingsFile, xml);
            // 必要工数
            xml.ChildNodes[1].ChildNodes[i].Attributes[0].InnerText = NecessaryManHours;
            // 星数
            xml.ChildNodes[1].ChildNodes[i].Attributes[1].InnerText = StarCount;
            // 必要品質
            xml.ChildNodes[1].ChildNodes[i].Attributes[2].InnerText = CraftControlCount;
            // 作るもの
            xml.ChildNodes[1].ChildNodes[i].Attributes[3].InnerText = WhatMakes;

            // 対象xmlを保存する
            xml.Save(settingsFile);
        }

        /// <summary>
        /// XMLから情報を取得する
        /// </summary>
        /// <param name="key"></param>
        /// <param name="settingsFile"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        private static int getXmlNodeInfo(string key, string settingsFile, XmlDocument xml)
        {
            // ループ処理を実施する
            for (int i = 0; xml.ChildNodes[1].ChildNodes.Count > i; i++)
            {
                // 比較対象のテキストを取得する
                var targetKey = xml.ChildNodes[1].ChildNodes[i].InnerText.ToString();

                // Keyと一致しているか比較を行う
                if (targetKey.Equals(key))
                {
                    return i;
                }

            }
            return -1;
        }
    }
}