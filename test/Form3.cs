using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        //初始化对话框
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

        //打开文件
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();//显示对话框接返回值
            if (result == DialogResult.OK)
            {
                richTextBox1.Text = RWStream.ReadFile(openFileDialog1.FileName);
            }

        }
        //确认文件
        private void button2_Click(object sender, EventArgs e)
        {

        }
        //保存
        private void button3_Click(object sender, EventArgs e)
        {

        }

        List<Grammar> Grammars = new List<Grammar>();
        List<Grammar> tool = new List<Grammar>();//用来删除的工具，和Grammars初始值一样
        List<char> Vn = new List<char>();//非终结符
        List<char> Vt = new List<char>();//终结符
        Dictionary<char, int> GrammarPairs = new Dictionary<char, int>();
        char begin;
        //构建预测表
        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                button1_Click(sender, e);
            }
            Grammars.Clear();
            tool.Clear();
            Vn.Clear();
            Vt.Clear();
            GrammarPairs.Clear();
            string text = richTextBox1.Text;
            text = Regex.Replace(text, @" ","");
            string []texts = text.Split('\n');
            //foreach (string s in texts)
            //{
            //    foreach (char c in s)
            //    {
            //        if(c == ' ' || c == '\t')
            //        {
            //            s.Remove(s.IndexOf(c), 1);
            //        }
            //    }
            //}
            foreach (string i in texts)//生成文法相关信息
            {
                
                if(i != "")//生成相应的文法
                {
                    string[] splits = i.Split(new char[] {'-','>' },StringSplitOptions.RemoveEmptyEntries);
                    int o_r = -1;
                    foreach (char j in splits[1])
                    {
                        if (j == '|')
                        {
                            o_r = splits[1].IndexOf(j);
                            break;
                        }
                    }
                    if (o_r != -1)
                    {
                        string[] splits2 = splits[1].Split('|');

                        List<string> rights1 = new List<string>();
                        List<string> rights2 = new List<string>();
                        foreach (string s in splits2)
                        {
                            rights1.Add(s);
                            rights2.Add(s);
                        }
                        if (!Vn.Contains(splits[0].ToCharArray()[0]))
                        {
                            Vn.Add(splits[0].ToCharArray()[0]);
                            
                            Grammar prod1 = new Grammar(splits[0],rights1);
                            Grammar prod2 = new Grammar(splits[0],rights2);
                            //Grammar prod2 = new Grammar(splits[0],splits2[1]);
                            Grammars.Add(prod1);
                            tool.Add(prod2);
                        }
                        else
                        {
                            for (int k = 0; k < Grammars.Count; k++)
                            {
                                if (Grammars[k].left == splits[0].ToCharArray()[0])
                                {
                                    Grammars[k].right.AddRange(rights1);
                                    tool[k].right.AddRange(rights2);
                                    break;
                                }
                            }
                        }
                        //Grammars.Add(prod2);
                    }
                    else
                    {

                        if (!Vn.Contains(splits[0].ToCharArray()[0])) { //说明该非终结符还未创建Grammar
                            Vn.Add(splits[0].ToCharArray()[0]);
                            List<string> rights1 = new List<string>();
                            List<string> rights2 = new List<string>();
                            rights1.Add(splits[1]);
                            rights2.Add(splits[1]);
                            Grammar prod1 = new Grammar(splits[0],rights1);
                            Grammar prod2 = new Grammar(splits[0],rights2);
                            Grammars.Add(prod1);
                            tool.Add(prod2);
                        }
                        else
                        {
                            for (int k =0; k<Grammars.Count; k++) //说明该非终结符已经创建Grammar，直接加入
                            {
                                if (Grammars[k].left == splits[0].ToCharArray()[0])
                                {
                                    Grammars[k].right.Add(splits[1]);
                                    tool[k].right.Add(splits[1]);
                                }
                            }
                        }
                    }
                }
                 
            }

            for (int i = 0; i < Grammars.Count; i++)//收集非终结符和相应的键值对
            {
                GrammarPairs.Add(Grammars[i].left, i);
                foreach (string s in Grammars[i].right)//收集终结符
                {
                    foreach (char c in s)
                    {
                        if (c != '|' && c != '$' && (c < 'A' || c > 'Z'))
                        {
                            Vt.Add(c);
                        }
                    }
                    
                }
            }
            begin = Grammars[0].left;
            Unrepeated(Vt);
            MakeisEmpty();
            MakeFirstSet();
            ShowFirstSet();
            MakeFollowSet();
            ShowFollowSet();
            MakeSelectSet();
            ShowForecast();
            isLL1();
        }
        //测试
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "i+i*i";
        }
        //句子检测
        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                if (Forecast.Rows.Count == 0)
                {
                    button4_Click(sender, e);
                }
                
                MakeAnalysis();
            }
            else
            {
                MessageBox.Show("请输入字符串");
            }
        }
        private void EliminateLeftRecursion()//消除左递归
        {

        }
        private void MakeisEmpty()//编译原理4.2节判断非终结符是否能推出空串
        {
            //List<Grammar> tool = new List<Grammar>();
            //tool.InsertRange(0,Grammars);
            List<Grammar> delg = new List<Grammar>();//需要删除的非终结符
            foreach (Grammar g in tool)
            {
                List<string> dels = new List<string>();//需要删除的产生式
                bool temp = false;
                foreach (string s in g.right)
                {
                    if (Vt.Contains(s[0]))
                    {
                        dels.Add(s);
                    }
                    else if (s[0] == '$')
                    {
                        temp = true;
                        break;
                    }
                }
                if(dels.Count == g.right.Count)
                {
                    int key;
                    GrammarPairs.TryGetValue(g.left,out key);
                    Grammars[key].isEmpty = 0;
                    delg.Add(g);
                }
                else if (temp)
                {
                    delg.Add(g);
                    int key;
                    GrammarPairs.TryGetValue(g.left, out key);
                    Grammars[key].isEmpty = 1;
                    Grammars[key].FIRST.Add('$');
                }
                foreach (string s in dels)
                {
                    g.right.Remove(s);
                }

               
            }

            foreach (Grammar g in delg)
            {
                tool.Remove(g);
            }

            while(true)
            {
                delg = new List<Grammar>();
                foreach (Grammar g in tool)
                {
                    List<string> dels = new List<string>();
                    for (int spos = 0; spos < g.right.Count; spos++)
                    {
                        string s = g.right[spos];
                        List<char> delc = new List<char>();//需要删除的字符
                        foreach (char c in s)
                        {
                            if (Vn.Contains(c))
                            {
                                int pos;
                                GrammarPairs.TryGetValue(c, out pos);
                                int p = Grammars[pos].isEmpty;
                                if (p == 1)
                                {
                                    delc.Add(c);
                                    if (s.Count() == 1)
                                    {
                                        
                                        int key;
                                        GrammarPairs.TryGetValue(g.left, out key);
                                        Grammars[key].isEmpty = 1;
                                        Grammars[key].FIRST.Add('$');
                                        delg.Add(Grammars[key]);
                                    }
                                }
                                else if(p == 0)
                                {
                                    dels.Add(s);
                                }
                            }
                        }
                        foreach (char c in delc)
                        {
                            int d = s.IndexOf(c);
                            g.right[spos] = s.Remove(d,1);
                     
                        }
                        
                    }
                    if (dels.Count == g.right.Count)
                    {
                        int key;
                        GrammarPairs.TryGetValue(g.left, out key);
                        Grammars[key].isEmpty = 0;
                        
                    }
                    foreach (string s in dels)
                    {
                        g.right.Remove(s);
                    }
                }
                foreach (Grammar g in delg)
                {
                    tool.Remove(g); 
                }
                bool outbreak = true;
                foreach(Grammar g in Grammars)
                {
                    if (g.isEmpty == -1)
                    {
                        outbreak = false;
                        break;
                    }
                }
                if (outbreak == true)
                {
                    break;
                }
            }


        }

        private void MakeFirstSet()
        {
            foreach (Grammar g in Grammars)
            {
                foreach (string s in g.right)
                {
                    foreach (char c in s)
                    {
                        if (Vt.Contains(c))//如果是终结符，则加入First集跳出
                        {
                            g.FIRST.Add(c);
                            break;
                        }
                        else if (Vn.Contains(c))//如果是非终结符开始递归查找
                        {
                            List<char> joinFrist = new List<char>();
                            DfsFindVt(c, joinFrist);
                            g.FIRST.AddRange(joinFrist);
                            
                            
                        }
                        else
                        {
                            break;
                        }
                        int pos;
                        GrammarPairs.TryGetValue(c, out pos);
                        if (Grammars[pos].isEmpty == 0)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                Unrepeated(g.FIRST);
            }


        }

        private void DfsFindVt(char inVn,List<char> joinFrist)//深度优先搜索终结符
        {
            int pos;           
            GrammarPairs.TryGetValue(inVn, out pos);//从字典中获取映射值
            foreach (string s in Grammars[pos].right)
            {
                foreach (char c in s)
                {
                    if (Vt.Contains(c))
                    {
                        joinFrist.Add(c);
                        break;
                    }
                    else if(Vn.Contains(c))
                    {
                        DfsFindVt(c, joinFrist);
                    }
                    else if(c == '$')
                    {
                        break;
                    }
                    GrammarPairs.TryGetValue(c, out pos);
                    if (Grammars[pos].isEmpty == 0)//如果该非终结符无法推出$可以直接跳出
                    {
                        break;
                    }
                    else//如果该非终结符可推出$那么还要继续递归查找下一个非终结符
                    {
                        continue;
                    }
                }
            }

        }
 
        private void ShowFirstSet()
        {
            First.Columns.Clear();
            DataGridViewTextBoxColumn title = new DataGridViewTextBoxColumn();
            title.HeaderText = "First集";
            title.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            First.Columns.Add(title);
            Vt.Add('$');//暂时将$加入终结符号集Vt中
            foreach (char c in Vt)//加入列
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderText = c.ToString();
                newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                First.Columns.Add(newColumn);
            }
            foreach (Grammar g in Grammars)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewTextBoxCell left = new DataGridViewTextBoxCell();
                left.Value = g.left.ToString();
                
                newRow.Cells.Add(left);
                foreach (char c in Vt)
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    if (g.FIRST.Contains(c))
                    {
                        newCell.Value = "1";
                    }
                    else
                    {
                        newCell.Value = " ";
                    }
                    newRow.Cells.Add(newCell);
                }
                First.Rows.Add(newRow);
            }
            Vt.Remove('$');//将空符号集删除
            First.CurrentCell = null;
        }    

        private void MakeFollowSet()//first加空 T不对少加follow（s）
        {
            while (true) 
            {
                int prior = FollowMounts();
                foreach (char v in Vn)
                {
                    int pos1;//建立FOLLOW集的非终结符的下标
                    GrammarPairs.TryGetValue(v, out pos1);
                    if (v == begin)
                    {
                        Grammars[pos1].FOLLOW.Add('#');
                        Unrepeated(Grammars[pos1].FOLLOW);
                    }
                    foreach (Grammar g in Grammars)
                    {
                        foreach (string s in g.right)
                        {
                            
                            int pos2 = s.IndexOf(v);//建立FOLLOW集的非终结符在文法右侧的下标
                            if (pos2 != -1 && pos2 != s.Length - 1)
                            {
                                int p = pos2 + 1;
                                while (p < s.Length)
                                {
                                    int pos3;//建立FOLLOW集的非终结符后方的非终结符的下标
                                    GrammarPairs.TryGetValue(s[p], out pos3);
                                    if (Vt.Contains(s[p]))
                                    {
                                        Grammars[pos1].FOLLOW.Add(s[p]);
                                        Unrepeated(Grammars[pos1].FOLLOW);
                                        break;
                                    }
                                    else if (Vn.Contains(s[p]))
                                    {
                                        //List<char> joinFollow = new List<char>();
                                        //DfsFindVt(s[p], joinFollow);
                                        Grammars[pos1].FOLLOW.AddRange(Grammars[pos3].FIRST);
                                        if (Grammars[pos3].isEmpty == 1)
                                        {
                                            Grammars[pos1].FOLLOW.Remove('$');
                                        }
                                        Unrepeated(Grammars[pos1].FOLLOW);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    
                                    if (Grammars[pos3].isEmpty == 0)
                                    {
                                        break;
                                    }
                                    else  if(Grammars[pos3].isEmpty == 1 && p == s.Length - 1)
                                    {
                                        Grammars[pos1].FOLLOW.AddRange(g.FOLLOW);
                                        Unrepeated(Grammars[pos1].FOLLOW);
                                        break;
                                    }
                                    p++;
                                }
                                
                            }
                            else if(pos2 != -1 && pos2 == s.Length - 1)
                            {
                                Grammars[pos1].FOLLOW.AddRange(g.FOLLOW);
                                Unrepeated(Grammars[pos1].FOLLOW);
                            }
                            else if (pos2 == -1)
                            {
                                continue;
                            }
                        }
                    
                    }
                }
                if(prior == FollowMounts())
                {
                    break;
                }
                
            }
        }

        private int  FollowMounts()
        {
            int sum = 0;
            foreach (Grammar g in Grammars)
            {
                sum = sum + g.FOLLOW.Count;
            }
            return sum;
        }

        private void ShowFollowSet()
        {
            Follow.Columns.Clear();
            DataGridViewTextBoxColumn title = new DataGridViewTextBoxColumn();
            title.HeaderText = "Follow集";
            title.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            Follow.Columns.Add(title);
            Vt.Add('#');//暂时将$加入终结符号集Vt中
            foreach (char c in Vt)//加入列
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderText = c.ToString();
                newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Follow.Columns.Add(newColumn);
            }
            foreach (Grammar g in Grammars)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewTextBoxCell left = new DataGridViewTextBoxCell();
                left.Value = g.left.ToString();

                newRow.Cells.Add(left);
                foreach (char c in Vt)
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    if (g.FOLLOW.Contains(c))
                    {
                        newCell.Value = "1";
                    }
                    else
                    {
                        newCell.Value = " ";
                    }
                    newRow.Cells.Add(newCell);
                }
                Follow.Rows.Add(newRow);
            }
            Vt.Remove('#');//将空符号集删除
            Follow.CurrentCell = null;
        }
        private void MakeSelectSet()
        {
            foreach (Grammar g in Grammars)
            {
                foreach (string s in g.right)
                {
                    List<char> joinSelect = new List<char>();
                    foreach (char c in s)
                    {
                        int pos;
                        GrammarPairs.TryGetValue(c, out pos);
                        if (Vn.Contains(c))
                        {
                            joinSelect.AddRange(Grammars[pos].FIRST);
                            joinSelect.Remove('$');
                            if (Grammars[pos].isEmpty == 0)
                            {
                                break;
                            }
                            else
                            {
                                if (s.IndexOf(c) == s.Length - 1)
                                {
                                    int leftpos;
                                    GrammarPairs.TryGetValue(c, out leftpos);
                                    joinSelect.AddRange(Grammars[leftpos].FOLLOW);
                                    //joinSelect.Remove('#');
                                }
                            }
                        }
                        else if (Vt.Contains(c))
                        {
                            joinSelect.Add(c);
                            break;
                        }
                        else if (c == '$')
                        {
                                int leftpos;
                                GrammarPairs.TryGetValue(c, out leftpos);
                                joinSelect.AddRange(Grammars[leftpos].FOLLOW);
                                //joinSelect.Remove('#');
                        }
                        
                    }
                    Unrepeated(joinSelect);
                    g.rightSELECT.Add(joinSelect);
                }
                //foreach ()
                //{

                //}
            }
           
        }

        private void isLL1()
        {
            bool b = true;
            foreach (Grammar g in Grammars)
            {
                foreach (List<char> list in g.rightSELECT)
                {
                    List<char> temp = new List<char>();
                    foreach (char c in list)
                    {
                        if (temp.IndexOf(c) != -1)
                        {
                            temp.Add(c);
                        }
                        else
                        {
                            b = false;
                            MessageBox.Show("该文法不是LL（1）");
                            break;
                        }
                        
                    }
                    if (!b)
                    {
                        break;
                    }
                }
                if (!b)
                {
                    break;
                }
            }

            if (b == true)
            {
                MessageBox.Show("该文法是LL（1）");
            }

        }
        private void ShowForecast()
        {
            Forecast.Columns.Clear();
            DataGridViewTextBoxColumn title = new DataGridViewTextBoxColumn();
            title.HeaderText = "预测表";
            title.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            Forecast.Columns.Add(title);
            Vt.Add('#');//暂时将$加入终结符号集Vt中
            foreach (char c in Vt)//加入列
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderText = c.ToString();
                newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Forecast.Columns.Add(newColumn);
            }
            foreach (Grammar g in Grammars)//加入行
            {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewTextBoxCell left = new DataGridViewTextBoxCell();
                left.Value = g.left.ToString();

                newRow.Cells.Add(left);
                foreach (char c in Vt)
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    foreach (List<char> l in g.rightSELECT)
                    {
                        if (l.Contains(c))
                        {
                            newCell.Value = "->" + g.right[g.rightSELECT.IndexOf(l)];
                        }
                    }
                    
                    newRow.Cells.Add(newCell);
                }
                Forecast.Rows.Add(newRow);
            }
            Vt.Remove('#');//将空符号集删除
            Follow.CurrentCell = null;
        }

        private void MakeAnalysis()
        {

            Analysis.Rows.Clear();
            string text = textBox1.Text;
            Stack<char> AnalysisStack = new Stack<char>();
            List<char> InputList = new List<char>();
            AnalysisStack.Push('#');
            AnalysisStack.Push(begin);//分析栈的初始化
            InputList.AddRange(textBox1.Text.ToCharArray());
            InputList.Add('#');//输入串的初始化
            int stepnum = 0;//步骤数
            bool end = false;

            foreach (char c in text)//先对句子的每一个进行匹配消除
            {
                int index = 0;
                foreach (DataGridViewTextBoxColumn column in Forecast.Columns)//找到当前匹配字符在预测表的哪一列
                {
                    if (column.HeaderText == c.ToString())
                    {
                         index = Forecast.Columns.IndexOf(column);
                    }
                }
                //foreach (DataGridViewRow ForecastRow in Forecast.Rows)
                //{
                while (true) 
                { 
                
                    char stackhead = AnalysisStack.Peek();//判断分析栈的栈顶符号为何种情况
                    if (Vn.Contains(stackhead))//如果栈顶符号为终结符
                    {
                        int pos;
                        GrammarPairs.TryGetValue(stackhead, out pos);
                        if (Forecast.Rows[pos].Cells[index].Value != null)//如果该非终结符有推出匹配字符的产生式则输出该步骤
                        {
                            stepnum++;//步骤数++
                            DataGridViewRow analRow = new DataGridViewRow();
                            DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                            DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//分析栈
                            DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                            DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                            Cell1.Value = stepnum.ToString();
                            Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                            Cell3.Value = String.Join("", InputList.ToArray());
                            Cell4.Value = Forecast.Rows[pos].Cells[0].Value.ToString() + Forecast.Rows[pos].Cells[index].Value.ToString();
                            analRow.Cells.Add(Cell1);
                            analRow.Cells.Add(Cell2);
                            analRow.Cells.Add(Cell3);
                            analRow.Cells.Add(Cell4);
                            Analysis.Rows.Add(analRow);
                            char[] fit = Forecast.Rows[pos].Cells[index].Value.ToString().ToCharArray();
                            Array.Reverse(fit);//将字符串反转
                            AnalysisStack.Pop();
                            for (int i = 0; i<fit.Count() - 2; i++)
                            {
                                AnalysisStack.Push(fit[i]);//把所用的产生式的右部加入栈
                            }
                                
                        }
                        else//如果该非终结符没有有推出匹配字符的产生式则判断，非终结符是否可以推出空
                        {
                            if (Grammars[pos].isEmpty == 1)//可以推出空就继续输出该步骤，并出栈
                            {
                                stepnum++;//步骤数++
                                DataGridViewRow analRow = new DataGridViewRow();
                                DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                                DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//分析栈
                                DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                                DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                                Cell1.Value = stepnum.ToString();
                                Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                                Cell3.Value = String.Join("", InputList.ToArray());
                                Cell4.Value = Forecast.Rows[pos].Cells[0].Value.ToString() + Forecast.Rows[pos].Cells[Forecast.Rows[pos].Cells.Count - 1].Value.ToString();
                                analRow.Cells.Add(Cell1);
                                analRow.Cells.Add(Cell2);
                                analRow.Cells.Add(Cell3);
                                analRow.Cells.Add(Cell4);
                                Analysis.Rows.Add(analRow);
                                AnalysisStack.Pop();
                            }
                            else if(Grammars[pos].isEmpty == 0)//如果推不出空，那么分析终止，失败
                            {
                                stepnum++;//步骤数++
                                DataGridViewRow analRow = new DataGridViewRow();
                                DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                                DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//分析栈
                                DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                                DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                                Cell1.Value = stepnum.ToString();
                                Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                                Cell3.Value = String.Join("", InputList.ToArray());
                                Cell4.Value = "失败";
                                analRow.Cells.Add(Cell1);
                                analRow.Cells.Add(Cell2);
                                analRow.Cells.Add(Cell3);
                                analRow.Cells.Add(Cell4);
                                Analysis.Rows.Add(analRow);
                                end = true;
                                break;
                            }
                        }
                        
                        //}
                    }
                    else if(Vt.Contains(stackhead) && stackhead == c)//如果栈顶元素为终结符且与匹配字符相同那么输出该步骤
                    {
                        stepnum++;//步骤数++
                        DataGridViewRow analRow = new DataGridViewRow();
                        DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                        DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//分析栈
                        DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                        DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                        Cell1.Value = stepnum.ToString();
                        Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                        Cell3.Value = String.Join("", InputList.ToArray());
                        Cell4.Value = stackhead.ToString() + "匹配";
                        analRow.Cells.Add(Cell1);
                        analRow.Cells.Add(Cell2);
                        analRow.Cells.Add(Cell3);
                        analRow.Cells.Add(Cell4);
                        Analysis.Rows.Add(analRow);
                        AnalysisStack.Pop();
                        break;
                    }
                    else//该字符既不是终结符也不是非终结符，直接失败
                    {
                        stepnum++;//步骤数++
                        DataGridViewRow analRow = new DataGridViewRow();
                        DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                        DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//分析栈
                        DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                        DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                        Cell1.Value = stepnum.ToString();
                        Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                        Cell3.Value = String.Join("", InputList.ToArray());
                        Cell4.Value = "失败";
                        analRow.Cells.Add(Cell1);
                        analRow.Cells.Add(Cell2);
                        analRow.Cells.Add(Cell3);
                        analRow.Cells.Add(Cell4);
                        Analysis.Rows.Add(analRow);
                        end = true;
                        break;
                    }
                }
                if (end)//分析已经失败直接跳出
                {
                    MessageBox.Show("输入串不是文法的句子");
                    break;
                }
                InputList.RemoveAt(0);
            }

            if (!end)
            {
                while (AnalysisStack.Count > 1)//继续处理分析栈中剩下的字符,因为输入串已经匹配玩，需要剩下的字符为空才对
                {
                    char stackhead = AnalysisStack.Peek();
                    if (Vn.Contains(stackhead))
                    {
                        int pos;
                        GrammarPairs.TryGetValue(stackhead, out pos);
                        if (Grammars[pos].isEmpty == 1)//可以推出空
                        {
                            stepnum++;//步骤数++
                            DataGridViewRow analRow = new DataGridViewRow();
                            DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                            DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//分析栈
                            DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                            DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                            Cell1.Value = stepnum.ToString();
                            Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                            Cell3.Value = String.Join("", InputList.ToArray());
                            Cell4.Value = Forecast.Rows[pos].Cells[0].Value.ToString() + Forecast.Rows[pos].Cells[Forecast.Rows[pos].Cells.Count - 1].Value.ToString();
                            analRow.Cells.Add(Cell1);
                            analRow.Cells.Add(Cell2);
                            analRow.Cells.Add(Cell3);
                            analRow.Cells.Add(Cell4);
                            Analysis.Rows.Add(analRow);
                            AnalysisStack.Pop();
                        }
                        else if (Grammars[pos].isEmpty == 0)//不可以推出空直接失败
                        {
                            stepnum++;//步骤数++
                            DataGridViewRow analRow = new DataGridViewRow();
                            DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                            DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//分析栈
                            DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                            DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                            Cell1.Value = stepnum.ToString();
                            Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                            Cell3.Value = String.Join("", InputList.ToArray());
                            Cell4.Value = "失败";
                            analRow.Cells.Add(Cell1);
                            analRow.Cells.Add(Cell2);
                            analRow.Cells.Add(Cell3);
                            analRow.Cells.Add(Cell4);
                            Analysis.Rows.Add(analRow);
                            end = true;
                            break;
                        }
                    }
                    else//终结符直接失败
                    {
                        stepnum++;//步骤数++
                        DataGridViewRow analRow = new DataGridViewRow();
                        DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                        DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//分析栈
                        DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                        DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                        Cell1.Value = stepnum.ToString();
                        Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                        Cell3.Value = String.Join("", InputList.ToArray());
                        Cell4.Value = "失败";
                        analRow.Cells.Add(Cell1);
                        analRow.Cells.Add(Cell2);
                        analRow.Cells.Add(Cell3);
                        analRow.Cells.Add(Cell4);
                        Analysis.Rows.Add(analRow);
                        end = true;
                        break;
                    }

                    
                }
                if(!end){
                    //最后说明分析成功
                    stepnum++;//步骤数++
                    DataGridViewRow _analRow = new DataGridViewRow();
                    DataGridViewTextBoxCell _Cell1 = new DataGridViewTextBoxCell();//步骤数
                    DataGridViewTextBoxCell _Cell2 = new DataGridViewTextBoxCell();//分析栈
                    DataGridViewTextBoxCell _Cell3 = new DataGridViewTextBoxCell();//剩余输入串
                    DataGridViewTextBoxCell _Cell4 = new DataGridViewTextBoxCell();//推导所用产生式
                    _Cell1.Value = stepnum.ToString();
                    _Cell2.Value = String.Join("", AnalysisStack.ToArray().Reverse());
                    _Cell3.Value = String.Join("", InputList.ToArray());
                    _Cell4.Value = "接受";
                    _analRow.Cells.Add(_Cell1);
                    _analRow.Cells.Add(_Cell2);
                    _analRow.Cells.Add(_Cell3);
                    _analRow.Cells.Add(_Cell4);
                    Analysis.Rows.Add(_analRow);
                    AnalysisStack.Pop();
                    MessageBox.Show("该输入串是文法的句子");
                }
                
           
            }

        }

        private void Unrepeated(List<char> indexs)//去除子集中重复的项
        {

            int count = indexs.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (indexs[i] == indexs[j])
                    {
                        indexs.RemoveAt(i);
                        count = indexs.Count;
                        i--;
                        break;
                    }

                }
            }
        }


    }

    class Grammar
    {
        public char left;
        public List<string> right = new List<string>();
        public List<char> FIRST = new List<char>();
        public List<char> FOLLOW = new List<char>();
        public List<List<char>> rightSELECT = new List<List<char>>();//该非终结符右部SELECT集
        public List<char> leftSELECT = new List<char>();//该非终结符右部SELECT集的交集作为左部的SELECT集
        public int isEmpty = -1;//能否推出空
        public int rightBegin = 0;//该非终结符的产生式总的的下标应该从哪开始 计算前面的产生式数量 
        public Grammar(string left,List<string> right)
        {
            
            this.left = left.ToCharArray()[0];
            this.right = right;
        }
    }



}
