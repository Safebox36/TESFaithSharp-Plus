using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ArrayList=System.Collections.ArrayList;
using ImageFormat=System.Drawing.Imaging.ImageFormat;

namespace TESFaith {
    public class MapDrawer {
        private const int SquareSizeL=12;
        private const int SquareSizeM=SquareSizeL/2;
        private const int SquareSizeS=SquareSizeL/4;

        private const int OutSizeL=16;
        private const int OutSizeM=OutSizeL/2;
        //private const int OutSizeS=OutSizeL/4;
        //private static Brush OverlayBrush=new SolidBrush(Color.FromArgb(127,127,127,0));

        private static ArrayList OverlaidCells=new ArrayList();
        private static Dictionary<string, Brush> OverlaidRegions=new Dictionary<string, Brush>();
        private static ArrayList OverlayNames=new ArrayList();
        private static ArrayList CurrentCells;
        private static Dictionary<string, Brush> CurrentRegions;
        private static int LeftCorner=-30;
        private static int TopCorner=30;
        private static int xSize=40;
        private static int ySize=40;
        public static System.Windows.Forms.Panel panel;
        private static int CurrentCellX=-30000;
        private static int CurrentCellY=-30000;
        public static int SelectedX,SelectedY;
        private static int overlays=0;
        private static bool selected;
        public static bool Selected {
            get { return selected; }
            set { selected=value; panel.Invalidate(); }
        }
        public static int Overlays {
            get { return overlays; }
        }

        public static void GetCellPos(int x,int y,out int xpos,out int ypos) {
            Point p=new Point(x,y);
            xpos=LeftCorner+(p.X/SquareSizeL);
            ypos=TopCorner-(p.Y/SquareSizeL);
        }

        public static bool CellExists(int xpos,int ypos) {
            foreach(ChangedCell cc in CurrentCells) {
                if(cc.OldX==xpos&&cc.OldY==ypos) {
                    return true;
                }
            }
            return false;
        }

        public static void Scroll(int x,int y) {
            LeftCorner+=x;
            TopCorner+=y;
            panel.Invalidate();
        }

        public static void UpdateSize(object o,EventArgs e) {
            int oldx=xSize;
            int oldy=ySize;
            xSize=(panel.Width/SquareSizeL)+1;
            ySize=(panel.Height/SquareSizeL)+1;
            if(xSize>oldx||ySize>oldy) panel.Invalidate();
        }

        public static void AddOverlay(ArrayList list,string plugin) {
            for(int i=0;i<list.Count;i++) {
                ChangedCell cc=(ChangedCell)list[i];
                cc.Plugin=plugin;
                cc.Overlay=overlays;
                OverlaidCells.Add(cc);
            }
            overlays++;
            OverlayNames.Add(plugin);
            panel.Invalidate();
        }

        public static void GetMousePos(int x,int y,out string message) {
            Point p=new Point(x,y);
            int xpos=LeftCorner+(p.X/SquareSizeL);
            int ypos=TopCorner-(p.Y/SquareSizeL);
            if(xpos!=CurrentCellX||ypos!=CurrentCellY) {
                CurrentCellX=xpos;
                CurrentCellY=ypos;
                message="("+xpos.ToString()+", "+ypos.ToString()+")";
                if(CurrentCells!=null) {
                    foreach(ChangedCell cc in CurrentCells) {
                        if(cc.OldX==xpos&&cc.OldY==ypos) {
                            message+="\n[ACTIVE] ";
                            if(cc.Name!="") message+="\""+cc.Name+"\", ";
                            else message+="no name, ";
                            if(cc.RegionName!="") message+="\""+cc.RegionName+"\"";
                            else message+="Cell has no associated region";
                            break;
                        }
                    }
                }
                foreach(ChangedCell cc in OverlaidCells) {
                    if(cc.OldX==xpos&&cc.OldY==ypos) {
                        message+="\n["+cc.Plugin+"] ";
                        if(cc.Name!="") message+="\""+cc.Name+"\", ";
                        else message+="no name, ";
                        if(cc.RegionName!="") message+="\""+cc.RegionName+"\"";
                        else message+="Cell has no associated region";
                    }
                }
            } else {
                message=null;
            }
        }

        public static void ClearOverlay() {
            overlays=0;
            OverlaidCells.Clear();
            OverlayNames.Clear();
            panel.Invalidate();
        }

        public static void SetActiveCells(ArrayList list) {
            if(list==null) {
                CurrentCells.Clear();
                CurrentCells=null;
            } else {
                CurrentCells=new ArrayList();
                CurrentCells.AddRange(list);
            }
            panel.Invalidate();
        }

