
using System.Drawing;
using System.Windows.Forms;

namespace CrafterMacroExecution.Model
{
    public partial class Coordinate : Form
    {
        //マウスのクリック位置を記憶
        public Point mousePoint;

        public Coordinate()
        {
            InitializeComponent();

            // F
            //this.ActivateFF14();

            //フォームの境界線をなくす
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 0.3;
            //自分自身のフォームを最大化
            //this.WindowState = FormWindowState.Maximized;

            //ディスプレイの高さ
            int h = Screen.PrimaryScreen.Bounds.Height;
            //ディスプレイの幅
            int w = Screen.PrimaryScreen.Bounds.Width;

            this.Height = h;
            this.Width = w;

            //フォームの最大化ボタンの表示、非表示を切り替える
            this.MaximizeBox = !this.MaximizeBox;
            //フォームの最小化ボタンの表示、非表示を切り替える
            this.MinimizeBox = !this.MinimizeBox;


            // マウスをクリックしたときのイベント
            this.panel1.MouseDown += new MouseEventHandler(this.Form1_MouseDown);

            // マウスが移動したときのイベント
            this.panel1.MouseMove += new MouseEventHandler(this.Form1_MouseMove);
        }

        //Form1のMouseDownイベントハンドラ
        //マウスのボタンが押されたとき
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }

            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                //位置を記憶する
                mousePoint = this.PointToScreen(new Point(e.X, e.Y));

                this.Hide();
            }
        }

        //Form1のMouseMoveイベントハンドラ
        //マウスが動いたとき
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;

                /*
                // ポイントの場所を判定する
                Point p = new Point(e.X, e.Y);

                // スクリーン判定
                Screen s = Screen.FromPoint(p);


                if (s.Primary)
                {
                    //自分自身のフォームを最大化
                    this.WindowState = FormWindowState.Maximized;
                }
                */

            }
        }

    }

}
