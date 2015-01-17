using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Unicornhat
{
    public class RasterFont
    {
        public RasterFont()
        {
            ReadCharDef();
        }


        private string charDefFileName = "./RasterFont.xml";                              // raster font definition file should be located in the current dir
        private Dictionary<char, ulong> segDict = new Dictionary<char, ulong> { };      // the ulong holds 8 bytes, each being a row of bits defining an 8*8 raster font char


        #region Public methods

        // check if char exists in dictionary
        public bool FindChar(char chr)
        {
            if (segDict.ContainsKey(chr))
                return true;
            else
                return false;
        }

        public byte[] GetCharRaster(char chr) 
        {
            byte[] rasterRows = new byte[8];   // 8 bytes each defining a row of rasters

            if (!FindChar(chr))
                return rasterRows;

            ulong bits;
            segDict.TryGetValue(chr, out bits);

            for (int y = 0; y < 8; y++)         // vertical row
            {
                for (int x = 0; x < 6; x++)     // horizontal row
                {
                    if ((bits & 1) != 0)               
                        rasterRows[y] = (byte)(rasterRows[y] * 2 + 1);
                    else
                        rasterRows[y] = (byte)(rasterRows[y] * 2);

                    bits = bits >> 1;                   // roll bits to the right
                }
            }

            return rasterRows;
        }


        #endregion




        #region Xml Read/Write

        // read character definitions
        // currently the file location is hardcoded to a specific directory
        internal void ReadCharDef()
        {
            XmlTextReader xmlReader = null;

            try
            {
                xmlReader = new XmlTextReader(charDefFileName);

                string chrs = "";
                ulong bits = 0;

                while (xmlReader.Read())
                {
                    // read matrix layout, currently not used
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.CompareTo("Layout") == 0)
                    {
                        int vertical = Int32.Parse(xmlReader.GetAttribute("Vertical").ToString());
                        int horizontal = Int32.Parse(xmlReader.GetAttribute("Horizontal").ToString());
                    }
                    // read character definitions
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.CompareTo("character") == 0)
                    {
                        chrs = xmlReader.GetAttribute("char").ToString();
                        bits = UInt64.Parse(xmlReader.GetAttribute("bits").ToString());
                        if (!FindChar(chrs[0])) 
                            segDict.Add(chrs[0], bits);
                    }
                }
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }

        #endregion



    }
}