        public static void SetActiveRegions(ArrayList list)
        {
            if (list==null) {
				CurrentRegions.Clear();
				CurrentRegions = null;
			}
			else
			{
                var brushes = new ArrayList(new Brush[]
                {
                    Brushes.Red,
                    Brushes.Orange,
                    Brushes.Gold,
					Brushes.Green,
					Brushes.Blue,
					Brushes.Indigo,
					Brushes.Violet,

                    Brushes.DarkRed,
					Brushes.OrangeRed,
					Brushes.Goldenrod,
					Brushes.OliveDrab,
					Brushes.SteelBlue,
					Brushes.DarkMagenta,
					Brushes.DarkViolet,

                    Brushes.Gray,
                    Brushes.SlateGray
                });

				CurrentRegions = new Dictionary<string, Brush>();
				for (int i = 0; i < list.Count; i++)
                {
                    if (CurrentRegions.ContainsKey(((ChangedCell)list[i]).RegionName)) continue;
                    CurrentRegions.Add(((ChangedCell)list[i]).RegionName, (Brush)brushes[CurrentRegions.Count % (brushes.Count - 1)]);
				}
			}
			panel.Invalidate();
        }

        public static void PaintMap(System.Windows.Forms.PaintEventArgs e, RadioButton radCells, RadioButton radRegions) {
            //Possibly not the most efficient way of doing things, but it works well enough
            e.Graphics.Clear(Color.White);
            
            foreach(ChangedCell cc in OverlaidCells) {
                if(cc.OldX<LeftCorner||cc.OldX>=LeftCorner+xSize) continue;
                if(cc.OldY>TopCorner||cc.OldY<=TopCorner-ySize) continue;
                e.Graphics.FillRectangle(cc.Name != "" ? Brushes.LightBlue : Brushes.Khaki,(cc.OldX-LeftCorner)*SquareSizeL,
                    (TopCorner-cc.OldY)*SquareSizeL,SquareSizeL,SquareSizeL);
            }
            if(CurrentCells!=null) {
                foreach(ChangedCell cc in CurrentCells) {
                    if(cc.OldX<LeftCorner||cc.OldX>=LeftCorner+xSize) continue;
                    if(cc.OldY>TopCorner||cc.OldY<=TopCorner-ySize) continue;
                    if (radCells.Checked)
                    {
                        e.Graphics.FillRectangle(cc.Name != "" ? Brushes.RoyalBlue : Brushes.LimeGreen, SquareSizeS + (cc.OldX - LeftCorner) * SquareSizeL,
                            SquareSizeS + (TopCorner - cc.OldY) * SquareSizeL, SquareSizeM, SquareSizeM);
                    }
                    else if (radRegions.Checked)
					{
						e.Graphics.FillRectangle(CurrentRegions[cc.RegionName], SquareSizeS + (cc.OldX - LeftCorner) * SquareSizeL,
							SquareSizeS + (TopCorner - cc.OldY) * SquareSizeL, SquareSizeM, SquareSizeM);
					}
                }
            }
            for(int x=0;x<xSize;x++) {
                e.Graphics.DrawLine((x == (SelectedX - LeftCorner) || x - 1 == (SelectedX - LeftCorner)) ? Pens.Tomato : Pens.LightGray,x*SquareSizeL,0,x*SquareSizeL,ySize*SquareSizeL);
                if (Selected)
                {
                    if (x == (SelectedX - LeftCorner))
                    {
                        e.Graphics.DrawLine(Pens.Tomato, x * SquareSizeL + 1, 0, x * SquareSizeL + 1, ySize * SquareSizeL);
                    }
                    else if (x - 1 == (SelectedX - LeftCorner))
                    {
						e.Graphics.DrawLine(Pens.Tomato, x * SquareSizeL - 1, 0, x * SquareSizeL - 1, ySize * SquareSizeL);
					}
                }
			}
            for(int y=0;y<ySize;y++) {
                e.Graphics.DrawLine((y == (TopCorner - SelectedY) || y - 1 == (TopCorner - SelectedY)) ? Pens.Tomato : Pens.LightGray,0,y*SquareSizeL,xSize*SquareSizeL,y*SquareSizeL);
				if (Selected)
				{
					if (y == (TopCorner - SelectedY))
					{
						e.Graphics.DrawLine(Pens.Tomato, 0, y * SquareSizeL + 1, xSize * SquareSizeL, y * SquareSizeL + 1);
					}
					else if (y - 1 == (TopCorner - SelectedY))
					{
						e.Graphics.DrawLine(Pens.Tomato, 0, y * SquareSizeL - 1, xSize * SquareSizeL, y * SquareSizeL - 1);
					}
				}
			}
   //         if(Selected) {
   //             e.Graphics.DrawRectangle(Pens.Tomato,(SelectedX-LeftCorner)*SquareSizeL,
   //                 (TopCorner-SelectedY)*SquareSizeL,SquareSizeL,SquareSizeL);
			//	e.Graphics.DrawRectangle(Pens.Tomato, (SelectedX - LeftCorner) * SquareSizeL + 1,
			//		(TopCorner - SelectedY) * SquareSizeL + 1, SquareSizeL - 2, SquareSizeL - 2);
			//}
        }

