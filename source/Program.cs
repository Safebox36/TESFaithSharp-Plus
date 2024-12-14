using System;
using System.Windows.Forms;

namespace TESFaith {
    public enum RuleType { All=0, Region, Cell }
    public enum RuleType2 { Move=0, Transpose, Copy, Delete, Ignore, IntCopy, IntDelete, IntIgnore }

    public struct Rule {
        public RuleType2 type2;
        public RuleType type;
        public string cellName;
        public string regionName;
        public string newName;
        public ChangedCell data;
    }


    public struct ChangedCell {
        public int OldX;
        public int OldY;
        public int NewX;
        public int NewY;
        public int Overlay;
        public string Name;
        public string RegionName;
        public string Plugin;

        public ChangedCell(int oldX,int oldY,string name,string regionName) {
            OldX=oldX;
            OldY=oldY;
            NewX=0;
            NewY=0;
            if(name!=null) Name=name; else Name="";
            if(regionName!=null) RegionName=regionName; else RegionName="";
            Plugin=null;
            Overlay=0;
        }
    }

    public enum CellType { Exterior, Interior, True, False }
    public struct CellData {
        public int size;
        public string name;
        public string region_name;
        public int current_x;
        public int current_y;
        public int new_x;
        public int new_y;
        public CellType type;
        public bool save;
        public bool copy;
    }

    public class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
