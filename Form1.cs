﻿using System;
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

        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e) {
            if((e.KeyCode == Keys.N) && ((Control.ModifierKeys & Keys.Control) == Keys.Control)) {
                dataGridView.Rows.Insert(dataGridView.CurrentCell.RowIndex , new DataGridViewRow());

                //行挿入で最終選択行がずれる
                lastSelectedRowIndex++;
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
    }
}
