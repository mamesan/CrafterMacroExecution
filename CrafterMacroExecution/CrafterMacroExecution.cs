using System;
using System.Linq;
using Advanced_Combat_Tracker;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.IO;
using CrafterMacroExecution.Utils;
using CrafterMacroExecution.Bean;
using CrafterMacroExecution.Exceptions;
using static CrafterMacroExecution.Data.KeyCodeList;
using static CrafterMacroExecution.Utils.Const;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CrafterMacroExecution.Impl;
using CrafterMacroExecution.Events;

namespace CrafterMacroExecution
{
    public partial class CrafterMacroExecution : UserControl, IActPluginV1
    {
        // ACTFF14プラグインを取得する
        private SettingsSerializer xmlSettings;

        // 実行構成を格納する 
        private List<ChocoboInfoBean> chocoboInfoBeanList = new List<ChocoboInfoBean>();

        // 残り回数取得用
        public int _count;
        // 全体残り回数取得用
        private int _AllCount;

        // ループ処理一時停止用
        public Boolean loopFlg = true;
        public Boolean loopFlg_Go = true;
        // 終了フラグ用
        public Boolean EndFlg = true;

        // ログ修理中断フラグ
        public Boolean RepairingFlg = false;
        // ログ薬中断フラグ
        public DateTime MedicineDateTime;
        // ログ飯中断フラグ
        public DateTime FoodDateTime;
        // ろえな用のフラグ
        public Boolean roenaFlg = false;

        // 飯の時間
        private int FoodTime = 0;
        // 薬の時間
        private int MedicineTime = 0;

        private ICharacterFormBean characterFormBean = new CharacterFormBean();

        [DllImport("user32")]
        public static extern void shortGetAsyncKeyState(Keys vKey);

        // C#
        // Windows API functions and constants
        [DllImport("user32")]
        static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, Keys vk);
        [DllImport("user32")]
        static extern int UnregisterHotKey(IntPtr hwnd, int id);
        [DllImport("kernel32",
        EntryPoint = "GlobalAddAtomA")]
        static extern short GlobalAddAtom(string lpString);
        [DllImport("kernel32")]
        static extern short GlobalDeleteAtom(short nAtom);

        private const int MOD_ALT = 0x01;
        private const int MOD_CONTROL = 0x02;
        private const int MOD_SHIFT = 0x04;
        private const int MOD_WIN = 0x08;
        private const int WM_HOTKEY = 0x312;

        short hotkeyID;

        /// <summary>
        /// 初期処理
        /// </summary>
        public CrafterMacroExecution()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void DeInitPlugin()
        {
            ACTInitSetting.SaveSettings(this.xmlSettings);
        }

        /// <summary>
        /// 初期表示処理
        /// </summary>
        /// <param name="pluginScreenSpace"></param>
        /// <param name="pluginStatusText"></param>
        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginScreenSpace.Controls.Add(this);
            this.Dock = DockStyle.Fill;

            // インターフェイス情報を格納する
            this.xmlSettings = new SettingsSerializer(this);

            // コントロール情報を取得する
            Control[] ct = FileController.GetAllControls(this);

            // 取得したコントロール情報を全て回し、初期表示用の情報を格納する
            foreach (Control tempct in ct)
            {
                if (tempct.Name.IndexOf(INITACTSETTING) > 0)
                {
                    // コントロールリストの情報を格納する
                    this.xmlSettings.AddControlSetting(tempct.Name, tempct);
                }
            }

            // 設定ファイルを読み込む
            ACTInitSetting.LoadSettings(xmlSettings);

            pluginScreenSpace.Text = TABNAME;
            pluginStatusText.Text = GOSTATUS;


            // this.言語切り替え.Text = "日本語";

            // キャラクター情報XMLが格納されているパスを一時取得する
            string path = ActGlobals.oFormActMain.AppDataFolder.FullName + "\\" + Const.FILE_PATH_CHARAINFO;

            // キャラクター情報格納用インスタンスを生成する
            List<string> Clist = new List<string>();

            // fileが存在してる場合、処理を実子する
            if (File.Exists(path))
            {
                // キャラクター情報の読み込み
                Clist = FileController.ReadAllXML(path);
            }

            // 値を格納する
            foreach (string str in Clist)
            {
                this.キャラリストボックス.Items.Add(str);
                this.キャラ一覧.Items.Add(str);
                // Hiddenが一致した場合
                /**
                if (this.hiddenキャラ名textBox_初期読込.Text.Equals(str))
                {
                    this.キャラリストボックス.SelectedItem = str;
                }
                */
            }

            string selectCharacter = this.キャラリストボックス.Text;

            // マクロ一覧を取得する
            List<string> list = Utils.Utils.FindFile(FILE_PATH_TEMPMACRO, FILEEXTENSION_XML);

            // 一行空欄を挿入する
            this.実行マクロ名_comboBox.Items.Add("");

            // 取得したマクロ名一覧を格納する
            foreach (string str in list)
            {
                this.Macro一覧.Items.Add(str.Substring(0, str.Length - 4));
                this.実行マクロ名_comboBox.Items.Add(str);
            }

            // 初期表示時の、オプションボタン活性、非活性をコントロールする
            this.setInitOption();
            /*/
            // 後々、全ボタンの処理をこっちに寄せたい
            this.マクロ内容修正ボタン.Click += (s1, e1) =>
            {
                using (アクション一覧 f = new アクション一覧())
                {
                    f.imageList2.ImageSize = new Size(30, 30);

                    // 何も選択されていない場合は、返却を行う
                    if (!String.IsNullOrWhiteSpace(this.Macro一覧.Text))
                    {

                        // マクロ情報を取得する
                        List<IPlayMacroInfoBean> Macrolist = FileController.GetTempMacroInfo(FILE_PATH_TEMPMACRO + this.Macro一覧.Text + ".xml");

                        // 指定されたマクロリストの内容を、チェックリストに格納する
                        foreach (IPlayMacroInfoBean playMacroInfoBean in Macrolist)
                        {
                            // スキル名のみトリムした名前を、リストに格納する
                            string Skill = playMacroInfoBean.Text.Substring(4);


                            Image original = Image.FromFile(ActGlobals.oFormActMain.AppDataFolder.FullName + "\\" + FILE_PATH_ICON + Skill + ".png");
                            Image thumbnail = Utils.Utils.createThumbnail(original, 25, 25);

                            f.imageList2.Images.Add(Skill, thumbnail);
                            f.listView2.LargeImageList = f.imageList2;

                            f.listView2.Items.Add(Skill, Skill);
                        }
                    }

                    // キャラクター情報を格納する
                    foreach (string str in this.キャラリストボックス.Items)
                    {
                        f.キャラリストボックス_シュミ.Items.Add(str);
                    }

                    // ダイアログを表示する
                    f.ShowDialog();

                    if (f.CloseFlg)
                    {
                        // 一度全てのアイテムを削除する
                        this.listBox1.Items.Clear();

                        // 設定したリストビュアーを取得し、設定する
                        foreach (ListViewItem lv in f.listView2.Items)
                        {
                            this.listBox1.Items.Add(lv.Text);
                        }
                    }
                    // リストビューを初期化する
                    f.Close();
                }
            };

            // マクロ実行構成を生成する
            マクロ実行構成 f2 = new マクロ実行構成();
            // 実行構成ボタン
            this.実行構成ボタン.Click += (s1, e1) =>
            {
                {
                    foreach (string str in this.実行マクロ名_comboBox.Items)
                    {
                        if (!f2.コンボボックス_実行マクロ一覧1.Items.Contains(str))
                        {
                            f2.コンボボックス_実行マクロ一覧1.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧2.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧3.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧4.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧5.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧6.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧7.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧8.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧9.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧10.Items.Add(str);
                            f2.コンボボックス_実行マクロ一覧11.Items.Add(str);
                        }
                    }

                    // 表示する
                    f2.ShowDialog();
                    if (!f2.CancelFlg)
                    {
                        List<ChocoboInfoBean> beanList = new List<ChocoboInfoBean>();

                        int chocoboCount = 1;

                        // 格納されているもの分ループ処理を実施する
                        foreach (ChocoboInfoBean chocoboInfoBean in f2.list)
                        {
                            beanList.Add(this.GetChocoboInfoBean(chocoboInfoBean, chocoboCount));

                            chocoboCount++;
                        }

                        // 取得したちょこぼリストを生成し、格納する
                        this.chocoboInfoBeanList = beanList;
                    }
                }
            };

            // 一回マクロを選択した場合
            this.マクロ一回実行_radioButton.CheckedChanged += (e1, s1) =>
            {
                this.実行マクロ名_comboBox.Enabled = true;
                this.作るものcomboBox.Enabled = true;
                this.ディレイ_textBox.Enabled = true;
                this.実行回数_textBox.Enabled = true;
                this.実行構成ボタン.Enabled = false;
            };

            // 複数マクロを選択した場合
            this.マクロ複数_radioButton.CheckedChanged += (e1, s1) =>
            {
                this.実行マクロ名_comboBox.Enabled = false;
                this.作るものcomboBox.Enabled = false;
                this.ディレイ_textBox.Enabled = false;
                this.実行回数_textBox.Enabled = false;
                this.実行構成ボタン.Enabled = true;
            };

            // 中断ボタン処理
            this.中断ボタン.Click += (s1, e1) =>
            {
                if (!this.loopFlg)
                {
                    this.中断ボタン.Text = "中断";
                    this.中断ボタン.Enabled = false;
                    this.loopFlg_Go = true;
                }
                else
                {
                    this.中断ボタン.Text = "再開";
                    this.中断ボタン.Enabled = false;
                    this.loopFlg = false;
                    this.loopFlg_Go = false;
                }
            };

            // 終了ボタン処理
            this.終了ボタン.Click += (s1, e1) =>
            {
                this.終了ボタン.Enabled = false;
                this.EndFlg = false;
            };

            // キャラクターのお名前制御
            this.textBox6.TextChanged += (s1, e1) =>
            {
                for (int i = 0; i < this.キャラ一覧.Items.Count; i++)
                {
                    if (this.キャラ一覧.Items[i].ToString() == this.textBox6.Text)
                    {
                        this.例のあの人更新ボタン.Enabled = true;
                        return;
                    }
                }
                this.例のあの人更新ボタン.Enabled = false;
            };

            // macroのお名前欄の制御
            this.textBox8.TextChanged += (s1, e1) =>
            {
                for (int i = 0; i < this.Macro一覧.Items.Count; i++)
                {
                    if (this.Macro一覧.Items[i].ToString() == this.textBox8.Text)
                    {
                        this.マクロ編集ボタン.Enabled = true;
                        return;
                    }
                }
                this.マクロ編集ボタン.Enabled = false;
            };
            */

