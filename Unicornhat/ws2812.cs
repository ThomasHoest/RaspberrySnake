using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Unicornhat
{
    public class ws2812
    {
        public ws2812 ()
        {
            rotation = Rotation.rot_0;
        }

        #region PInvoke c library ws2812-RPi.so 

        // Initialize the Unicorn hardware
        [DllImport("ws2812-RPi.so", EntryPoint = "init")]
        static extern void f_init(int numPixels);
        public void Init() { f_init(64); }

        //  Update UnicornHat with the contents of the display buffer
        [DllImport("ws2812-RPi.so", EntryPoint = "show")]
        static extern void f_show();
        public void Show() { f_show(); }

        // Clear the display buffer
        [DllImport("ws2812-RPi.so", EntryPoint = "clear")]
        static extern void f_clear();
        public void Clear() { f_clear(); }

        //  Set the display brightness between 0.0 and 1.0 - 0.2 is highly recommended, UnicornHat can get painfully bright!
        [DllImport("ws2812-RPi.so", EntryPoint = "setBrightness")]
        static extern byte f_setBrightness(float b);
        public byte SetBrightness(float b = (float) 0.2) 
        {
            if (b > 1 || b < 0)
                throw new Exception("Brightness must be between 0.0 and 1.0");
            else return f_setBrightness(b); 
        }

        [DllImport("ws2812-RPi.so", EntryPoint = "setPixelColor")]
        static extern byte f_setPixelColor(int index, byte r, byte g, byte b);
        private byte SetPixelColor(int index, byte r, byte g, byte b) { return f_setPixelColor(index, r, g, b); }

        #endregion

       
        #region Simple utility functions
        
        public void TurnDisplayOff()
        {
          Clear();
          Show();
        }

        // Store the rotation of UnicornHat, defaults to 0 wwhich places 0,0 on the top left with the B+ HDMI port facing downwards
        public enum Rotation 
        {
            rot_0,
            rot_90,
            rot_180,
            rot_270
        }
        public Rotation rotation { get; set; }

        // Set a single pixel to the RGB value. Coordinate are based on the current rotation. (x,y)=(0,0) is top left corner in current rotation.
        public void SetPixel(int x, int y, byte r, byte g, byte b)
        {
            SetPixelColor(GetIndexFromXY(x, y), r, g, b);
        }

        // Set a row of 8 pixels to the RGB color
        public void SetPixelRow(int row, byte pixels, byte r, byte g, byte b) 
        {
            if (row < 0 || row > 7)
                throw new Exception("row must be in the interval 0 to 7");

            for (int i=7; i >= 0; i--)
            {
                if ((pixels & (1 << i)) != 0)
                  SetPixelColor(GetIndexFromXY(7-i, row), r, g, b);
            }
        }


        #endregion

        #region Private members and methods

        // A map of pixel indexes for translating x, y coordinates
        int [,] pixelMap = new int [8, 8] 
        { 
            {7 ,6 ,5 ,4 ,3 ,2 ,1 ,0 },
            {8 ,9 ,10,11,12,13,14,15},
            {23,22,21,20,19,18,17,16},
            {24,25,26,27,28,29,30,31},
            {39,38,37,36,35,34,33,32},
            {40,41,42,43,44,45,46,47},
            {55,54,53,52,51,50,49,48},
            {56,57,58,59,60,61,62,63}
        };

        int GetIndexFromXY(int x, int y)
        {
            if (x < 0 || x > 7)
                throw new Exception("X coordinate must be in the interval 0 to 7");
            if (y < 0 || y > 7)
                throw new Exception("Y coordinate must be in the interval 0 to 7");

            int t = 0;
            switch (rotation)
            {
                case Rotation.rot_0: y = 7 - y; break;
                case Rotation.rot_90: t = x;  x = 7 - y; y = 7 - t; break;
                case Rotation.rot_180: x = 7 - x; break;
                case Rotation.rot_270: t = x;  x = y; y = t; break;
            }

            return pixelMap[x,y];
        }

        #endregion
    }
}
