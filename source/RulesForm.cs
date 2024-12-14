using System;
using System.Windows.Forms;
using ArrayList=System.Collections.ArrayList;

namespace TESFaith {
    public class RulesForm : Form {
        private Button bRun;
        private Button bCancel;
        private ComboBox cmbType;
        private ContextMenu DudMenu;
        private RadioButton rbAll;
        private RadioButton rbRegion;
        private RadioButton rbCell;
        private TextBox tbCellName;
        private TextBox tbNewY;
        private TextBox tbNewName;
        private TextBox tbNewX;
        private TextBox tbOldX;
        private TextBox tbOldY;
        private Button bNext;
        private Button bPrevious;
        private Button bNew;
        private Button bDelete;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        public CheckBox cbGenLog;
        public CheckBox cbVerboseLog;
        public CheckBox cbModScripts;
        public CheckBox cbNewEsp;
		private GroupBox grpMisc;
		private TextBox tbRegionName;
		private Label label5;
		private GroupBox grpRule;
		private GroupBox grpDetails;
		private Label lblSeperator;
		#region FormDesigner
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components=null;

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
			this.bRun = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.cmbType = new System.Windows.Forms.ComboBox();
			this.DudMenu = new System.Windows.Forms.ContextMenu();
			this.rbAll = new System.Windows.Forms.RadioButton();
			this.rbRegion = new System.Windows.Forms.RadioButton();
			this.rbCell = new System.Windows.Forms.RadioButton();
			this.tbCellName = new System.Windows.Forms.TextBox();
			this.tbNewY = new System.Windows.Forms.TextBox();
			this.tbNewName = new System.Windows.Forms.TextBox();
			this.tbNewX = new System.Windows.Forms.TextBox();
			this.tbOldX = new System.Windows.Forms.TextBox();
			this.tbOldY = new System.Windows.Forms.TextBox();
			this.bNext = new System.Windows.Forms.Button();
			this.bPrevious = new System.Windows.Forms.Button();
			this.bNew = new System.Windows.Forms.Button();
			this.bDelete = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbGenLog = new System.Windows.Forms.CheckBox();
			this.cbVerboseLog = new System.Windows.Forms.CheckBox();
			this.cbModScripts = new System.Windows.Forms.CheckBox();
			this.cbNewEsp = new System.Windows.Forms.CheckBox();
			this.grpMisc = new System.Windows.Forms.GroupBox();
			this.tbRegionName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.grpRule = new System.Windows.Forms.GroupBox();
			this.grpDetails = new System.Windows.Forms.GroupBox();
			this.lblSeperator = new System.Windows.Forms.Label();
			this.grpMisc.SuspendLayout();
			this.grpRule.SuspendLayout();
			this.grpDetails.SuspendLayout();
			this.SuspendLayout();
			// 
			// bRun
			// 
			this.bRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bRun.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bRun.Location = new System.Drawing.Point(198, 373);
			this.bRun.Name = "bRun";
			this.bRun.Size = new System.Drawing.Size(64, 24);
			this.bRun.TabIndex = 1;
			this.bRun.Text = "Run";
			// 
			// bCancel
			// 
			this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(268, 373);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(64, 24);
			this.bCancel.TabIndex = 2;
			this.bCancel.Text = "Cancel";
			// 
			// cmbType
			// 
			this.cmbType.ContextMenu = this.DudMenu;
			this.cmbType.Items.AddRange(new object[] {
            "Move",
            "Shift",
            "Copy",
            "Delete",
            "Ignore",
            "Interior Copy",
            "Interior Delete"});
			this.cmbType.Location = new System.Drawing.Point(6, 49);
			this.cmbType.Name = "cmbType";
			this.cmbType.Size = new System.Drawing.Size(119, 21);
			this.cmbType.TabIndex = 3;
			this.cmbType.Text = "Move";
			this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
			this.cmbType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IgnoreKeypress);
			// 
			// rbAll
			// 
			this.rbAll.AutoSize = true;
			this.rbAll.Checked = true;
			this.rbAll.Location = new System.Drawing.Point(131, 50);
			this.rbAll.Name = "rbAll";
			this.rbAll.Size = new System.Drawing.Size(36, 17);
			this.rbAll.TabIndex = 5;
			this.rbAll.TabStop = true;
			this.rbAll.Text = "All";
			this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
			// 
			// rbRegion
			// 
			this.rbRegion.AutoSize = true;
			this.rbRegion.Location = new System.Drawing.Point(221, 50);
			this.rbRegion.Name = "rbRegion";
			this.rbRegion.Size = new System.Drawing.Size(59, 17);
			this.rbRegion.TabIndex = 7;
			this.rbRegion.Text = "Region";
			this.rbRegion.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
			// 
			// rbCell
			// 
			this.rbCell.AutoSize = true;
			this.rbCell.Location = new System.Drawing.Point(173, 50);
			this.rbCell.Name = "rbCell";
			this.rbCell.Size = new System.Drawing.Size(42, 17);
			this.rbCell.TabIndex = 6;
			this.rbCell.Text = "Cell";
			this.rbCell.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
			// 
			// tbCellName
			// 
			this.tbCellName.Location = new System.Drawing.Point(6, 19);
			this.tbCellName.MaxLength = 128;
			this.tbCellName.Name = "tbCellName";
			this.tbCellName.Size = new System.Drawing.Size(192, 20);
			this.tbCellName.TabIndex = 8;
			// 
			// tbNewY
			// 
			this.tbNewY.Location = new System.Drawing.Point(105, 97);
			this.tbNewY.MaxLength = 4;
			this.tbNewY.Name = "tbNewY";
			this.tbNewY.Size = new System.Drawing.Size(93, 20);
			this.tbNewY.TabIndex = 9;
			this.tbNewY.Text = "0";
			this.tbNewY.TextChanged += new System.EventHandler(this.tbNewY_TextChanged);
			this.tbNewY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitsOnly);
			// 
			// tbNewName
			// 
			this.tbNewName.Location = new System.Drawing.Point(6, 125);
			this.tbNewName.MaxLength = 32;
			this.tbNewName.Name = "tbNewName";
			this.tbNewName.Size = new System.Drawing.Size(192, 20);
			this.tbNewName.TabIndex = 10;
			// 
			// tbNewX
			// 
			this.tbNewX.Location = new System.Drawing.Point(6, 97);
			this.tbNewX.MaxLength = 4;
			this.tbNewX.Name = "tbNewX";
			this.tbNewX.Size = new System.Drawing.Size(93, 20);
			this.tbNewX.TabIndex = 11;
			this.tbNewX.Text = "0";
			this.tbNewX.TextChanged += new System.EventHandler(this.tbNewX_TextChanged);
			this.tbNewX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitsOnly);
			// 
			// tbOldX
			// 
			this.tbOldX.Location = new System.Drawing.Point(6, 45);
			this.tbOldX.MaxLength = 4;
			this.tbOldX.Name = "tbOldX";
			this.tbOldX.Size = new System.Drawing.Size(93, 20);
			this.tbOldX.TabIndex = 12;
			this.tbOldX.Text = "0";
			this.tbOldX.TextChanged += new System.EventHandler(this.tbOldX_TextChanged);
			this.tbOldX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitsOnly);
			// 
			// tbOldY
			// 
			this.tbOldY.Location = new System.Drawing.Point(105, 45);
			this.tbOldY.MaxLength = 4;
			this.tbOldY.Name = "tbOldY";
			this.tbOldY.Size = new System.Drawing.Size(93, 20);
			this.tbOldY.TabIndex = 13;
			this.tbOldY.Text = "0";
			this.tbOldY.TextChanged += new System.EventHandler(this.tbOldY_TextChanged);
			this.tbOldY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DigitsOnly);
			// 
			// bNext
			// 
			this.bNext.Enabled = false;
			this.bNext.Location = new System.Drawing.Point(243, 19);
			this.bNext.Name = "bNext";
			this.bNext.Size = new System.Drawing.Size(73, 24);
			this.bNext.TabIndex = 17;
			this.bNext.Text = "Next";
			this.bNext.Click += new System.EventHandler(this.bNext_Click);
			// 
			// bPrevious
			// 
			this.bPrevious.Enabled = false;
			this.bPrevious.Location = new System.Drawing.Point(164, 19);
			this.bPrevious.Name = "bPrevious";
			this.bPrevious.Size = new System.Drawing.Size(73, 24);
			this.bPrevious.TabIndex = 16;
			this.bPrevious.Text = "Previous";
			this.bPrevious.Click += new System.EventHandler(this.bPrevious_Click);
			// 
			// bNew
			// 
			this.bNew.Location = new System.Drawing.Point(6, 19);
			this.bNew.Name = "bNew";
			this.bNew.Size = new System.Drawing.Size(73, 24);
			this.bNew.TabIndex = 14;
			this.bNew.Text = "New Rule";
			this.bNew.Click += new System.EventHandler(this.bNew_Click);
			// 
			// bDelete
			// 
			this.bDelete.Enabled = false;
			this.bDelete.Location = new System.Drawing.Point(85, 19);
			this.bDelete.Name = "bDelete";
			this.bDelete.Size = new System.Drawing.Size(73, 24);
			this.bDelete.TabIndex = 15;
			this.bDelete.Text = "Delete Rule";
			this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(204, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 18;
			this.label1.Text = "CELL name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(204, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 13);
			this.label2.TabIndex = 19;
			this.label2.Text = "New CELL name";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(204, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(94, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "CELL co-ordinates";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(204, 100);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(90, 13);
			this.label4.TabIndex = 21;
			this.label4.Text = "New co-ordinates";
			// 
			// cbGenLog
			// 
			this.cbGenLog.AutoSize = true;
			this.cbGenLog.Location = new System.Drawing.Point(6, 19);
			this.cbGenLog.Name = "cbGenLog";
			this.cbGenLog.Size = new System.Drawing.Size(87, 17);
			this.cbGenLog.TabIndex = 22;
			this.cbGenLog.Text = "Generate log";
			// 
			// cbVerboseLog
			// 
			this.cbVerboseLog.AutoSize = true;
			this.cbVerboseLog.Location = new System.Drawing.Point(102, 19);
			this.cbVerboseLog.Name = "cbVerboseLog";
			this.cbVerboseLog.Size = new System.Drawing.Size(82, 17);
			this.cbVerboseLog.TabIndex = 23;
			this.cbVerboseLog.Text = "Verbose log";
			// 
			// cbModScripts
			// 
			this.cbModScripts.AutoSize = true;
			this.cbModScripts.Location = new System.Drawing.Point(6, 42);
			this.cbModScripts.Name = "cbModScripts";
			this.cbModScripts.Size = new System.Drawing.Size(90, 17);
			this.cbModScripts.TabIndex = 24;
			this.cbModScripts.Text = "Modify scripts";
			// 
			// cbNewEsp
			// 
			this.cbNewEsp.AutoSize = true;
			this.cbNewEsp.Location = new System.Drawing.Point(102, 42);
			this.cbNewEsp.Name = "cbNewEsp";
			this.cbNewEsp.Size = new System.Drawing.Size(104, 17);
			this.cbNewEsp.TabIndex = 25;
			this.cbNewEsp.Text = "Create new ESP";
			// 
			// grpMisc
			// 
			this.grpMisc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpMisc.AutoSize = true;
			this.grpMisc.Controls.Add(this.cbGenLog);
			this.grpMisc.Controls.Add(this.cbNewEsp);
			this.grpMisc.Controls.Add(this.cbModScripts);
			this.grpMisc.Controls.Add(this.cbVerboseLog);
			this.grpMisc.Location = new System.Drawing.Point(12, 277);
			this.grpMisc.Name = "grpMisc";
			this.grpMisc.Size = new System.Drawing.Size(320, 90);
			this.grpMisc.TabIndex = 26;
			this.grpMisc.TabStop = false;
			// 
			// tbRegionName
			// 
			this.tbRegionName.Location = new System.Drawing.Point(6, 71);
			this.tbRegionName.MaxLength = 128;
			this.tbRegionName.Name = "tbRegionName";
			this.tbRegionName.Size = new System.Drawing.Size(192, 20);
			this.tbRegionName.TabIndex = 27;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(204, 74);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(78, 13);
			this.label5.TabIndex = 28;
			this.label5.Text = "REGION name";
			// 
			// grpRule
			// 
			this.grpRule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpRule.AutoSize = true;
			this.grpRule.Controls.Add(this.bNext);
			this.grpRule.Controls.Add(this.bPrevious);
			this.grpRule.Controls.Add(this.bNew);
			this.grpRule.Controls.Add(this.bDelete);
			this.grpRule.Controls.Add(this.cmbType);
			this.grpRule.Controls.Add(this.rbAll);
			this.grpRule.Controls.Add(this.rbRegion);
			this.grpRule.Controls.Add(this.rbCell);
			this.grpRule.Location = new System.Drawing.Point(12, 12);
			this.grpRule.Name = "grpRule";
			this.grpRule.Size = new System.Drawing.Size(323, 89);
			this.grpRule.TabIndex = 29;
			this.grpRule.TabStop = false;
			// 
			// grpDetails
			// 
			this.grpDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpDetails.AutoSize = true;
			this.grpDetails.Controls.Add(this.lblSeperator);
			this.grpDetails.Controls.Add(this.tbCellName);
			this.grpDetails.Controls.Add(this.label1);
			this.grpDetails.Controls.Add(this.label2);
			this.grpDetails.Controls.Add(this.label5);
			this.grpDetails.Controls.Add(this.label4);
			this.grpDetails.Controls.Add(this.tbNewName);
			this.grpDetails.Controls.Add(this.tbRegionName);
			this.grpDetails.Controls.Add(this.label3);
			this.grpDetails.Controls.Add(this.tbNewX);
			this.grpDetails.Controls.Add(this.tbNewY);
			this.grpDetails.Controls.Add(this.tbOldX);
			this.grpDetails.Controls.Add(this.tbOldY);
			this.grpDetails.Location = new System.Drawing.Point(12, 107);
			this.grpDetails.Name = "grpDetails";
			this.grpDetails.Size = new System.Drawing.Size(320, 164);
			this.grpDetails.TabIndex = 30;
			this.grpDetails.TabStop = false;
			// 
			// lblSeperator
			// 
			this.lblSeperator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblSeperator.Location = new System.Drawing.Point(6, 120);
			this.lblSeperator.Name = "lblSeperator";
			this.lblSeperator.Size = new System.Drawing.Size(292, 2);
			this.lblSeperator.TabIndex = 31;
			// 
			// RulesForm
			// 
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(344, 409);
			this.Controls.Add(this.grpDetails);
			this.Controls.Add(this.grpRule);
			this.Controls.Add(this.grpMisc);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bRun);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RulesForm";
			this.Text = "Create rules (1 of 1)";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RulesForm_FormClosing);
			this.Load += new System.EventHandler(this.RulesForm_Load);
			this.grpMisc.ResumeLayout(false);
			this.grpMisc.PerformLayout();
			this.grpRule.ResumeLayout(false);
			this.grpRule.PerformLayout();
			this.grpDetails.ResumeLayout(false);
			this.grpDetails.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        public RulesForm() {
            InitializeComponent();
        }
        #endregion

        public ArrayList Rules=new ArrayList();
        private Rule CurrentRule;
        private int CurrentRuleIndex=0;

        private void CommitChanges() {
            CurrentRule.cellName=tbCellName.Text;
			CurrentRule.regionName=tbRegionName.Text;
            CurrentRule.newName=tbNewName.Text;
            CurrentRule.type2=(RuleType2)cmbType.SelectedIndex;
            if(rbAll.Checked) CurrentRule.type=RuleType.All;
            else if(rbRegion.Checked) CurrentRule.type=RuleType.Region;
            else CurrentRule.type=RuleType.Cell;
            CurrentRule.data.OldX=Convert.ToInt32(tbOldX.Text);
            CurrentRule.data.OldY=Convert.ToInt32(tbOldY.Text);
            CurrentRule.data.NewX=Convert.ToInt32(tbNewX.Text);
            CurrentRule.data.NewY=Convert.ToInt32(tbNewY.Text);
            Rules[CurrentRuleIndex]=CurrentRule;
        }

        private void UpdateTitle() {
            Text="Create Rules ("+(CurrentRuleIndex+1).ToString()+" of "+Rules.Count.ToString()+")";
        }

        private void ResetControls() {
            CurrentRule=new Rule();
            CurrentRule.cellName="";
			CurrentRule.regionName="";
            CurrentRule.newName="";
			tbCellName.Text="";
			tbRegionName.Text="";
			tbOldX.Text = "0";
			tbOldY.Text = "0";
			tbNewName.Text = "";
			tbNewX.Text = "0";
			tbNewY.Text = "0";
            tbNewName.Enabled=false;
            tbNewX.Enabled=true;
            cmbType.SelectedIndex=0;
            cmbType_SelectedIndexChanged(null,null);
            UpdateTitle();
        }

        private void SelectRule(int index) {
            if(CurrentRuleIndex>=0) CommitChanges();
            CurrentRule=(Rule)Rules[index];
            CurrentRuleIndex=index;
            cmbType.SelectedIndex=(int)CurrentRule.type2;
            if(CurrentRule.type==RuleType.All) rbAll.Checked=true;
            else if(CurrentRule.type==RuleType.Region) rbRegion.Checked=true;
            else rbCell.Checked=true;
            if(index==0) bPrevious.Enabled=false;
            else bPrevious.Enabled=true;
            if(index==Rules.Count-1) bNext.Enabled=false;
            else bNext.Enabled=true;
            if(Rules.Count>1) bDelete.Enabled=true;
            else bDelete.Enabled=false;
            UpdateTitle();
        }

        private void IgnoreKeypress(object sender,KeyPressEventArgs e) {
            e.Handled=true;
        }

        private void DigitsOnly(object sender,KeyPressEventArgs e) {
            if(!char.IsDigit(e.KeyChar)&&e.KeyChar!='-'&&e.KeyChar!='\b') e.Handled=true;
        }

        private void cmbType_SelectedIndexChanged(object sender,EventArgs e) {
            CurrentRule.type2=(RuleType2)cmbType.SelectedIndex;
            switch(cmbType.SelectedIndex) {
                case 0:
                    tbNewName.Enabled=false;
                    tbNewX.Enabled=true;
                    tbNewY.Enabled=true;
                    break;
                case 1:
                    tbNewName.Enabled=false;
                    tbNewX.Enabled=true;
                    tbNewY.Enabled=true;
                    break;
                case 2:
                    tbNewName.Enabled=false;
                    tbNewX.Enabled=false;
                    tbNewY.Enabled=false;
                    break;
                case 3:
                    tbNewName.Enabled=false;
                    tbNewX.Enabled=false;
                    tbNewY.Enabled=false;
                    break;
                case 4:
                    tbNewName.Enabled=false;
                    tbNewX.Enabled=false;
                    tbNewY.Enabled=false;
                    break;
                case 5:
                    tbCellName.Enabled=true;
                    tbNewName.Enabled=true;
                    tbNewX.Enabled=false;
                    tbNewY.Enabled=false;
                    tbOldX.Enabled=false;
                    tbOldY.Enabled=false;
                    break;
                case 6:
                    tbCellName.Enabled=true;
                    tbNewName.Enabled=false;
                    tbNewX.Enabled=false;
                    tbNewY.Enabled=false;
                    tbOldX.Enabled=false;
                    tbOldY.Enabled=false;
                    break;
            }
            if(cmbType.SelectedIndex<5) rbAll_CheckedChanged(null,null);
        }

        private void RulesForm_Load(object sender,EventArgs e) {
            Rules.Add(new Rule());
            ResetControls();
        }

        private void rbAll_CheckedChanged(object sender,EventArgs e) {
            if(rbAll.Checked) {
                tbCellName.Enabled=false;
				tbRegionName.Enabled=false;
                tbOldX.Enabled=false;
                tbOldY.Enabled=false;
            } else if(rbRegion.Checked) {
                tbCellName.Enabled=false;
				tbRegionName.Enabled=true;
                tbOldX.Enabled=false;
                tbOldY.Enabled=false;
            } else if(rbCell.Checked) {
                tbCellName.Enabled=true;
				tbRegionName.Enabled=false;
                tbOldX.Enabled=true;
                tbOldY.Enabled=true;
            }
        }

        private void bNew_Click(object sender,EventArgs e) {
            CommitChanges();
            Rules.Add(new Rule());
            CurrentRuleIndex=Rules.Count-1;
            ResetControls();
            bPrevious.Enabled=true;
            bNext.Enabled=false;
            bDelete.Enabled=true;
        }

        private void bDelete_Click(object sender,EventArgs e) {
            Rules.RemoveAt(CurrentRuleIndex);
            CurrentRuleIndex=-1;
            SelectRule(0);
        }

        private void bNext_Click(object sender,EventArgs e) {
            SelectRule(CurrentRuleIndex+1);
        }

        private void bPrevious_Click(object sender,EventArgs e) {
            SelectRule(CurrentRuleIndex-1);
        }

        private void tbOldX_TextChanged(object sender,EventArgs e) {
            try {
                Convert.ToInt32(tbOldX.Text);
            } catch { tbOldX.Text="0"; }
        }

        private void tbOldY_TextChanged(object sender,EventArgs e) {
            try {
                Convert.ToInt32(tbOldY.Text);
            } catch { tbOldY.Text="0"; }
        }

        private void tbNewX_TextChanged(object sender,EventArgs e) {
            try {
                Convert.ToInt32(tbNewX.Text);
            } catch { tbNewX.Text="0"; }
        }

        private void tbNewY_TextChanged(object sender,EventArgs e) {
            try {
                Convert.ToInt32(tbNewY.Text);
            } catch { tbNewY.Text="0"; }
        }

        private void RulesForm_FormClosing(object sender,System.ComponentModel.CancelEventArgs e) {
            CommitChanges();
        }

    }
}
