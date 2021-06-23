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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            comboBox1.SelectedItem = "SLR(1)分析";
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

        //打开文件
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();//显示对话框接返回值
            if (result == DialogResult.OK)
            {
                richTextBox1.Text = RWStream.ReadFile(openFileDialog1.FileName);
            }
             
        }

        List<Grammar> Grammars = new List<Grammar>();
        List<Grammar> tool = new List<Grammar>();//用来删除的工具，和Grammars初始值一样
        List<char> Vn = new List<char>();//非终结符
        List<char> Vt = new List<char>();//终结符
        Dictionary<char, int> GrammarPairs = new Dictionary<char, int>();
        char begin;
        /*-----------------------------------------------------------------------------------*/
        Projects Pr0jects = new Projects();//增广文法的项集集合
        List<Project> Cores = new List<Project>();//内核项（没用到）
        List<Projects> LR0Collections = new List<Projects>();//项集簇的DFA
        Dictionary<char, int> ActionPairs = new Dictionary<char, int>();
        Dictionary<char, int> GotoPairs = new Dictionary<char, int>();

        //构建预测分析表
        private void button4_Click(object sender, EventArgs e)//构建文法
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
            begin = text[0];
            text = Regex.Replace(text, @" ", "");
            string argument = String.Format("@->{0}\n", begin);//补充一个增广文法 @->begin
            text = argument + text;
            string[] texts = text.Split('\n');
            int rightindex = 0;
            foreach (string i in texts)//生成文法相关信息
            {
                
                if (i != "")//生成相应的文法
                {
                    string[] splits = i.Split(new char[] { '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
                    //string[] splitRight = splits[1].Split('|');
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
                            rightindex++;
                        }
                        if (!Vn.Contains(splits[0].ToCharArray()[0]))
                        {
                            Vn.Add(splits[0].ToCharArray()[0]);

                            Grammar prod1 = new Grammar(splits[0], rights1);
                            Grammar prod2 = new Grammar(splits[0], rights2);
                            //Grammar prod2 = new Grammar(splits[0],splits2[1]);
                            prod1.rightBegin = rightindex - prod1.right.Count;
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
                                    rightindex++;
                                    break;
                                }
                            }
                        }
                        //Grammars.Add(prod2);
                    }
                    else
                    {

                        if (!Vn.Contains(splits[0].ToCharArray()[0]))
                        { //说明该非终结符还未创建Grammar
                            Vn.Add(splits[0].ToCharArray()[0]);
                            List<string> rights1 = new List<string>();
                            List<string> rights2 = new List<string>();
                            rights1.Add(splits[1]);
                            rights2.Add(splits[1]);
                            rightindex++;
                            Grammar prod1 = new Grammar(splits[0], rights1);
                            Grammar prod2 = new Grammar(splits[0], rights2);
                            prod1.rightBegin = rightindex - prod1.right.Count;
                            Grammars.Add(prod1);
                            tool.Add(prod2);
                            
                        }
                        else
                        {
                            for (int k = 0; k < Grammars.Count; k++) //说明该非终结符已经创建Grammar，直接加入
                            {
                                if (Grammars[k].left == splits[0].ToCharArray()[0])
                                {
                                    
                                    Grammars[k].right.Add(splits[1]);
                                    tool[k].right.Add(splits[1]);
                                    rightindex++;
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
            MakeFollowSet();
            MakeProjects();
            FindAllCores();
            MakeLR0Collections();
            ShowStateInfo();

            int select = comboBox1.SelectedIndex;
            switch (select)
            {
                case 0: MakeLR0AnalysisTable(); break;
                case 1: MakeSLR1AnalysisTable(); break;
                case 2: MessageBox.Show("该功能尚未完成"); break;
                default: MakeSLR1AnalysisTable(); break;
            }
            
        }
        //退出 测试
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "i*i+i";
        }
        //进行句子分析
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DoAnalysis();
            }
            else
            {
                MessageBox.Show("请输入句子");
            }
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
                if (dels.Count == g.right.Count)
                {
                    int key;
                    GrammarPairs.TryGetValue(g.left, out key);
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

            while (true)
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
                                else if (p == 0)
                                {
                                    dels.Add(s);
                                }
                            }
                        }
                        foreach (char c in delc)
                        {
                            int d = s.IndexOf(c);
                            g.right[spos] = s.Remove(d, 1);

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
                foreach (Grammar g in Grammars)
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

        private void MakeFirstSet()//编译原理4.2节计算First集的方法
        {
            while (true)
            {
                int prior = FirstMounts();
                foreach (Grammar g in Grammars)
                {
                    if (g.isEmpty == 1)//能到空先把空加进来
                    {
                        g.FIRST.Add('$');
                    }
                    foreach (string s in g.right)
                    {
                        foreach (char c in s)
                        {
                            if (Vt.Contains(c))//终结符直接加进来然后退出，判断下一句文法
                            {
                                g.FIRST.Add(c);
                                Unrepeated(g.FIRST);
                                break;
                            }
                            else if (Vn.Contains(c))//非终结符要把它的First集加进来
                            {
                                int pos;
                                if (GrammarPairs.TryGetValue(c, out pos))
                                {
                                    g.FIRST.AddRange(Grammars[pos].FIRST);
                                }
                                Unrepeated(g.FIRST);
                                if (Grammars[pos].isEmpty == 0)//如果非终结符推不到空，则退出，否则继续往后遍历
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (prior == FirstMounts())
                {
                    break;
                }

            }
            
        }//非递归计算First集 可以获取非LL文法的First集
        private int FirstMounts()
        {
            int sum = 0;
            foreach (Grammar g in Grammars)
            {
                sum = sum + g.FIRST.Count;
            }
            return sum;
        }//计算first集数量
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
                                    else if (Grammars[pos3].isEmpty == 1 && p == s.Length - 1)
                                    {
                                        Grammars[pos1].FOLLOW.AddRange(g.FOLLOW);
                                        Unrepeated(Grammars[pos1].FOLLOW);
                                        break;
                                    }
                                    p++;
                                }

                            }
                            else if (pos2 != -1 && pos2 == s.Length - 1)
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
                if (prior == FollowMounts())
                {
                    break;
                }

            }
        }

        private int FollowMounts()
        {
            int sum = 0;
            foreach (Grammar g in Grammars)
            {
                sum = sum + g.FOLLOW.Count;
            }
            return sum;
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

        private void MakeProjects()//初始化项集 给每个位置插入.
        {
            Pr0jects.items.Clear();
            foreach (Grammar g in Grammars)
            {
                foreach (string s in g.right)
                {
                    if (s == "$")
                    {
                        Project proj = new Project();
                        proj.left = g.left;
                        proj.right = ".";
                        Pr0jects.Add(proj);
                        continue;
                    }

                    for (int i = 0; i <= s.Length; i++)
                    {
                        string newproj = s;
                        newproj =  newproj.Insert(i, ".");
                        Project p = new Project();
                        p.left = g.left;
                        p.right = newproj;
                        Pr0jects.Add(p);
                    }
                }
            }
        }

        private void FindAllCores()//找所有内核项 可以不写
        {
            Cores.Clear();
            Cores.Add(Pr0jects.items[0]);
            foreach (Project p in Pr0jects.items)
            {
                foreach (char c in p.right)
                {
                    if (c == '.' && p.right.IndexOf(c) != 0)
                    {
                        Cores.Add(p);
                    }
                }
            }
        }

        private Projects CLOSURE(Project core)//闭包函数
        {
            Projects J = new Projects();
            J.Add(core);
            Pr0jects.initAdded();
            while (true)
            {
                char pointRight = ' ';//存贮.右侧的非终结符

                for (int j = 0; j<J.items.Count; j++)
                {
                    if (J.items[j].added == false)
                    {
                        J.items[j].added = true;
                    }
                    else
                    {
                        continue;
                    }
                    for (int i = 0; i < J.items[j].right.Length; i++)//对J中每个项进行查找 A->a.Xb
                    {
                        if (J.items[j].right[i] == '.')
                        {
                            if (i != J.items[j].right.Length - 1)
                            {
                                if (Vn.Contains(J.items[j].right[i+1]))
                                {
                                    pointRight = J.items[j].right[i + 1];
                                    break;
                                }

                            }
                            else if (i == J.items[j].right.Length - 1 && i == 0)
                            {
                                 pointRight = '$';
                            }
                        }
                    }

                    
                    if (pointRight != ' ')
                    {
                        foreach (Project p in Pr0jects.items)
                        {
                            if (p.left == pointRight && p.right[0] == '.')//在Pr0jects中查找 X->.y加入J
                            {
                                J.Add(p);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                    
                }

                bool end = true;
                foreach (Project j in J.items)
                {
                    if (j.added == false)
                    {
                        end = false;
                        break;
                    }
                }
                if (end)
                {
                    break;
                }
            }
            return J;
        }

        private Projects GOTO(Projects I , char X)//找A->a.Xb 对应的 A->aX.b集合的闭包
        {
            Projects J = new Projects();
            foreach (Project p in I.items)
            {
                for (int i = 0; i < p.right.Length; i++)
                {
                    if ( i != 0 && p.right[i] == X && p.right[i - 1] == '.')//对A->a.Xb的点进行转移
                    {
                        string str = String.Copy(p.right);
                        str = str.Insert(i + 1, ".");
                        str = str.Remove(i - 1, 1);
                        
                        Project proj = new Project();
                        proj.left = p.left;
                        proj.right = str;
                        J.Add(proj);
                        if (i + 1 == p.right.Length)
                        {
                            J.Reduction = J.items.Count - 1;
                        }
                        if (J.Reduction != -1 && J.next.Count !=0)
                        {
                            J.Conflict = true;
                        }
                        Projects closure = CLOSURE(proj);//对项集簇进行闭包生成
                        foreach (Project clos in closure.items)
                        {
                            if (!J.items.Contains(clos))
                            {
                                J.items.Add(clos);
                            }
                            
                        }
                        break;
                    }
                    else if (i == 0)
                    {
                        continue;
                    }
                }
 
            }
            return J;
        }

        private void MakeLR0Collections()
        {
            LR0Collections.Clear();
            LR0Collections.Add(CLOSURE(Cores[0]));//加入第一个项集簇
            List<char> ch = new List<char>();//项集簇的所有文法符号
            while (true)
            {
                int before = LR0Collections.Count;
                for (int i = 0; i < LR0Collections.Count; i++)
                {
                    if (LR0Collections[i].gotoed == true)//已经使用GOTO函数的项集簇直接跳过
                    {
                        continue;
                    }
                    else
                    {
                        LR0Collections[i].gotoed = true;
                    }

                    ch.Clear();
                    for (int j = 0; j < LR0Collections[i].items.Count; j++)//找该项集簇的所有文法符号
                    {
                        for(int k = 0; k < LR0Collections[i].items[j].right.Length; k++)
                        {
                            if (k!= LR0Collections[i].items[j].right.Length-1 && LR0Collections[i].items[j].right[k] == '.')
                            {
                                if (!ch.Contains(LR0Collections[i].items[j].right[k + 1]))
                                {
                                    ch.Add(LR0Collections[i].items[j].right[k + 1]);
                                }
                            }
                        }
                        
                    }

                    foreach (char c in ch)//对符号集的每个符号进行goto操作
                    {
                        Projects projs = GOTO(LR0Collections[i], c);
                        int ifexist = ExistInCollection(LR0Collections, projs);//判断该项集簇是否已经存在在DFA中
                        if (projs.items.Count > 0 && ifexist == -1)
                        {
                            path path = new path();
                            path.num = LR0Collections.Count;
                            path.symbol = c;
                            LR0Collections[i].next.Add(path);
                            LR0Collections.Add(projs);//不存在则插入并且在next中加入path
                        }
                        else if (projs.items.Count > 0 && ifexist > -1)
                        {
                            path path = new path();
                            path.num = ifexist;
                            path.symbol = c;
                            LR0Collections[i].next.Add(path);//存在则只更改项集簇的path
                        }
                    }
                }

                int after = LR0Collections.Count;
                if(before == after)
                {
                    break;
                }
            }
        }

        private int ExistInCollection(List<Projects> collection, Projects projs)//判断该项集簇是否已经存在在DFA中
        {

            int index = -1;
            if (projs.items.Count > 0)
            {
                foreach (Projects ps in collection)
                {
                    if (ps.items[0].left == projs.items[0].left && ps.items[0].right == projs.items[0].right)
                    {
                        index = collection.IndexOf(ps);
                        break;
                    }
                }
            }
            else
            {
                index = -2;
            }

            return index;
        }

        private void ShowStateInfo()
        {
            StateInfo.Rows.Clear();
            foreach (Projects ps in LR0Collections)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewTextBoxCell newCellnum = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell newCellset = new DataGridViewTextBoxCell();
                newCellnum.Value = LR0Collections.IndexOf(ps);
                string str = "";
                foreach (Project p in ps.items)
                {
                    
                    str = str + p.left + "->" + p.right + ",";
                }
                newCellset.Value = "{" + str + "}";
                newRow.Cells.Add(newCellnum);
                newRow.Cells.Add(newCellset);
                StateInfo.Rows.Add(newRow);
            }
        }

        private void MakeLR0AnalysisTable()//通过DFA构建LR0表
        {
            AnalysisTable.Columns.Clear();
            ActionPairs.Clear();
            GotoPairs.Clear();
            DataGridViewTextBoxColumn title = new DataGridViewTextBoxColumn();
            title.HeaderText = "状态";
            title.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            AnalysisTable.Columns.Add(title);
            Vt.Add('$');
            foreach (char c in Vt)
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderText = c.ToString();
                newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                AnalysisTable.Columns.Add(newColumn);
                ActionPairs.Add(c, AnalysisTable.Columns.Count - 1);
            }
            foreach (char c in Vn)
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderText = c.ToString();
                newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                AnalysisTable.Columns.Add(newColumn);
                GotoPairs.Add(c, AnalysisTable.Columns.Count - 1);
            }

            foreach (Projects ps in LR0Collections)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewTextBoxCell statenum = new DataGridViewTextBoxCell();
                statenum.Value = LR0Collections.IndexOf(ps).ToString();
                newRow.Cells.Add(statenum);
               
                
                if (ps.items[0].right.IndexOf('.') != ps.items[0].right.Length - 1)//如果'.'不在在产生式的最右侧那么进行移进操作 否则进行规约
                {
                    foreach (char c in Vt)
                    {
                        DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                        bool findph = false;
                        foreach (path ph in ps.next)
                        {
                            if (ph.symbol == c)
                            {
                                findph = true;
                                newCell.Value = "S" + ph.num.ToString();
                                break;
                            }
                        }
                        if (!findph)
                        {
                            newCell.Value = " ";
                        }
                        newRow.Cells.Add(newCell);
                    }
                    foreach (char c in Vn)
                    {
                        DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                        bool find = false;
                        foreach (path ph in ps.next)
                        {
                            if (ph.symbol == c)
                            {
                                find = true;
                                newCell.Value = ph.num.ToString();
                                break;
                            }
                        }
                        if (!find)
                        {
                            newCell.Value = " ";
                        }
                        newRow.Cells.Add(newCell);
                    }
                    AnalysisTable.Rows.Add(newRow);
                }
                else//.在最后一位则进行规约
                {
                    int pos;
                    GrammarPairs.TryGetValue(ps.items[0].left,out pos);
                    string compare = String.Copy(ps.items[0].right);

                    compare = compare.Remove(compare.IndexOf('.'), 1);
                    int Inright = Grammars[pos].right.IndexOf(compare);
                    int rightindex = Inright + Grammars[pos].rightBegin;
                    foreach (char c in Vt)
                    {
                        DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                        if (rightindex != 0)
                        {
                            newCell.Value = "r" + rightindex;
                        }
                        else
                        {
                            newCell.Value = " ";
                            if (c == '$')
                            {
                                newCell.Value = "acc";
                            }
                        }
                        
                        newRow.Cells.Add(newCell);   
                    }
                    AnalysisTable.Rows.Add(newRow);
                }
            }
            Vt.Remove('$');
        }

        private void MakeSLR1AnalysisTable()//通过DFA构建SLR表
        {
            AnalysisTable.Columns.Clear();
            ActionPairs.Clear();
            GotoPairs.Clear();
            DataGridViewTextBoxColumn title = new DataGridViewTextBoxColumn();
            title.HeaderText = "状态";
            title.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            AnalysisTable.Columns.Add(title);
            Vt.Add('#');
            foreach (char c in Vt)//给终结符添加列
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderText = c.ToString();
                newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                AnalysisTable.Columns.Add(newColumn);
                ActionPairs.Add(c,AnalysisTable.Columns.Count - 1);
            }
            foreach (char c in Vn)//给非终结符添加列
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderText = c.ToString();
                newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                AnalysisTable.Columns.Add(newColumn);
                GotoPairs.Add(c,AnalysisTable.Columns.Count - 1);
            }

            foreach (Projects ps in LR0Collections)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewTextBoxCell statenum = new DataGridViewTextBoxCell();
                statenum.Value = LR0Collections.IndexOf(ps).ToString();
                newRow.Cells.Add(statenum);

                if (ps.Reduction == -1)//如果'.'不在在产生式的最右侧那么进行移进操作 否则进行规约
                {
                    foreach (char c in Vt)
                    {
                        DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                        bool findph = false;
                        foreach (path ph in ps.next)
                        {
                            if (ph.symbol == c)
                            {
                                findph = true;
                                newCell.Value = "S" + ph.num.ToString();
                                break;
                            }
                        }
                        if (!findph)
                        {
                            newCell.Value = " ";
                        }
                        newRow.Cells.Add(newCell);
                    }
                    foreach (char c in Vn)
                    {
                        DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                        bool find = false;
                        foreach (path ph in ps.next)
                        {
                            if (ph.symbol == c)
                            {
                                find = true;
                                newCell.Value = ph.num.ToString();
                                break;
                            }
                        }
                        if (!find)
                        {
                            newCell.Value = " ";
                        }
                        newRow.Cells.Add(newCell);
                    }
                    AnalysisTable.Rows.Add(newRow);
                }
                else
                {
                    int pos;
                    GrammarPairs.TryGetValue(ps.items[ps.Reduction].left, out pos);
                    string compare = String.Copy(ps.items[ps.Reduction].right);

                    compare = compare.Remove(compare.IndexOf('.'), 1);
                    int Inright = Grammars[pos].right.IndexOf(compare);//right在内部的序号
                    int rightindex = Inright + Grammars[pos].rightBegin;//内部序号 + 外部序号 = 实际的序号

                    foreach (char c in Vt)
                    {
                        DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                        newCell.Value = " ";
                        if (rightindex != 0)
                        {
                            if (Grammars[pos].FOLLOW.Contains(c))
                            {
                                newCell.Value = "r" + rightindex;
                            }

                        }
                        else
                        {  
                            if (c == '#')
                            {
                                newCell.Value = "acc";
                            }
                        }

                        foreach (path ph in ps.next)
                        {
                            if (ph.symbol == c)
                            {
                                newCell.Value = "S" + ph.num.ToString();
                                break;
                            }
                        }
                        newRow.Cells.Add(newCell);
                    }
                    AnalysisTable.Rows.Add(newRow);
                }
            }
            
            Vt.Remove('#');
        }

        private void DoAnalysis()
        {
            StepTable.Rows.Clear();
            string inputText = textBox1.Text;
            Stack<int> stateStack = new Stack<int>();
            Stack<char> symbolStack = new Stack<char>();
            stateStack.Push(0);
            symbolStack.Push('#');
            inputText = inputText.Insert(inputText.Length, "#");
            int stepnum = 0;
            if (comboBox1.SelectedIndex == 1)
            {
                Vt.Add('#');
            }
            else
            {
                Vt.Add('$');
            }

            bool end = false;
            while (true)
            {
                stepnum++;
                if (Vt.Contains(inputText[0]))//Action表中的操作
                {

                    DataGridViewRow stepRow = new DataGridViewRow();
                    DataGridViewTextBoxCell Cell1 = new DataGridViewTextBoxCell();//步骤数
                    DataGridViewTextBoxCell Cell2 = new DataGridViewTextBoxCell();//状态栈
                    DataGridViewTextBoxCell Cell3 = new DataGridViewTextBoxCell();//符号栈
                    DataGridViewTextBoxCell Cell4 = new DataGridViewTextBoxCell();//输入串
                    DataGridViewTextBoxCell Cell5 = new DataGridViewTextBoxCell();//推导所用产生式
                    int pos;
                    ActionPairs.TryGetValue(inputText[0], out pos);
                    string step = AnalysisTable.Rows[stateStack.Peek()].Cells[pos].Value.ToString();
                    Cell1.Value = stepnum.ToString();
                    Cell2.Value = String.Join("",stateStack.ToArray().Reverse());
                    Cell3.Value = String.Join("",symbolStack.ToArray().Reverse());
                    Cell4.Value = inputText;
                    
                    
                    if (step[0] == 'S' && inputText[0] != '#' && inputText[0] != '$')
                    {
                        Cell5.Value = "移入" + step;
                        string strnum = step.Remove(0,1);//删除第一个字符
                        int nextstate = Convert.ToInt32(strnum);
                        stateStack.Push(nextstate);
                        symbolStack.Push(inputText[0]);
                        inputText = inputText.Remove(0,1);
                    }
                    else if(step[0] == 'r')
                    {
                        Cell5.Value = "用" + step + "规约";
                        string strnum = step.Remove(0, 1);//删除第一个字符
                        int statustate = Convert.ToInt32(strnum);
                        Project proj = Statute(statustate);
                        for (int i = 0; i < proj.right.Length; i++)//根据规约右部的长度对状态栈和符号栈进行删除
                        {
                            stateStack.Pop();
                            if (proj.right[proj.right.Length - i -1] == symbolStack.Peek()) {
                                symbolStack.Pop();
                            }
                            else
                            {
                                MessageBox.Show("规约失败");
                                Cell5.Value = "规约失败";
                                end = true;
                                break;
                            }
                        }
                        if (!end)
                        {
                            int gotopos;
                            GotoPairs.TryGetValue(proj.left, out gotopos);//在goto表中找列坐标
                            int gotonum = Convert.ToInt32(AnalysisTable.Rows[stateStack.Peek()].Cells[gotopos].Value.ToString());
                            stateStack.Push(gotonum);
                            symbolStack.Push(proj.left);
                        }
                        
                    }
                    else if(step[0] == 'a')
                    {
                        Cell5.Value = "接受";
                        end = true;
                      
                    }
                    else
                    {
                        Cell5.Value = "分析失败";
                        end = true;
                        
                    }

                    stepRow.Cells.Add(Cell1);
                    stepRow.Cells.Add(Cell2);
                    stepRow.Cells.Add(Cell3);
                    stepRow.Cells.Add(Cell4);
                    stepRow.Cells.Add(Cell5);
                    StepTable.Rows.Add(stepRow);
                    
                }
                else
                {
                    MessageBox.Show("....");
                }
                if (end)
                {
                    break;
                }
                //if (Vn.Contains(symbolStack.Peek()))//Goto表中的操作
                //{


                //}
            }

            Vt.Remove('#');
            Vt.Remove('$');

        }


        private Project Statute(int num)
        {
            Project proj = new Project();
            for (int i = Grammars.Count -1; i >= 0; i--)
            {
                if(Grammars[i].rightBegin <= num)
                {
                    int index = num - Grammars[i].rightBegin;
                    proj.left = Grammars[i].left;
                    proj.right = Grammars[i].right[index];
                    break;
                }

            }
            return proj;
        }



    }

    class Projects
    {
        public List<Project> items = new List<Project>();
        public List<path> next = new List<path>();
        public bool gotoed = false;
        public int Reduction = -1;
        public bool Conflict = false;
        public void Add(Project p)
        {
            if (!items.Contains(p))
            {
                items.Add(p);
            }
            else
            {
                //Console.WriteLine("Same!");
            }
        }

        public void initAdded()
        {
            foreach (Project p in items)
            {
                p.added = false;
            }
        }
    }

    class Project{
        public char left;
        public string right;
        public bool added = false;
    }

    class path
    {
        public int num = -1;
        public char symbol;
    }
}
