using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;

namespace GugudanQuiz
{
    public partial class Form1 : Form
    {

        int min = 2, sec = 60, answer = 0, answerCounter = 0, userAnswer = 0;
        bool timeOver = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void tbxUserAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                button2_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            confirm_answer();
        }

        /* public void time()
         {
             //int i = 0, j = 0;
             //timeOver = false;
             Object obj = new Object();
             Monitor.Enter(obj);
             for (int i = 2; i >= 0; i--)
             {
                 for (int j = 59; j >= 0; j--)
                 {
                     try
                     {
                         this.lblTimer.Invoke(new Action(delegate ()
                         {
                             lblTimer.Text = $"{i}분 {j}초";
                         }));
                         Thread.Sleep(1000);
                     } catch(Exception e) {
                         MessageBox.Show(e.Message); 
                     }

                 }
             }
             Monitor.Exit(obj);
         }*/

        private void button1_Click(object sender, EventArgs e)
        {
            //Thread timer = new Thread(new ThreadStart(time));
            bool restart = false;
            if (!restart)
            {
                timer1.Start();
                questions();
                restart = true;
                button1.Text = "다시 시작";
                tbxUserAnswer.Focus();
            } else
            {
                timer1.Stop();
                answerCounter = 0;
                answer = 0;
                lblAnswerCounter.Text = $"맞힌 수 : {answerCounter}";
                timer1.Start();
                questions();
                tbxUserAnswer.Focus();
            }
            //timer.Join();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        /*private void tbxUserAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                //confirm_answer();
                do
                {
                    userAnswer = int.Parse(tbxUserAnswer.Text);
                    if (answer == userAnswer)
                    {
                        answerCounter += 1;
                        lblAnswerCounter.Text = $"맞힌 수 : {answerCounter}";
                        questions();
                    }
                } while (!timeOver) ;
            }
        }*/

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (min != 0 && sec == 0)
            {
                min--;
                sec = 60;
            }
            else if (min == 0 && sec == 0)
            {
                lblTimer.Text = "시간 초과!!!";
                saveDB();
                timeOver = true;
                timer1.Enabled = false;
                button1.Text = "시작";
            }
            else
            {
                sec--;
                lblTimer.Text = $"{min}분 {sec}초";
            }

            
            /*for (int min = 2; min >= 0; min--)
            {
                for (int sec = 59; sec >= 0; sec--)
                {
                    lblTimer.Text = $"{min}분 {sec}초";
                    try {
                        Thread.Sleep(1000);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message);
                    }
                }
            }*/
        }

        private void saveDB()
        {
            WebClient wb = new WebClient();
            NameValueCollection data = new NameValueCollection();

            string url = "http://paul81web.000webhostapp.com/php/gugudan_insert.php";

            data["name"] =  tbxName.Text;
            data["score"] = answerCounter.ToString();

            try
            {
                byte[] response = wb.UploadValues(url, data);
                //MessageBox.Show(response.ToString());
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void questions()
        {
            Random rand = new Random();
            int value1 = 0, value2 = 0;
            value1 = rand.Next(2, 10);
            value2 = rand.Next(2, 10);
            answer = value1 * value2;
            lblQuestion.Text = $"{value1} X {value2} =";
        }

        private void confirm_answer()
        {
            string strAnswer = null; 
            //bool bl = false;
            int returnVal = 0;
            try
            {
                if (!int.TryParse(strAnswer, out returnVal))
                {
                    do
                    {
                        strAnswer = tbxUserAnswer.Text;
                        userAnswer = Int32.Parse(strAnswer); //throw new Exception();
                        tbxUserAnswer.Text = "";
                        if (answer == userAnswer)
                        {
                            answerCounter += 1;
                            lblAnswerCounter.Text = $"맞힌 수 : {answerCounter}";
                            questions();
                            tbxUserAnswer.Focus();
                        }
                    } while (!timeOver);
                } else if (String.IsNullOrEmpty(tbxUserAnswer.Text))
                {
                    MessageBox.Show("답을 쓰세요.");
                    //throw new Exception(tbxUserAnswer.Text);
                    tbxUserAnswer.Focus();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                //throw new FormatException();//Exception(tbxUserAnswer.Text);
            }
        }
    }
}
