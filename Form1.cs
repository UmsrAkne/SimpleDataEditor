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


        public Form1() {
            InitializeComponent();
            String sourceText = readSourceTextFile(@"C:\sampleData.txt");
            List<String[]> currentText = splitTextData(sourceText);
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
                    System.Console.WriteLine(st2);
                }
            }

            return moreSeparated;
        }

    }
}
