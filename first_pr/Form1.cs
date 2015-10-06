using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace first_pr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exit()
        {
            DialogResult rsl = MessageBox.Show("Are you sure that you want to exit?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rsl == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            exit();
        }

        public Image currImage;
        static public Image Mem;


        private void Load_Image()
        {
            openFileDialog1.InitialDirectory = "c:";
            openFileDialog1.Filter = "image (JPEG) files (*.jpg)|*.jpg|All files (*.*)|*.*|image (BMP) files (*.bmp)|*.bmp|image (PNG) files (*.png)|*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try 
                {
                    
                    currImage = Image.FromFile(openFileDialog1.FileName);
                    Mem = Image.FromFile(openFileDialog1.FileName);
                    button6.Enabled = true;
                    if ((currImage.Height <= 256) && (currImage.Width <= 256))
                    {
                        pictureBox1.Image = currImage;
                    }
                    else 
                    {
                        DialogResult rsl = MessageBox.Show("Wrong size of picture! Press 'Yes' if you want to continue. Press 'No' if you want to reload image.", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (rsl == DialogResult.Yes)
                        {
                            pictureBox1.Image = currImage;
                        }
                    }
                }
                catch (Exception e) 
                {
                    MessageBox.Show("Can't load file.\n" + e.StackTrace, "Warning!",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            Load_Image();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var bmp = (Bitmap)Mem;
                for (int x = 0; x < bmp.Width; ++x)
                    for (int y = 0; y < bmp.Height; ++y)
                    {
                        Color curr = bmp.GetPixel(x, y);
                        Color next = Color.FromArgb((byte)(1 - curr.R), (byte)(1 - curr.G), (byte)(1 - curr.B)); // RGB -> CMY
                        bmp.SetPixel(x, y, next);
                    }
                pictureBox2.Image = bmp;
                pictureBox3.Image = Mem;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Forgot to open file: load it first.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Save_image()
        {
            if (pictureBox3.Image != null)
            {
                Bitmap bt = (Bitmap)pictureBox3.Image;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = "bmp";
                sfd.Filter = "Image files (*.bmp)|*.bmp|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bt.Save(sfd.FileName);
                }
            }
            else
            {
                throw new Exception("No image");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                Save_image();   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

      
        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                Bitmap bmp = (Bitmap)Mem;
                for (int x = 0; x < bmp.Width; ++x)
                    for (int y = 0; y < bmp.Height; ++y)
                    {
                        Color curr = bmp.GetPixel(x, y);
                        float r = (11*curr.B + 30*curr.R + 59*curr.G )/100;
                        Color next = Color.FromArgb((byte) r, (byte) r, (byte) r); // RGB -> BW
                        bmp.SetPixel(x, y, next);
                    }
                pictureBox2.Image = bmp;
                pictureBox3.Image = Mem;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Forgot to open file: load it first.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = (Bitmap)Mem;
                for (int x = 0; x < (bmp.Width); ++x)
                    for (int y = 0; y < (bmp.Height); ++y)
                    {
                        Color curr = bmp.GetPixel(x, y);
                        float r = (11 * curr.B + 30 * curr.R + 59 * curr.G) / 100;
                        Color next = Color.FromArgb((byte)r, (byte)r, (byte)r); // RGB -> BW
                        bmp.SetPixel(x, y, next);
                    }
                pictureBox2.Image = bmp;
                pictureBox2.Refresh();
                for (int x = 1; x < (bmp.Width - 1); ++x)
                    for (int y = 1; y < (bmp.Height - 1); ++y)
                    {
                        int B1 = bmp.GetPixel(x - 1, y).R + bmp.GetPixel(x + 1, y).R 
                            + bmp.GetPixel(x, y - 1).R + bmp.GetPixel(x, y + 1).R + bmp.GetPixel(x, y).R;
                        float r = B1 / 5;
                        Color next = Color.FromArgb((byte)r, (byte)r, (byte)r); // Smooth
                        bmp.SetPixel(x, y, next);
                    }
                pictureBox3.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Forgot to open file: load it first.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Mem = Image.FromFile(openFileDialog1.FileName);
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
        }

        public static List<vertex> vertexes = new List<vertex>();

        public class vertex
        {
            public int x;
            public int y;
            public Color currColor;
            public bool free;

            public vertex(int _x, int _y, Color _curr)
            {
                x = _x;
                y = _y;
                currColor = _curr;
                free = true;
            }

            public bool Equals(vertex other)
            {
                return ((this.x == other.x) && (this.y == other.y));
            }

            public override int GetHashCode()
            {
                return x.GetHashCode() ^ y.GetHashCode();
            }
        };

        public class Triangle
        {
            public vertex v1;
            public vertex v2;
            public vertex v3;
            public Triangle(vertex _v1, vertex _v2, vertex _v3)
            {
                v1 = _v1;
                v2 = _v2;
                v3 = _v3;
            }
        };

        public List<Triangle> triangles = new List<Triangle>();
        //public bool Equal(int f, int s)
        //{
        //    if ((triangles[f].v1.x == triangles[s].v1.x || triangles[f].v1.x == triangles[s].v2.x || triangles[f].v1.x == triangles[s].v3.x) &&
        //        (triangles[f].v1.y == triangles[s].v1.y || triangles[f].v1.y == triangles[s].v2.y || triangles[f].v1.y == triangles[s].v3.y))
        //    {
        //        if ((triangles[f].v2.x == triangles[s].v1.x || triangles[f].v2.x == triangles[s].v2.x || triangles[f].v2.x == triangles[s].v3.x) &&
        //            (triangles[f].v2.y == triangles[s].v1.y || triangles[f].v2.y == triangles[s].v2.y || triangles[f].v2.y == triangles[s].v3.y))
        //        {
        //            if ((triangles[f].v3.x == triangles[s].v1.x || triangles[f].v3.x == triangles[s].v2.x || triangles[f].v3.x == triangles[s].v3.x) &&
        //                (triangles[f].v3.y == triangles[s].v1.y || triangles[f].v3.y == triangles[s].v2.y || triangles[f].v3.y == triangles[s].v3.y))
        //            {
        //                return false;
        //            }
        //        }

        //    }
        //    return true;
        //}


        public static Bitmap bmpLines = new Bitmap(256,256);
        public static Graphics graphics = Graphics.FromImage(bmpLines);

        public static void initBitmap()
        {
            for (var i = 0; i < 256; i++)
            {
                for(var j = 0; j < 256; j++)
                {
                    bmpLines.SetPixel(i, j, Color.White);
                }
            }
        }

        public void showTops()
        {
            Form1.initBitmap();
            for (int i = 0; i < vertexes.Count; i++) 
            {
                Form1.bmpLines.SetPixel(vertexes[i].x, vertexes[i].y, vertexes[i].currColor);
            }
            this.pictureBox4.Image = Form1.bmpLines;
        }

        public static void markRed(int x, int y)
        {
            Bitmap bmp = (Bitmap)Form1.Mem;
            bmpLines.SetPixel(x, y, Color.Red);
            Color curr = bmp.GetPixel(x, y);
            Form1.vertexes.Add(new vertex(x, y, curr));

        }

        abstract class DivideFigure
        {
            public static int fullSize = 256;         
            protected int currHight;          /// Store hight(Y) of current figure
            protected int currWidth;          /// Store wigth(X) of current figure
            protected int x;                  /// X coord for upper left point of current figure.
            protected int y;                  /// Y coord for upper left point of current figure.
            public Bitmap bmp;
                                              
            abstract public void divideImage(int bright, Rectangle _parent);        /// Divide figure to smaller figures.
                                                       
            public void drawLine(int _x, int _y, int _currWidth, int _currHight, bool vert)                     ///
            {
                bmp = (Bitmap)Form1.Mem;
                if (vert)
                {
                    for (int i = _y; i < _currHight + y; i++)
                    {
                        Form1.bmpLines.SetPixel((_currWidth + x), i, Color.Black);
                        if ((_currHight == 1 || _currHight == 2) && (_currWidth == 1 || _currWidth == 2))
                        {
                            Color curr = bmp.GetPixel((_currWidth + x), i);
                            vertex M1 = new vertex((_currWidth + x), i, curr);
                            Form1.vertexes.Add(M1);
                            if (_currWidth + x != 256 && _currWidth + x + 1 != 256 && (i + 1) != 256 && (i - 1) >= 0 && (_currWidth + x - 1) >= 0)
                            {
                                curr = bmp.GetPixel((_currWidth + x), i + 1);
                                Form1.vertexes.Add(new vertex((_currWidth + x), i + 1, curr));
                                curr = bmp.GetPixel((_currWidth + x + 1), i + 1);
                                Form1.vertexes.Add(new vertex((_currWidth + x + 1), i + 1, curr));
                                curr = bmp.GetPixel((_currWidth + x), i - 1);
                                Form1.vertexes.Add(new vertex((_currWidth + x), i - 1, curr));
                                curr = bmp.GetPixel((_currWidth + x - 1), i - 1);
                                Form1.vertexes.Add(new vertex((_currWidth + x - 1), i - 1, curr));
                                Form1.bmpLines.SetPixel((_currWidth + x), i, Color.Red);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = _x; i < _currWidth + x; i++)
                    {
                        Form1.bmpLines.SetPixel(i, _currHight + y, Color.Black);
                        if ((_currHight == 1 || _currHight == 2) && (_currWidth == 1 || _currWidth == 2))
                        {
                            Color curr = Form1.bmpLines.GetPixel(i, _currHight + y);
                            vertex M1 = new vertex(i, _currHight + y, curr);
                            Form1.vertexes.Add(M1);
                            Form1.bmpLines.SetPixel(i, _currHight + y, Color.Red);
                            if (_currHight + y != 256 && _currHight + y + 1 != 256 && (i + 1) != 256 && (i - 1) >= 0 && (_currHight + y - 1) >= 0)
                            {
                                curr = bmp.GetPixel(i + 1, _currHight + y);
                                Form1.vertexes.Add(new vertex(i + 1, _currHight + y, curr));
                                curr = bmp.GetPixel((i + 1), _currHight + y + 1);
                                Form1.vertexes.Add(new vertex((i + 1), _currHight + y + 1, curr));
                                curr = bmp.GetPixel(i - 1, _currHight + y);
                                Form1.vertexes.Add(new vertex(i - 1, _currHight + y, curr));
                                curr = bmp.GetPixel(i - 1, (_currHight + y - 1));
                                Form1.vertexes.Add(new vertex(i - 1, (_currHight + y - 1), curr));
                                Form1.bmpLines.SetPixel(i, (_currHight + y), Color.Red);
                            }
                        }
                    }
                }
            }


            public bool checkBrightnes(int x, int y, int currWidth, int currHight, int bright)    /// Method that finds two points in figure, that have difference in brightness more than needed.
            {
                float r = (11 * bmp.GetPixel(x, y).B + 30 * bmp.GetPixel(x, y).R + 59 * bmp.GetPixel(x, y).G) / 100;
                    for (var i = x; i < (currWidth + x); i++)
                    {
                        for (var j = y; j < (currHight + y); j++)
                        {
                            Color curr = bmp.GetPixel(i, j);
                            float r1 = (11 * curr.B + 30 * curr.R + 59 * curr.G) / 100;
                            if ((r - r1) >= bright)
                            {
                                return true;
                            }
                            if (r1 > r)
                            {
                                r = r1;
                            }
                        }
                    }
                    return false;
                }
        };

        class Rectangle : DivideFigure
        {
            private Form1 form;
            private bool lineVert;                     /// Store if this figure need to divide horisontal or vertical line
            private Rectangle parent;
            private int level;
                                                       
            public Rectangle(Form1 _form, int _x, int _y, int _currWidth, int _currHight, bool _lineVert, Rectangle _parent, int _level) 
            {
                this.form = _form;
                this.x = _x;
                this.y = _y;
                this.currHight = _currHight;
                this.currWidth = _currWidth;
                this.lineVert = _lineVert;
                this.bmp = (Bitmap)Form1.Mem;
                this.parent = _parent;
                this.level = _level;
            }

            public override void divideImage(int bright, Rectangle _parent)
            {
                if (lineVert)
                {
                    currWidth = currWidth / 2;
                    var R = new Rectangle(form, x, y, currWidth, currHight, false, _parent, this.level + 1);
                    if (R.checkBrightnes(x, y, currWidth, currHight, bright))
                    {
                        drawLine(currWidth, y, currWidth, currHight, true);
                        form.pictureBox2.Image = Form1.bmpLines;
                        //form.pictureBox2.Refresh();
                        R.divideImage(bright, R);
                    }
                    else
                    {
                        
                    }
                    var R1 = new Rectangle(form, x + currWidth, y, currWidth, currHight, false, _parent, this.level + 1);
                    if (R1.checkBrightnes ((x + currWidth), y, currWidth, currHight, bright))
                    {
                        drawLine(currWidth, y, currWidth, currHight, true);
                       // Form1.markRed(x + currWidth, y);
                        //form.pictureBox2.Refresh();
                        R1.divideImage(bright, R1);
                    }
                }
                else
                {
                    currHight = currHight / 2;
                    var R = new Rectangle(form, x, y, currWidth, currHight, true, _parent, this.level + 1);
                    if (R.checkBrightnes(x, y, currWidth, currHight, bright))
                    {
                        drawLine(x, currHight, currWidth, currHight, false);
                        form.pictureBox2.Image = Form1.bmpLines;
                        //form.pictureBox2.Refresh();
                        R.divideImage(bright,_parent);
                    }

                    var R1 = new Rectangle(form, x, y + currHight, currWidth, currHight, true, parent, this.level + 1);
                    if (R1.checkBrightnes(x,  (y + currHight), currWidth, currHight, bright))
                    {
                        drawLine(x, currHight + y, currWidth, currHight, false);
                        Form1.markRed(x, y + currHight);
                        //form.pictureBox2.Refresh();
                        R1.divideImage(bright, _parent);
                    }

                }
            }

        };

        private void DivImage_Click(object sender, EventArgs e)
        {
            Rectangle rootRect = new Rectangle(this, 0, 0, 256, 256, true, null, 0);
            initBitmap();
            int brightness = (int)numericUpDown1.Value;
            if (rootRect.checkBrightnes(0, 0, 256, 256, brightness))
            {
                rootRect.divideImage(brightness, rootRect);
            }
            vertexes.Add(new vertex(0, 0, ((Bitmap)Mem).GetPixel(0, 0)));
            vertexes.Add(new vertex(pictureBox2.Size.Width - 1, 0, ((Bitmap)Mem).GetPixel(pictureBox2.Size.Width - 1, 0)));
            vertexes.Add(new vertex(0, pictureBox2.Size.Height - 1, ((Bitmap)Mem).GetPixel(0, pictureBox2.Size.Height - 1)));
            vertexes.Add(new vertex(pictureBox2.Size.Width - 1, pictureBox2.Size.Height - 1, ((Bitmap)Mem).GetPixel(pictureBox2.Size.Width - 1, pictureBox2.Size.Height - 1)));
            //vertexes.Add(new vertex(100, 100, Color.Black));
            //vertexes.Add(new vertex(160, 40, Color.Blue));
            //vertexes.Add(new vertex(40, 20, Color.Red));
            //vertexes.Add(new vertex(80, 80, Color.Red));
            //vertexes.Add(new vertex(60, 130, Color.Black));
            //vertexes.Add(new vertex(180, 100, Color.Black));
           

           
            foreach (vertex v in vertexes)
            {
                bmpLines.SetPixel(v.x, v.y, v.currColor);
            }

            pictureBox2.Image = bmpLines;
            pictureBox2.Refresh();
            showTops();
        }
        
        public void drawSide( vertex begin, vertex end)
        {           
            graphics.DrawLine(new Pen(Color.Black), new Point(begin.x, begin.y), new Point(end.x, end.y));
        }

        private int getVertexDir(vertex first, vertex second, vertex third)
        {
            int turn = (third.x - first.x) * (second.y - first.y) - (third.y - first.y) * (second.x - first.x);
            if (turn > 0)
            {
                return 1; //r
            }
            if (turn < 0)
            {
                return -1; //l
            }
            return 0;
        }
      
        /// <summary>
        ///  Choose two points of triangle and add
        ///  dir: a forbidden one (0 stands for "both allowed")
        /// </summary>
        public void triangulate(Queue<int> qeVertexes1, Queue<int> qeVertexes2, Queue<int> qeDirection, List<HashSet<int>> matrix) 
        {
            int f = qeVertexes1.Dequeue();
            int s = qeVertexes2.Dequeue();
            vertex first = vertexes[f];
            vertex second = vertexes[s];
            int dir = qeDirection.Dequeue();
            drawSide(first, second);
            bool checkVert = false;
            double min = 2;
            int n = 0; 
            for (int m = 0; m < vertexes.Count; m++)
            {
                if ((dir != 0 && getVertexDir(first, second, vertexes[m]) == dir))
                {
                    continue;
                }

                if (matrix[f].Contains(m) && matrix[s].Contains(m))
                {
                    triangles.Add(new Triangle(first, second, vertexes[m]));
                    return;
                }

                double fs = Math.Pow(first.x - second.x, 2) + Math.Pow(first.y - second.y, 2);
                double fm = Math.Pow(first.x - vertexes[m].x, 2) + Math.Pow(first.y - vertexes[m].y, 2);
                double ms = Math.Pow(vertexes[m].x - second.x, 2) + Math.Pow(vertexes[m].y - second.y, 2);
                double curr = (-fs + fm + ms) / (2 * Math.Sqrt(fm * ms));
                if (curr < min) 
                {
                    min = curr;
                    checkVert = true;
                    n = m;
                }        
            }

            if (checkVert)
            {
                triangles.Add(new Triangle(first, second, vertexes[n]));
                matrix[f].Add(n);
                matrix[s].Add(n);
                matrix[n].Add(f);
                matrix[n].Add(s);
                drawSide(first, vertexes[n]);
                drawSide(second, vertexes[n]);
                qeVertexes1.Enqueue(f);
                qeVertexes2.Enqueue(n);
                qeVertexes1.Enqueue(n);
                qeVertexes2.Enqueue(s);
                qeDirection.Enqueue(-getVertexDir(first, second, vertexes[n]));
                qeDirection.Enqueue(-getVertexDir(first, second, vertexes[n]));
            }
        }

        //void getTriangles(List<HashSet<int>> matrix)
        //{
        //    for (int i = 0; i < matrix.Count ; i++)
        //    {
        //        for (int j = 0; j < matrix[i].Count; j++)
        //        {
        //            int curr = matrix[i].ElementAt(j);
        //            if (curr >= i)
        //            {
        //                foreach (int elem in matrix[curr])
        //                {
        //                    for (int m = 0; m < matrix[i].Count ; i++)
        //                    {
        //                        int elem2 = matrix[i].ElementAt(m);
        //                        if (elem == elem2)
        //                        {

        //                            //foreach()
        //                            //if (matrix[elem].Any(i == elem) &&) // check that we have elem and elem1 in row with number of their common value
        //                            //{
        //                            triangles.Add(new Triangle(matrix[elem][i], matrix[elem2][j], matrix[++elem]));
                                    
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //}

        private bool checkRule()
        {
            throw new NotImplementedException();
        }


        public void gradient(Point[] points, PathGradientBrush pgbrush, Triangle tre)
        {
            Color[] mySurroundColor = { tre.v1.currColor, tre.v2.currColor, tre.v3.currColor};
            pgbrush.SurroundColors = mySurroundColor;
            int averageR = (tre.v1.currColor.R + tre.v2.currColor.R + tre.v3.currColor.R) / 3;
            int averageG = (tre.v1.currColor.G + tre.v2.currColor.G + tre.v3.currColor.G) / 3;
            int averageB = (tre.v1.currColor.B + tre.v2.currColor.B + tre.v3.currColor.B) / 3;
            Color centerCol = Color.FromArgb((byte) averageR, (byte)averageG, (byte)averageB);
            pgbrush.CenterColor = centerCol;
            pgbrush.SetBlendTriangularShape(1.0f, 1.0f);
            graphics.FillPolygon(pgbrush, points);
            pgbrush.Dispose();
        }

        private void Treangulation_Click(object sender, EventArgs e)
        {
            Bitmap bmpTre = bmpLines;
            vertexes = vertexes.Distinct().ToList();

            List<HashSet<int>> matrix = new List<HashSet<int>>();
            for (int k = 0; k < vertexes.Count; k++)
            {
                matrix.Add(new HashSet<int>());
            }

            this.Text = "Triangulation in progress...";
            
            int i = 0, j = 1;
            if (vertexes[i].x < vertexes[j].x)
            {
                int temp = i;
                i = j;
                j = temp;
            }
            for (int k = 2; k < vertexes.Count; k++)
            {
                if (vertexes[k].x > vertexes[i].x)
                {
                    j = i;
                    i = k;
                }
                else
                {
                    if (vertexes[k].x > vertexes[j].x)
                    {
                        j = k;
                    }
                }
            }
            
            Queue<int> qeVertexes1 = new Queue<int>();
            Queue<int> qeVertexes2 = new Queue<int>();
            Queue<int> qeDirection = new Queue<int>();
            qeVertexes1.Enqueue(i);
            qeVertexes2.Enqueue(j);
            qeDirection.Enqueue(0);
            while (qeVertexes1.Count() != 0 && qeVertexes2.Count() != 0)    {
                triangulate(qeVertexes1, qeVertexes2, qeDirection, matrix);
            }
            this.Text = "ImageChanger";
            pictureBox5.Image = bmpLines;
            //showTops();

            //for (int n = 1; n < (triangles.Count - 1); n++)
            //{
            //    for (int m = (n + 1); m < triangles.Count; m++)
            //    {
            //        if (Equal(n, m))
            //        {
            //            triangles[n] = null;
            //            break;
            //        }
            //    }
            //}
            
            
            
        }

        private void Draw_Click(object sender, EventArgs e)
        {
            this.Text = "Drawing in progress...";
            Point[] points = new Point[3];
            GraphicsPath brushPath = new GraphicsPath();
            brushPath.AddPolygon(points);
            pictureBox3.Image = bmpLines;
            List<PathGradientBrush> createdBrushes = new List<PathGradientBrush>();
            for (int i = 0; i < triangles.Count; i++)
            {
                points[0] = new Point(triangles[i].v1.x, triangles[i].v1.y);
                points[1] = new Point(triangles[i].v2.x, triangles[i].v2.y);
                points[2] = new Point(triangles[i].v3.x, triangles[i].v3.y);
                brushPath.AddPolygon(points);
                using (var brush = new PathGradientBrush(brushPath)) 
                {
                    gradient(points, brush, triangles[i]);
                    brush.Dispose();
                    //pictureBox3.Refresh();
                }
                
            }
            this.Text = "ImageChanger";   
        }

        private void Info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Maximum level of tree with recursions: " +  "/n" +"Number of contrast points: " + (vertexes.Count).ToString() + "\n" + "Number of triangles: " + (triangles.Count).ToString());
        }


    }
}
