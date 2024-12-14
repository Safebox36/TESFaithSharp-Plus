using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ArrayList=System.Collections.ArrayList;
using Path=System.IO.Path;
//TODO: Cell names are null terminated

namespace TESFaith {
    public class MainForm : Form {
        #region FormDesignerGunk
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components=null;
        private Button bOpen;
        private Button bBatch;
        private Button bClose;
        private Panel MapPanel;
        private Button bOverlay;
		private FlowLayoutPanel bSFlow;
		private Label lPanel;
        private Button bSLeft;
        private Button bSRight;
        private Button bSUp;
        private Button bSDown;
        private Button bClearOverlay;
        private ToolTip ttCellData;
        private Button bMove;
        private Button bCopy;
        private Button bDelete;
        private Button bCellList;
        private Button bMap;
        private SaveFileDialog SaveMapDialog;
		private FlowLayoutPanel flowLayoutPanel1;
		private Label lCell;
		private Label lSeperator;
		private ListBox lstCells;
		private Label lCellCount;
		private RadioButton radNamedCells;
		private RadioButton radRegions;
		private TextBox txtFilter;
		private OpenFileDialog OpenModDialog;
		private StatusStrip statusBottom;
		private ToolStripStatusLabel tipMouse;
		private ToolStripProgressBar tipProgress;
		private ArrayList cellList;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing&&(components!=null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.bOpen = new System.Windows.Forms.Button();
			this.bBatch = new System.Windows.Forms.Button();
			this.OpenModDialog = new System.Windows.Forms.OpenFileDialog();
			this.bClose = new System.Windows.Forms.Button();
			this.MapPanel = new System.Windows.Forms.Panel();
			this.bOverlay = new System.Windows.Forms.Button();
			this.lPanel = new System.Windows.Forms.Label();
			this.bSLeft = new System.Windows.Forms.Button();
			this.bSRight = new System.Windows.Forms.Button();
			this.bSUp = new System.Windows.Forms.Button();
			this.bSDown = new System.Windows.Forms.Button();
			this.bClearOverlay = new System.Windows.Forms.Button();
			this.ttCellData = new System.Windows.Forms.ToolTip(this.components);
			this.bMove = new System.Windows.Forms.Button();
			this.bCopy = new System.Windows.Forms.Button();
			this.bDelete = new System.Windows.Forms.Button();
			this.bCellList = new System.Windows.Forms.Button();
			this.bMap = new System.Windows.Forms.Button();
			this.SaveMapDialog = new System.Windows.Forms.SaveFileDialog();
			this.bSFlow = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lCell = new System.Windows.Forms.Label();
			this.lSeperator = new System.Windows.Forms.Label();
			this.lstCells = new System.Windows.Forms.ListBox();
			this.lCellCount = new System.Windows.Forms.Label();
			this.radNamedCells = new System.Windows.Forms.RadioButton();
			this.radRegions = new System.Windows.Forms.RadioButton();
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.statusBottom = new System.Windows.Forms.StatusStrip();
			this.tipMouse = new System.Windows.Forms.ToolStripStatusLabel();
			this.tipProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.cellList = new ArrayList();
			this.bSFlow.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.statusBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// bOpen
			// 
			this.bOpen.Location = new System.Drawing.Point(12, 12);
			this.bOpen.Name = "bOpen";
			this.bOpen.Size = new System.Drawing.Size(144, 24);
			this.bOpen.TabIndex = 0;
			this.bOpen.Text = "Open Mod (Ctrl+O)";
			this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
			// 
			// bBatch
			// 
			this.bBatch.Enabled = false;
			this.bBatch.Location = new System.Drawing.Point(312, 12);
			this.bBatch.Name = "bBatch";
			this.bBatch.Size = new System.Drawing.Size(92, 54);
			this.bBatch.TabIndex = 1;
			this.bBatch.Text = "Batch Process\r\n(Ctrl+B)";
			this.bBatch.Click += new System.EventHandler(this.bBatch_Click);
			// 
			// OpenModDialog
			// 
			this.OpenModDialog.DefaultExt = "esp";
			this.OpenModDialog.Filter = "Morrowind plugins (esp/esm)|*.esp;*.esm|All files|*.*";
			this.OpenModDialog.RestoreDirectory = true;
			this.OpenModDialog.Title = "Select mod to open";
			// 
			// bClose
			// 
			this.bClose.Enabled = false;
			this.bClose.Location = new System.Drawing.Point(12, 42);
			this.bClose.Name = "bClose";
			this.bClose.Size = new System.Drawing.Size(144, 24);
			this.bClose.TabIndex = 3;
			this.bClose.Text = "Close Mod (Ctrl+W)";
			this.bClose.Click += new System.EventHandler(this.bClose_Click);
			// 
			// MapPanel
			// 
			this.MapPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MapPanel.Location = new System.Drawing.Point(12, 72);
			this.MapPanel.Name = "MapPanel";
			this.MapPanel.Size = new System.Drawing.Size(978, 548);
			this.MapPanel.TabIndex = 4;
			this.MapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MapPanel_Paint);
			this.MapPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseDown);
			this.MapPanel.MouseLeave += new System.EventHandler(this.MapPanel_MouseLeave);
			this.MapPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseMove);
			// 
			// bOverlay
			// 
			this.bOverlay.Location = new System.Drawing.Point(162, 12);
			this.bOverlay.Name = "bOverlay";
			this.bOverlay.Size = new System.Drawing.Size(144, 24);
			this.bOverlay.TabIndex = 5;
			this.bOverlay.Text = "Overlay Plugin (Ctrl+T)";
			this.bOverlay.Click += new System.EventHandler(this.bOverlay_Click);
			// 
			// lPanel
			// 
			this.lPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lPanel.AutoSize = true;
			this.lPanel.Location = new System.Drawing.Point(3, 8);
			this.lPanel.Name = "lPanel";
			this.lPanel.Size = new System.Drawing.Size(26, 13);
			this.lPanel.TabIndex = 0;
			this.lPanel.Text = "Pan";
			this.lPanel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// bSLeft
			// 
			this.bSLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bSLeft.Location = new System.Drawing.Point(35, 3);
			this.bSLeft.Name = "bSLeft";
			this.bSLeft.Size = new System.Drawing.Size(64, 24);
			this.bSLeft.TabIndex = 6;
			this.bSLeft.Text = "Left (A)";
			this.bSLeft.Click += new System.EventHandler(this.bScrollClick);
			// 
			// bSRight
			// 
			this.bSRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bSRight.Location = new System.Drawing.Point(105, 3);
			this.bSRight.Name = "bSRight";
			this.bSRight.Size = new System.Drawing.Size(64, 24);
			this.bSRight.TabIndex = 7;
			this.bSRight.Text = "Right (D)";
			this.bSRight.Click += new System.EventHandler(this.bScrollClick);
			// 
			// bSUp
			// 
			this.bSUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bSUp.Location = new System.Drawing.Point(175, 3);
			this.bSUp.Name = "bSUp";
			this.bSUp.Size = new System.Drawing.Size(64, 24);
			this.bSUp.TabIndex = 8;
			this.bSUp.Text = "Up (W)";
			this.bSUp.Click += new System.EventHandler(this.bScrollClick);
			// 
			// bSDown
			// 
			this.bSDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bSDown.Location = new System.Drawing.Point(245, 3);
			this.bSDown.Name = "bSDown";
			this.bSDown.Size = new System.Drawing.Size(64, 24);
			this.bSDown.TabIndex = 9;
			this.bSDown.Text = "Down (S)";
			this.bSDown.Click += new System.EventHandler(this.bScrollClick);
			// 
			// bClearOverlay
			// 
			this.bClearOverlay.Enabled = false;
			this.bClearOverlay.Location = new System.Drawing.Point(162, 42);
			this.bClearOverlay.Name = "bClearOverlay";
			this.bClearOverlay.Size = new System.Drawing.Size(144, 24);
			this.bClearOverlay.TabIndex = 10;
			this.bClearOverlay.Text = "Clear Overlays (Ctrl+R)";
			this.bClearOverlay.Click += new System.EventHandler(this.bClearOverlay_Click);
			// 
			// bMove
			// 
			this.bMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bMove.Enabled = false;
			this.bMove.Location = new System.Drawing.Point(33, 3);
			this.bMove.Name = "bMove";
			this.bMove.Size = new System.Drawing.Size(92, 24);
			this.bMove.TabIndex = 11;
			this.bMove.Text = "Move (Ctrl+M)";
			this.bMove.Click += new System.EventHandler(this.bMove_Click);
			// 
			// bCopy
			// 
			this.bCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bCopy.Enabled = false;
			this.bCopy.Location = new System.Drawing.Point(131, 3);
			this.bCopy.Name = "bCopy";
			this.bCopy.Size = new System.Drawing.Size(92, 24);
			this.bCopy.TabIndex = 12;
			this.bCopy.Text = "Copy (Ctrl+C)";
			this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
			// 
			// bDelete
			// 
			this.bDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bDelete.Enabled = false;
			this.bDelete.Location = new System.Drawing.Point(229, 3);
			this.bDelete.Name = "bDelete";
			this.bDelete.Size = new System.Drawing.Size(92, 24);
			this.bDelete.TabIndex = 13;
			this.bDelete.Text = "Delete (Del)";
			this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
			// 
			// bCellList
			// 
			this.bCellList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bCellList.Enabled = false;
			this.bCellList.Location = new System.Drawing.Point(1160, 12);
			this.bCellList.Name = "bCellList";
			this.bCellList.Size = new System.Drawing.Size(92, 54);
			this.bCellList.TabIndex = 14;
			this.bCellList.Text = "List Cells\n(Ctrl+L)";
			this.bCellList.Click += new System.EventHandler(this.bCellList_Click);
			// 
			// bMap
			// 
			this.bMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bMap.Enabled = false;
			this.bMap.Location = new System.Drawing.Point(327, 3);
			this.bMap.Name = "bMap";
			this.bMap.Size = new System.Drawing.Size(92, 24);
			this.bMap.TabIndex = 15;
			this.bMap.Text = "Save (Ctrl+S)";
			this.bMap.Click += new System.EventHandler(this.bMap_Click);
			// 
			// SaveMapDialog
			// 
			this.SaveMapDialog.Filter = "BMP | *.bmp";
			// 
			// bSFlow
			// 
			this.bSFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bSFlow.AutoSize = true;
			this.bSFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.bSFlow.Controls.Add(this.lPanel);
			this.bSFlow.Controls.Add(this.bSLeft);
			this.bSFlow.Controls.Add(this.bSRight);
			this.bSFlow.Controls.Add(this.bSUp);
			this.bSFlow.Controls.Add(this.bSDown);
			this.bSFlow.Location = new System.Drawing.Point(12, 626);
			this.bSFlow.Name = "bSFlow";
			this.bSFlow.Size = new System.Drawing.Size(312, 30);
			this.bSFlow.TabIndex = 16;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.lCell);
			this.flowLayoutPanel1.Controls.Add(this.bMove);
			this.flowLayoutPanel1.Controls.Add(this.bCopy);
			this.flowLayoutPanel1.Controls.Add(this.bDelete);
			this.flowLayoutPanel1.Controls.Add(this.bMap);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(330, 626);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(422, 30);
			this.flowLayoutPanel1.TabIndex = 17;
			// 
			// lCell
			// 
			this.lCell.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lCell.AutoSize = true;
			this.lCell.Location = new System.Drawing.Point(3, 8);
			this.lCell.Name = "lCell";
			this.lCell.Size = new System.Drawing.Size(24, 13);
			this.lCell.TabIndex = 0;
			this.lCell.Text = "Cell";
			this.lCell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lSeperator
			// 
			this.lSeperator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lSeperator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lSeperator.Location = new System.Drawing.Point(327, 626);
			this.lSeperator.Name = "lSeperator";
			this.lSeperator.Size = new System.Drawing.Size(2, 30);
			this.lSeperator.TabIndex = 0;
			// 
			// lstCells
			// 
			this.lstCells.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstCells.BackColor = System.Drawing.SystemColors.Control;
			this.lstCells.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstCells.FormattingEnabled = true;
			this.lstCells.HorizontalScrollbar = true;
			this.lstCells.Location = new System.Drawing.Point(996, 97);
			this.lstCells.MinimumSize = new System.Drawing.Size(256, 0);
			this.lstCells.Name = "lstCells";
			this.lstCells.Size = new System.Drawing.Size(256, 522);
			this.lstCells.TabIndex = 18;
			// 
			// lCellCount
			// 
			this.lCellCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lCellCount.Location = new System.Drawing.Point(996, 14);
			this.lCellCount.Name = "lCellCount";
			this.lCellCount.Size = new System.Drawing.Size(158, 48);
			this.lCellCount.TabIndex = 19;
			this.lCellCount.Text = "0 CELLs\r\n0 named CELLs\r\n0 REGIONs";
			this.lCellCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radNamedCells
			// 
			this.radNamedCells.Checked = true;
			this.radNamedCells.Location = new System.Drawing.Point(410, 14);
			this.radNamedCells.Name = "radNamedCells";
			this.radNamedCells.Size = new System.Drawing.Size(580, 22);
			this.radNamedCells.TabIndex = 20;
			this.radNamedCells.TabStop = true;
			this.radNamedCells.Text = "Display CELLs";
			this.radNamedCells.UseVisualStyleBackColor = true;
			this.radNamedCells.CheckedChanged += new System.EventHandler(this.radNamedCells_CheckedChanged);
			// 
			// radRegions
			// 
			this.radRegions.Location = new System.Drawing.Point(410, 42);
			this.radRegions.Name = "radRegions";
			this.radRegions.Size = new System.Drawing.Size(580, 24);
			this.radRegions.TabIndex = 21;
			this.radRegions.Text = "Display REGIONs";
			this.radRegions.UseVisualStyleBackColor = true;
			// 
			// txtFilter
			// 
			this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilter.Location = new System.Drawing.Point(996, 72);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(256, 20);
			this.txtFilter.TabIndex = 22;
			this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			this.txtFilter.Enter += new System.EventHandler(this.txtFilter_Enter);
			this.txtFilter.Leave += new System.EventHandler(this.txtFilter_Leave);
			// 
			// statusBottom
			// 
			this.statusBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tipMouse,
            this.tipProgress});
			this.statusBottom.Location = new System.Drawing.Point(0, 659);
			this.statusBottom.Name = "statusBottom";
			this.statusBottom.Size = new System.Drawing.Size(1264, 22);
			this.statusBottom.TabIndex = 23;
			this.statusBottom.Text = "statusStrip1";
			// 
			// tipMouse
			// 
			this.tipMouse.Name = "tipMouse";
			this.tipMouse.Size = new System.Drawing.Size(94, 17);
			this.tipMouse.Text = "LMB - Select cell";
			// 
			// tipProgress
			// 
			this.tipProgress.Name = "tipProgress";
			this.tipProgress.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this.tipProgress.Size = new System.Drawing.Size(100, 16);
			this.tipProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.tipProgress.Visible = false;
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(1264, 681);
			this.Controls.Add(this.statusBottom);
			this.Controls.Add(this.txtFilter);
			this.Controls.Add(this.radRegions);
			this.Controls.Add(this.radNamedCells);
			this.Controls.Add(this.lCellCount);
			this.Controls.Add(this.lstCells);
			this.Controls.Add(this.lSeperator);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.bSFlow);
			this.Controls.Add(this.bCellList);
			this.Controls.Add(this.bClearOverlay);
			this.Controls.Add(this.bOverlay);
			this.Controls.Add(this.MapPanel);
			this.Controls.Add(this.bClose);
			this.Controls.Add(this.bBatch);
			this.Controls.Add(this.bOpen);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(1280, 720);
			this.Name = "MainForm";
			this.Text = "TESFaith# Plus";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.bSFlow.ResumeLayout(false);
			this.bSFlow.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.statusBottom.ResumeLayout(false);
			this.statusBottom.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        #endregion

        private string OpenPlugin=null;
        private RuleType2 CurrentRule=RuleType2.Ignore;


        public MainForm() {
            InitializeComponent();
        }

        private void bOpen_Click(object sender,EventArgs e) {
            OpenModDialog.Multiselect=false;
            if(OpenModDialog.ShowDialog()!=DialogResult.OK) return;
            OpenPlugin=OpenModDialog.FileName;
            bOpen.Enabled=false;
            bClose.Enabled=true;
            bBatch.Enabled=true;
            bCellList.Enabled=true;
            bMove.Enabled=true;
            bCopy.Enabled=true;
            bDelete.Enabled=true;
            UpdateCurrentCells();
			var so = MapDrawer.Overlays + " overlay" + (MapDrawer.Overlays > 1 ? "s)" : ")");
			var sa = "TESFaith# Plus (" + Path.GetFileName(OpenPlugin) + " - " + so;
			var sb = "TESFaith# Plus (" + Path.GetFileName(OpenPlugin) + ")";
			Text = MapDrawer.Overlays > 0 ? sa : sb;
        }

        private void UpdateCurrentCells() {
			if (OpenPlugin == null)
			{
				MapDrawer.SetActiveCells(null);
				MapDrawer.SetActiveRegions(null);
			}
			else
			{
				TESFaith.SetOptions(false, true, false, false, false, true);
				TESFaith.Rules = new ArrayList();
				this.tipMouse.Visible = false;
				this.tipProgress.Visible = true;
				TESFaith.RunRules(OpenPlugin, ref this.tipProgress);
				this.tipMouse.Visible = true;
				this.tipProgress.Visible = false;
				MapDrawer.SetActiveCells(TESFaith.CellList);
				MapDrawer.SetActiveRegions(TESFaith.CellList);
			}
        }

        private void bClose_Click(object sender,EventArgs e) {
            OpenPlugin=null;
            bOpen.Enabled=true;
            bClose.Enabled=false;
            bBatch.Enabled=false;
            bCellList.Enabled=false;
            bMove.Enabled=false;
            bCopy.Enabled=false;
            bDelete.Enabled=false;
			lstCells.Items.Clear();
			cellList.Clear();
			txtFilter.Text = "";
            UpdateCurrentCells();
            var so = MapDrawer.Overlays + " overlay" + (MapDrawer.Overlays > 1 ? "s)" : ")");
			var sa = "TESFaith# Plus (" + so;
			var sb = "TESFaith# Plus";
			Text = MapDrawer.Overlays > 0 ? sa : sb;
            CurrentRule=RuleType2.Ignore;
        }

        private void bBatch_Click(object sender,EventArgs e) {
            RulesForm rf=new RulesForm();
            if(rf.ShowDialog()!=DialogResult.OK) return;
            TESFaith.Rules=rf.Rules;
            TESFaith.SetOptions(rf.cbVerboseLog.Checked,false,rf.cbModScripts.Checked,rf.cbNewEsp.Checked,
                rf.cbGenLog.Checked,false);
			this.tipMouse.Visible = false;
			this.tipProgress.Visible = true;
			TESFaith.RunRules(OpenPlugin, ref this.tipProgress);
			this.tipMouse.Visible = true;
			this.tipProgress.Visible = false;
			UpdateCurrentCells();
        }

        private void MainForm_Load(object sender,EventArgs e) {
            MapDrawer.panel=MapPanel;
            MapPanel.SizeChanged+=new EventHandler(MapDrawer.UpdateSize);
            MapDrawer.UpdateSize(null,null);
        }

        private void MapPanel_Paint(object sender,PaintEventArgs e) {
            MapDrawer.PaintMap(e, radNamedCells, radRegions);
        }

        private void bOverlay_Click(object sender,EventArgs e) {
            OpenModDialog.Multiselect=true;
            if(OpenModDialog.ShowDialog()!=DialogResult.OK) return;
            TESFaith.SetOptions(false,true,false,false,false,true);
            foreach(string s in OpenModDialog.FileNames) {
				this.tipMouse.Visible = false;
				this.tipProgress.Visible = true;
				TESFaith.RunRules(s, ref this.tipProgress);
				this.tipMouse.Visible = true;
				this.tipProgress.Visible = false;
				MapDrawer.AddOverlay(TESFaith.CellList,Path.GetFileName(s));
            }
			var so = MapDrawer.Overlays + " overlay" + (MapDrawer.Overlays > 1 ? "s)" : ")");
			var sa = "TESFaith# Plus (" + Path.GetFileName(OpenPlugin) + " - " + so;
			var sb = "TESFaith# Plus (" + so;
			Text = OpenPlugin != null ? sa : sb;
			this.bMap.Enabled = true;
			this.bClearOverlay.Enabled = true;
        }

        private void bClearOverlay_Click(object sender,EventArgs e) {
            MapDrawer.ClearOverlay();
			var sa = "TESFaith# Plus (" + Path.GetFileName(OpenPlugin) + ")";
			var sb = "TESFaith# Plus";
			Text = OpenPlugin != null ? sa : sb;
			this.bMap.Enabled = false;
			this.bClearOverlay.Enabled = false;
        }

        private void bScrollClick(object sender,EventArgs e) {
            if(sender==bSLeft)
                MapDrawer.Scroll(-1,0);
            else if(sender==bSRight)
                MapDrawer.Scroll(1,0);
            else if(sender==bSUp)
                MapDrawer.Scroll(0,1);
            else if(sender==bSDown)
                MapDrawer.Scroll(0,-1);
        }
        
        private void MapPanel_MouseMove(object sender,MouseEventArgs e) {
            string s;
            MapDrawer.GetMousePos(e.X,e.Y,out s);
            if(s!=null) {
                //ttCellData.Show(s,MapPanel,e.X+16,e.Y+16,5000);
                ttCellData.SetToolTip(MapPanel,s);
            }
			tipMouse.Visible = true;
			switch (CurrentRule)
			{
				case RuleType2.Transpose:
					tipMouse.Text = "LMB - Move cell | RMB - Unselect cell";
					break;
				case RuleType2.Copy:
					tipMouse.Text = "LMB - Copy cell | RMB - Unselect cell";
					break;
				case RuleType2.Ignore:
					tipMouse.Text = "LMB - Select cell";
					break;
			}
        }

        private void MapPanel_MouseLeave(object sender,EventArgs e) {
            //ttCellData.Hide(MapPanel);
            ttCellData.RemoveAll();
			tipMouse.Visible = false;
		}

        private void MapPanel_MouseDown(object sender,MouseEventArgs e) {
            if(OpenPlugin==null) return;
            if((e.Button&MouseButtons.Right)!=0) {
				switch (CurrentRule)
				{
					case RuleType2.Ignore:
						MapDrawer.Selected = false;
						MapDrawer.SelectedX = 0;
						MapDrawer.SelectedY = 0;
						break;
					default:
						CurrentRule = RuleType2.Ignore;
						bMove.BackColor = System.Drawing.SystemColors.Control;
						bCopy.BackColor = System.Drawing.SystemColors.Control;
						bDelete.BackColor = System.Drawing.SystemColors.Control;
						break;
				}
            } else if((e.Button&MouseButtons.Left)!=0) {
                int xpos,ypos;
                Rule r;
                MapDrawer.GetCellPos(e.X,e.Y,out xpos,out ypos);
                switch(CurrentRule) {
                    case RuleType2.Move:
                        r=new Rule();
                        r.data.OldX=MapDrawer.SelectedX;
                        r.data.OldY=MapDrawer.SelectedY;
                        r.data.NewX=xpos;
                        r.data.NewY=ypos;
                        r.type2=RuleType2.Move;
                        r.type=RuleType.Cell;
                        TESFaith.Rules=new ArrayList();
                        TESFaith.Rules.Add(r);
                        MapDrawer.Selected=false;
                        TESFaith.SetOptions(false,false,true,false,false,false);
						this.tipMouse.Visible = false;
						this.tipProgress.Visible = true;
						TESFaith.RunRules(OpenPlugin, ref this.tipProgress);
						this.tipMouse.Visible = true;
						this.tipProgress.Visible = false;
						UpdateCurrentCells();
                        break;
                    case RuleType2.Copy:
                        r=new Rule();
                        r.data.OldX=MapDrawer.SelectedX;
                        r.data.OldY=MapDrawer.SelectedY;
                        r.data.NewX=xpos;
                        r.data.NewY=ypos;
                        r.type2=RuleType2.Move;
                        r.type=RuleType.Cell;
                        TESFaith.Rules=new ArrayList();
                        TESFaith.Rules.Add(r);
                        r=new Rule();
                        r.data.OldX=MapDrawer.SelectedX;
                        r.data.OldY=MapDrawer.SelectedY;
                        r.type2=RuleType2.Copy;
                        r.type=RuleType.Cell;
                        TESFaith.Rules.Add(r);
                        MapDrawer.Selected=false;
                        TESFaith.SetOptions(false,false,true,false,false,false);
						this.tipMouse.Visible = false;
						this.tipProgress.Visible = true;
						TESFaith.RunRules(OpenPlugin, ref this.tipProgress);
						this.tipMouse.Visible = true;
						this.tipProgress.Visible = false;
						UpdateCurrentCells();
                        break;
                    case RuleType2.Ignore:
                        if(MapDrawer.CellExists(xpos,ypos)) {
                            MapDrawer.SelectedX=xpos;
                            MapDrawer.SelectedY=ypos;
                            MapDrawer.Selected=true;
							this.tipMouse.Text = "LMB - Select cell | RMB - Unselect cell";
						}
						break;
                }
                CurrentRule=RuleType2.Ignore;
                bMove.BackColor=System.Drawing.SystemColors.Control;
                bCopy.BackColor=System.Drawing.SystemColors.Control;
                bDelete.BackColor=System.Drawing.SystemColors.Control;
            }
        }

        private void bCellList_Click(object sender,EventArgs e) {
            TESFaith.SetOptions(false,true,false,false,false,false);
			this.tipMouse.Visible = false;
			this.tipProgress.Visible = true;
			if (File.Exists("CellDump.txt")) File.Delete("CellDump.txt");
			TESFaith.RunRules(OpenPlugin, ref this.tipProgress);
			this.tipMouse.Visible = true;
			this.tipProgress.Visible = false;

			FileStream fs = File.Open("CellDump.txt", FileMode.Open);
			StreamReader sr = new StreamReader(fs);
			this.cellList.Clear();
			this.lstCells.Items.Clear();
			this.txtFilter.Text = "";
			int cellCount = 0;
			HashSet<string> cellList = new HashSet<string>();
			HashSet<string> regionList = new HashSet<string>();
			while (!sr.EndOfStream)
			{
				var s = sr.ReadLine();
				if (s.Length == 0) continue;
				cellCount++;
				cellList.Add(s.Split('\t')[1]);
				regionList.Add(s.Split('\t')[2]);
				this.cellList.Add(s);
				this.lstCells.Items.Add(s);
			}
			this.lCellCount.Text = string.Format("{0:n0} CELLs\r\n{1:n0} named CELLs\r\n{2:n0} REGIONs\r\n", cellCount, cellList.Count, regionList.Count);
			sr.Close();
			fs.Close();
		}

        private void bMove_Click(object sender,EventArgs e) {
            if(!MapDrawer.Selected) {
                MessageBox.Show("Select a CELL to move first.", "Oops!");
                return;
            }
			if (CurrentRule == RuleType2.Move)
			{
				CurrentRule = RuleType2.Ignore;
				bMove.BackColor = System.Drawing.SystemColors.Control;
			}
			else
			{
				CurrentRule = RuleType2.Move;
				bMove.BackColor = System.Drawing.Color.LimeGreen;
				bCopy.BackColor = System.Drawing.SystemColors.Control;
				bDelete.BackColor = System.Drawing.SystemColors.Control;
			}
        }

        private void bCopy_Click(object sender,EventArgs e) {
            if(!MapDrawer.Selected) {
                MessageBox.Show("Select a CELL to copy first.", "Oops!");
                return;
            }
			if (CurrentRule == RuleType2.Copy)
			{
				CurrentRule = RuleType2.Ignore;
				bCopy.BackColor = System.Drawing.SystemColors.Control;
			}
			else
			{
				CurrentRule = RuleType2.Copy;
				bMove.BackColor = System.Drawing.SystemColors.Control;
				bCopy.BackColor = System.Drawing.Color.LimeGreen;
				bDelete.BackColor = System.Drawing.SystemColors.Control;
			}
        }

        private void bDelete_Click(object sender,EventArgs e) {
            if(!MapDrawer.Selected) {
                MessageBox.Show("Select a CELL to delete first.", "Oops!");
                return;
            }
			var result = MessageBox.Show("Are you sure you want to delete the selected CELL?","Delete CELL", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				Rule r = new Rule();
				r.data.OldX = MapDrawer.SelectedX;
				r.data.OldY = MapDrawer.SelectedY;
				r.type2 = RuleType2.Delete;
				r.type = RuleType.Cell;
				MapDrawer.Selected = false;
				TESFaith.Rules = new ArrayList();
				TESFaith.Rules.Add(r);
				TESFaith.SetOptions(false, false, false, false, false, false);
				this.tipMouse.Visible = false;
				this.tipProgress.Visible = true;
				TESFaith.RunRules(OpenPlugin, ref this.tipProgress);
				this.tipMouse.Visible = true;
				this.tipProgress.Visible = false;
				CurrentRule = RuleType2.Ignore;
				bMove.BackColor = System.Drawing.SystemColors.Control;
				bCopy.BackColor = System.Drawing.SystemColors.Control;
				bDelete.BackColor = System.Drawing.SystemColors.Control;
				UpdateCurrentCells();
			}
        }

        private void bMap_Click(object sender, EventArgs e) {
            if(SaveMapDialog.ShowDialog() == DialogResult.OK) {
                MapDrawer.SaveMap(SaveMapDialog.FileName,SaveMapDialog.FilterIndex, Path.GetFileName(OpenPlugin));
            }
        }

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				switch (e.KeyCode)
				{
					case Keys.S:
						bMap_Click(this.bMap, null);
						break;
					case Keys.M:
						bMove_Click(this.bMove, null);
						break;
					case Keys.C:
						bCopy_Click(this.bCopy, null);
						break;
					case Keys.L:
						bCellList_Click(this.bBatch, null);
						break;
					case Keys.O:
						bOpen_Click(this.bOpen, null);
						break;
					case Keys.W:
						bClose_Click(this.bClose, null);
						break;
					case Keys.T:
						bOverlay_Click(this.bOverlay, null);
						break;
					case Keys.R:
						bClearOverlay_Click(this.bClearOverlay, null);
						break;
					case Keys.B:
						bBatch_Click(this.bBatch, null);
						break;
					case Keys.F:
						this.txtFilter.Focus();
						break;
					case Keys.Tab:
						radNamedCells.Checked = !radNamedCells.Checked;
						radRegions.Checked = !radRegions.Checked;
						break;
					case Keys.Left:
						if (MapDrawer.Selected) MapDrawer.SelectedX--;
						break;
					case Keys.Right:
						if (MapDrawer.Selected) MapDrawer.SelectedX++;
						break;
					case Keys.Up:
						if (MapDrawer.Selected) MapDrawer.SelectedY--;
						break;
					case Keys.Down:
						if (MapDrawer.Selected) MapDrawer.SelectedY++;
						break;
				}
			}
			else
			{
				switch (e.KeyCode)
				{
					case Keys.A:
						bScrollClick(this.bSLeft, null);
						break;
					case Keys.D:
						bScrollClick(this.bSRight, null);
						break;
					case Keys.W:
						bScrollClick(this.bSUp, null);
						break;
					case Keys.S:
						bScrollClick(this.bSDown, null);
						break;
					case Keys.Delete:
						bDelete_Click(this.bDelete, null);
						break;
				}
			}
		}

		private void txtFilter_TextChanged(object sender, EventArgs e)
		{
			if (txtFilter.Text.Length > 2 && cellList != null)
			{
				this.lstCells.Items.Clear();
				foreach (string cell in cellList)
				{
					if (cell.ToUpper().Contains(this.txtFilter.Text.ToUpper().Trim()))
					{
						this.lstCells.Items.Add(cell);
					}
				}
			}
			else if (txtFilter.Text.Length == 0)
			{
				foreach (string cell in cellList)
				{
					this.lstCells.Items.Add(cell);
				}
			}
		}

		private void txtFilter_Enter(object sender, EventArgs e)
		{
			this.KeyPreview = false;
		}

		private void txtFilter_Leave(object sender, EventArgs e)
		{
			this.KeyPreview = true;
		}

		private void radNamedCells_CheckedChanged(object sender, EventArgs e)
		{
			this.MapPanel.Invalidate();
		}
	}
}
