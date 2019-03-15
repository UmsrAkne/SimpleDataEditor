using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace SimpleDataEditor {
    public partial class Form1 : Form {

        private int lastSelectedRowIndex = 0;
        private DataGridViewRow dgvClipBoard;

        public Form1() {
            InitializeComponent();
            String sourceText = readSourceTextFile(@"C:\sampleData.txt");
            List<String[]> currentText = splitTextData(sourceText);

            //読み込んだテキストファイルの行辺りの要素数を確認。データグリッドのColumnsを追加する。
            if (currentText != null || currentText[0].Length != 0) {
                for (int i = 0; i < currentText[0].Length; i++) {
                    dataGridView.Columns.Add(i.ToString(), i.ToString());
                    dataGridView.Columns[ dataGridView.Columns.Count -1 ].SortMode =
                        DataGridViewColumnSortMode.Programmatic;
                }
            }

            dataGridView.Rows.Add(new DataGridViewRow());
            dataGridView.Rows[0].Frozen = true;
            for(int i = 0; i < dataGridView.Columns.Count; i++) {
                DataGridViewButtonCell hideButton = new DataGridViewButtonCell();
                hideButton.Value = "><";
                dataGridView[i , 0] = hideButton;
            }

            //データグリッドに読み込んだテキストファイルを流し込む
            foreach (String[] ss in currentText) {
                dataGridView.Rows.Add(ss);
            }


            dataGridView.SelectionChanged += dataGirdView_SelectionChanged;
            dataGridView.KeyDown += dataGridView_KeyDown;
            dataGridView.CellContentClick += dataGridView_CellContentClick;

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            DataGridView dgv = dataGridView;

            const int HIDE_BUTTONS_ROW = 0;
            if (dgv.CurrentCell.RowIndex == HIDE_BUTTONS_ROW) {
                dgv.Columns[dgv.CurrentCell.ColumnIndex].Visible = false;
            }
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e) {
            DataGridView dgv = dataGridView;

            if ((e.KeyCode == Keys.N) && ((Control.ModifierKeys & Keys.Control) == Keys.Control)) {
                dataGridView.Rows.Insert(dataGridView.CurrentCell.RowIndex, new DataGridViewRow());

                //行挿入で最終選択行がずれる
                lastSelectedRowIndex++;
            }

            if ((e.KeyCode == Keys.X) && ((Control.ModifierKeys & Keys.Control) == Keys.Control)) {
                dgvClipBoard = dgv.Rows[dgv.CurrentRow.Index];
                dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
            }

            if ((e.KeyCode == Keys.V) && ((Control.ModifierKeys & Keys.Control) == Keys.Control)) {
                if (dgvClipBoard != null) {
                    dgv.Rows.Insert(dgv.CurrentRow.Index, dgvClipBoard);
                    dgvClipBoard = null;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private String readSourceTextFile(String filePath) {
            StreamReader reader = new StreamReader(filePath);
            String text = reader.ReadToEnd();
            reader.Close();
            return text;
        }

        private List<String[]> splitTextData( String sourceText ) {

            //分割は改行で切った後に、カンマで切り、二重配列にする
            string[] separated = sourceText.Split('\n');
            List<string[]> moreSeparated = new List<string[]>();

            foreach (String s in separated) {
                moreSeparated.Add(s.Split(','));
            }

            foreach (var st in moreSeparated) {
                foreach (var st2 in st) {
                }
            }

            return moreSeparated;
        }

        private void dataGirdView_SelectionChanged(object sender , EventArgs e) {

            //最後に選択されていたセルの背景色を先に変更しなければ、変更された色が残ったままになる。
            changeRowBackColor(lastSelectedRowIndex, Color.White);
            changeRowBackColor(dataGridView.CurrentCell.RowIndex, Color.AliceBlue);

            lastSelectedRowIndex = dataGridView.CurrentCell.RowIndex;
        }

        private void changeRowBackColor( int targetRowIndex , Color paintingColor ) {
            DataGridViewCellCollection cells = dataGridView.Rows[targetRowIndex].Cells;
            foreach (DataGridViewCell cell in cells) {
                cell.Style.BackColor = paintingColor;
            }
        }

        private void exportTextFile() {

            String unitText = "";

            foreach( DataGridViewRow row in dataGridView.Rows) {
                DataGridViewCellCollection cells = row.Cells;
                String unitRow = "";

                //空要素しかない空行は出力するテキストには含まない。
                foreach( DataGridViewCell cell in cells ){
                    if (cell.Value == null) {
                        continue;
                    }
                    unitRow += cell.Value + ",";
                }

                if (unitRow.Length > 0) {
                    unitRow = unitRow.Remove(unitRow.Length - 1);
                }

                unitText += unitRow + "\n";
            }

            File.WriteAllText(@"C:\sampleExpoted.txt", unitText);

        }
    }
}
