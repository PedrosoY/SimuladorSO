using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniSOVisual {
    public partial class FormPrincipal : Form {
        private Sistema sisteminha;
        private BindingList<ProcessoViewModel> bindingProntos = new BindingList<ProcessoViewModel>();
        private BindingList<ProcessoViewModel> bindingBloqueados = new BindingList<ProcessoViewModel>();
        private BindingList<ProcessoViewModel> bindingFinalizados = new BindingList<ProcessoViewModel>();

        private System.Windows.Forms.Timer runTimer;
        private bool isRunning = false;

        public FormPrincipal() {
            InitializeComponent();
            runTimer = new System.Windows.Forms.Timer();
            runTimer.Interval = 20;
            runTimer.Tick += RunTimer_Tick;
        }

        private void Inicializar() {
            sisteminha = new Sistema();
            Console.SetOut(new ControlWriter(AppendLog));
            dgvProntos.DataSource = bindingProntos;
            dgvBloqueados.DataSource = bindingBloqueados;
            dgvFinalizados.DataSource = bindingFinalizados;
            nudRAM.Value = sisteminha.QuantidadeMemoriaRAM_Sistema;
            nudQuantum.Value = sisteminha.Quantum;
            if (cbAlgorithm.Items.Contains(sisteminha.AlgoritimoEscalonamento))
                cbAlgorithm.SelectedItem = sisteminha.AlgoritimoEscalonamento;
            else
                cbAlgorithm.SelectedIndex = 0;
            PopulateResources();
            UpdateUiFromSistema();
        }

        private void AppendLog(string texto) {
            if (string.IsNullOrEmpty(texto)) return;
            if (txtLog.InvokeRequired) {
                txtLog.BeginInvoke(new Action(() => {
                    txtLog.AppendText(texto + Environment.NewLine);
                    txtLog.SelectionStart = txtLog.Text.Length;
                    txtLog.ScrollToCaret();
                }));
            }
            else {
                txtLog.AppendText(texto + Environment.NewLine);
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.ScrollToCaret();
            }
        }

        private void UpdateUiFromSistema() {
            if (InvokeRequired) {
                BeginInvoke(new Action(UpdateUiFromSistema));
                return;
            }

            tslblRAM.Text = $"RAM: {sisteminha.QuantidadeMemoriaRAM_Sistema} MB";
            tslblClock.Text = $"Clock: {sisteminha.cpuSistema.CicloClockAtual}";

            var prontos = sisteminha.ListaDeProcessosProntos.ToList();
            var bloqueados = sisteminha.ListaDeProcessosBloqueados.ToList();
            var finalizados = sisteminha.ListaDeProcessosFinalizados.ToList();

            bindingProntos.Clear();
            foreach (var p in prontos)
                bindingProntos.Add(ProcessoViewModel.FromProcesso(p));

            bindingBloqueados.Clear();
            foreach (var p in bloqueados)
                bindingBloqueados.Add(ProcessoViewModel.FromProcesso(p));

            bindingFinalizados.Clear();
            foreach (var p in finalizados)
                bindingFinalizados.Add(ProcessoViewModel.FromProcesso(p));

            PopulateResources();
            UpdateMetrics();
        }

        private void PopulateResources() {
            lstResources.Items.Clear();
            foreach (var r in sisteminha.ListaDeRecursosSistema) {
                var s = r.estaAlocado ? $"{r.Id} - {r.nomeRecurso} (alocado por {r.processoAlocador?.IdentificadorProcesso})" : $"{r.Id} - {r.nomeRecurso} (livre)";
                lstResources.Items.Add(s);
            }
        }
        private void UpdateMetrics() {
            if (sisteminha == null) return;

            var finalizados = sisteminha.ListaDeProcessosFinalizados.ToList();

            int totalProc = sisteminha.TotalProcessesCreated;
            int finished = sisteminha.TotalProcessesFinished;
            int contextSwitches = sisteminha.ContextSwitches;
            double throughput = 0;
            if (sisteminha.SimulationStart != default && (DateTime.Now - sisteminha.SimulationStart).TotalSeconds > 0)
                throughput = finished / (DateTime.Now - sisteminha.SimulationStart).TotalSeconds;

            double avgTurnMs = 0;
            double avgWaitMs = 0;
            double avgCpuCycles = 0;

            if (finalizados.Count > 0) {
                avgTurnMs = finalizados.Select(p => (p.FinishedAt.Value - p.CreatedAt).TotalMilliseconds).Average();

                avgCpuCycles = finalizados.Select(p => (double)p.TotalCiclosExecutados).Average();
                avgWaitMs = finalizados.Select(p =>
                {
                    var turn = (p.FinishedAt.Value - p.CreatedAt).TotalMilliseconds;
                    var cpuMs = p.TotalCiclosExecutados;
                    return turn - cpuMs;
                }).Average();
            }

            lblTotalProcessesValue.Text = totalProc.ToString();
            lblContextSwitchesValue.Text = contextSwitches.ToString();
            lblAvgTurnaroundValue.Text = $"{avgTurnMs:F1} ms";
            lblAvgWaitingValue.Text = $"{avgWaitMs:F1} ms (approx)";
            lblAvgCpuCyclesValue.Text = $"{avgCpuCycles:F1}";
            lblThroughputValue.Text = $"{throughput:F2}";
        }

        private void btnApplySettings_Click(object sender, EventArgs e) {
            sisteminha.QuantidadeMemoriaRAM_Sistema = (int)nudRAM.Value;
            sisteminha.Quantum = (int)nudQuantum.Value;
            sisteminha.AlgoritimoEscalonamento = cbAlgorithm.SelectedItem?.ToString() ?? "RR";
            AppendLog($"[UI] Configurações aplicadas: RAM={sisteminha.QuantidadeMemoriaRAM_Sistema} Quantum={sisteminha.Quantum} Alg={sisteminha.AlgoritimoEscalonamento}");
            UpdateUiFromSistema();
        }

        private void btnCreateProcess_Click(object sender, EventArgs e) {
            int count = (int)nudCreateCount.Value;
            for (int i = 0; i < count; i++)
                sisteminha.CriarProcesso();
            UpdateUiFromSistema();
        }

        private void tsBtnCreateProc_Click(object sender, EventArgs e) => btnCreateProcess_Click(sender, e);

        private void tsBtnRunStep_Click(object sender, EventArgs e) {
            if (!sisteminha.ListaDeProcessosProntos.Any()) {
                sisteminha.VerificarTimeoutsBloqueados();
                UpdateUiFromSistema();

                if (!sisteminha.ListaDeProcessosProntos.Any()) {
                    AppendLog("[UI] Nenhum processo pronto.");
                    return;
                }
            }

            var p = sisteminha.EscolherProximoProcesso(cbAlgorithm.SelectedItem?.ToString() ?? "RR");
            if (p == null) {
                AppendLog("[UI] Nenhum processo pronto.");
                return;
            }

            sisteminha.ExecutarProcesso(p);
            UpdateUiFromSistema();
        }


        private void tsBtnRunAll_Click(object sender, EventArgs e) {
            if (!isRunning) {
                isRunning = true;
                tsBtnRunAll.Text = "Parar";
                btnCreateProcess.Enabled = false;
                btnApplySettings.Enabled = false;
                cbAlgorithm.Enabled = false;
                nudCreateCount.Enabled = false;
                runTimer.Start();
                AppendLog("[UI] Iniciando execução em lote (Run All) via Timer...");
            }
            else {
                runTimer.Stop();
                isRunning = false;
                tsBtnRunAll.Text = "Run";
                btnCreateProcess.Enabled = true;
                btnApplySettings.Enabled = true;
                cbAlgorithm.Enabled = true;
                nudCreateCount.Enabled = true;
                AppendLog("[UI] Execução em lote parada pelo usuário.");
            }
        }

        private void RunTimer_Tick(object sender, EventArgs e) {
            if (!(sisteminha.ListaDeProcessosProntos.Any() || sisteminha.ListaDeProcessosBloqueados.Any())) {
                runTimer.Stop();
                isRunning = false;
                tsBtnRunAll.Text = "Run";
                btnCreateProcess.Enabled = true;
                btnApplySettings.Enabled = true;
                cbAlgorithm.Enabled = true;
                nudCreateCount.Enabled = true;
                AppendLog("[UI] Execução em lote finalizada (sem processos restantes).");
                UpdateUiFromSistema();
                return;
            }

            if (!sisteminha.ListaDeProcessosProntos.Any()) {
                sisteminha.VerificarTimeoutsBloqueados();
                UpdateUiFromSistema();
                return;
            }

            var algoritmoSelecionado = cbAlgorithm.SelectedItem?.ToString() ?? "RR";
            var processo = sisteminha.EscolherProximoProcesso(algoritmoSelecionado);
            if (processo != null)
                sisteminha.ExecutarProcesso(processo);

            UpdateUiFromSistema();
        }

        private void btnClearLogs_Click(object sender, EventArgs e) => txtLog.Clear();

        private void btnSendCommand_Click(object sender, EventArgs e) => ProcessCommand(txtCommand.Text);

        private void txtCommand_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ProcessCommand(txtCommand.Text);
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void ProcessCommand(string cmd) {
            if (string.IsNullOrWhiteSpace(cmd)) return;
            AppendLog($"> {cmd}");
            var parts = cmd.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            var c = parts[0].ToLowerInvariant();
            try {
                if (c == "criar" || c == "create") {
                    int n = 1;
                    if (parts.Length > 1 && int.TryParse(parts[1], out var parsed)) n = parsed;
                    for (int i = 0; i < n; i++) sisteminha.CriarProcesso();
                    UpdateUiFromSistema();
                }
                else if (c == "rodar" || c == "run") {
                    tsBtnRunAll_Click(this, EventArgs.Empty);
                }
                else if (c == "passo" || c == "step") {
                    tsBtnRunStep_Click(this, EventArgs.Empty);
                }
                else if (c == "liberar" || c == "release") {
                    if (parts.Length < 2) AppendLog("Use: liberar <nomeRecursoOuId>");
                    else {
                        var key = parts[1];
                        Recursos found = null;
                        if (int.TryParse(key, out var id)) found = sisteminha.ListaDeRecursosSistema.FirstOrDefault(r => r.Id == id);
                        if (found == null) found = sisteminha.ListaDeRecursosSistema.FirstOrDefault(r => r.nomeRecurso.Equals(key, StringComparison.OrdinalIgnoreCase));

                        if (found != null && found.processoAlocador != null) {
                            sisteminha.LiberarRecursos(found.processoAlocador, found);
                            UpdateUiFromSistema();
                        }
                        else AppendLog("Recurso não encontrado ou não alocado.");
                    }
                }
                else if (c == "status") {
                    UpdateUiFromSistema();
                }
                else {
                    AppendLog("Comando desconhecido. Ex.: criar 3 | run | step | liberar Impressora | status");
                }
            }
            catch (Exception ex) {
                AppendLog("Erro ao processar comando: " + ex.Message);
            }
            finally {
                txtCommand.Clear();
            }
        }

        private void FormPrincipal_Load(object sender, EventArgs e) {
            Inicializar();
        }
    }

    public class ProcessoViewModel {
        public string Identificador { get; set; }
        public int Memoria { get; set; }
        public string Estado { get; set; }
        public int Threads { get; set; }
        public bool MemoriaAlocada { get; set; }

        public static ProcessoViewModel FromProcesso(Processo p) {
            return new ProcessoViewModel {
                Identificador = p.IdentificadorProcesso,
                Memoria = p.MemoriaTotalDoProcesso,
                Estado = p.EstadosProcessoAtual.ToString(),
                Threads = p.ListaDeMinhasThreads?.Count ?? 0,
                MemoriaAlocada = p.MemoriaFoiAlocada
            };
        }
    }

    public class ControlWriter : System.IO.TextWriter {
        private readonly Action<string> _append;
        public ControlWriter(Action<string> append) {
            _append = append;
        }
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string value) {
            _append?.Invoke(value);
        }

        public override void Write(string value) {
            if (string.IsNullOrEmpty(value)) return;
            var parts = value.Replace('\r', ' ').Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts) _append?.Invoke(p);
        }
    }
}