        public static void SaveMap(string FilePath,int FileType, string PluginName) {
            //if(Overlays==0) {
            //    System.Windows.Forms.MessageBox.Show("You need to overlay some mods to make a map of.", "Oops!");
            //    return;
            //}
            if(overlays>=64) {
                System.Windows.Forms.MessageBox.Show("Can only make a map of up to 63 overlayed mods.", "Oops!");
                return;
            }
            //Find the left,right,top and bottom
            int LeftMost=0,RightMost=0,TopMost=0,BottomMost=0;
            foreach(ChangedCell cc in OverlaidCells) {
                if(cc.OldX>RightMost) RightMost=cc.OldX;
                if(cc.OldX<LeftMost) LeftMost=cc.OldX;
                if(cc.OldY<BottomMost) BottomMost=cc.OldY;
                if(cc.OldY>TopMost) TopMost=cc.OldY;
            }
            BottomMost-=1;
            RightMost+=1;
            //Keep track of how many cells are at each spot
            byte[,] CellTotalCount=new byte[1+RightMost-LeftMost, 1+TopMost-BottomMost];
            byte[,] CellCounted=new byte[1+RightMost-LeftMost, 1+TopMost-BottomMost];
            foreach(ChangedCell cc in OverlaidCells) {
                CellTotalCount[cc.OldX-LeftMost, TopMost-cc.OldY]++;
            }
            Brush[] brushes=new Brush[overlays];
            byte[] colours=new byte[3];
            byte m,last;
            for(byte i=1;i<=overlays;i++) {
                m=(byte)(i%3);
                last=(byte)(i%4);
                colours[(m+0)%3]=(byte)(last*64);
                last=(byte)(((i-last)/4)%4);
                colours[(m+1)%3]=(byte)(last*64);
                last=(byte)(((i-last)/4)%4);
                colours[(m+2)%3]=(byte)(last*64);
                colours[1]=(byte)((colours[1]+128)%256);
                brushes[i-1]=new SolidBrush(Color.FromArgb(colours[0],+colours[1],colours[2]));
            }
            brushes[overlays] = new SolidBrush(Color.FromArgb(255 - colours[0], 255 - colours[1], 255 - colours[2]));
            //Create an image
            Bitmap bmp=new Bitmap((3+RightMost-LeftMost)*OutSizeL, (3+TopMost-BottomMost)*OutSizeL+10*overlays);
            Graphics g=Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.TranslateTransform(OutSizeL, OutSizeM);
            //Draw the cells
            byte BarHeight;
            byte BarOffset;
            foreach(ChangedCell cc in OverlaidCells) {
                BarHeight=(byte)(OutSizeL/CellTotalCount[cc.OldX-LeftMost, TopMost-cc.OldY]);
                BarOffset=(byte)(BarHeight*CellCounted[cc.OldX-LeftMost, TopMost-cc.OldY]++);
                g.FillRectangle(cc.Name != "" ? brushes[overlays] : brushes[cc.Overlay], (1+cc.OldX-LeftMost)*OutSizeL,
                    (1+TopMost-cc.OldY)*OutSizeL+BarOffset, OutSizeL, BarHeight);
            }
            //Draw the lines
            for(int x=1;x<(2+RightMost-LeftMost);x++) {
                g.DrawLine(Pens.LightGray, x*OutSizeL, OutSizeL, x*OutSizeL, (1+TopMost-BottomMost)*OutSizeL);
            }
            for(int y=1;y<(2+TopMost-BottomMost);y++) {
                g.DrawLine(Pens.LightGray, OutSizeL, y*OutSizeL, (1+RightMost-LeftMost)*OutSizeL, y*OutSizeL);
            }
            //Draw the co-ordinates
            Font font=new Font("arial", 8);
            StringFormat sf=new StringFormat();
            sf.Alignment=StringAlignment.Center;
            sf.LineAlignment=StringAlignment.Center;
            for(int x=LeftMost;x<RightMost;x++) {
                g.DrawString(x.ToString(), font, Brushes.Black, (1+x-LeftMost)*OutSizeL+OutSizeM, OutSizeM,sf);
            }
            for(int y=TopMost;y>BottomMost;y--) {
                g.DrawString(y.ToString(), font, Brushes.Black, 0, OutSizeL-(y-TopMost)*OutSizeL+OutSizeM,sf);
            }
            //Draw the key
            g.DrawString(PluginName, font, Brushes.Black, OutSizeL, (1+TopMost-BottomMost)*OutSizeL);
			for (int i=0;i<overlays;i++) {
                g.DrawString((i+1).ToString().PadLeft(2, '0')+" - "+(string)OverlayNames[i], font, brushes[i], OutSizeL, (1+TopMost-BottomMost)*OutSizeL+10*(i+2));
            }
            //Save it
            g.Dispose();
            ImageFormat format;
            switch(FileType) {
                case 0: format=ImageFormat.Bmp; break;
                default: format=ImageFormat.Bmp; break;
            }
            bmp.Save(FilePath,format);
        }
    }
}
