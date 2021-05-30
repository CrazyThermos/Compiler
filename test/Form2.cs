using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
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
        //读入nfa文件
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();//显示对话框接返回值
            if (result == DialogResult.OK)
            {
                try
                {
                    string CodeText = RWStream.ReadFile(openFileDialog1.FileName);
                    string[] CodePart = CodeText.Split('\n');



                    string[] nbstr = CodePart[0].Split(new char[] { '开', '始', '符', ':', ';' });
                    NFAb = Convert.ToInt32(nbstr[1]);
                    string[] nestr = CodePart[1].Split(new char[] { '终', '结', '符', ':' });
                    NFAe = Convert.ToInt32(nestr[1]);
                    string[] s = CodePart[2].Split(new char[] { '符', '号', '集', ':' });
                    string[] ss = s[1].Split(';');

                    NFA.EdgeSet.Clear();
                    NFA.StartState.StateName = NFAb;
                    NFA.EndState.StateName = NFAe;
                    transymbol.Clear();

                    for (int i = 0; i < ss.Length; i++)
                    {
                        char[] c = ss[i].ToCharArray();
                        if ('a' <= c[0] && c[0] <= 'z')
                        {
                            transymbol.Add(c[0]);
                        }
                    }

                    for (int i = 3; i < CodePart.Length; i++)
                    {
                        string[] edgeCh = CodePart[i].Split('\t');
                        edge newEdge = new edge();
                        newEdge.StartState.StateName = Convert.ToInt32(edgeCh[0]);
                        newEdge.TransSymbol = Convert.ToChar(edgeCh[1]);
                        newEdge.EndState.StateName = Convert.ToInt32(edgeCh[2]);
                        NFA.EdgeSet.Add(newEdge);
                    }
                    NFA.EdgeCount = NFA.EdgeSet.Count;
                    //richTextBox1.Text = RWStream.ReadFile(openFileDialog1.FileName);
                    WriteNFA();
                    READFILE = true;
                    char[] ch = new char[1];
                    NFAToDFA(ch);
                    MFAmax = DFAe + 1;
                    DFAToMFA(ch);
                    READFILE = false;
                }
                catch(Exception)
                {
                    MessageBox.Show("文件格式错误，请检查");
                }


            }
        }
        //读入dfa文件
        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();//显示对话框接返回值
            if (result == DialogResult.OK)
            {
                try
                {
                    string CodeText = RWStream.ReadFile(openFileDialog1.FileName);
                    string[] CodePart = CodeText.Split('\n');

                    DFA.EdgeSet.Clear();
                    DFA.StartState.StateName = DFAb;
                    DFA.EndState.StateName = DFAe;
                    transymbol.Clear();
                    MFAset = new CloseSet();
                    MFAes.Clear();
                    DFAes.Clear();

                    string[] nbstr = CodePart[0].Split(new char[] { ':', ';' });
                    DFAb = Convert.ToInt32(nbstr[1]);
                    string[] nestr = CodePart[1].Split(new char[] { ':', '\r' });
                    string[] nestrs = nestr[1].Split(';');
                    for (int i = 0; i < nestrs.Length; i++)
                    {
                        if (nestrs[i] != "")
                            DFAes.Add(Convert.ToInt32(nestrs[i]));
                    }
                    string[] countstr = CodePart[2].Split(new char[] { ':' });
                    int counts = Convert.ToInt32(countstr[1]);
                    MFAmax = counts;
                    string[] s = CodePart[3].Split(new char[] { ':' });
                    string[] ss = s[1].Split(';');




                    for (int i = 0; i < ss.Length; i++)
                    {
                        char[] c = ss[i].ToCharArray();
                        if ('a' <= c[0] && c[0] <= 'z')
                        {
                            transymbol.Add(c[0]);
                        }
                    }

                    for (int i = 4; i < CodePart.Length; i++)//建立dfa
                    {
                        if (CodePart[i] != "")
                        {
                            string[] edgeCh = CodePart[i].Split('\t');
                            edge newEdge = new edge();
                            newEdge.StartState.StateName = Convert.ToInt32(edgeCh[0]);
                            newEdge.TransSymbol = Convert.ToChar(edgeCh[1]);
                            newEdge.EndState.StateName = Convert.ToInt32(edgeCh[2]);
                            DFA.EdgeSet.Add(newEdge);
                        }

                    }

                    for (int i = 0; i < counts + 1; i++)
                    {
                        TSet newSet = new TSet();
                        for (int j = 0; j < transymbol.Count; j++)
                        {
                            newSet.next[j] = -2;
                        }
                        MFAset.CSet.Add(newSet);
                    }
                    for (int i = 4; i < CodePart.Length; i++)
                    {
                        string[] edgeCh = CodePart[i].Split(new char[] { ':', '\r', '\t' });
                        if (edgeCh[2] != "")
                        {
                            MFAset.CSet[Convert.ToInt32(edgeCh[0])]
                              .next[transymbol.IndexOf(Convert.ToChar(edgeCh[1]))] = Convert.ToInt32(edgeCh[2]);
                        }


                    }

                    DFA.EdgeCount = DFA.EdgeSet.Count;
                    //richTextBox1.Text = RWStream.ReadFile(openFileDialog1.FileName);
                    WriteDFA();
                    READFILE = true;
                    char[] ch = new char[1];
                    DFAToMFA(ch);
                    READFILE = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("文件格式错误，请检查");
                }
                
            }
        }
        //验证表达式的按钮
        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            if (Detection(str) != true)
            {
                MessageBox.Show("表达式错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //退出
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "(a|b)*abb";
        }
        //NormalToNFA的按钮
        private void button4_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            opLevel.Clear();
            calculate.Clear();
            output.Clear();
            if (Detection(str) != true)
            {
                MessageBox.Show("表达式错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Char[] ch = str.ToCharArray();
                stateName = 0;
                NormalToNFA(ch);
            }
        }
        //NFAToDFA

        private void button7_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            opLevel.Clear();
            calculate.Clear();
            output.Clear();
            if (Detection(str) != true)
            {
                MessageBox.Show("表达式错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Char[] ch = str.ToCharArray();
                stateName = 0;
                NFAToDFA(ch);
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            opLevel.Clear();
            calculate.Clear();
            output.Clear();
            if (Detection(str) != true)
            {
                MessageBox.Show("表达式错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Char[] ch = str.ToCharArray();
                stateName = 0;
                DFAToMFA(ch);
            }
        }
        /*
         *  '(',')'中间不能为空，必须对应
         *  '*'前面不为'(','|',只能在')','a','b'后面
         *  '|'左不得为'('右不得为')'
         *  'a''b'    
         *  这些算符的优先顺序为先'*'，再'+''-'，最后’|’
         */
        List<char> transymbol = new List<char>();
       
        private Boolean Detection(String str)
        {
            Char[] ch = str.ToCharArray();
            if (ch.Length == 0)
            {
                return false;
            }
            int ls = 0;//判断左右括号数量 右括号不能大于左括号
            int rs = 0;
            if (ch[0] == '|' || ch[0] == '*' || ch[0] == ')') {
                return false;
            }
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] == '(')
                {
                    ls++;
                    if (i + 1 != ch.Length)
                    {
                        if (ch[i + 1] == '|' || ch[i + 1] == '*' || ch[i + 1] == ')')
                        {
                            return false;
                        }
                    }
                }
                else if (ch[i] == ')')
                {
                    rs++;
                }
                else if (ch[i] == '|')
                {
                    if (i + 1 != ch.Length)
                    {
                        if (ch[i + 1] == '|' || ch[i + 1] == '*' || ch[i + 1] == ')')
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (ch[i] == '*')
                {
                    if (i + 1 != ch.Length)
                    {
                        if (ch[i + 1] == '*')
                        {
                    //        return false;
                        }
                    }
                }
                else if (ch[i] > 'z' || ch[i] < 'a')
                {
                    return false;
                }

                if (rs > ls)//右括号数量大于左括号
                {
                    return false;
                }
            }
            if (rs != ls)//左右括号数量不相等
            {
                return false;
            }
            return true;
        }

        List<char> output = new List<char>();
        Stack<char> opLevel = new Stack<char>();
        //Stack<char> calculate1 = new Stack<char>();
        Stack<cell> calculate = new Stack<cell>();

        
        cell NFA = new cell();
        cell DFA = new cell();
        cell MFA = new cell();
        CloseSet MFAset = new CloseSet();

        List<List<state>> P;
        
        //List<edge> aEdge = new List<edge>();
        //List<edge> bEdge = new List<edge>();
        int NFAb = 0;
        int NFAe = 0;
        int DFAb = 0;
        int DFAe = 0;
        int MFAb = 0;
        int MFAe = 0;
        int MFAmax = 0;
        //多个终结状态的情况
        List<int> DFAes = new List<int>();
        List<int> MFAes = new List<int>();
        int stateName = 0;
        bool READFILE = false;
        private void NormalToNFA(char[] ch)
        {
           
            List<char> chList = new List<char>();
            

            for(int i = 0; i < ch.Length; i++)
            {
                chList.Add(ch[i]);
            }
            transymbol.Clear();
            transymbol.AddRange(chList.Distinct());

            for(int i = 0; i < transymbol.Count; i++)
            {
                if (transymbol[i] < 'a' || transymbol[i] > 'z')
                {
                    transymbol.RemoveAt(i);
                    i--;
                }
            }
            
            int len = chList.Count;
            /*
             * 
             * 插入加号
             * 加号表示左->右 
             * 考虑 a+b a* ()左右两边的情况 
             * 
             */
            for(int i = 0; i < chList.Count; i++)
            {
                if (chList[i] == '(' && i > 0)
                {
                    if (chList[i - 1] != '(')
                    {
                            chList.Insert(i, '+');
                            i++;
                    }
                    
                }
                else if (transymbol.Contains(chList[i]) && i != 0)
                {
                    if (transymbol.Contains(chList[i - 1]) || chList[i - 1] == ')' || chList[i - 1] == '*')
                    {
                        chList.Insert(i, '+');
                        i++;

                    }
                }
                else if (transymbol.Contains(chList[i]) && i != 0)
                {
                    if (transymbol.Contains(chList[i - 1]) || chList[i - 1] == ')' || chList[i - 1] == '*')
                    {
                        chList.Insert(i, '+');
                        i++;

                    }
                }
            }
            
            string str = String.Join("", chList);
            char[] newch = chList.ToArray();
            
            FrontToBack(newch);
            Calculation();

            NFA = calculate.Pop();
            cell result = NFA;
            string output = null;
            output = output + "开始状态    接受符号    到达状态\n";
            for (int i = 0; i < result.EdgeCount; i++)
            {
                string s = String.Format("{0,-12}{1,-12}{2,-12}\n",result.EdgeSet[i].StartState.StateName, result.EdgeSet[i].TransSymbol, result.EdgeSet[i].EndState.StateName);
                output = output + s;
                
            }

            richTextBox1.Text = output;

            NFAb = NFA.StartState.StateName;
            NFAe = NFA.EndState.StateName;
            textBox2.Text = NFAb.ToString();
            textBox3.Text = NFAe.ToString();


        }

        private void WriteNFA()
        {
            cell result = NFA;
            string output = null;
            output = output + "开始状态    接受符号    到达状态\n";
            for (int i = 0; i < result.EdgeCount; i++)
            {
                string s = String.Format("{0,-12}{1,-12}{2,-12}\n", result.EdgeSet[i].StartState.StateName, result.EdgeSet[i].TransSymbol, result.EdgeSet[i].EndState.StateName);
                output = output + s;

            }

            richTextBox1.Text = output;

            NFAb = NFA.StartState.StateName;
            NFAe = NFA.EndState.StateName;
            textBox2.Text = NFAb.ToString();
            textBox3.Text = NFAe.ToString();
        }//将读取的NFA输出

        private void WriteDFA()
        {
            cell result = DFA;
            string output = null;
            output = output + "开始状态    接受符号    到达状态\n";
            for (int i = 0; i < result.EdgeCount; i++)
            {
                string s = String.Format("{0,-12}{1,-12}{2,-12}\n", result.EdgeSet[i].StartState.StateName, result.EdgeSet[i].TransSymbol, result.EdgeSet[i].EndState.StateName);
                output = output + s;

            }

            richTextBox2.Text = output;

            
            textBox4.Text = DFAb.ToString();

            string d = String.Join(",", DFAes.ToArray());
            textBox5.Text = d;

            
        }

        private void NFAToDFA(char[] ch )
        {
            if(READFILE == false)
            {
                NormalToNFA(ch);
            }
            
            CloseSet C = new CloseSet();
            TSet T0 = new TSet();
            //T0.tSet.Add(NFAb);
            List<int> t = new List<int>();
            t.Add(NFAb);
            T0 = E_Closure(t);

            T0.tSet.Sort();
            //T0.Unrepeated();
            C.CSet.Add(T0);
            


            while (C.Check() != true)
            {
                int order = C.Sign();
                TSet Ta = new TSet();
                TSet Tb = new TSet();

                List<TSet> Tedges = new List<TSet>();

                TSet T = new TSet();
                /*-------------------------------------------------------------------------*/
                for (int i = 0; i < C.CSet[order].tSet.Count; i++)
                {
                    T.tSet.Add(C.CSet[order].tSet[i]);
                }

                for (int i = 0; i < transymbol.Count; i++)
                {
                    TSet temp = E_Closure(Move(T, transymbol[i]));
                    temp.tSet.Sort();
                    Tedges.Add(temp);
                }

                for (int i = 0; i < transymbol.Count; i++)
                {
                   
                    if (C.JoinNew(Tedges[i]) == -1)
                    {
                        C.CSet.Add(Tedges[i]);
                        C.CSet[order].next[i] = C.CSet.Count - 1;
                    }
                    else
                    {
                        C.CSet[order].next[i] = C.JoinNew(Tedges[i]);
                    }
                }
               
            }

            MFAset = C;
            DFAes.Add(MFAset.CSet.Count-1);
            DFA = MakeDFAcell(C);

            cell result = DFA;
            string output = null;
            output = output + "开始状态    接受符号    到达状态\n";
            for (int i = 0; i < result.EdgeCount; i++)
            {
                string s = String.Format("{0,-12}{1,-12}{2,-12}\n", result.EdgeSet[i].StartState.StateName, result.EdgeSet[i].TransSymbol, result.EdgeSet[i].EndState.StateName);
                output = output + s;

            }

            richTextBox2.Text = output;

            DFAb = DFA.StartState.StateName;
            DFAe = DFA.EndState.StateName;
            textBox4.Text = DFAb.ToString();
            textBox5.Text = DFAe.ToString();

            if(READFILE == false)
            {
                textBox5.Text = DFAe.ToString();
            }
            else
            {
                List<int> ends = new List<int>();
                DFAes = ends;
                for(int i = 0; i < C.CSet.Count; i++)
                {
                    if (C.CSet[i].tSet.Contains(NFAe) == true)
                    {
                        ends.Add(i);
                    }
                }
                string d = String.Join(",", ends.ToArray());
                textBox5.Text = d;

            }
        }

        private void DFAToMFA(char[] ch)
        {
            if(READFILE == false)
            {
                NFAToDFA(ch);
                MFAmax = DFAe+1;
            }
            
            P = new List<List<state>>();
            P = InitP();
            while (true)
            {
                int lastCount = P.Count;
                for(int i = 0; i < P.Count; i++)
                {
                    for(int j = 0; j < transymbol.Count; j++)
                    {
                        List<List<state>> D = Divide(P[i], transymbol[j]);
                        P.RemoveAt(i);
                        P.InsertRange(i, D);
                    }

                }
                if (P.Count==lastCount)
                {
                    break;
                }
            }
            Simplification();

            MFA = MakeMFAcell();
            cell result = MFA;
            string output = null;
            output = output + "开始状态    接受符号    到达状态\n";
            for (int i = 0; i < result.EdgeCount; i++)
            {
                string s = String.Format("{0,-12}{1,-12}{2,-12}\n", result.EdgeSet[i].StartState.StateName, result.EdgeSet[i].TransSymbol, result.EdgeSet[i].EndState.StateName);
                output = output + s;

            }

            richTextBox3.Text = output;

            MFAb = MFA.StartState.StateName;
            MFAe = MFA.EndState.StateName;
            textBox6.Text = MFAb.ToString();
            if (READFILE == false)
            {
                textBox7.Text = MFAe.ToString();
            }
            else
            {
                string m = String.Join(",", MFAes.ToArray());
                textBox7.Text = m;
            }

        }

        /*-------------------------------------------------------DFA转MFA----------------------------------------------------------*/
        
        private cell MakeMFAcell()
        {
            cell m = new cell();
            /*-------------------------------------------------------------------------*/
            for (int i = 0; i < P.Count; i++)
            {
                for (int j = 0; j < transymbol.Count; j++)
                {
                    if(P[i][0].next[j] != -2)
                    {
                        edge e = new edge();
                        e.StartState.StateName = P[i][0].StateName;
                        e.TransSymbol = transymbol[j];
                        e.EndState.StateName = P[i][0].next[j];
                        m.EdgeSet.Add(e);

                    }
                    
                }

            }

            m.StartState.StateName = 0;
            m.EndState.StateName = DFA.EndState.StateName;
            m.EdgeCount = m.EdgeSet.Count;
            return m;
        }
        private void Simplification()//将P简化
        {
            for(int i = 0; i < P.Count; i++)//先改next
            {
                /*-------------------------------------------------------------------------*/
                for (int j = 0; j < transymbol.Count; j++)
                {
                    int temp = FindInP(P[i][0].next[j]);
                    if ( temp != -1)//针对特殊情况如 a ab abb等
                    {
                        P[i][0].next[j] = P[FindInP(P[i][0].next[j])][0].StateName;
                    }
                    
                }

            }
            for (int i = 0; i < P.Count; i++)//先改next
            {

                for(int k = 0; k < P[i].Count; k++)//找终结点 优化？
                {
                    bool jump = false;
                    for(int j = 0; j < DFAes.Count; j++)
                    {
                        if (P[i][k].StateName==DFAes[j])
                        {
                            MFAes.Add(P[i][0].StateName);
                            jump = true;
                            break;
                        }
                    }
                    if (jump == true)
                    {
                        break;
                    }
                    
                }

                if (P[i].Count > 1)
                {
                    for(int j = 1; j < P[i].Count; j++)
                    {
                        P[i].RemoveAt(j);
                    }
                }
                
            }
            Unrepeated(MFAes);
        }
     
        private List<List<state>> Divide(List<state> S, char ch)//切割函数
        {
            List<List<state>> D = new List<List<state>>(new List<state>[MFAmax]);
            for(int i = 0; i < MFAmax; i++)
            {
                D[i] = new List<state>();
            }
            List<int> indexs = new List<int>();
            /*-------------------------------------------------------------------------*/
            for (int i = 0; i < transymbol.Count; i++)
            {
                if (ch == transymbol[i])
                {
                    for (int j = 0; j < S.Count; j++)
                    {
                        int index = FindInP(S[j].next[i]);
                        if (index != -1)
                        {
                            D[index].Add(S[j]);
                            indexs.Add(index);
                        }
                        else//针对特殊情况如 a ab abb等 在符号边不存在的情况，直接新建一个节点放在最后
                        {
                            List<state> temp = new List<state>();
                            temp.Add(S[j]);
                            D.Add(temp);
                            indexs.Add(D.Count-1);
                        }

                    }
                }
            }

            List<List<state>> saparate = new List<List<state>>();
            Unrepeated(indexs);
            for (int i = 0; i < indexs.Count; i++)
            {
                saparate.Add(D[indexs[i]]);
            }
            return saparate;
        }

        private int FindInP(int next)//在P中查找属于哪个集合，返回下标
        {
            for(int i = 0; i < P.Count; i++)
            {
                for(int j = 0; j < P[i].Count; j++)
                {
                    if (P[i][j].StateName == next)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void Unrepeated(List<int> indexs)//去除子集中重复的项
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

        private List<List<state>> InitP()//初始化P
        {

            List<state> L = new List<state>();
            List<state> R = new List<state>();
            for (int i = DFAb; i < MFAset.CSet.Count; i++)
            {

                if (DFAes.Contains(i) == false)
                {
                    state s = new state();
                    s.StateName = i;
                    /*-------------------------------------------------------------------------*/
                    for (int j = 0; j < transymbol.Count; j++)
                    {
                        s.next[j] = MFAset.CSet[i].next[j];
                    }

                    L.Add(s);
                }
                else
                {
                    state s = new state();
                    s.StateName = i;
                    /*-------------------------------------------------------------------------*/
                    for (int j = 0; j < transymbol.Count; j++)
                    {
                        s.next[j] = MFAset.CSet[i].next[j];
                    }

                    R.Add(s);
                }

            }
            P.Add(L);
            P.Add(R);
            return P;
        }
        /*-------------------------------------------------------NFA转DFA----------------------------------------------------------*/


        private List<int> Move(TSet T,char ch)//move状态集
        {
            List<int> move = new List<int>();
            for(int i = 0; i < T.tSet.Count; i++)
            {
                for(int j = 0; j < NFA.EdgeSet.Count; j++)
                {
                    if (NFA.EdgeSet[j].TransSymbol == ch && NFA.EdgeSet[j].StartState.StateName == T.tSet[i])
                    {
                        move.Add(NFA.EdgeSet[j].EndState.StateName);
                    }
                }
            }
            return move;
        }

        private TSet E_Closure(List<int> move)//e-closure状态集
        {

            TSet e = new TSet();
            e.tSet.AddRange(move);
            List<int> redis = new List<int>();
            redis.AddRange(move);//上次遍历到的状态集合
            while (true)//广度优先搜索
            {
                int lastCount = e.tSet.Count;
                List<int> redis_c = new List<int>();//这次遍历到的状态集合
                for (int i = 0; i < NFA.EdgeSet.Count; i++)
                {
                    //判断是否符合条件
                    if (NFA.EdgeSet[i].TransSymbol == '#' && redis.Contains(NFA.EdgeSet[i].StartState.StateName) && !redis.Contains(NFA.EdgeSet[i].EndState.StateName))
                    {
                        redis_c.Add(NFA.EdgeSet[i].EndState.StateName);
                    }
                }
                redis = redis_c;//用于下次循环
                e.tSet.AddRange(redis);//把新发现的节点加入到e-closure状态集中
                if (lastCount == e.tSet.Count)//如果本次遍历没有增加值，那么跳出while
                {
                    break;
                }
            }
            return e;
        }

        private cell MakeDFAcell(CloseSet C)
        {
            cell c = new cell();
            /*-------------------------------------------------------------------------*/
            for (int i = 0; i < C.CSet.Count; i++)
            {
                for (int j = 0; j < transymbol.Count; j++)
                {
                    if (C.CSet[i].next[j] != -2)
                    {
                        edge e = new edge();
                        e.StartState.StateName = i;
                        e.TransSymbol = transymbol[j];
                        e.EndState.StateName = C.CSet[i].next[j];
                        c.EdgeSet.Add(e);
                    }
                    
                }
            }

            c.StartState.StateName = 0;
            c.EndState.StateName = C.CSet.Count - 1;
            c.EdgeCount = c.EdgeSet.Count;
            return c;
        }

        /*-------------------------------------------------------正规式转NFA----------------------------------------------------------
        * 运算符优先级
        * 1 (
        * 2 |
        * 3 + -
        * 4 *
        * 5 )
        * 
        */
        private int getLevel(char c)//获得优先级的函数
        {
            if (c == '(')
            {
                return 1;
            }
            else if (c == '|')
            {
                return 2;
            }
            else if (c == '+' || c=='-')
            {
                return 3;
            }
            else if (c == '*')
            {
                return 4;
            }
            else if (c == ')')
            {
                return 5;
            }
            else
            {
                return 0;
            }
      
        }

        private int compareLevel(char x,char y)//比较优先级函数
        {
            int xn = getLevel(x);
            int yn = getLevel(y);
            if (xn <= yn)
            {
                return 0;
            }
            else 
            {
                return 1;
            }
            
        }

        private void FrontToBack(char[] ch)//中缀转后缀
        {

            for(int i = 0; i < ch.Length; i++)
            {
                if ( ch[i] == '+' || ch[i] == '-' || ch[i] == '|' || ch[i] == '*' )
                {
                    if (opLevel.Count != 0)
                    {   
                        while(compareLevel(ch[i], opLevel.Peek())==0)
                        {   
                            output.Add(opLevel.Peek());
                            opLevel.Pop();
                            if (opLevel.Count == 0)
                            {
                                opLevel.Push(ch[i]);
                                break;
                            }
                        }
                        if (compareLevel(ch[i], opLevel.Peek()) == 1)
                        {
                            opLevel.Push(ch[i]);
                        }
                    }
                    else
                    {
                        opLevel.Push(ch[i]);
                    }
                    
                }
                else if (ch[i] == '(')
                {
                    opLevel.Push(ch[i]);
                }
                else if (ch[i] == ')')
                {
                    while (opLevel.Peek() != '(')
                    {
                        output.Add(opLevel.Peek());
                        opLevel.Pop();
                    }
                    opLevel.Pop();
                }
                else
                {
                    output.Add(ch[i]);
                }
            }

            while (opLevel.Count != 0)
            {
                output.Add(opLevel.Peek());
                opLevel.Pop();
            }
        }

        private void Calculation()
        {
            cell c = new cell();
      
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i] == '+')
                {
                    
                    List<cell> para = new List<cell>();
                    for(int j = 0; j < 2; j++)
                    {
                    
                            para.Add(calculate.Pop());//查看计算过的Cell
                    
                    }
                    calculate.Push(PLUS(para[1], para[0]));//左->右
                }
                else if(output[i] == '|')
                {
                   
                    List<cell> para = new List<cell>();
                    for (int j = 0; j < 2; j++)
                    {
                      
                            para.Add(calculate.Pop());
                      

                    }
                    calculate.Push(OR(para[0], para[1]));
                }
                else if(output[i] == '*')
                {
                   
                    List<cell> para = new List<cell>();
                  
                    para.Add(calculate.Pop());
                    calculate.Push(CLOSURE(para[0]));
                }
                else
                {
                    calculate.Push(MakeCell(output[i]));
                }
            }
        }

        private cell MakeCell(char symbol)
        {
            cell NewCell = new cell();
            NewCell.EdgeCount = 0;
            edge NewEdge = new edge();
            //新节点
            state StartState = new state();
            state EndState = new state();
            StartState.StateName = ++stateName;
            EndState.StateName = ++stateName;
            //新边，加入两个新节点
            NewEdge.StartState = StartState;
            NewEdge.EndState = EndState;
            NewEdge.TransSymbol = symbol;
            //构建新单元
            NewCell.EdgeSet.Add(NewEdge);
            NewCell.StartState = NewCell.EdgeSet[0].StartState;
            NewCell.EndState = NewCell.EdgeSet[0].EndState;
            NewCell.EdgeCount = NewCell.EdgeSet.Count;
            return NewCell;
        }

        private cell OR(cell Left,cell Right)
        {
            cell NewCell = new cell();
            NewCell.EdgeCount = 0;
            edge Edge1 = new edge();
            edge Edge2 = new edge();
            edge Edge3 = new edge(); 
            edge Edge4 = new edge();

            state StartState = new state();
            state EndState = new state();
            StartState.StateName = ++stateName;
            EndState.StateName = ++stateName;
            //构建边
            Edge1.StartState = StartState;
            Edge1.EndState = Left.EdgeSet[0].StartState;
            Edge1.TransSymbol = '#';

            Edge2.StartState = StartState;
            Edge2.EndState = Right.EdgeSet[0].StartState;
            Edge2.TransSymbol = '#';

            Edge3.StartState = Left.EdgeSet[Left.EdgeSet.Count-1].EndState;
            Edge3.EndState = EndState;
            Edge3.TransSymbol = '#';

            Edge4.StartState = Right.EdgeSet[Right.EdgeSet.Count-1].EndState;
            Edge4.EndState = EndState;
            Edge4.TransSymbol = '#';

            //构建单元
            NewCell.EdgeSet.AddRange(Left.EdgeSet);
            NewCell.EdgeSet.AddRange(Right.EdgeSet);

            NewCell.EdgeSet.Add(Edge1);
            NewCell.EdgeSet.Add(Edge2);
            NewCell.EdgeSet.Add(Edge3);
            NewCell.EdgeSet.Add(Edge4);
            NewCell.EdgeCount = NewCell.EdgeSet.Count;

            NewCell.StartState = StartState;
            NewCell.EndState = EndState;
            return NewCell;
        }//'|'的运算

        private cell PLUS(cell Left,cell Right)
        {
            for(int i = 0; i < Right.EdgeCount; i++)
            {
                if (Right.EdgeSet[i].StartState.StateName == Right.StartState.StateName)
                {
                    Right.EdgeSet[i].StartState = Left.EndState;
                }
                else if(Right.EdgeSet[i].EndState.StateName == Right.StartState.StateName)
                {
                    Right.EdgeSet[i].EndState = Left.EndState;
                }
             
            }
            //

            Right.StartState = Left.EndState;
            Left.EdgeSet.AddRange(Right.EdgeSet);
            Left.EndState = Right.EndState;
            Left.EdgeCount = Left.EdgeSet.Count;
            return Left;
        }//'+''-'的运算

        private cell CLOSURE(cell Cell)
        {
            cell NewCell = new cell();
            NewCell.EdgeCount = 0;
            edge Edge1 = new edge();
            edge Edge2 = new edge();
            edge Edge3 = new edge();
            edge Edge4 = new edge();

            state StartState = new state();
            state EndState = new state();
            StartState.StateName = ++stateName;
            EndState.StateName = ++stateName;
            
            //e1
            Edge1.StartState = StartState;
            Edge1.EndState = EndState;
            Edge1.TransSymbol = '#';  //闭包取空串
            //e2
            Edge2.StartState = Cell.EndState;
            Edge2.EndState = Cell.StartState;
            Edge2.TransSymbol = '#';  //取字符，自连接
            //e3
            Edge3.StartState = StartState;
            Edge3.EndState = Cell.StartState;
            Edge3.TransSymbol = '#';
            //e4
            Edge4.StartState = Cell.EndState;
            Edge4.EndState = EndState;
            Edge4.TransSymbol = '#';

            NewCell.EdgeSet.AddRange(Cell.EdgeSet);

            NewCell.EdgeSet.Add(Edge1);
            NewCell.EdgeSet.Add(Edge2);
            NewCell.EdgeSet.Add(Edge3);
            NewCell.EdgeSet.Add(Edge4);

            NewCell.StartState = StartState;
            NewCell.EndState = EndState;
            NewCell.EdgeCount = NewCell.EdgeSet.Count;
            return NewCell;
        }//'*'的运算


    }


    /*-------------------------------------------------------类定义----------------------------------------------------------*/

    //NFA的节点，定义成结构体，便于以后扩展
    class state
    {
        public int StateName;
        //public int bnext;
        //public int anext;
        public List<int> next = new List<int>(new int[26]);
    };

    //NFA的边，空转换符用'#'表示
    class edge
    {
        public state StartState = new state();
        public state EndState = new state();
        public char TransSymbol;  //转换符号
    };

    //NFA单元，一个大的NFA单元可以是由很多小单元通过规则拼接起来
    class cell
    {
        public List<edge> EdgeSet = new List<edge>();  //这个NFA拥有的边
        public int EdgeCount;  //边数
        public state StartState = new state();  //开始状态
        public state EndState = new state();  //结束状态
    };



    //闭包的集合
    class CloseSet
    {
        public List<TSet> CSet = new List<TSet>();

        public bool Check()//检查标记情况
        {
            for(int i = 0; i < CSet.Count; i++)
            {
                if (CSet[i].sign == false)
                {
                    return false;
                }
            }
            return true;
        }

        public int Sign()//进行标记，并返回下标
        {
            for(int i = 0; i < CSet.Count; i++)
            {
                if (CSet[i].sign == false)
                {
                    CSet[i].sign = true;
                    return i;
                }
            }
            return -1;
        }

        public int JoinNew(TSet T)//检查集合是否已经存在，需不需要加入
        {
            if (T.tSet.Count == 0)
            {
                return -2;
            }
            for(int i = 0; i < CSet.Count; i++)
            {
                //if (CSet[i].tSet.Equals(T) == true)
                //{
                //    return i;
                //}
                int j;
                for(j = 0; j < CSet[i].tSet.Count; j++)
                {
                    if (CSet[i].tSet.Count!=T.tSet.Count)
                    {
                        break;
                    }
                    else if(T.tSet[j] != CSet[i].tSet[j])
                    {
                        break;
                    }
                }
                if (j == CSet[i].tSet.Count)
                {
                    return i;
                }
            }
            return -1;
        }
    }
    //闭包的子集
    class TSet
    {
        public List<int> tSet = new List<int>();
        public bool sign = false;
        //public int bnext;
        //public int anext;
        public List<int> next = new List<int>(new int[26]);

    }
}
