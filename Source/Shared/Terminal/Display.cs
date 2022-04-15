using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal
{
    public class Display
    {
        const int DEFAULT_HEIGHT = 48;
        const int DEFAULT_WIDTH = 120;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int[,] Data { get; private set; }

        public Display()
        {
            Height = DEFAULT_HEIGHT;
            Width = DEFAULT_WIDTH;

            Data = new int[Height, Width];
        }

        public Display(int width, int height)
        {
            Height = height;
            Width = width;

            Data = new int[Height, Width];
        }

        public void Resize(int width, int height)
        {
            Height = height;
            Width = width;

            Data = new int[Height, Width];
        }

    }
}