using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {   
        List<string> Words = new List<string>();
        List<string> Operators = new List<string>();
        List<string> Signs = new List<string>();
        List<WordStuff[]> WordCuts = new List<WordStuff[]>();
        List<ErrorStuff> Errors = new List<ErrorStuff>();
        public Form1()
        {   
            InitializeComponent();
            initDialog();
            InitKeys();
        }

        public void initDialog()
        {
            openFileDialog1.InitialDirectory = @"C:\Users\67406\Desktop\compile";//设置起始文件夹
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";//设置文件筛选类型
            openFileDialog1.FileName = "";//设施初始的文件名为空
            openFileDialog1.CheckFileExists = true;//检查文件是否存在
            openFileDialog1.CheckPathExists = true;//检查路径是否存在

            saveFileDialog1.InitialDirectory = @"C:\Users\67406\Desktop\compile";
            saveFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FileName = "文件名";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();//显示对话框接返回值
            if (result == DialogResult.OK)
            {
                richTextBox1.Text = RWStream.ReadFile(openFileDialog1.FileName);
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string Text = richTextBox2.Text + richTextBox3.Text; 
                bool IsOk = RWStream.WriteFile(saveFileDialog1.FileName, Text);
                if (IsOk)
                    MessageBox.Show("文件已保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            richTextBox2.ReadOnly = false;
            richTextBox3.ReadOnly = false;

        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            richTextBox2.ReadOnly = true;
            richTextBox3.ReadOnly = true;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            WordCuts.Clear();
            Errors.Clear();
            string CodeText = richTextBox1.Text;
            WordSeparate(CodeText);
            WordAnalysis();
            ErrorAnalysis();
            WordPrint(sender,e);
            ErrorPrint();
            
        }

        private void InitKeys()
        {
            Words.Add("auto");
            Words.Add("break");
            Words.Add("case");
            Words.Add("char");
            Words.Add("const");
            Words.Add("continue");
            Words.Add("default");
            Words.Add("do");
            Words.Add("double");
            Words.Add("else");
            Words.Add("enum");
            Words.Add("extern");
            Words.Add("float");
            Words.Add("for");
            Words.Add("goto");
            Words.Add("if");
            Words.Add("int");
            Words.Add("long");
            Words.Add("register");
            Words.Add("return");
            Words.Add("short");
            Words.Add("signed");
            Words.Add("sizeof");
            Words.Add("static");
            Words.Add("struct");
            Words.Add("switch");
            Words.Add("typedef");
            Words.Add("union");
            Words.Add("unsigned");
            Words.Add("void");
            Words.Add("volatile");
            Words.Add("while");

            //Operators.Add("(");
            //Operators.Add(")");
            Operators.Add("[");
            Operators.Add("]");
            Operators.Add("->");
            Operators.Add(".");
            Operators.Add("!");
            Operators.Add("~");
            Operators.Add("++");
            Operators.Add("--");
            Operators.Add("-");
            //Operator.Add("(type)");
            //Operator.Add("*var");
            Operators.Add("&");
            Operators.Add("*");
            Operators.Add("/");
            Operators.Add("%");
            Operators.Add("+");
            Operators.Add("-");
            Operators.Add("<<");
            Operators.Add(">>");
            Operators.Add("<");
            Operators.Add("<=");
            Operators.Add(">");
            Operators.Add(">=");
            Operators.Add("==");
            Operators.Add("!=");
            Operators.Add("&");
            Operators.Add("^");
            Operators.Add("&&");
            Operators.Add("||");
            Operators.Add("?");
            Operators.Add("=");
            Operators.Add("+=");
            Operators.Add("-=");
            Operators.Add("*=");
            Operators.Add("/=");
            Operators.Add("%=");
            Operators.Add("<<=");
            Operators.Add(">>=");
            Operators.Add("&=");
            Operators.Add("^=");
            Operators.Add("|=");
            //Operators.Add(",");

            Signs.Add("#");
            Signs.Add("{");
            Signs.Add("}");
            Signs.Add("(");
            Signs.Add(")");
            Signs.Add("<");
            Signs.Add(">");
            Signs.Add("'");
            Signs.Add("\"");
            Signs.Add(";");
            Signs.Add(",");
            Signs.Add("\\");
            Signs.Add(" ");


        }
        private void WordSeparate(string CodeText)
        {
            string[] CodePart = CodeText.Split('\n');
            for(int i= 0; i < CodePart.Length; i++)//对第i行进行处理
            {   
                WordStuff[] stuffs=new WordStuff[1000];
                for(int j = 0; j < 1000; j++)
                {
                    stuffs[j] = new WordStuff();
                }
                int num = 0;
                int indexB = 0;
                int indexE = 0;
                while (indexE<CodePart[i].Length)//对每个字符进行遍历
                {
        
                    string forSC = CodePart[i].Substring(indexB,1);
                    if (Operators.IndexOf(forSC) < 0 && Signs.IndexOf(forSC) < 0)//不是运算符也不是界符
                    {
                        int k;
                        for (k = 1; indexE + k < CodePart[i].Length; k++)
                        {
                            if (Operators.IndexOf(CodePart[i].Substring(indexE + k, 1)) > 0 || Signs.IndexOf(CodePart[i].Substring(indexE + k, 1)) > 0)
                            {
                                if (CodePart[i].Substring(indexE + k, 1) == "." && indexE + k + 1 == CodePart[i].Length) 
                                {
                                    k = k + 1;
                                    indexE = indexE + k;
                                    break;
                                }
                                else if(CodePart[i].Substring(indexE+k,1)=="."&& CodePart[i].Substring(indexE + k + 1, 1).CompareTo("0")>=0&& CodePart[i].Substring(indexE + k, 1).CompareTo("9") <= 0|| CodePart[i].Substring(indexE + k, 1).CompareTo(".") == 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    indexE = indexE + k - 1;
                                    break;
                                }
                                
                            }
                            
                     
                        }
                        if ((indexE + k) == CodePart[i].Length)
                        {
                            indexE = indexE + k;
                            //break;
                        }

                        char[] ch = CodePart[i].ToCharArray(indexB,k);
                        string str = new string(ch);
                        //Console.WriteLine(str);

                        stuffs[num].word = str;
                        num++;
                
                    }
                    else if (Operators.IndexOf(forSC) > 0)//是运算符
                    {
                        int k;
                        for(k = 1; indexE+k < CodePart[i].Length; k++)
                        {
                            if(Operators.IndexOf(CodePart[i].Substring(indexE+k, 1)) < 0)
                            {
                                indexE = indexE + k - 1;
                                break;
                            }
                            if (indexE + k == CodePart[i].Length)
                            {
                                indexE = indexE + k;
                                break;
                            }
                            if(k >= 2)
                            {
                                indexE = indexE + k - 1;
                                break;
                            }
                            
                        }
                        
                        char[] ch = CodePart[i].ToCharArray(indexB,k);
                        string str = new string(ch);
                        //Console.WriteLine(str);

                        stuffs[num].word = str;
                        stuffs[num].type = "运算符";
                        num++;


                    }
                    else if (Signs.IndexOf(forSC) > 0)//是界符
                    {
                        //Console.WriteLine(forSC);

                        stuffs[num].word = forSC;
                        num++;

                    }

                    indexE++;
                    indexB = indexE;
                }
                WordCuts.Add(stuffs);
            }
            
        }

        private void WordAnalysis()
        {
            for(int i = 0; i < WordCuts.Count; i++)
            {
                for (int j = 0; WordCuts[i][j].word!=null; j++)//末尾可能有空字符
                {
                    if (Words.Contains(WordCuts[i][j].word) == true)
                    {
                        WordCuts[i][j].lineNum = i;
                        WordCuts[i][j].type = "保留字";
                        WordCuts[i][j].iflegel = 0;
                        WordCuts[i][j].wordNum = Words.IndexOf(WordCuts[i][j].word);

                    }
                    else if (WordCuts[i][j].type=="运算符")
                    {
                        if (Operators.Contains(WordCuts[i][j].word) == true)
                        {
                            WordCuts[i][j].lineNum = i;
                            WordCuts[i][j].iflegel = 0;
                            WordCuts[i][j].wordNum = Words.Count + Operators.IndexOf(WordCuts[i][j].word);
                        }
                        else
                        {
                            WordCuts[i][j].lineNum = i;
                            WordCuts[i][j].iflegel = 1;
                            WordCuts[i][j].wordNum = Operators.IndexOf(WordCuts[i][j].word);
                        }

                    }
                    else if (Signs.Contains(WordCuts[i][j].word) == true)
                    {
                        WordCuts[i][j].lineNum = i;
                        WordCuts[i][j].type = "界符";
                        WordCuts[i][j].iflegel = 0;
                        WordCuts[i][j].wordNum = Words.Count + Operators.Count + Signs.IndexOf(WordCuts[i][j].word);
                    }
                    else 
                    { 
                        int k;
                        for(k = 0; k<WordCuts[i][j].word.Length; k++)
                        {
                            if ((WordCuts[i][j].word[k] <= 'Z' && WordCuts[i][j].word[k] >= 'A') || (WordCuts[i][j].word[k] <= 'z' && WordCuts[i][j].word[k] >= 'a')) 
                            {
                                WordCuts[i][j].lineNum = i;
                                WordCuts[i][j].type = "标识符";
                                WordCuts[i][j].wordNum = 89;
                                if (WordCuts[i][j].word[0] == '_' || (WordCuts[i][j].word[0] <= 'Z' && WordCuts[i][j].word[0] >= 'A') || (WordCuts[i][j].word[0] <= 'z' && WordCuts[i][j].word[0] >= 'a'))
                                {
                                    WordCuts[i][j].iflegel = 0;
                                }
                                else 
                                {
                                    WordCuts[i][j].iflegel = 1;
                                }
                                break;
                            }
                           
                        }

                        if (k == WordCuts[i][j].word.Length)
                        {
                            WordCuts[i][j].lineNum = i;
                            WordCuts[i][j].type = "常量";
                            WordCuts[i][j].iflegel = 0;
                            int counts = 0;
                            for(int f = 0; f < k; f++)
                            {
                                if (WordCuts[i][j].word[f] == '.')
                                {
                                    counts++;
                                    if (counts > 1)
                                    {
                                        WordCuts[i][j].iflegel = 1;
                                        break;
                                    }
                                }
                            }
                            WordCuts[i][j].wordNum = 90;
                        }
                    }
                }
                
            }
        }
        
        private void WordPrint(object sender,EventArgs e)
        {
            string output = null;
            output = output + "标识符单词码为89 常量单词码为90                            \n";
            output = output + "------------------------------------------------------\n";
            output = output + "行号    单词    类型      是否合法    单词码               \n";
            output = output + "------------------------------------------------------\n";
            for(int i = 0; i < WordCuts.Count; i++)
            {
                for(int j = 0; WordCuts[i][j].word != null; j++)
                {
                    if (WordCuts[i][j].word != " ") 
                    {
                        string para3= WordCuts[i][j].type.PadRightWhileDouble(12,' ');
                        string para4= WordCuts[i][j].iflegel == 0 ? "合法".PadRightWhileDouble(12, ' ') : "非法".PadRightWhileDouble(12,' ');
                        //Console.WriteLine(para3+para4);
                        string str = String.Format("{0,-8}{1,-8}{2}{3}{4,-5}{5,-9}\n", WordCuts[i][j].lineNum + 1, WordCuts[i][j].word, para3, para4, WordCuts[i][j].wordNum, WordCuts[i][j].iflegel==0?"":"<-(错误行)");
                        output = output + str;
                    }
                        
                }
            }
            richTextBox2.Text = output;
        }
        private void ErrorAnalysis()
        {
            for(int i = 0; i < WordCuts.Count; i++)
            {
                for(int j = 0; WordCuts[i][j].word != null; j++)
                {
                    if (WordCuts[i][j].iflegel == 1 && WordCuts[i][j].type == "标识符")
                    {
                        WordCuts[i][j].wordNum = -1;
                        ErrorStuff error = new ErrorStuff();
                        error.word = WordCuts[i][j].word;
                        error.linenum = WordCuts[i][j].lineNum;
                        error.errortext = "标识符错误";
                        Errors.Add(error);
                    }
                    else if (WordCuts[i][j].iflegel == 1 && WordCuts[i][j].type == "运算符")
                    {
                        WordCuts[i][j].wordNum = -2;
                        ErrorStuff error = new ErrorStuff();
                        error.word = WordCuts[i][j].word;
                        error.linenum = WordCuts[i][j].lineNum;
                        error.errortext = "运算符错误";
                        Errors.Add(error);
                    }
                    else if(WordCuts[i][j].iflegel == 1 && WordCuts[i][j].type == "常量")
                    {
                        WordCuts[i][j].wordNum = -3;
                        ErrorStuff error = new ErrorStuff();
                        error.word = WordCuts[i][j].word;
                        error.linenum = WordCuts[i][j].lineNum;
                        error.errortext = "常量错误";
                        Errors.Add(error);
                    }
                }
            }
        }

        private void ErrorPrint()
        {
            string output = null;
            output = output + String.Format("{0}个错误                 \n",Errors.Count);
            output = output + "------------------------------------------------------\n";
            for (int i = 0; i < Errors.Count; i++)
            {
                        //string para1 = WordCuts[i][j].type.PadRightWhileDouble(12, ' ');
                        //string para4 = WordCuts[i][j].iflegel == 0 ? "合法".PadRightWhileDouble(12, ' ') : "非法".PadRightWhileDouble(12, ' ');
                        //Console.WriteLine(para3+para4);
                        string str = String.Format("{0}行\t{1,-8}{2}\n", Errors[i].linenum + 1, Errors[i].word,Errors[i].errortext);
                        output = output + str;
            }
            richTextBox3.Text = output;
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(sender, e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(sender, e);
        }

        private void 允许编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(sender, e);
        }

        private void 退出编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton5_Click(sender, e);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }
    }






    class WordStuff
    {
        public int lineNum = 0;
        public string word = null;
        public string type = null;
        public int iflegel = 0;
        public int wordNum = 0;
    }

    class ErrorStuff
    {
        public int linenum=0;
        public string word=null;
        public string errortext=null;
    }
    class RWStream
    {
        public static string ReadFile(string folder)
        {
            string content = "";//返回的结果字符串
            using (StreamReader sr = new StreamReader(folder))
            {
                content = sr.ReadToEnd();//一次性把文本内容读完
            }
            return content;
        }
        public static bool WriteFile(string folder, string content)
        {
            bool flag = true;
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(folder);//创建StreamWriter对象
                sw.WriteLine(content);


            }
            catch (Exception ex)
            {
                Console.WriteLine("写入流异常：" + ex.ToString());
                flag = false;
            }
            finally
            {
                sw.Close();//确保最后总会关闭流
                Console.WriteLine("写入流关闭");
            }
            return flag;
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// 按单字节字符串向左填充长度
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        public static string PadLeftWhileDouble(this string input, int length, char paddingChar = '\0')
        {
            var singleLength = GetSingleLength(input);
            return input.PadLeft(length - singleLength + input.Length, paddingChar);
        }
        private static int GetSingleLength(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
            return Regex.Replace(input, @"[^\x00-\xff]", "aa").Length;//计算得到该字符串对应单字节字符串的长度
        }
        /// <summary>
        /// 按单字节字符串向右填充长度
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        public static string PadRightWhileDouble(this string input, int length, char paddingChar = '\0')
        {
            var singleLength = GetSingleLength(input);
            return input.PadRight(length - singleLength + input.Length, paddingChar);
        }
    }

}
