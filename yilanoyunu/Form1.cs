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

namespace yilanoyunu
{
    public partial class Form1 : Form
    {

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
        public int score = 0;
        public int X = 1, Y = 1; 
        public Direction dr=Direction.Right;
        public Location feed = new Location(-1, -1);
        public List<Location> tales=new List<Location>();
        public bool Game = true;

        public Form1()
        {
            InitializeComponent();

            tales.Add(new yilanoyunu.Location(0, 0));

            CalcTable();

            Thread thread = new Thread(new ThreadStart(new Action(() =>
            {
                while (Game)
                {
                    if (dr == Direction.Right) X++;
                    if (dr==Direction.Down) Y++;
                    if(dr==Direction.Up) Y--;
                    if(dr==Direction.Left) X--;
                    CalcTable();
                    Thread.Sleep(100);
                }
               
            })));
            thread.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "S") dr = Direction.Down;
            else if (e.KeyCode.ToString() == "A") dr = Direction.Left;
            else if (e.KeyCode.ToString() == "D") dr = Direction.Right;
            else if (e.KeyCode.ToString() == "W") dr = Direction.Up;

        }

        

        public void CalcTable()
        {
            try { Invoke(new Action(() => label1.Text = "Total:" + score)); }
            catch { }

            Random rnd = new Random();
            Bitmap bitmap = new Bitmap(500, 500);


            //tales coll
            if (tales.Count != 1)
            {
                for (int i = 1; i < tales.Count; i++)
                {
                    if (tales[i].x == X && tales[i].y == Y)
                    {
                        Game = false;   
                        MessageBox.Show("Kaybettin!");
                        
                    }
                }
            }


            //feed coll
            if (X==feed.x && Y==feed.y)
            {
                score++;
                tales.Add(new yilanoyunu.Location(feed.x, feed.y));
                feed = new Location(-1, -1);

            }

            //outside coll
            if(X<=0 || Y<=0 || X == 51 || Y == 51)
            {

                Game=false;
                MessageBox.Show("Kaybettin!");
               
            }
            else
            {
                //snake
                for (int i = (X - 1) * 10; i < X * 10; i++)
                {
                    for (int j = (Y - 1) * 10; j < Y * 10; j++)
                    {
                        bitmap.SetPixel(i, j, Color.Black);
                    }
                }
            }
       




            //tales
            if (tales.Count != 1)
            {
                for (int k = 0; k < tales.Count; k++)
                {
                    for (int i = (tales[k].x - 1) * 10; i < tales[k].x * 10; i++)
                    {
                        for (int j = (tales[k].y - 1) * 10; j < tales[k].y * 10; j++)
                        {
                            bitmap.SetPixel(i, j, Color.Black);
                        }
                    }
                }
            }

            tales[0] = new Location(X, Y);
            for(int i =tales.Count - 1; i > 0; i--)
            {
                tales[i] = tales[i - 1];
            }

         


            //feed
            if (feed.x==-1)
            {
                feed= new Location(rnd.Next(1, 50), rnd.Next(1, 50));
            }

            for (int i = (feed.x - 1) * 10; i < feed.x * 10; i++)
            {
                for (int j = (feed.y - 1) * 10; j < feed.y * 10; j++)
                {
                    bitmap.SetPixel(i, j, Color.Red);
                }
            }

            game.Image= bitmap;
        }

    }

    public class Location
    {
        public int x, y;
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

}
