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
        //cell NFAcells;
        //cell DFAcells;
        //cell MFAcells;
        
        public Form2()
        {
            InitializeComponent();
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
                //GolbalPara.set();
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
                //GolbalPara.set();
                stateName = 0;
                NFAToDFA(ch);
            }
        }
        /*
         *  '(',')'中间不能为空，必须对应
         *  '*'前面不为'(','|',只能在')','a','b'后面
         *  '|'左不得为'('右不得为')'
         *  'a''b'    
         *  这些算符的优先顺序为先'*'，再'+''-'，最后’|’
         */
        private Boolean Detection(String str)
        {
            Char[] ch = str.ToCharArray();
            if (ch.Length == 0)
            {
                return false;
            }
            int ls = 0;//判断左右括号数量
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
                else if (ch[i] != 'a' && ch[i] != 'b')
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
        List<edge> aEdge = new List<edge>();
        List<edge> bEdge = new List<edge>();
        int NFAb = 0;
        int NFAe = 0;
        //class GolbalPara
        //{
        //    public static int stateName = 0;
        //    public GolbalPara()
        //    {
        //        stateName = 0;
        //    }
        //    public int get()
        //    {
        //        return stateName;
        //    }
        //    public static void set()
        //    {
        //        stateName = 0;
        //    }
        //}
        int stateName = 0;
       
        private void NormalToNFA(char[] ch)
        {
           
            List<char> chList = new List<char>();
            int nums = 0;

            for(int i = 0; i < ch.Length; i++)
            {
                chList.Add(ch[i]);
            }
            int len = chList.Count;
            /*
             * 
             * 
             * 插入加号减号
             * 加号表示左->
             * 右减号表示右->左
             * 
             * 考虑 a+b a* ()左右两边的情况 
             * 只有优先级的运算前面为'+'否则为'-'，其中仅有第一次的ab运算会插入'+'，'('会重置到第一次
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
                else if (chList[i] == 'a' && i != 0)
                {
                    if (chList[i - 1] == 'a' || chList[i - 1] == 'b' || chList[i - 1] == ')' || chList[i - 1] == '*')
                    {
                        chList.Insert(i, '+');
                        i++;

                    }
                }
                else if (chList[i] == 'b' && i != 0)
                {
                    if (chList[i - 1] == 'a' || chList[i - 1] == 'b' || chList[i - 1] == ')' || chList[i - 1] == '*')
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

            NFA = calculate.Peek();
            cell result = calculate.Pop();
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

        private void NFAToDFA(char[] ch)
        {
            NormalToNFA(ch);
            CloseSet C = new CloseSet();
            TSet T0 = new TSet();
            T0.tSet.Add(NFAb);
          
            for (int i = 0; i < NFA.EdgeSet.Count; i++)
            {
                if (NFA.EdgeSet[i].TransSymbol == '#')
                {
                    T0.tSet.Add(NFA.EdgeSet[i].EndState.StateName);
                }
            }
            T0.Unrepeated();
            C.CSet.Add(T0);
            
            for(int i = 0; i < NFA.EdgeSet.Count; i++)
            {
                if (NFA.EdgeSet[i].TransSymbol == 'a')
                {
                    aEdge.Add(NFA.EdgeSet[i]);
                }
                if (NFA.EdgeSet[i].TransSymbol == 'b')
                {
                    bEdge.Add(NFA.EdgeSet[i]);
                }
            }

            while (C.Check() != true)
            {
                int order = C.Sign();
                TSet Ta = new TSet();
                TSet Tb = new TSet();
                Ta.tSet.AddRange(C.CSet[order].tSet);
                Tb.tSet.AddRange(C.CSet[order].tSet);
                for(int i = 0; i < C.CSet[order].tSet.Count; i++)
                {
                    for(int j = 0; j < aEdge.Count; j++)
                    {
                        if (C.CSet[order].tSet[i] == aEdge[j].StartState.StateName)
                        {
                            Ta.tSet.Add(aEdge[j].EndState.StateName);
                        }
                        if (C.CSet[order].tSet[i] == bEdge[j].StartState.StateName)
                        {
                            Tb.tSet.Add(aEdge[j].EndState.StateName);
                        }
                    }
                    
                }

                if (C.JoinNew(Ta) == -1)
                {
                    Ta.Unrepeated();
                    C.CSet.Add(Ta);
                    C.CSet[order].anext = C.CSet.Count-1;
                }
                else
                {
                    C.CSet[order].anext = C.JoinNew(Ta);
                }

                if (C.JoinNew(Tb) == -1)
                {
                    Tb.Unrepeated();
                    C.CSet.Add(Tb);
                    C.CSet[order].bnext = C.CSet.Count - 1;
                }
                else
                {
                    C.CSet[order].bnext = C.JoinNew(Tb);
                }

            }


        }

        private void DFAToMFA(char[] ch)
        {

        }
        /*
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
            return 0;
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
                    
                            para.Add(calculate.Pop());//优先查看计算过的Cell
                    
                    }
                    calculate.Push(PLUS(para[1], para[0]));//左->右
                }
                //else if (output[i] == '-')
                //{
                //    char symbol;
                //    List<cell> para = new List<cell>();
                //    for (int j = 0; j < 2; j++)
                //    {
                //            para.Add(calculate.Pop());//优先查看计算过的Cell
                      

                //    }
                //    calculate.Push(PLUS(para[0], para[1]));//右->左
                //}
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

        private cell MakeCell(char symbol,int start,int end)
        {
            cell NewCell = new cell();
            NewCell.EdgeCount = 0;
            edge NewEdge = new edge();
            //新节点
            state StartState = new state();
            state EndState = new state();
            StartState.StateName = start;
            EndState.StateName = end;
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

    //NFA的节点，定义成结构体，便于以后扩展
    class state
    {
        public int StateName;
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
            for(int i = 0; i < CSet.Count; i++)
            {
                if (CSet[i].Equals(T) == true)
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
        public int bnext;
        public int anext;

        public void Unrepeated()//去除子集中重复的项
        {

            int count = tSet.Count;

            int time = 0;

            count = tSet.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    time++;
                    if (tSet[i] == tSet[j])
                    {
                        tSet.RemoveAt(i);
                        count = tSet.Count;
                        i--;
                        break;
                    }

                }
            }
       
        }
    }
}
