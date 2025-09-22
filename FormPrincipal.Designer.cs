namespace MiniSOVisual {
    partial class FormPrincipal {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Controles gerados
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsBtnRunStep;
        private System.Windows.Forms.ToolStripButton tsBtnRunAll;
        private System.Windows.Forms.ToolStripButton tsBtnCreateProc;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslblRAM;
        private System.Windows.Forms.ToolStripStatusLabel tslblClock;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.GroupBox groupBoxSystem;
        private System.Windows.Forms.Label labelRAM;
        private System.Windows.Forms.NumericUpDown nudRAM;
        private System.Windows.Forms.Label labelQuantum;
        private System.Windows.Forms.NumericUpDown nudQuantum;
        private System.Windows.Forms.Label labelAlg;
        private System.Windows.Forms.ComboBox cbAlgorithm;
        private System.Windows.Forms.Button btnApplySettings;

        private System.Windows.Forms.GroupBox groupBoxActions;
        private System.Windows.Forms.Button btnCreateProcess;
        private System.Windows.Forms.NumericUpDown nudCreateCount;
        private System.Windows.Forms.Label labelCreateCount;
        private System.Windows.Forms.Button btnClearLogs;

        private System.Windows.Forms.GroupBox groupBoxResources;
        private System.Windows.Forms.ListBox lstResources;

        private System.Windows.Forms.TabControl tabControlCenter;
        private System.Windows.Forms.TabPage tabProntos;
        private System.Windows.Forms.TabPage tabBloqueados;
        private System.Windows.Forms.TabPage tabFinalizados;

        private System.Windows.Forms.DataGridView dgvProntos;
        private System.Windows.Forms.DataGridView dgvBloqueados;
        private System.Windows.Forms.DataGridView dgvFinalizados;

        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Button btnSendCommand;

        private System.Windows.Forms.GroupBox groupBoxMetrics;
        private System.Windows.Forms.Label lblTotalProcesses;
        private System.Windows.Forms.Label lblContextSwitches;
        private System.Windows.Forms.Label lblAvgTurnaround;
        private System.Windows.Forms.Label lblAvgWaiting;
        private System.Windows.Forms.Label lblAvgCpuCycles;
        private System.Windows.Forms.Label lblThroughput;

        private System.Windows.Forms.Label lblTotalProcessesValue;
        private System.Windows.Forms.Label lblContextSwitchesValue;
        private System.Windows.Forms.Label lblAvgTurnaroundValue;
        private System.Windows.Forms.Label lblAvgWaitingValue;
        private System.Windows.Forms.Label lblAvgCpuCyclesValue;
        private System.Windows.Forms.Label lblThroughputValue;

        #endregion

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            menuStrip1 = new MenuStrip();
            arquivoToolStripMenuItem = new ToolStripMenuItem();
            sairToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            tsBtnRunStep = new ToolStripButton();
            tsBtnRunAll = new ToolStripButton();
            tsBtnCreateProc = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            tslblRAM = new ToolStripStatusLabel();
            tslblClock = new ToolStripStatusLabel();
            tableLayoutPanelMain = new TableLayoutPanel();
            panelLeft = new Panel();
            groupBoxResources = new GroupBox();
            lstResources = new ListBox();
            groupBoxActions = new GroupBox();
            labelCreateCount = new Label();
            nudCreateCount = new NumericUpDown();
            btnCreateProcess = new Button();
            btnClearLogs = new Button();
            groupBoxSystem = new GroupBox();
            labelRAM = new Label();
            nudRAM = new NumericUpDown();
            labelQuantum = new Label();
            nudQuantum = new NumericUpDown();
            labelAlg = new Label();
            cbAlgorithm = new ComboBox();
            btnApplySettings = new Button();
            rightContainer = new SplitContainer();
            tabControlCenter = new TabControl();
            tabProntos = new TabPage();
            dgvProntos = new DataGridView();
            tabBloqueados = new TabPage();
            dgvBloqueados = new DataGridView();
            tabFinalizados = new TabPage();
            dgvFinalizados = new DataGridView();
            txtLog = new TextBox();
            txtCommand = new TextBox();
            btnSendCommand = new Button();
            panelRight = new Panel();


            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            tableLayoutPanelMain.SuspendLayout();
            panelLeft.SuspendLayout();
            groupBoxResources.SuspendLayout();
            groupBoxActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCreateCount).BeginInit();
            groupBoxSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudRAM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQuantum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rightContainer).BeginInit();
            rightContainer.Panel1.SuspendLayout();
            rightContainer.Panel2.SuspendLayout();
            rightContainer.SuspendLayout();
            tabControlCenter.SuspendLayout();
            tabProntos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProntos).BeginInit();
            tabBloqueados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBloqueados).BeginInit();
            tabFinalizados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFinalizados).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { arquivoToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1000, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            arquivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { sairToolStripMenuItem });
            arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            arquivoToolStripMenuItem.Size = new Size(61, 20);
            arquivoToolStripMenuItem.Text = "Arquivo";
            // 
            // sairToolStripMenuItem
            // 
            sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            sairToolStripMenuItem.Size = new Size(93, 22);
            sairToolStripMenuItem.Text = "Sair";
            sairToolStripMenuItem.Click += sairToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsBtnRunStep, tsBtnRunAll, tsBtnCreateProc });
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1000, 25);
            toolStrip1.TabIndex = 1;
            // 
            // tsBtnRunStep
            // 
            tsBtnRunStep.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnRunStep.Name = "tsBtnRunStep";
            tsBtnRunStep.Size = new Size(34, 22);
            tsBtnRunStep.Text = "Step";
            tsBtnRunStep.Click += tsBtnRunStep_Click;
            // 
            // tsBtnRunAll
            // 
            tsBtnRunAll.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnRunAll.Name = "tsBtnRunAll";
            tsBtnRunAll.Size = new Size(32, 22);
            tsBtnRunAll.Text = "Run";
            tsBtnRunAll.Click += tsBtnRunAll_Click;
            // 
            // tsBtnCreateProc
            // 
            tsBtnCreateProc.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnCreateProc.Name = "tsBtnCreateProc";
            tsBtnCreateProc.Size = new Size(36, 22);
            tsBtnCreateProc.Text = "Criar";
            tsBtnCreateProc.Click += tsBtnCreateProc_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { tslblRAM, tslblClock });
            statusStrip1.Location = new Point(0, 640);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1000, 22);
            statusStrip1.TabIndex = 2;
            // 
            // tslblRAM
            // 
            tslblRAM.Name = "tslblRAM";
            tslblRAM.Size = new Size(66, 17);
            tslblRAM.Text = "RAM: 0 MB";
            // 
            // tslblClock
            // 
            tslblClock.Name = "tslblClock";
            tslblClock.Size = new Size(49, 17);
            tslblClock.Text = "Clock: 0";
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 2;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360F));
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(panelLeft, 0, 0);
            tableLayoutPanelMain.Controls.Add(rightContainer, 1, 0);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Location = new Point(0, 49);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 1;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Size = new Size(1000, 591);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(groupBoxResources);
            panelLeft.Controls.Add(groupBoxActions);
            panelLeft.Controls.Add(groupBoxSystem);
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Location = new Point(3, 3);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(354, 585);
            panelLeft.TabIndex = 0;
            // 
            // groupBoxResources
            // 
            groupBoxResources.Controls.Add(lstResources);
            groupBoxResources.Dock = DockStyle.Top;
            groupBoxResources.Location = new Point(0, 260);
            groupBoxResources.Name = "groupBoxResources";
            groupBoxResources.Size = new Size(354, 160);
            groupBoxResources.TabIndex = 0;
            groupBoxResources.TabStop = false;
            groupBoxResources.Text = "Recursos";
            // 
            // lstResources
            // 
            lstResources.Dock = DockStyle.Fill;
            lstResources.Location = new Point(3, 19);
            lstResources.Name = "lstResources";
            lstResources.Size = new Size(348, 138);
            lstResources.TabIndex = 0;
            // 
            // groupBoxActions
            // 
            groupBoxActions.Controls.Add(labelCreateCount);
            groupBoxActions.Controls.Add(nudCreateCount);
            groupBoxActions.Controls.Add(btnCreateProcess);
            groupBoxActions.Controls.Add(btnClearLogs);
            groupBoxActions.Dock = DockStyle.Top;
            groupBoxActions.Location = new Point(0, 140);
            groupBoxActions.Name = "groupBoxActions";
            groupBoxActions.Padding = new Padding(8);
            groupBoxActions.Size = new Size(354, 120);
            groupBoxActions.TabIndex = 1;
            groupBoxActions.TabStop = false;
            groupBoxActions.Text = "Ações";
            // 
            // labelCreateCount
            // 
            labelCreateCount.AutoSize = true;
            labelCreateCount.Location = new Point(12, 24);
            labelCreateCount.Name = "labelCreateCount";
            labelCreateCount.Size = new Size(72, 15);
            labelCreateCount.TabIndex = 0;
            labelCreateCount.Text = "Quantidade:";
            // 
            // nudCreateCount
            // 
            nudCreateCount.Location = new Point(90, 20);
            nudCreateCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCreateCount.Name = "nudCreateCount";
            nudCreateCount.Size = new Size(120, 23);
            nudCreateCount.TabIndex = 1;
            nudCreateCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnCreateProcess
            // 
            btnCreateProcess.Location = new Point(12, 54);
            btnCreateProcess.Name = "btnCreateProcess";
            btnCreateProcess.Size = new Size(160, 23);
            btnCreateProcess.TabIndex = 2;
            btnCreateProcess.Text = "Criar Processo(s)";
            btnCreateProcess.Click += btnCreateProcess_Click;
            // 
            // btnClearLogs
            // 
            btnClearLogs.Location = new Point(180, 54);
            btnClearLogs.Name = "btnClearLogs";
            btnClearLogs.Size = new Size(140, 23);
            btnClearLogs.TabIndex = 3;
            btnClearLogs.Text = "Limpar Logs";
            btnClearLogs.Click += btnClearLogs_Click;
            // 
            // groupBoxSystem
            // 
            groupBoxSystem.Controls.Add(labelRAM);
            groupBoxSystem.Controls.Add(nudRAM);
            groupBoxSystem.Controls.Add(labelQuantum);
            groupBoxSystem.Controls.Add(nudQuantum);
            groupBoxSystem.Controls.Add(labelAlg);
            groupBoxSystem.Controls.Add(cbAlgorithm);
            groupBoxSystem.Controls.Add(btnApplySettings);
            groupBoxSystem.Dock = DockStyle.Top;
            groupBoxSystem.Location = new Point(0, 0);
            groupBoxSystem.Name = "groupBoxSystem";
            groupBoxSystem.Padding = new Padding(8);
            groupBoxSystem.Size = new Size(354, 140);
            groupBoxSystem.TabIndex = 2;
            groupBoxSystem.TabStop = false;
            groupBoxSystem.Text = "Sistema";
            // 
            // labelRAM
            // 
            labelRAM.AutoSize = true;
            labelRAM.Location = new Point(12, 24);
            labelRAM.Name = "labelRAM";
            labelRAM.Size = new Size(65, 15);
            labelRAM.TabIndex = 0;
            labelRAM.Text = "RAM (MB):";
            // 
            // nudRAM
            // 
            nudRAM.Location = new Point(90, 20);
            nudRAM.Maximum = new decimal(new int[] { 16384, 0, 0, 0 });
            nudRAM.Minimum = new decimal(new int[] { 128, 0, 0, 0 });
            nudRAM.Name = "nudRAM";
            nudRAM.Size = new Size(120, 23);
            nudRAM.TabIndex = 1;
            nudRAM.Value = new decimal(new int[] { 1024, 0, 0, 0 });
            // 
            // labelQuantum
            // 
            labelQuantum.AutoSize = true;
            labelQuantum.Location = new Point(12, 55);
            labelQuantum.Name = "labelQuantum";
            labelQuantum.Size = new Size(61, 15);
            labelQuantum.TabIndex = 2;
            labelQuantum.Text = "Quantum:";
            // 
            // nudQuantum
            // 
            nudQuantum.Location = new Point(90, 50);
            nudQuantum.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudQuantum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudQuantum.Name = "nudQuantum";
            nudQuantum.Size = new Size(120, 23);
            nudQuantum.TabIndex = 3;
            nudQuantum.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // labelAlg
            // 
            labelAlg.AutoSize = true;
            labelAlg.Location = new Point(12, 86);
            labelAlg.Name = "labelAlg";
            labelAlg.Size = new Size(74, 15);
            labelAlg.TabIndex = 4;
            labelAlg.Text = "Escalonador:";
            // 
            // cbAlgorithm
            // 
            cbAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAlgorithm.Items.AddRange(new object[] { "RR", "FCFS", "SJF" });
            cbAlgorithm.Location = new Point(90, 82);
            cbAlgorithm.Name = "cbAlgorithm";
            cbAlgorithm.Size = new Size(120, 23);
            cbAlgorithm.TabIndex = 5;
            // 
            // btnApplySettings
            // 
            btnApplySettings.Location = new Point(220, 20);
            btnApplySettings.Name = "btnApplySettings";
            btnApplySettings.Size = new Size(100, 23);
            btnApplySettings.TabIndex = 6;
            btnApplySettings.Text = "Aplicar";
            btnApplySettings.Click += btnApplySettings_Click;
            // 
            // rightContainer
            // 
            rightContainer.Dock = DockStyle.Fill;
            rightContainer.Location = new Point(363, 3);
            rightContainer.Name = "rightContainer";
            rightContainer.Orientation = Orientation.Horizontal;
            // 
            // rightContainer.Panel1
            // 
            rightContainer.Panel1.Controls.Add(tabControlCenter);
            // 
            // rightContainer.Panel2
            // 
            rightContainer.Panel2.Controls.Add(txtLog);
            rightContainer.Panel2.Controls.Add(txtCommand);
            rightContainer.Panel2.Controls.Add(btnSendCommand);
            rightContainer.Size = new Size(634, 585);
            rightContainer.SplitterDistance = 415;
            rightContainer.TabIndex = 1;
            // 
            // tabControlCenter
            // 
            tabControlCenter.Controls.Add(tabProntos);
            tabControlCenter.Controls.Add(tabBloqueados);
            tabControlCenter.Controls.Add(tabFinalizados);
            tabControlCenter.Dock = DockStyle.Fill;
            tabControlCenter.Location = new Point(0, 0);
            tabControlCenter.Name = "tabControlCenter";
            tabControlCenter.SelectedIndex = 0;
            tabControlCenter.Size = new Size(634, 415);
            tabControlCenter.TabIndex = 0;
            // 
            // tabProntos
            // 
            tabProntos.Controls.Add(dgvProntos);
            tabProntos.Location = new Point(4, 24);
            tabProntos.Name = "tabProntos";
            tabProntos.Size = new Size(626, 387);
            tabProntos.TabIndex = 0;
            tabProntos.Text = "Prontos";
            // 
            // dgvProntos
            // 
            dgvProntos.AllowUserToAddRows = false;
            dgvProntos.AllowUserToDeleteRows = false;
            dgvProntos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProntos.Dock = DockStyle.Fill;
            dgvProntos.Location = new Point(0, 0);
            dgvProntos.Name = "dgvProntos";
            dgvProntos.ReadOnly = true;
            dgvProntos.Size = new Size(626, 387);
            dgvProntos.TabIndex = 0;
            // 
            // tabBloqueados
            // 
            tabBloqueados.Controls.Add(dgvBloqueados);
            tabBloqueados.Location = new Point(4, 24);
            tabBloqueados.Name = "tabBloqueados";
            tabBloqueados.Size = new Size(192, 72);
            tabBloqueados.TabIndex = 1;
            tabBloqueados.Text = "Bloqueados";
            // 
            // dgvBloqueados
            // 
            dgvBloqueados.AllowUserToAddRows = false;
            dgvBloqueados.AllowUserToDeleteRows = false;
            dgvBloqueados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBloqueados.Dock = DockStyle.Fill;
            dgvBloqueados.Location = new Point(0, 0);
            dgvBloqueados.Name = "dgvBloqueados";
            dgvBloqueados.ReadOnly = true;
            dgvBloqueados.Size = new Size(192, 72);
            dgvBloqueados.TabIndex = 0;
            // 
            // tabFinalizados
            // 
            tabFinalizados.Controls.Add(dgvFinalizados);
            tabFinalizados.Location = new Point(4, 24);
            tabFinalizados.Name = "tabFinalizados";
            tabFinalizados.Size = new Size(192, 72);
            tabFinalizados.TabIndex = 2;
            tabFinalizados.Text = "Finalizados";
            // 
            // dgvFinalizados
            // 
            dgvFinalizados.AllowUserToAddRows = false;
            dgvFinalizados.AllowUserToDeleteRows = false;
            dgvFinalizados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFinalizados.Dock = DockStyle.Fill;
            dgvFinalizados.Location = new Point(0, 0);
            dgvFinalizados.Name = "dgvFinalizados";
            dgvFinalizados.ReadOnly = true;
            dgvFinalizados.Size = new Size(192, 72);
            dgvFinalizados.TabIndex = 0;
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.Location = new Point(0, 0);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(634, 113);
            txtLog.TabIndex = 0;
            // 
            // txtCommand
            // 
            txtCommand.Dock = DockStyle.Bottom;
            txtCommand.Location = new Point(0, 113);
            txtCommand.Name = "txtCommand";
            txtCommand.Size = new Size(634, 23);
            txtCommand.TabIndex = 1;
            txtCommand.KeyDown += txtCommand_KeyDown;
            // 
            // btnSendCommand
            // 
            btnSendCommand.Dock = DockStyle.Bottom;
            btnSendCommand.Location = new Point(0, 136);
            btnSendCommand.Name = "btnSendCommand";
            btnSendCommand.Size = new Size(634, 30);
            btnSendCommand.TabIndex = 2;
            btnSendCommand.Text = "Enviar";
            btnSendCommand.Click += btnSendCommand_Click;
            // 
            // panelRight
            // 
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(0, 0);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(8);
            panelRight.Size = new Size(200, 100);
            panelRight.TabIndex = 0;

            // groupBoxMetrics
            groupBoxMetrics = new GroupBox();
            lblTotalProcesses = new Label();
            lblContextSwitches = new Label();
            lblAvgTurnaround = new Label();
            lblAvgWaiting = new Label();
            lblAvgCpuCycles = new Label();
            lblThroughput = new Label();

            lblTotalProcessesValue = new Label();
            lblContextSwitchesValue = new Label();
            lblAvgTurnaroundValue = new Label();
            lblAvgWaitingValue = new Label();
            lblAvgCpuCyclesValue = new Label();
            lblThroughputValue = new Label();

            groupBoxMetrics.Text = "Métricas";
            groupBoxMetrics.Dock = DockStyle.Top;
            groupBoxMetrics.Height = 140;
            groupBoxMetrics.Padding = new Padding(8);

            // labels positions (ajuste se quiser)
            lblTotalProcesses.Text = "Total proc.:";
            lblTotalProcesses.AutoSize = true;
            lblTotalProcesses.Location = new Point(12, 22);
            lblTotalProcessesValue.Location = new Point(140, 20);
            lblTotalProcessesValue.AutoSize = true;

            lblContextSwitches.Text = "Trocas (CS):";
            lblContextSwitches.AutoSize = true;
            lblContextSwitches.Location = new Point(12, 42);
            lblContextSwitchesValue.Location = new Point(140, 40);
            lblContextSwitchesValue.AutoSize = true;

            lblAvgTurnaround.Text = "Tempo médio turnaround:";
            lblAvgTurnaround.AutoSize = true;
            lblAvgTurnaround.Location = new Point(12, 62);
            lblAvgTurnaroundValue.Location = new Point(180, 60);
            lblAvgTurnaroundValue.AutoSize = true;

            lblAvgWaiting.Text = "Tempo médio espera:";
            lblAvgWaiting.AutoSize = true;
            lblAvgWaiting.Location = new Point(12, 82);
            lblAvgWaitingValue.Location = new Point(140, 80);
            lblAvgWaitingValue.AutoSize = true;

            lblAvgCpuCycles.Text = "Média ciclos CPU:";
            lblAvgCpuCycles.AutoSize = true;
            lblAvgCpuCycles.Location = new Point(12, 102);
            lblAvgCpuCyclesValue.Location = new Point(140, 100);
            lblAvgCpuCyclesValue.AutoSize = true;

            lblThroughput.Text = "Throughput (proc/s):";
            lblThroughput.AutoSize = true;
            lblThroughput.Location = new Point(12, 122);
            lblThroughputValue.Location = new Point(160, 120);
            lblThroughputValue.AutoSize = true;

            // add labels to group
            groupBoxMetrics.Controls.Add(lblTotalProcesses);
            groupBoxMetrics.Controls.Add(lblTotalProcessesValue);
            groupBoxMetrics.Controls.Add(lblContextSwitches);
            groupBoxMetrics.Controls.Add(lblContextSwitchesValue);
            groupBoxMetrics.Controls.Add(lblAvgTurnaround);
            groupBoxMetrics.Controls.Add(lblAvgTurnaroundValue);
            groupBoxMetrics.Controls.Add(lblAvgWaiting);
            groupBoxMetrics.Controls.Add(lblAvgWaitingValue);
            groupBoxMetrics.Controls.Add(lblAvgCpuCycles);
            groupBoxMetrics.Controls.Add(lblAvgCpuCyclesValue);
            groupBoxMetrics.Controls.Add(lblThroughput);
            groupBoxMetrics.Controls.Add(lblThroughputValue);

            // add to panelLeft (insira antes de groupBoxResources para ficar acima)
            panelLeft.Controls.Add(groupBoxMetrics);

            // 
            // FormPrincipal
            // 
            ClientSize = new Size(1000, 662);
            Controls.Add(tableLayoutPanelMain);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(statusStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MiniSO - Visual";
            Load += FormPrincipal_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tableLayoutPanelMain.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            groupBoxResources.ResumeLayout(false);
            groupBoxActions.ResumeLayout(false);
            groupBoxActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCreateCount).EndInit();
            groupBoxSystem.ResumeLayout(false);
            groupBoxSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudRAM).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQuantum).EndInit();
            rightContainer.Panel1.ResumeLayout(false);
            rightContainer.Panel2.ResumeLayout(false);
            rightContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)rightContainer).EndInit();
            rightContainer.ResumeLayout(false);
            tabControlCenter.ResumeLayout(false);
            tabProntos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProntos).EndInit();
            tabBloqueados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBloqueados).EndInit();
            tabFinalizados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvFinalizados).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer rightContainer;
    }
}
