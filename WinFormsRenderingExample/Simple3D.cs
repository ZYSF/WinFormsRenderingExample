using System;

// A simple 3D renderer (Zak's C# implementation of the algorithms at http://blog.rogach.org/2015/08/how-to-create-your-own-simple-3d-render.html)
namespace Simple3D
{
    public class Vertex<T>
    {
        public T X;
        public T Y;
        public T Z;

        public Vertex(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    public class Triangle<T>
    {
        public Vertex<T> A;
        public Vertex<T> B;
        public Vertex<T> C;
        public uint RGBA;

        public Triangle(Vertex<T> a, Vertex<T> b, Vertex<T> c, uint rgba)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.RGBA = rgba;
        }
    }

    public class SimpleMatrix
    {
        double[] Values;

        public SimpleMatrix(double[] values)
        {
            this.Values = values;
        }
        
        public SimpleMatrix Mul(SimpleMatrix m)
        {
            double[] tmp = new double[9];
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        tmp[y * 3 + x] += Values[(y * 3) + i] * m.Values[(i * 3) + x];
                    }
                }
            }
            return new SimpleMatrix(tmp);
        }

        public Vertex<double> Apply(Vertex<double> v)
        {
            return new Vertex<double>(
                v.X * Values[0] + v.Y * Values[3] + v.Z * Values[6],
                v.X * Values[1] + v.Y * Values[4] + v.Z * Values[7],
                v.X * Values[2] + v.Y * Values[5] + v.Z * Values[8]);
        }
    }

    public class Renderer
    {
        public int Width, Height;
        public uint[] Pixels;
        public double[] Depths;

        public Renderer(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;

            Pixels = new uint[Width * Height];
            Depths = new double[Width * Height];
        }

        public void Paint(SimpleMatrix matrix, Triangle<double> t)
        {
            Vertex<double> a = matrix.Apply(t.A);
            a.X += Width / 2;
            a.Y += Height / 2;
            Vertex<double> b = matrix.Apply(t.B);
            b.X += Width / 2;
            b.Y += Height / 2;
            Vertex<double> c = matrix.Apply(t.C);
            c.X += Width / 2;
            c.Y += Height / 2;

            Vertex<double> ab = new Vertex<double>(b.X - a.X, b.Y - a.Y, b.Z - a.Z);
            Vertex<double> ac = new Vertex<double>(c.X - a.X, c.Y - a.Y, c.Z - a.Z);

            Vertex<double> n = new Vertex<double>(
                ab.Y * ac.Z - ab.Z * ac.Y,
                ab.Z * ac.X - ab.X * ac.Z,
                ab.X * ac.Y - ab.Y * ac.X);
            double nlen = Math.Sqrt(n.X * n.X + n.Y * n.Y + n.Z * n.Z);
            n.X /= nlen;
            n.Y /= nlen;
            n.Z /= nlen;

            double acos = Math.Abs(n.Z);

            int minx = (int)Math.Max(0, Math.Ceiling(Math.Min(a.X, Math.Max(b.X, c.X))));
            int maxx = (int)Math.Min(Width - 1, Math.Floor(Math.Max(a.X, Math.Max(b.X, c.X))));

            int miny = (int)Math.Max(0, Math.Ceiling(Math.Min(a.Y, Math.Max(b.Y, c.Y))));
            int maxy = (int)Math.Min(Height - 1, Math.Floor(Math.Max(a.Y, Math.Max(b.Y, c.Y))));

            double tarea = (a.Y - c.Y) * (b.X - c.X) + (b.Y - c.Y) * (c.X - a.X);

            for (int y = miny; y <= maxy; y++)
            {
                for (int x = minx; x <= maxx; x++)
                {
                    double pa = ((y - c.Y) * (b.X - c.X) + (b.Y - c.Y) * (c.X - x)) / tarea;
                    double pb = ((y - a.Y) * (c.X - a.X) + (c.Y - a.Y) * (a.X - x)) / tarea;
                    double pc = ((y - b.Y) * (a.X - b.X) + (a.Y - b.Y) * (b.X - x)) / tarea;
                    if (pa >= 0 && pa <= 1 && pb >= 0 && pb <= 1 && pc >= 0 && pc <= 1)
                    {
                        double d = pa * a.Z + pb * b.Z + pc * c.Z;
                        int idx = y * Width + x;
                        if (Depths[idx] < d)
                        {
                            Pixels[idx] = t.RGBA;
                            Depths[idx] = d;
                        }
                    }
                }
            }
        }
    }
}