            // オプションタブのイベントを呼び出す
            this.OptionEvents();

            // イベントタブの一覧
            ButtonEventList();
        }

        private void ButtonEventList()
        {
            キャラクター追加ボタン.Click += (s1, e1) =>
            {
                EditCharacter.createCharacterInfo(this);
            };
            キャラクター情報更新ボタン.Click += (s1, e1) =>
            {
                EditCharacter.editCharacterInfo(this);
            };
            キャラクタ情報削除ボタン.Click += (s1, e1) =>
            {
                EditCharacter.deleteCharacterInfo(this);
            };
            Macro一覧.SelectedIndexChanged += (s1, e1) =>
            {
                string listBoxText = ((ListBox)s1).Text;
                if (String.IsNullOrWhiteSpace(listBoxText)) return;
                MacroImpl.Macro一覧_click(this, listBoxText);
            };
            マクロ削除ボタン.Click += (s1, e1) =>
            {
                string listBoxText = ((ListBox)s1).Text;
                if (String.IsNullOrWhiteSpace(listBoxText)) return;
                MacroImpl.マクロ削除ボタン_click(this, listBoxText);
            };
            マクロ追加ボタン.Click += (s1, e1) =>
            {
                MacroImpl.マクロ追加ボタン_click(this);
            };
            マクロ編集ボタン.Click += (s1, e1) =>
            {
                MacroImpl.マクロ編集ボタン_click(this);
            };
        }




        /// <summary>
        /// チョコボリストに、必要な情報を格納していくクラス
        /// </summary>
        /// <param name="bean"></param>
        /// <param name="chocoboCount"></param>
        /// <returns></returns>
        private ChocoboInfoBean GetChocoboInfoBean(ChocoboInfoBean bean, int chocoboCount)
        {
            /**
            if (chocoboCount == 1)
            {
                this.setBeanInfo(
                    this.ちょこぼ1KeyRadio_初期読込.Checked,
                    this.ちょこぼ1Key_初期読込,
                    this.ちょこぼ1X座標_初期読込,
                    this.ちょこぼ1Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 2)
            {
                this.setBeanInfo(
                    this.ちょこぼ2KeyRadio_初期読込.Checked,
                    this.ちょこぼ2Key_初期読込,
                    this.ちょこぼ2X座標_初期読込,
                    this.ちょこぼ2Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 3)
            {
                this.setBeanInfo(
                    this.ちょこぼ3KeyRadio_初期読込.Checked,
                    this.ちょこぼ3Key_初期読込,
                    this.ちょこぼ3X座標_初期読込,
                    this.ちょこぼ3Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 4)
            {
                this.setBeanInfo(
                    this.ちょこぼ4KeyRadio_初期読込.Checked,
                    this.ちょこぼ4Key_初期読込,
                    this.ちょこぼ4X座標_初期読込,
                    this.ちょこぼ4Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 5)
            {
                this.setBeanInfo(
                    this.ちょこぼ5KeyRadio_初期読込.Checked,
                    this.ちょこぼ5Key_初期読込,
                    this.ちょこぼ5X座標_初期読込,
                    this.ちょこぼ5Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 6)
            {
                this.setBeanInfo(
                    this.ちょこぼ6KeyRadio_初期読込.Checked,
                    this.ちょこぼ6Key_初期読込,
                    this.ちょこぼ6X座標_初期読込,
                    this.ちょこぼ6Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 7)
            {
                this.setBeanInfo(
                    this.ちょこぼ7KeyRadio_初期読込.Checked,
                    this.ちょこぼ7Key_初期読込,
                    this.ちょこぼ7X座標_初期読込,
                    this.ちょこぼ7Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 8)
            {
                this.setBeanInfo(
                    this.ちょこぼ8KeyRadio_初期読込.Checked,
                    this.ちょこぼ8Key_初期読込,
                    this.ちょこぼ8X座標_初期読込,
                    this.ちょこぼ8Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 9)
            {
                this.setBeanInfo(
                    this.ちょこぼ9KeyRadio_初期読込.Checked,
                    this.ちょこぼ9Key_初期読込,
                    this.ちょこぼ9X座標_初期読込,
                    this.ちょこぼ9Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 10)
            {
                this.setBeanInfo(
                    this.ちょこぼ10KeyRadio_初期読込.Checked,
                    this.ちょこぼ10Key_初期読込,
                    this.ちょこぼ10X座標_初期読込,
                    this.ちょこぼ10Y座標_初期読込,
                    bean);
            }
            if (chocoboCount == 11)
            {
                this.setBeanInfo(
                    this.ちょこぼ11KeyRadio_初期読込.Checked,
                    this.ちょこぼ11Key_初期読込,
                    this.ちょこぼ11X座標_初期読込,
                    this.ちょこぼ11Y座標_初期読込,
                    bean);
            }
            */
            return bean;

        }

        /// <summary>
        /// チョコボリストの情報を格納し、返却する
        /// </summary>
        /// <param name="flg"></param>
        /// <param name="Key"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="bean"></param>
        /// <returns></returns>
        private ChocoboInfoBean setBeanInfo(Boolean flg, TextBox Key, TextBox X, TextBox Y, ChocoboInfoBean bean)
        {
            if (flg)
            {
                bean.Key = Key.Text;
            }
            else
            {
                bean.X = X.Text;
                bean.Y = Y.Text;
            }
            return bean;
        }

        /// <summary>
        /// 初期表示のオプション設定
        /// </summary>
        private void setInitOption()
        {
            /*
            // 飯
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.飯Key_初期読込,
                this.飯座標ボタン,
                this.飯X座標_初期読込,
                this.飯Y座標_初期読込,
                this.飯KeyRadio_初期読込.Checked
               );

            // 薬
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.薬Key_初期読込,
                this.薬座標ボタン,
                this.薬X座標_初期読込,
                this.薬Y座標_初期読込,
                this.薬KeyRadio_初期読込.Checked
               );

            // 修理
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.修理Key_初期読込,
                this.修理座標ボタン,
                this.修理X座標_初期読込,
                this.修理Y座標_初期読込,
                this.修理_全部座標ボタン,
                this.修理_全部X座標_初期読込,
                this.修理_全部Y座標_初期読込,
                this.修理_はい座標ボタン,
                this.修理_はいX座標_初期読込,
                this.修理_はいY座標_初期読込,
                this.修理KeyRadio_初期読込.Checked
               );

            // 修理
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.作成開始Key_初期読込,
                this.作成開始座標ボタン,
                this.作成開始X座標_初期読込,
                this.作成開始Y座標_初期読込,
                this.作成開始KeyRadio_初期読込.Checked
               );

            // ちょこぼ1
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ1Key_初期読込,
                this.ちょこぼ1座標ボタン,
                this.ちょこぼ1X座標_初期読込,
                this.ちょこぼ1Y座標_初期読込,
                this.ちょこぼ1KeyRadio_初期読込.Checked
               );

            // ちょこぼ2
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ2Key_初期読込,
                this.ちょこぼ2座標ボタン,
                this.ちょこぼ2X座標_初期読込,
                this.ちょこぼ2Y座標_初期読込,
                this.ちょこぼ2KeyRadio_初期読込.Checked
               );

            // ちょこぼ3
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ3Key_初期読込,
                this.ちょこぼ3座標ボタン,
                this.ちょこぼ3X座標_初期読込,
                this.ちょこぼ3Y座標_初期読込,
                this.ちょこぼ3KeyRadio_初期読込.Checked
               );

            // ちょこぼ4
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ4Key_初期読込,
                this.ちょこぼ4座標ボタン,
                this.ちょこぼ4X座標_初期読込,
                this.ちょこぼ4Y座標_初期読込,
                this.ちょこぼ4KeyRadio_初期読込.Checked
               );

            // ちょこぼ5
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ5Key_初期読込,
                this.ちょこぼ5座標ボタン,
                this.ちょこぼ5X座標_初期読込,
                this.ちょこぼ5Y座標_初期読込,
                this.ちょこぼ5KeyRadio_初期読込.Checked
               );

            // ちょこぼ6
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ6Key_初期読込,
                this.ちょこぼ6座標ボタン,
                this.ちょこぼ6X座標_初期読込,
                this.ちょこぼ6Y座標_初期読込,
                this.ちょこぼ6KeyRadio_初期読込.Checked
               );

            // ちょこぼ7
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ7Key_初期読込,
                this.ちょこぼ7座標ボタン,
                this.ちょこぼ7X座標_初期読込,
                this.ちょこぼ7Y座標_初期読込,
                this.ちょこぼ7KeyRadio_初期読込.Checked
               );

            // ちょこぼ8
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ8Key_初期読込,
                this.ちょこぼ8座標ボタン,
                this.ちょこぼ8X座標_初期読込,
                this.ちょこぼ8Y座標_初期読込,
                this.ちょこぼ8KeyRadio_初期読込.Checked
               );

            // ちょこぼ9
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ9Key_初期読込,
                this.ちょこぼ9座標ボタン,
                this.ちょこぼ9X座標_初期読込,
                this.ちょこぼ9Y座標_初期読込,
                this.ちょこぼ9KeyRadio_初期読込.Checked
               );

            // ちょこぼ10
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ10Key_初期読込,
                this.ちょこぼ10座標ボタン,
                this.ちょこぼ10X座標_初期読込,
                this.ちょこぼ10Y座標_初期読込,
                this.ちょこぼ10KeyRadio_初期読込.Checked
               );

            // ちょこぼ11
            ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                this.ちょこぼ11Key_初期読込,
                this.ちょこぼ11座標ボタン,
                this.ちょこぼ11X座標_初期読込,
                this.ちょこぼ11Y座標_初期読込,
                this.ちょこぼ11KeyRadio_初期読込.Checked
               );
               */
        }

        /// <summary>
        /// オプションタブイベントの総まとめ
        /// </summary>
        private void OptionEvents()
        {
            /*
            // オプションボタンのイベントを生成する
            // 飯ボタン
            this.飯座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.飯X座標_初期読込,
                   this.飯Y座標_初期読込
                   );
            };

            // 薬ボタン
            this.薬座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.薬X座標_初期読込,
                   this.薬Y座標_初期読込
                   );
            };

            // 修理ボタン
            this.修理座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.修理X座標_初期読込,
                   this.修理Y座標_初期読込
                   );
            };

            // 修理_全部ボタン
            this.修理_全部座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.修理_全部X座標_初期読込,
                   this.修理_全部Y座標_初期読込
                   );
            };

            // 修理_はいボタン
            this.修理_はい座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.修理_はいX座標_初期読込,
                   this.修理_はいY座標_初期読込
                   );
            };

            // 作成開始ボタン
            this.作成開始座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.作成開始X座標_初期読込,
                   this.作成開始Y座標_初期読込
                   );
            };

            // ちょこぼ1ボタン
            this.ちょこぼ1座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ1X座標_初期読込,
                   this.ちょこぼ1Y座標_初期読込
                   );
            };

            // ちょこぼ2ボタン
            this.ちょこぼ2座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ2X座標_初期読込,
                   this.ちょこぼ2Y座標_初期読込
                   );
            };

            // ちょこぼ3ボタン
            this.ちょこぼ3座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ3X座標_初期読込,
                   this.ちょこぼ3Y座標_初期読込
                   );
            };

            // ちょこぼ4ボタン
            this.ちょこぼ4座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ4X座標_初期読込,
                   this.ちょこぼ4Y座標_初期読込
                   );
            };

            // ちょこぼ5ボタン
            this.ちょこぼ5座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ5X座標_初期読込,
                   this.ちょこぼ5Y座標_初期読込
                   );
            };

            // ちょこぼ6ボタン
            this.ちょこぼ6座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ6X座標_初期読込,
                   this.ちょこぼ6Y座標_初期読込
                   );
            };

            // ちょこぼ7ボタン
            this.ちょこぼ7座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ7X座標_初期読込,
                   this.ちょこぼ7Y座標_初期読込
                   );
            };

            // ちょこぼ8ボタン
            this.ちょこぼ8座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ8X座標_初期読込,
                   this.ちょこぼ8Y座標_初期読込
                   );
            };

            // ちょこぼ9ボタン
            this.ちょこぼ9座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ9X座標_初期読込,
                   this.ちょこぼ9Y座標_初期読込
                   );
            };

            // ちょこぼ10ボタン
            this.ちょこぼ10座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ10X座標_初期読込,
                   this.ちょこぼ10Y座標_初期読込
                   );
            };

            // ちょこぼ11ボタン
            this.ちょこぼ11座標ボタン.Click += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.OptionEventCoordinateClick(
                   this.ちょこぼ11X座標_初期読込,
                   this.ちょこぼ11Y座標_初期読込
                   );
            };

            // 飯ラジオボタン
            this.飯KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.飯Key_初期読込,
                    this.飯座標ボタン,
                    this.飯X座標_初期読込,
                    this.飯Y座標_初期読込,
                    this.飯KeyRadio_初期読込.Checked
                    );
            };

            // 薬ラジオボタン
            this.薬KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.薬Key_初期読込,
                    this.薬座標ボタン,
                    this.薬X座標_初期読込,
                    this.薬Y座標_初期読込,
                    this.薬KeyRadio_初期読込.Checked
                    );
            };

            // 修理ラジオボタン
            this.修理KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.修理Key_初期読込,
                    this.修理座標ボタン,
                    this.修理X座標_初期読込,
                    this.修理Y座標_初期読込,
                    this.修理_全部座標ボタン,
                    this.修理_全部X座標_初期読込,
                    this.修理_全部Y座標_初期読込,
                    this.修理_はい座標ボタン,
                    this.修理_はいX座標_初期読込,
                    this.修理_はいY座標_初期読込,
                    this.修理KeyRadio_初期読込.Checked
                   );
            };

            // 作成開始ラジオボタン
            this.作成開始KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.作成開始Key_初期読込,
                    this.作成開始座標ボタン,
                    this.作成開始X座標_初期読込,
                    this.作成開始Y座標_初期読込,
                    this.作成開始KeyRadio_初期読込.Checked
                    );
            };
            // ちょこぼ1ラジオボタン
            this.ちょこぼ1KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ1Key_初期読込,
                    this.ちょこぼ1座標ボタン,
                    this.ちょこぼ1X座標_初期読込,
                    this.ちょこぼ1Y座標_初期読込,
                    this.ちょこぼ1KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ2ラジオボタン
            this.ちょこぼ2KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ2Key_初期読込,
                    this.ちょこぼ2座標ボタン,
                    this.ちょこぼ2X座標_初期読込,
                    this.ちょこぼ2Y座標_初期読込,
                    this.ちょこぼ2KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ3ラジオボタン
            this.ちょこぼ3KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ3Key_初期読込,
                    this.ちょこぼ3座標ボタン,
                    this.ちょこぼ3X座標_初期読込,
                    this.ちょこぼ3Y座標_初期読込,
                    this.ちょこぼ3KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ4ラジオボタン
            this.ちょこぼ4KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ4Key_初期読込,
                    this.ちょこぼ4座標ボタン,
                    this.ちょこぼ4X座標_初期読込,
                    this.ちょこぼ4Y座標_初期読込,
                    this.ちょこぼ4KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ5ラジオボタン
            this.ちょこぼ5KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ5Key_初期読込,
                    this.ちょこぼ5座標ボタン,
                    this.ちょこぼ5X座標_初期読込,
                    this.ちょこぼ5Y座標_初期読込,
                    this.ちょこぼ5KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ6ラジオボタン
            this.ちょこぼ6KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ6Key_初期読込,
                    this.ちょこぼ6座標ボタン,
                    this.ちょこぼ6X座標_初期読込,
                    this.ちょこぼ6Y座標_初期読込,
                    this.ちょこぼ6KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ7ラジオボタン
            this.ちょこぼ7KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ7Key_初期読込,
                    this.ちょこぼ7座標ボタン,
                    this.ちょこぼ7X座標_初期読込,
                    this.ちょこぼ7Y座標_初期読込,
                    this.ちょこぼ7KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ8ラジオボタン
            this.ちょこぼ8KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ8Key_初期読込,
                    this.ちょこぼ8座標ボタン,
                    this.ちょこぼ8X座標_初期読込,
                    this.ちょこぼ8Y座標_初期読込,
                    this.ちょこぼ8KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ9ラジオボタン
            this.ちょこぼ9KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ9Key_初期読込,
                    this.ちょこぼ9座標ボタン,
                    this.ちょこぼ9X座標_初期読込,
                    this.ちょこぼ9Y座標_初期読込,
                    this.ちょこぼ9KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ10ラジオボタン
            this.ちょこぼ10KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ10Key_初期読込,
                    this.ちょこぼ10座標ボタン,
                    this.ちょこぼ10X座標_初期読込,
                    this.ちょこぼ10Y座標_初期読込,
                    this.ちょこぼ10KeyRadio_初期読込.Checked
                    );
            };

            // ちょこぼ11ラジオボタン
            this.ちょこぼ11KeyRadio_初期読込.CheckedChanged += (s1, e1) =>
            {
                ACT_CrafterSimulator.Events.OptionEvents.setOptionActivity(
                    this.ちょこぼ11Key_初期読込,
                    this.ちょこぼ11座標ボタン,
                    this.ちょこぼ11X座標_初期読込,
                    this.ちょこぼ11Y座標_初期読込,
                    this.ちょこぼ11KeyRadio_初期読込.Checked
                    );
            };
            */
        }

        /// <summary>
        /// キャラ読込
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void キャラリスト_初期読込_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Hiddenのテキストボックスに、一時値を格納する
            // this.hiddenキャラ名textBox_初期読込.Text = this.キャラリストボックス.Text;

            // 対象キャラクター情報を取得する
            ICharacterBean characterBean = FileController.ReadCharacterInfo(this.キャラリストボックス.Text);

        }

        /// <summary>
        /// キャラ情報読込
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void キャラ一覧_初期読込_SelectedIndexChanged(object sender, EventArgs e)
        {
            // キャストを行う
            ListBox listBox = (ListBox)sender;

            // 空だった場合、返却
            if (String.IsNullOrWhiteSpace(listBox.Text))
            {
                return;
            }

            // 対象キャラクター情報を取得する
            ICharacterBean characterBean = FileController.ReadCharacterInfo(listBox.Text);

            // キャラ名
            this.キャラクターのお名前.Text = listBox.Text;

            this.木工師_textBox.Text = characterBean.Carpenter;
            this.鍛冶師_textBox.Text = characterBean.Blacksmith;
            this.甲冑師_textBox.Text = characterBean.Armorer;
            this.彫金師_textBox.Text = characterBean.Goldsmith;
            this.革細工師_textBox.Text = characterBean.Leatherworker;
            this.裁縫師_textBox.Text = characterBean.Weaver;
            this.錬金術師_textBox.Text = characterBean.Alchemist;
            this.調理師_textBox.Text = characterBean.Culinarian;
            this.予備1_textBox.Text = characterBean.ChocoboMeister;
            this.予備2_textBox.Text = characterBean.AwayukiKusushi;
            // this.マイスター1_comboBox.Text = characterBean.Meister1;
            // this.マイスター2_comboBox.Text = characterBean.Meister2;
            // this.マイスター3_comboBox.Text = characterBean.Meister3;

        }

        /// <summary>
        /// 入力されている情報を元に、マクロを実施する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button7_Click(object sender, EventArgs e)
        {
            /**
            // 空白チェック
            if (String.IsNullOrWhiteSpace(this.実行回数_textBox.Text) && this.マクロ一回実行_radioButton.Checked)
            {
                MessageBox.Show("回数指定してね！\r\n10回以下がおすすめだよ！100回とか間違って指定するとめんどくさいからやめようね！");
            }
            else if (String.IsNullOrWhiteSpace(this.ディレイ_textBox.Text) && this.マクロ一回実行_radioButton.Checked)
            {
                MessageBox.Show("ディレイ指定してね！\r\n10秒くらいがおすすめだよ？");
            }
            else if (String.IsNullOrWhiteSpace(this.実行マクロ名_comboBox.Text) && this.マクロ一回実行_radioButton.Checked)
            {
                MessageBox.Show("実行するマクロを指定してね！");
            }
            else if (String.IsNullOrWhiteSpace(this.キャラリストボックス.Text))
            {
                MessageBox.Show("キャラ名を指定してね！");
            }
            else
            {
                // 一回だけにチェックが入っていなかった場合
                if (!this.マクロ一回実行_radioButton.Checked)
                {
                    // 実行構成にチェックが入っていた場合、中身のチェックを実施する
                    if (this.chocoboInfoBeanList.Count == 0)
                    {
                        MessageBox.Show("例のあの人が一人も存在していません。", "ERROR");
                        // 返却を行う
                        return;
                    }
                    else
                    {
                        string message = "以下内容で実行します。よろしいですか？\r\n \r\n";
                        int i = 1;
                        foreach (ChocoboInfoBean chocoboInfoBean in this.chocoboInfoBeanList)
                        {
                            message += "●例のあの人" + i + "人目";
                            message += "\r\n　マクロ名：" + chocoboInfoBean.XMLName;
                            message += "\r\n　回数：" + chocoboInfoBean.Count + "回";
                            message += "\r\n";

                            // 実行回数の合計を取得する
                            this._AllCount += int.Parse(chocoboInfoBean.Count);

                            i++;
                        }

                        // メッセージボックスを生成する
                        DialogResult result = MessageBox.Show(message, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                        //何が選択されたか調べる
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("処理を中断しました。", "中断");
                            return;
                        }
                    }
                }
                else
                {
                    string message = "以下内容で実行します。よろしいですか？\r\n \r\n";
                    message += "●例のあの人";
                    message += "\r\n　マクロ名：" + this.実行マクロ名_comboBox.Text;
                    message += "\r\n　回数：" + this.実行回数_textBox.Text + "回";

                    // メッセージボックスを生成する
                    DialogResult result = MessageBox.Show(message, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                    //何が選択されたか調べる
                    if (result == DialogResult.No)
                    {
                        MessageBox.Show("処理を中断しました。", "中断");
                        return;
                    }

                    this.chocoboInfoBeanList = new List<ChocoboInfoBean>();
                    ChocoboInfoBean chocoboInfoBean = new ChocoboInfoBean();

                    chocoboInfoBean.XMLName = this.実行マクロ名_comboBox.Text;
                    chocoboInfoBean.Count = this.実行回数_textBox.Text;
                    chocoboInfoBean.WhatMakes = this.作るものcomboBox.Text;

                    // 実行回数の合計を取得する
                    this._AllCount = int.Parse(chocoboInfoBean.Count);

                    if (this.ちょこぼ1KeyRadio_初期読込.Checked)
                    {
                        chocoboInfoBean.Key = this.ちょこぼ1Key_初期読込.Text;
                    }
                    else
                    {
                        chocoboInfoBean.X = this.ちょこぼ1X座標_初期読込.Text;
                        chocoboInfoBean.Y = this.ちょこぼ1Y座標_初期読込.Text;
                    }
                    chocoboInfoBean.Job = "変えませーん";

                    this.chocoboInfoBeanList.Add(chocoboInfoBean);
                }

                // フラグを設定する
                Boolean flg = true;

                // FF14をアクティベート化する
                flg = Utils.Utils.ActivateFF14();
                // 会社なんで、一時コメントアウト
                //Utils.Utils.WaitTime(3);

                // FF14起動判定
                if (!flg)
                {
                    MessageBox.Show("FF14が起動してません。");
                    return;
                }
                else
                {
                    // ホットキーのために唯一無二のIDを生成する
                    hotkeyID = GlobalAddAtom("GlobalHotKey " + this.GetHashCode().ToString());
                    // Ctrl+F2キーを登録する
                    RegisterHotKey(this.Handle, hotkeyID, MOD_ALT, Keys.F2);

                    // 一時ボタンを無効化する
                    this.実行ボタン.Enabled = false;

                    // 中断、再開ボタンを活性化させる
                    this.終了ボタン.Enabled = true;
                    this.中断ボタン.Enabled = true;

                    // フラグを設定する
                    this.EndFlg = true;
                    this.loopFlg = true;
                    try
                    {
                        // マクロを実行する
                        await Task.Run(() => GoMacro());
                    }
                    catch (StopInterruptionException)
                    {
                        // 該当Exceptionの場合、何もせずに処理を終了する
                    }
                    finally
                    {
                        //非活性、活性を元に戻す
                        this.実行ボタン.Enabled = true;
                        this.中断ボタン.Enabled = false;
                        this.終了ボタン.Enabled = false;

                        // グローバルホットキーの登録のみ解除する
                        this.GlobalDeleteAtomClosing();
                    }
                }
            }
            */
        }

        /// <summary>
        /// マクロを指定した際に実施されるイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // senderをキャストする
            ComboBox tempbox = (ComboBox)sender;

            // 何も選択されていない場合は、返却を行う
            if (String.IsNullOrWhiteSpace(tempbox.Text))
            {
                return;
            }

            // 対象マクロ情報を取得する
            IMacroInfoBean macroInfoBean = FileController.ReadMacroInfo(tempbox.Text.Substring(0, tempbox.Text.Length - 4));

            // 作るものに、対象のものを入れる
            this.作るものcomboBox.Text = macroInfoBean.WhatMakes;

            /**
            // HQ作成
            if ("HQ".Equals(macroInfoBean.WhatMakes))
            {
                this.ディレイ_textBox.Enabled = false;
                this.作るものcomboBox.Enabled = false;
                this.ディレイ_textBox.Text = "5";
            }

            // ろえな作成
            if ("ろえな".Equals(macroInfoBean.WhatMakes))
            {
                this.ディレイ_textBox.Enabled = false;
                this.作るものcomboBox.Enabled = false;
                this.ディレイ_textBox.Text = "6";
            }

            // 作るだけ作成
            if ("作るだけ".Equals(macroInfoBean.WhatMakes))
            {
                this.ディレイ_textBox.Enabled = false;
                this.作るものcomboBox.Enabled = false;
                this.ディレイ_textBox.Text = "5";
            }

            // 自分で作成
            if ("自分で編集".Equals(macroInfoBean.WhatMakes))
            {
                this.作るものcomboBox.Enabled = true;
                this.ディレイ_textBox.Text = "";
            }

            // 空白を選択された時
            if ("".Equals(macroInfoBean.WhatMakes))
            {
                this.ディレイ_textBox.Enabled = true;
                this.ディレイ_textBox.Text = "";
            }
            */
        }

        /// <summary>
        /// マクロを実施するメソッド
        /// </summary>
        /// <param name="list"></param>
        /// <param name="str"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private async Task GoMacro()
        {
            // ログを拾うイベント
            ActGlobals.oFormActMain.OnLogLineRead += this.OFormActMain_OnLogLineRead;

            int chocoboCount = 0;
            this.RepairingFlg = false;
            this.roenaFlg = false;

            // 一度ろえな処理を無条件で行う
            List<IPlayMacroInfoBean> roenaList = new List<IPlayMacroInfoBean>();
            IPlayMacroInfoBean roenaPlayMacroInfoBean = new PlayMacroInfoBean();
            roenaPlayMacroInfoBean.No = "ろえな";
            roenaPlayMacroInfoBean.Wait = "0";
            roenaPlayMacroInfoBean.Text = "/ac 蒐集品製作";

            roenaList.Add(roenaPlayMacroInfoBean);
            // キーを送信する
            SendKeys.SendWait("{ENTER}");
            this.WaitTime(30);
            // キーを送信する(ろえな実行)
            this.SendKeyAction(roenaList);
            //this.WaitTime(200);


            // 格納されているマクロの数だけ実施する
            foreach (ChocoboInfoBean chocoboInfoBean in this.chocoboInfoBeanList)
            {

                // 一度チョコボリストの内容を、全て取得する
                string MacroName = chocoboInfoBean.XMLName;
                this._count = int.Parse(chocoboInfoBean.Count);
                int delay = int.Parse(CreateDley(chocoboInfoBean.WhatMakes));
                int GsKey = this.JobInfo(chocoboInfoBean.Job, this.キャラリストボックス.Text);

                if (GsKey != -1)
                {
                    // ジョブ変更用のインスタンスを生成
                    List<IPlayMacroInfoBean> JobList = new List<IPlayMacroInfoBean>();
                    IPlayMacroInfoBean playMacroInfoBean = new PlayMacroInfoBean();
                    // キーを送信する
                    SendKeys.SendWait("{ENTER}");

                    playMacroInfoBean.Wait = "0";
                    playMacroInfoBean.No = "Job";
                    playMacroInfoBean.Text = "/gs change " + GsKey;

                    JobList.Add(playMacroInfoBean);

                    // キーを送信する
                    this.SendKeyAction(JobList);
                }

                // マクロ情報を取得する
                List<IPlayMacroInfoBean> list = FileController.GetTempMacroInfo(FILE_PATH_TEMPMACRO + MacroName);

                // 1秒停止する
                this.WaitTime(1000);

                /**
                // アディッショナルの設定有無を判定する
                if (this.checkBox5.Checked)
                {
                    // キーを送信する
                    SendKeys.SendWait("{ENTER}");

                    // アディッショナルマクロ用インスタンスを生成する
                    List<IPlayMacroInfoBean> AdditionalList = new List<IPlayMacroInfoBean>();
                    IPlayMacroInfoBean playMacroInfoBean = new PlayMacroInfoBean();

                    playMacroInfoBean.Wait = "0";
                    playMacroInfoBean.No = "アディ";
                    playMacroInfoBean.Text = "/aaction clear";
                    AdditionalList.Add(playMacroInfoBean);

                    // いれたアディを格納しておく
                    List<string> addAdiList = new List<string>();

                    // アディッショナルの入替を実施する
                    foreach (IPlayMacroInfoBean targetBean in list)
                    {
                        // 初期化する
                        playMacroInfoBean = new PlayMacroInfoBean();

                        // マクロ名を一度取得する
                        string temp = targetBean.Text.Substring(4);
                        if (AdditionalListEdit.ConfirmCode(temp))
                        {
                            // リストに格納してないアディを設定させる
                            if (!addAdiList.Contains(temp))
                            {
                                playMacroInfoBean.Wait = "0";
                                playMacroInfoBean.No = "アディ";
                                playMacroInfoBean.Text = "/aaction " + temp + " on";

                                // リストに格納する
                                addAdiList.Add(temp);
                                AdditionalList.Add(playMacroInfoBean);
                            }
                        }
                    }

                    // キーを送信する
                    this.SendKeyAction(AdditionalList);

                    // 中断処理
                    this.StopAndInterruptionAction();
                }
                */

                /*
                // FF14プラグイン取得処理
                // ACT.FF14プラグインを変数に格納する
                var plugin = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x =>
                                x.pluginFile.Name.ToUpper().Contains(ACTPLUGIN_NAME.ToUpper()));

                // FF14オブジェクトを生成する
                var ffxivPlugin = plugin.pluginObj;

                // dynamicによるSettingsフィールドの取り出し
                this.ffxivPluginAsDynamic = (dynamic)ffxivPlugin;
                */

                // ろえな処理
                if (chocoboInfoBean.WhatMakes.Equals("ろえな"))
                {
                    while (!this.roenaFlg)
                    {
                        //this.WaitTime(1000);
                        // えろな用の値を格納する
                        roenaList = new List<IPlayMacroInfoBean>();
                        roenaPlayMacroInfoBean = new PlayMacroInfoBean();
                        roenaPlayMacroInfoBean.No = "ろえな";
                        roenaPlayMacroInfoBean.Wait = "0";
                        roenaPlayMacroInfoBean.Text = "/ac 蒐集品製作";

                        roenaList.Add(roenaPlayMacroInfoBean);
                        // キーを送信する(ろえな実行)
                        this.SendKeyAction(roenaList);
                        this.WaitTime(1000);
                    }
                }
                else
                {
                    while (this.roenaFlg)
                    {
                        //this.WaitTime(1000);
                        // えろな用の値を格納する
                        roenaList = new List<IPlayMacroInfoBean>();
                        roenaPlayMacroInfoBean = new PlayMacroInfoBean();
                        roenaPlayMacroInfoBean.No = "ろえな";
                        roenaPlayMacroInfoBean.Wait = "1";
                        roenaPlayMacroInfoBean.Text = "/ac 蒐集品製作";

                        roenaList.Add(roenaPlayMacroInfoBean);
                        // キーを送信する(ろえな実行)
                        this.SendKeyAction(roenaList);
                        this.WaitTime(1000);
                    }
                }


                /**
                // 飯
                if (this.checkBox3.Checked)
                {
                    // ご飯を食べるイベント
                    this.FoodStartEvent();
                    // 現在日時を取得し、ごはんの秒数を加算する
                    this.FoodDateTime = DateTime.Now.AddSeconds(this.FoodTime - 120);
                }

                // 薬
                if (this.checkBox4.Checked)
                {
                    // 薬を飲むイベント
                    this.MedicineStartEvent();
                    // 現在日時を取得し、薬の秒数を加算する
                    this.MedicineDateTime = DateTime.Now.AddSeconds(this.MedicineTime - 120);
                }
                */

                // チョコボを押下する
                this.SendChocoboExecutionKey(chocoboInfoBean);

                // this._count の回数文ループする
                while (this._count != 0 && this.EndFlg)
                {
                    // 作成実行キーを押下する
                    this.SendCreatingExecutionKey();

                    // 中断処理
                    this.StopAndInterruptionAction();

                    // 実行回数表示 
                    this.label27.Text = "残り" + this._count + "回";

                    // キーを送信する
                    SendKeys.SendWait("{ENTER}");

                    // キーを送信する(macro実行)
                    this.SendKeyAction(list);

                    // 中断処理
                    this.StopAndInterruptionAction();

                    // ろえな処理
                    if (chocoboInfoBean.WhatMakes.Equals("ろえな"))
                    {
                        this.WaitTime(1000);
                        // ESCを押下する
                        Utils.Utils.KeySim(VK_ESCAPE);
                    }

                    // 残り回数が1回ではなかった場合、ディレイをスキップする
                    if (this._count != 1 && !chocoboInfoBean.WhatMakes.Equals("ろえな"))
                    {
                        // ディレイ秒待機する
                        this.WaitTime(delay * 1000);
                    }
                    else
                    {
                        // 2.5秒待機する
                        this.WaitTime(2500);

                    }

                    //this.WaitTime(1000);

                    // ここで、修理中断フラグを確認する
                    if (this._AllCount > 3 && this.RepairingFlg)
                    {
                        // 待機する
                        this.WaitTime(1000);

                        string str = this.label27.Text;
                        this.label27.Text = "-- 修理休憩 --";

                        // ここで処理をよぶ
                        this.RepairingEvent(chocoboInfoBean);

                        this.WaitTime(2000);

                        // 修理フラグを戻す
                        this.RepairingFlg = false;
                        this.label27.Text = str;
                    }

                    // 中断処理
                    this.StopAndInterruptionAction();

                    /**
                    // ここで、飯の中断フラグを確認する
                    if (this.checkBox3.Checked)
                    {
                        if (this._AllCount != 1 && checkDateTime(this.FoodDateTime))
                        {
                            // 待機する
                            this.WaitTime(1000);

                            string str = this.label27.Text;
                            this.label27.Text = "-- 飯休憩 --";

                            // ここで処理をよぶ
                            this.FoodEvent(chocoboInfoBean);

                            this.WaitTime(2000);

                            // 現在日時を取得し、飯の秒数を加算する
                            this.FoodDateTime = DateTime.Now.AddSeconds(this.FoodTime - 120);

                            // フラグを戻す
                            this.label27.Text = str;
                        }
                    }
                    */

                    // 中断処理
                    this.StopAndInterruptionAction();

                    /**
                    // ここで、薬の中断フラグを確認する 
                    if (this.checkBox4.Checked)
                    {
                        if (this._AllCount != 1 && checkDateTime(this.MedicineDateTime))
                        {
                            // 待機する
                            this.WaitTime(1000);

                            string str = this.label27.Text;
                            this.label27.Text = "-- 薬休憩 --";

                            // ここで処理をよぶ
                            this.MedicineEvent(chocoboInfoBean);

                            this.WaitTime(2000);

                            // 現在日時を取得し、薬の秒数を加算する
                            this.MedicineDateTime = DateTime.Now.AddSeconds(this.MedicineTime - 120);

                            // フラグを戻す
                            this.label27.Text = str;
                        }
                    }
                    */
                    this._count--;
                    this._AllCount--;
                }

                chocoboCount++;

                // ここで、次のリストの格納の確認を実施する
                if (this.chocoboInfoBeanList.Count != chocoboCount)
                {
                    // 待機する
                    this.WaitTime(7000);
                    // キーを送信する
                    Utils.Utils.KeySim(VK_ESCAPE);
                    // 待機する
                    this.WaitTime(1000);
                }
            }

            // ****************  以下アディ入替処理  ****************　//
            // 待機する
            this.WaitTime(7000);

            // キーを送信する
            Utils.Utils.KeySim(VK_ESCAPE);

            // 待機する
            this.WaitTime(500);

            /**
            // アディッショナルの設定有無を判定する
            if (this.checkBox5.Checked == true)
            {

                // 終了用設定アディッショナルを取得する
                List<IPlayMacroInfoBean> tempAdiMacroList = FileController.GetAdditionalInfo(FILE_PATH_ADDITIONAL);

                // いったんアディリストを作成しなおす
                List<IPlayMacroInfoBean> adiMacroList = new List<IPlayMacroInfoBean>();

                // Npを全てアディに置き換える
                foreach (IPlayMacroInfoBean playMacroInfoBean in tempAdiMacroList)
                {
                    playMacroInfoBean.No = "アディ";
                    adiMacroList.Add(playMacroInfoBean);
                }

                // キーを送信する
                SendKeys.SendWait("{ENTER}");

                // アディッショナル入替
                this.label27.Text = "アディ入替なう";
                this.SendKeyAction(adiMacroList);

                // 中断処理
                this.StopAndInterruptionAction();
            }
            */

            // 終了処理
            this.実行ボタン.Enabled = true;
            // フラグを設定しなおす
            this.loopFlg = true;
            this.EndFlg = true;
            // 中断、再開ボタンを非活性化させる
            this.終了ボタン.Enabled = false;
            this.中断ボタン.Enabled = false;
            this.label27.Text = "おわっちゃった";

            throw new StopInterruptionException("スレッド停止のために、強制的にExceptionを発生させる");

        }

        /// <summary>
        /// ログを取得した際に発生するイベント
        /// </summary>
        /// <param name="isImport"></param>
        /// <param name="logInfo"></param>
        private void OFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {

            // キャラの名前を先に取得しておく
            string CharaName = this.キャラリストボックス.Text;

            // 18文字以下のログは読み捨てる
            // なぜならば、タイムスタンプ＋ログタイプのみのログだから
            if (logInfo.logLine.Length <= 18)
            {
                return;
            }

            /**
            // 装備が壊れた時のログを挿入する
            if (logInfo.logLine.Contains("の耐久度が10%以下になった。")
                 && this.checkBox7.Checked)
            {
                // ログ中断フラグを立てる
                this.RepairingFlg = true;
            }

            // 強化薬を使ったときの秒数を取得するイベント
            if (logInfo.logLine.Contains(CharaName + " gains the effect of 強化薬 from " + CharaName + " for ")
                && this.checkBox4.Checked)
            {
                // 格納する
                this.MedicineTime = int.Parse(logInfo.logLine.Substring(logInfo.logLine.Length - 15, 3));
            }

            // 飯を使ったときの秒数を取得するイベント
            if (logInfo.logLine.Contains(CharaName + " gains the effect of 食事 from " + CharaName + " for ")
                && this.checkBox3.Checked)
            {
                // 格納する
                this.FoodTime = int.Parse(logInfo.logLine.Substring(logInfo.logLine.Length - 16, 4));
            }

            // ろえな作成用蒐集品つけたときのイベントフラグ
            if (logInfo.logLine.Contains(CharaName + " gains the effect of 蒐集品製作 from " + CharaName + " for 9999.00 Seconds."))
            {
                this.roenaFlg = true;
            }

            // ろえな作成用蒐集品消えたときのイベントフラグ
            if (logInfo.logLine.Contains(CharaName + " loses the effect of 蒐集品製作 from " + CharaName + "."))
            {
                this.roenaFlg = false;
            }
            */
        }

        /// <summary>
        /// キー入力を行う
        /// </summary>
        /// <param name="Keylist"></param>        
        private void SendKeyAction(List<IPlayMacroInfoBean> Keylist)
        {

            // 取得したマクロ情報をループする
            foreach (IPlayMacroInfoBean bean in Keylist)
            {

                // 文字を一文字ずつ取得する
                foreach (char c in bean.Text)
                {
                    // 貼り付け実施
                    SendKeys.SendWait(c.ToString());
                    // 
                    this.WaitTime(50);
                }

                // ちょっとだけ待機する
                this.WaitTime(500);

                // 4文字以下の場合、すこし待機時間を長くする
                if (bean.Text.Length <= 4)
                {
                    // ちょっとだけ待機する
                    this.WaitTime(500);
                }

                // キーを送信する
                SendKeys.SendWait("{ENTER}");

                if (!"0".Equals(bean.Wait) && !"アディ".Equals(bean.No) && !"ろえな".Equals(bean.No))
                {
                    if ("2".Equals(bean.Wait) && !string.IsNullOrWhiteSpace(bean.Wait))
                    {
                        // 停止する
                        this.WaitTime((int.Parse(bean.Wait) * 1000) - 1430);
                    }
                    else if (!string.IsNullOrWhiteSpace(bean.Wait))
                    {
                        // 停止する
                        this.WaitTime((int.Parse(bean.Wait) * 1000) - 1430);
                    }

                }
                else
                {
                    // アディの場合
                    this.WaitTime(40);
                }
            }
        }

        /// <summary>
        /// ディレイを設定
        /// </summary>
        /// <param name="Make"></param>
        /// <returns></returns>
        private string CreateDley(string Make)
        {
            // HQ作成
            if ("HQ".Equals(Make))
            {
                return "5";
            }

            // ろえな作成
            if ("ろえな".Equals(Make))
            {
                return "6";
            }

            // 作るだけ作成
            if ("作るだけ".Equals(Make))
            {
                return "5";
            }

            throw new Exception("test");
        }

        /// <summary>
        /// ホットキーの登録を解除する
        /// </summary>
        private void GlobalDeleteAtomClosing()
        {
            // ホットキーの登録を解除し、アトムを削除する
            UnregisterHotKey(this.Handle, hotkeyID);
            GlobalDeleteAtom(hotkeyID);
        }

        /// <summary>
        /// ホットキーが押下された際のイベント
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_HOTKEY)
            {
                // ホットキーが押されたときの処理
                Debug.WriteLine("Ctrl+A");
                if (!this.loopFlg)
                {
                    this.中断ボタン.Text = "中断";
                    this.中断ボタン.Enabled = false;
                    this.loopFlg_Go = true;
                }
                else
                {
                    this.中断ボタン.Text = "再開";
                    this.中断ボタン.Enabled = false;
                    this.loopFlg = false;
                    this.loopFlg_Go = false;
                }

            }

        }

        /// <summary>
        /// 作るものが変更された際のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 作るものcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            /**
            if (sender != null)
            {
                ComboBox cb = (ComboBox)sender;

                // HQ作成
                if ("HQ".Equals(cb.Text))
                {
                    this.ディレイ_textBox.Enabled = false;
                    this.ディレイ_textBox.Text = "5";
                }

                // ろえな作成
                if ("ろえな".Equals(cb.Text))
                {
                    this.ディレイ_textBox.Enabled = false;
                    this.ディレイ_textBox.Text = "6";
                }

                // 作るだけ作成
                if ("作るだけ".Equals(cb.Text))
                {
                    this.ディレイ_textBox.Enabled = false;
                    this.ディレイ_textBox.Text = "5";
                }

                // 自分で作成
                if ("自分で編集".Equals(cb.Text))
                {
                    this.作るものcomboBox.Enabled = true;
                    this.ディレイ_textBox.Text = "";
                }

                // 空白を選択された時
                if ("".Equals(cb.Text))
                {
                    this.ディレイ_textBox.Enabled = true;
                    this.ディレイ_textBox.Text = "10";
                }
            }
            */
        }


        /// <summary>
        /// 引数より、ジョブ情報を返却する
        /// </summary>
        /// <param name="jobStr"></param>
        /// <returns></returns>
        private int JobInfo(string job, string Key)
        {
            // キャラクター情報を取得する
            ICharacterBean characterBean = FileController.ReadCharacterInfo(Key);

            if (job.Equals("木工師"))
            {
                return int.Parse(characterBean.Carpenter);
            }
            else if (job.Equals("鍛冶師"))
            {
                return int.Parse(characterBean.Blacksmith);
            }
            else if (job.Equals("甲冑師"))
            {
                return int.Parse(characterBean.Armorer);
            }
            else if (job.Equals("彫金師"))
            {
                return int.Parse(characterBean.Goldsmith);
            }
            else if (job.Equals("革細工師"))
            {
                return int.Parse(characterBean.Leatherworker);
            }
            else if (job.Equals("裁縫師"))
            {
                return int.Parse(characterBean.Weaver);
            }
            else if (job.Equals("錬金術師"))
            {
                return int.Parse(characterBean.Alchemist);
            }
            else if (job.Equals("調理師"))
            {
                return int.Parse(characterBean.Culinarian);
            }
            else if (job.Equals("ちょこぼ師"))
            {
                return int.Parse(characterBean.ChocoboMeister);
            }
            else if (job.Equals("例のあの人"))
            {
                return int.Parse(characterBean.AwayukiKusushi);
            }
            else if (job.Equals("変えませーん"))
            {
                return -1;
            }
            else
            {
                throw new Exception("変な例のあの人が、実行構成に紛れ込んでそうですね。");
            }
        }

        /// <summary>
        /// 現在日付よりも大きい場合、True、小さい場合false
        /// </summary>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        private Boolean checkDateTime(DateTime targetTime)
        {
            DateTime NowTime = DateTime.Now;
            // 比較
            if (NowTime < targetTime)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 作成実行キーの設定により、
        /// </summary>
        private void SendCreatingExecutionKey()
        {
            /**
            // 作成実行キーを押下するか判定を行う
            if (this.作成開始KeyRadio_初期読込.Checked)
            {

                // Num0を押下する(いったんDef)
                Utils.Utils.KeySim(NUMPAD0);
                this.WaitTime(1000);
                // Num0を押下する
                Utils.Utils.KeySim(NUMPAD0);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(this.作成開始X座標_初期読込.Text, this.作成開始Y座標_初期読込.Text);
            }
            */

            // 作成を少しだけ待つ
            this.WaitTime(1500);
        }

        /// <summary>
        /// 作成実行キーを判定して、送信するイベント(作成開始前)
        /// </summary>
        private void SendChocoboExecutionKey(ChocoboInfoBean chocoboInfoBean)
        {
            // 判定を行い、決められた動作を行う
            if (!String.IsNullOrWhiteSpace(chocoboInfoBean.Key))
            {
                // キーを送信する
                SendKeys.SendWait(chocoboInfoBean.Key);
            }
            else
            {
                // 座標を指定し、クリックイベントを発生させる
                Utils.Utils.mouse_Click(chocoboInfoBean.X, chocoboInfoBean.Y);
            }

            // 遅延を挿入する
            this.WaitTime(1000);

            /**
            // 作成実行キーを押下するか判定を行う
            if (this.作成開始KeyRadio_初期読込.Checked)
            {
                // Num0を押下する(いったん暫定デフォルトNum0)
                Utils.Utils.KeySim(NUMPAD0);
                // 遅延を挿入する
                this.WaitTime(1000);
            }
            */
        }

        /// <summary>
        /// 修理イベントを生成する
        /// </summary>
        private void RepairingEvent(ChocoboInfoBean chocoboInfoBean)
        {
            this.WaitTime(1000);
            // キーを送信する
            // ESC
            Utils.Utils.KeySim(VK_ESCAPE);
            this.WaitTime(2000);
            /**
            // 作成実行キーを押下するか判定を行う
            if (this.修理KeyRadio_初期読込.Checked)
            {
                SendKeys.SendWait(this.修理Key_初期読込.Text);
                this.WaitTime(1000);
                // Num6を押下する
                Utils.Utils.KeySim(NUMPAD6);
                this.WaitTime(1000);
                // Num0を押下する
                Utils.Utils.KeySim(NUMPAD0);
                this.WaitTime(1000);
                // Num6を押下する
                Utils.Utils.KeySim(NUMPAD6);
                this.WaitTime(1000);
                // Num0を押下する
                Utils.Utils.KeySim(NUMPAD0);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(this.修理X座標_初期読込.Text, this.修理Y座標_初期読込.Text);
                this.WaitTime(1000);
                Utils.Utils.mouse_Click(this.修理_全部X座標_初期読込.Text, this.修理_全部Y座標_初期読込.Text);
                this.WaitTime(1000);
                Utils.Utils.mouse_Click(this.修理_はいX座標_初期読込.Text, this.修理_はいY座標_初期読込.Text);
            }
            */
            // 修理中の待機時間
            this.WaitTime(4000);
            // ESCキーを送信する
            Utils.Utils.KeySim(VK_ESCAPE);
            this.WaitTime(1000);
            // 作成実行に移行する
            // 作成実行キーを押下するか判定を行う
            if (!String.IsNullOrWhiteSpace(chocoboInfoBean.Key))
            {
                // キーを送信する
                SendKeys.SendWait(chocoboInfoBean.Key);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(chocoboInfoBean.X, chocoboInfoBean.Y);
            }
            this.WaitTime(1000);
            // Num0を押下する
            Utils.Utils.KeySim(NUMPAD0);
        }

        /// <summary>
        /// 薬の飲むイベント
        /// </summary>
        private void MedicineStartEvent()
        {
            /**
            this.WaitTime(1000);
            if (this.薬KeyRadio_初期読込.Checked)
            {
                // キーを送信する
                SendKeys.SendWait(this.薬Key_初期読込.Text);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(this.薬X座標_初期読込.Text, this.薬Y座標_初期読込.Text);
            }
            this.WaitTime(4000);
            */
        }

        /// <summary>
        /// 薬を延長するイベント
        /// </summary>
        private void MedicineEvent(ChocoboInfoBean chocoboInfoBean)
        {

            this.WaitTime(1000);
            // キーを送信する
            // ESC
            Utils.Utils.KeySim(VK_ESCAPE);
            this.WaitTime(2000);
            /**
            if (this.薬KeyRadio_初期読込.Checked)
            {
                // キーを送信する
                SendKeys.SendWait(this.薬Key_初期読込.Text);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(this.薬X座標_初期読込.Text, this.薬Y座標_初期読込.Text);
            }
            */
            // 停止処理を挿入する
            this.WaitTime(4000);
            // 作成実行に移行する
            // 作成実行キーを押下するか判定を行う
            if (!String.IsNullOrWhiteSpace(chocoboInfoBean.Key))
            {
                // キーを送信する
                SendKeys.SendWait(chocoboInfoBean.Key);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(chocoboInfoBean.X, chocoboInfoBean.Y);
            }
            this.WaitTime(1000);
            // Num0を押下する
            Utils.Utils.KeySim(NUMPAD0);

        }

        /// <summary>
        /// 飯を食べるイベント
        /// </summary>
        private void FoodStartEvent()
        {
            this.WaitTime(1000);
            /**
            if (this.飯KeyRadio_初期読込.Checked)
            {
                // キーを送信する
                SendKeys.SendWait(this.飯Key_初期読込.Text);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(this.飯X座標_初期読込.Text, this.飯Y座標_初期読込.Text);
            }
            */
            this.WaitTime(4000);
        }

        /// <summary>
        /// 飯を延長するイベント
        /// </summary>
        private void FoodEvent(ChocoboInfoBean chocoboInfoBean)
        {
            this.WaitTime(1000);
            // キーを送信する
            // ESC
            Utils.Utils.KeySim(VK_ESCAPE);
            this.WaitTime(2000);
            /**
            if (this.飯KeyRadio_初期読込.Checked)
            {
                // キーを送信する
                SendKeys.SendWait(this.飯Key_初期読込.Text);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(this.飯X座標_初期読込.Text, this.飯Y座標_初期読込.Text);
            }
            */
            this.WaitTime(4000);
            // 作成実行に移行する
            // 作成実行キーを押下するか判定を行う
            if (!String.IsNullOrWhiteSpace(chocoboInfoBean.Key))
            {
                // キーを送信する
                SendKeys.SendWait(chocoboInfoBean.Key);
            }
            else
            {
                // マウスイベントを実施する
                Utils.Utils.mouse_Click(chocoboInfoBean.X, chocoboInfoBean.Y);
            }
            this.WaitTime(1000);
            // Num0を押下する
            Utils.Utils.KeySim(NUMPAD0);
        }

        /// <summary>
        /// 引数秒待機する
        /// </summary>
        private void WaitTime(int Time)
        {
            Thread.Sleep(Time);
            // 中断処理をいったん入れてみる
            this.StopAndInterruptionAction();
        }

        /// <summary>
        /// キャンセル、停止処理の総まとめ
        /// </summary>
        public void StopAndInterruptionAction()
        {
            // 終了処理
            if (!this.EndFlg)
            {
                // 終了処理
                this.実行ボタン.Enabled = true;
                // フラグを設定しなおす
                this.loopFlg = true;
                this.EndFlg = true;
                // 中断、再開ボタンを非活性化させる
                this.終了ボタン.Enabled = false;
                this.中断ボタン.Enabled = false;
                this.label27.Text = "残り***回";

                // 終了する
                throw new StopInterruptionException();
            }

            // 中断処理
            while (!this.loopFlg)
            {
                if (!this.loopFlg_Go)
                {
                    this.中断ボタン.Enabled = true;
                }
                this.label27.Text = "残り" + this._count + "回" + " --- 中断 ---";
                // 停止処理を確認
                if (!this.EndFlg)
                {
                    // 終了処理
                    this.実行ボタン.Enabled = true;
                    // フラグを設定しなおす
                    this.loopFlg = true;
                    this.EndFlg = true;
                    // 中断、再開ボタンを非活性化させる
                    this.終了ボタン.Enabled = false;
                    this.中断ボタン.Enabled = false;
                    this.label27.Text = "残り***回";

                    // 終了する
                    throw new StopInterruptionException();
                }
                if (this.loopFlg_Go)
                {
                    this.label27.Text = "残り" + this._count + "回\r\n5秒後に処理が再開します。";

                    // CraSimu.AutoClosingMessageBox.Show("5秒後に処理が再開します。", "再開", 5000);
                    this.中断ボタン.Enabled = true;
                    this.loopFlg = true;
                    // 停止処理を確認
                    if (!this.EndFlg)
                    {
                        // 終了処理
                        this.実行ボタン.Enabled = true;
                        // フラグを設定しなおす
                        this.loopFlg = true;
                        this.EndFlg = true;
                        // 中断、再開ボタンを非活性化させる
                        this.終了ボタン.Enabled = false;
                        this.中断ボタン.Enabled = false;
                        this.label27.Text = "残り***回";

                        // 終了する
                        throw new StopInterruptionException();
                    }
                    Utils.Utils.ActivateFF14();
                    this.label27.Text = "残り" + this._count + "回";
                }

            }

        }

        /// <summary>
        /// メッセージボックス拡張クラス
        /// </summary>
        public class AutoClosingMessageBox
        {
            int text_max_length = 200; //最大の文字列長の指定
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                if (text.Length > text_max_length) //内容の長さを制限する追加部分
                {
                    text = text.Substring(0, text_max_length);
                }
                //text = ( text.length > text_max_length ) ? text.Substring( 0, text_max_length ) : text; //三項目演算での記述方法　? の左側が true の時 : の左側が代入されます。 falseの時は右側
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed, null, timeout, Timeout.Infinite);
                MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                {
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

    }
}

