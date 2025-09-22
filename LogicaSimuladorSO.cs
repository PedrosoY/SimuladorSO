using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace MiniSOVisual {

    public enum EstadosProcesso_Threads {
        Pronto,
        Executando,
        Bloqueado,
        Finalizado
    }

    public static class Utilidades {
        public static readonly Random GlobalRandom = new Random();
    }

    public static class Logger {
        private static readonly object _lock = new object();

        private static void WriteMessage(string nivel, ConsoleColor cor, string mensagem, int indentLevel = 0) {
            lock (_lock) {
                if (mensagem == null) mensagem = string.Empty;

                var rawLines = mensagem.Replace("\r", "").Split('\n');
                var lines = rawLines
                    .Select(l => l.Trim())
                    .Where(l => l.Length > 0)
                    .ToArray();

                if (lines.Length == 0) return;

                string prefix = $"[{DateTime.Now:HH:mm:ss}] {nivel}: ";
                Console.ForegroundColor = cor;

                Console.WriteLine(prefix + lines[0]);

                string continuationIndent = new string(' ', prefix.Length + indentLevel * 4);
                for (int i = 1; i < lines.Length; i++) {
                    Console.WriteLine(continuationIndent + lines[i]);
                }

                Console.ResetColor();
            }
        }

        public static void Info(string mensagem) => WriteMessage("INFO", ConsoleColor.Green, mensagem);
        public static void WarningInfo(string mensagem) => WriteMessage("WAFO", ConsoleColor.Magenta, mensagem);
        public static void Warning(string mensagem) => WriteMessage("WARN", ConsoleColor.Yellow, mensagem);
        public static void Error(string mensagem) => WriteMessage("ERROR", ConsoleColor.Red, mensagem);
        public static void Debug(string mensagem) => WriteMessage("DEBUG", ConsoleColor.Cyan, mensagem);

        public static void InfoIndented(string mensagem, int indentLevel = 1) {
            WriteMessage("INFO", ConsoleColor.Green, mensagem, indentLevel);
        }
    }

    public class Recursos {
        public int Id { get; set; } = 0;
        public string nomeRecurso { get; set; } = "";
        public bool estaAlocado { get; set; } = false;
        public Processo processoAlocador { get; set; }
    }

    public class Sistema {

        public Sistema() {
            SimulationStart = DateTime.Now;
            Logger.Info($"Sistema iniciado com {QuantidadeMemoriaRAM_Sistema} MB de RAM");
            QuantidadeMemoriaOriginalSistema = QuantidadeMemoriaRAM_Sistema;
        }
        public DateTime SimulationStart { get; set; }
        public int ContextSwitches { get; set; } = 0;
        public int TotalProcessesCreated { get; set; } = 0;
        public int TotalProcessesFinished { get; set; } = 0;

        public int QuantidadeMemoriaRAM_Sistema { get; set; } = 1024;
        public int QuantidadeMemoriaOriginalSistema { get; set; }
        public int Quantum { get; set; } = 100;
        public string AlgoritimoEscalonamento { get; set; } = "RR";
        public Escalonador escalonadorSistema { get; set; } = new Escalonador();
        public CPU cpuSistema { get; set; } = new CPU();
        public List<Recursos> ListaDeRecursosSistema { get; set; } = new List<Recursos> {

            new Recursos { Id = 5, nomeRecurso = "Teclado" },
            new Recursos { Id = 6, nomeRecurso = "Mouse" },
            new Recursos { Id = 7, nomeRecurso = "Monitor" },
            new Recursos { Id = 8, nomeRecurso = "Placa de Vídeo" },
            new Recursos { Id = 9, nomeRecurso = "Placa de Som" },
            new Recursos { Id = 10, nomeRecurso = "Impressora" },
            new Recursos { Id = 11, nomeRecurso = "Scanner" },
            new Recursos { Id = 12, nomeRecurso = "Porta USB" },
            new Recursos { Id = 15, nomeRecurso = "Placa de Rede" },
            new Recursos { Id = 16, nomeRecurso = "Wi-Fi" },
            new Recursos { Id = 17, nomeRecurso = "Bluetooth" },
            new Recursos { Id = 18, nomeRecurso = "Webcam" },
            new Recursos { Id = 19, nomeRecurso = "Microfone" },
            new Recursos { Id = 20, nomeRecurso = "Alto-falantes" },
        };


        public List<Processo> ListaDeProcessosProntos { get; set; } = new List<Processo>();
        public List<Processo> ListaDeProcessosBloqueados { get; set; } = new List<Processo>();
        public List<Processo> ListaDeProcessosFinalizados { get; set; } = new List<Processo>();

        public void CriarProcesso(string? identificadorProcesso = null, int? quantidadeMemoriaRAMUsada = null) {
            var novoProcesso = new Processo();

            if (identificadorProcesso != null)
                novoProcesso.IdentificadorProcesso = identificadorProcesso;

            int memoriaUsada = quantidadeMemoriaRAMUsada ??
                Utilidades.GlobalRandom.Next((int)(0.1 * QuantidadeMemoriaOriginalSistema), (int)(0.25 * QuantidadeMemoriaOriginalSistema));

            novoProcesso.MemoriaTotalDoProcesso = memoriaUsada;

            if (memoriaUsada > QuantidadeMemoriaRAM_Sistema) {
                novoProcesso.EstadosProcessoAtual = EstadosProcesso_Threads.Bloqueado;
                ListaDeProcessosBloqueados.Add(novoProcesso);
                Logger.Warning($"[BLOQUEADO] Processo {novoProcesso.IdentificadorProcesso} excedeu a RAM ({memoriaUsada} MB).");
            }
            else {
                QuantidadeMemoriaRAM_Sistema -= memoriaUsada;
                novoProcesso.MemoriaFoiAlocada = true;
                ListaDeProcessosProntos.Add(novoProcesso);
                Logger.Info($"[CRIADO] Processo {novoProcesso.IdentificadorProcesso} alocado com {memoriaUsada} MB. RAM restante: {QuantidadeMemoriaRAM_Sistema} MB");

                novoProcesso.CriarThreadsDinamicamente();
            }
            TotalProcessesCreated++;

        }

        public void FinalizarProcesso(Processo processo) {
            processo.EstadosProcessoAtual = EstadosProcesso_Threads.Finalizado;

            if (processo.MemoriaFoiAlocada) {
                QuantidadeMemoriaRAM_Sistema += processo.MemoriaTotalDoProcesso;
            }
            else {
                Logger.Warning($"[FINALIZAR-ANOMALIA] Processo {processo.IdentificadorProcesso} finalizou sem MemoriaFoiAlocada=true. Restaurando {processo.MemoriaTotalDoProcesso} MB para evitar vazamento.");
                QuantidadeMemoriaRAM_Sistema += processo.MemoriaTotalDoProcesso;
            }

            if (ListaDeProcessosProntos.Contains(processo))
                ListaDeProcessosProntos.Remove(processo);

            if (!ListaDeProcessosFinalizados.Contains(processo)) {
                processo.FinishedAt = DateTime.Now;
                TotalProcessesFinished++;
                ListaDeProcessosFinalizados.Add(processo);
            }

            Logger.Info($"\n[FINALIZADO] Processo {processo.IdentificadorProcesso} liberou {processo.MemoriaTotalDoProcesso} MB. RAM atual: {QuantidadeMemoriaRAM_Sistema} MB");

            for (int i = ListaDeProcessosBloqueados.Count - 1; i >= 0; i--) {
                var processoBloqueado = ListaDeProcessosBloqueados[i];

                if (!processoBloqueado.MemoriaFoiAlocada) {
                    if (TentarDesbloquearProcessos(processoBloqueado)) {
                        QuantidadeMemoriaRAM_Sistema -= processoBloqueado.MemoriaTotalDoProcesso;
                        processoBloqueado.MemoriaFoiAlocada = true;

                        processoBloqueado.EstadosProcessoAtual = EstadosProcesso_Threads.Pronto;
                        ListaDeProcessosProntos.Add(processoBloqueado);
                        ListaDeProcessosBloqueados.RemoveAt(i);

                        Logger.Info($"[DESBLOQUEADO] Processo {processoBloqueado.IdentificadorProcesso} voltou para a lista de prontos. RAM restante: {QuantidadeMemoriaRAM_Sistema} MB");
                    }
                }
                else {
                    // processo bloqueado por recurso, não por memória: não mexer aqui
                }
            }
        }


        public bool TentarDesbloquearProcessos(Processo processoAnalisar) {
            return processoAnalisar.MemoriaTotalDoProcesso <= QuantidadeMemoriaRAM_Sistema;
        }

        public Processo EscolherProximoProcesso(string algoritmo = "FCFS") {
            AlgoritimoEscalonamento = algoritmo;
            return escalonadorSistema.EscolherProximoProcesso(ListaDeProcessosProntos, algoritmo);
        }

        public bool SolicitarRecursos(Processo processoSolicitador, Recursos recursoRequerido) {
            int maxIdPosse = 0;
            if (processoSolicitador.ListaDeRecursosAlocados.Any()) {
                maxIdPosse = processoSolicitador.ListaDeRecursosAlocados.Max(r => r.Id);
            }

            if (maxIdPosse > recursoRequerido.Id) {
                Logger.Info($"[ORDER] Processo {processoSolicitador.IdentificadorProcesso} chamou recurso com Id menor ({recursoRequerido.Id}) que um que já possui ({maxIdPosse}). Fazendo backoff: liberando recursos e tentando depois.");
                LiberarRecursos(processoSolicitador);
                return false;
            }

            if (!recursoRequerido.estaAlocado) {
                recursoRequerido.estaAlocado = true;
                recursoRequerido.processoAlocador = processoSolicitador;

                if (!processoSolicitador.ListaDeRecursosAlocados.Contains(recursoRequerido))
                    processoSolicitador.ListaDeRecursosAlocados.Add(recursoRequerido);

                Logger.WarningInfo($"O processo {processoSolicitador.IdentificadorProcesso} pegou o recurso de {recursoRequerido.nomeRecurso}");
                return true;
            }
            else {
                Logger.Warning($"O processo {processoSolicitador.IdentificadorProcesso} bloqueado enquanto espera o {recursoRequerido.nomeRecurso}");
                processoSolicitador.EstadosProcessoAtual = EstadosProcesso_Threads.Bloqueado;

                if (!processoSolicitador.ListaDeRecursosEsperados.Contains(recursoRequerido))
                    processoSolicitador.ListaDeRecursosEsperados.Add(recursoRequerido);

                processoSolicitador.BlockedSince = DateTime.Now;

                ListaDeProcessosBloqueados.Add(processoSolicitador);
                ListaDeProcessosProntos.Remove(processoSolicitador);
                return false;
            }
        }


        public void LiberarRecursos(Processo processoLiberador, Recursos? recursoFinalizado = null) {
            List<Recursos> recursosParaLiberar = new List<Recursos>();

            if (recursoFinalizado == null) {
                recursosParaLiberar = ListaDeRecursosSistema
                    .Where(r => r.processoAlocador?.IdentificadorProcesso == processoLiberador.IdentificadorProcesso)
                    .ToList();
            }
            else {
                if (recursoFinalizado.processoAlocador?.IdentificadorProcesso == processoLiberador.IdentificadorProcesso) {
                    recursosParaLiberar.Add(recursoFinalizado);
                }
                else {
                    Logger.Warning($"Processo {processoLiberador.IdentificadorProcesso} tentou liberar um recurso que não alocou: {recursoFinalizado.nomeRecurso}");
                    return;
                }
            }

            foreach (var recurso in recursosParaLiberar) {
                processoLiberador.ListaDeRecursosAlocados.Remove(recurso);

                recurso.processoAlocador = null;
                recurso.estaAlocado = false;

                Logger.Info($"Recurso {recurso.nomeRecurso} liberado pelo processo {processoLiberador.IdentificadorProcesso}");

                var processosEsperandoEsseRecurso = ListaDeProcessosBloqueados
                    .Where(p => p.ListaDeRecursosEsperados.Contains(recurso))
                    .ToList();

                foreach (var processoBloqueado in processosEsperandoEsseRecurso) {
                    processoBloqueado.ListaDeRecursosEsperados.Remove(recurso);

                    processoBloqueado.EstadosProcessoAtual = EstadosProcesso_Threads.Pronto;
                    ListaDeProcessosProntos.Add(processoBloqueado);
                    ListaDeProcessosBloqueados.Remove(processoBloqueado);

                    recurso.estaAlocado = true;
                    recurso.processoAlocador = processoBloqueado;
                    if (!processoBloqueado.ListaDeRecursosAlocados.Contains(recurso))
                        processoBloqueado.ListaDeRecursosAlocados.Add(recurso);

                    Logger.Info($"[DESBLOQUEADO] Processo {processoBloqueado.IdentificadorProcesso} pegou o recurso {recurso.nomeRecurso} e voltou para pronto.");
                    break;
                }
            }
        }


        public void ExecutarProcesso(Processo processo) {
            if (Utilidades.GlobalRandom.NextDouble() < 0.5) {
                var recursoAleatorio = ListaDeRecursosSistema[Utilidades.GlobalRandom.Next(ListaDeRecursosSistema.Count)];
                bool conseguiu = SolicitarRecursos(processo, recursoAleatorio);

                if (!conseguiu) {
                    Logger.Info($"[RECURSO] Processo {processo.IdentificadorProcesso} não obteve o recurso {recursoAleatorio.nomeRecurso} no momento.");
                    return;
                }
            }

            int ciclosRestantes;

            if (AlgoritimoEscalonamento == "RR") {
                ciclosRestantes = Quantum;
                Logger.Warning($"\n[EXECUÇÃO - RR] Processo {processo.IdentificadorProcesso} com quantum {ciclosRestantes}");
            }
            else {
                ciclosRestantes = int.MaxValue;
                Logger.Warning($"\n[EXECUÇÃO - {AlgoritimoEscalonamento}] Processo {processo.IdentificadorProcesso}");
            }

            if (processo.StartedAt == null) processo.StartedAt = DateTime.Now;

            ContextSwitches++;

            int ciclosAntes = cpuSistema.CicloClockAtual;
            cpuSistema.ExecutarThreads(processo.ListaDeMinhasThreads, ciclosRestantes);
            int ciclosDepois = cpuSistema.CicloClockAtual;

            int ciclosExecutadosAgora = Math.Max(0, ciclosDepois - ciclosAntes);
            processo.TotalCiclosExecutados += ciclosExecutadosAgora;

            processo.CalcularEstadoProcesso();

            if (processo.EstadosProcessoAtual == EstadosProcesso_Threads.Finalizado) {
                LiberarRecursos(processo);
                FinalizarProcesso(processo);
            }
            else {
                Logger.Info($"[RETORNO] Processo {processo.IdentificadorProcesso} ainda possui threads em execução.");
                ListaDeProcessosProntos.Remove(processo);
                if (AlgoritimoEscalonamento == "RR") {
                    ListaDeProcessosProntos.Add(processo);
                }
                else {
                    ListaDeProcessosProntos.Insert(0, processo);
                }
            }
        }


        public void VerificarTimeoutsBloqueados() {
            var timeout = TimeSpan.FromMilliseconds(2000);

            var bloqueadosCopia = ListaDeProcessosBloqueados.ToList();

            foreach (var p in bloqueadosCopia) {
                if (p.BlockedSince != null && (DateTime.Now - p.BlockedSince.Value) > timeout) {
                    Logger.Warning($"[TIMEOUT] Processo {p.IdentificadorProcesso} ficou bloqueado por mais de {timeout.TotalMilliseconds}ms. Aplicando backoff: liberando recursos e voltando para pronto.");

                    p.ListaDeRecursosEsperados.Clear();
                    p.BlockedSince = null;

                    LiberarRecursos(p);


                    if (!p.MemoriaFoiAlocada) {
                        if (TentarDesbloquearProcessos(p)) {
                            QuantidadeMemoriaRAM_Sistema -= p.MemoriaTotalDoProcesso;
                            p.MemoriaFoiAlocada = true;
                            Logger.Info($"[ALLOC-TIMEOUT] Processo {p.IdentificadorProcesso} recebeu alocação de {p.MemoriaTotalDoProcesso} MB na verificação de timeout. RAM restante: {QuantidadeMemoriaRAM_Sistema} MB");
                        }
                        else {
                            Logger.Info($"[ALLOC-TIMEOUT] Processo {p.IdentificadorProcesso} ainda não possui RAM disponível. Continua bloqueado.");
                            continue;
                        }
                    }

                    if (ListaDeProcessosBloqueados.Contains(p)) {
                        ListaDeProcessosBloqueados.Remove(p);
                    }

                    if (!ListaDeProcessosProntos.Contains(p)) {
                        p.EstadosProcessoAtual = EstadosProcesso_Threads.Pronto;
                        ListaDeProcessosProntos.Add(p);
                    }
                }
            }
        }


    }

    public class Processo {

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? StartedAt { get; set; } = null;
        public DateTime? FinishedAt { get; set; } = null;
        public int TotalCiclosExecutados { get; set; } = 0;


        public string IdentificadorProcesso { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8);
        public EstadosProcesso_Threads EstadosProcessoAtual { get; set; } = EstadosProcesso_Threads.Pronto;
        public int MemoriaTotalDoProcesso { get; set; } = 1;
        public bool MemoriaFoiAlocada { get; set; } = false;
        public List<MinhasThreads> ListaDeMinhasThreads { get; private set; } = new List<MinhasThreads>();

        public List<Recursos> ListaDeRecursosAlocados { get; private set; } = new List<Recursos>();
        public List<Recursos> ListaDeRecursosEsperados { get; private set; } = new List<Recursos>();

        public DateTime? BlockedSince { get; set; } = null;

        public void CriarThreadsDinamicamente() {
            int memoriaDisponivel = MemoriaTotalDoProcesso;

            int minPorcento = Math.Max((int)(0.1 * memoriaDisponivel), 1);
            int maxPorcento = Math.Max((int)(0.25 * memoriaDisponivel), 1);

            while (memoriaDisponivel >= minPorcento) {
                int usoMemoria = Utilidades.GlobalRandom.Next(minPorcento, Math.Min(maxPorcento, memoriaDisponivel) + 1);

                if (memoriaDisponivel - usoMemoria < minPorcento) {
                    usoMemoria = memoriaDisponivel;
                }

                var thread = new MinhasThreads(usoMemoria);
                ListaDeMinhasThreads.Add(thread);
                memoriaDisponivel -= usoMemoria;
            }

            Logger.Info($"Processo {IdentificadorProcesso} criou {ListaDeMinhasThreads.Count} threads:");
            foreach (var t in ListaDeMinhasThreads) {
                Logger.InfoIndented($"- Thread {t.IdentificadorThread} | Memória: {t.MemoriaConsumida} MB | Ciclos: {t.CiclosRestantes}");
            }
        }

        public void CalcularEstadoProcesso() {
            if (ListaDeMinhasThreads.All(t => t.EstadoThread == EstadosProcesso_Threads.Finalizado)) {
                EstadosProcessoAtual = EstadosProcesso_Threads.Finalizado;
            }
            else {
                EstadosProcessoAtual = EstadosProcesso_Threads.Pronto;
            }
        }
    }

    public class MinhasThreads {
        public string IdentificadorThread { get; set; }
        public int MemoriaConsumida { get; set; }
        public EstadosProcesso_Threads EstadoThread { get; set; } = EstadosProcesso_Threads.Pronto;
        public int CiclosRestantes { get; set; }

        public MinhasThreads(int memoria) {
            IdentificadorThread = Guid.NewGuid().ToString("N").Substring(0, 6);
            MemoriaConsumida = memoria;
            CiclosRestantes = Utilidades.GlobalRandom.Next(1, 100);
        }

        public bool ExecutarMinhaThread() {
            if (CiclosRestantes > 0) {
                CiclosRestantes--;
                return CiclosRestantes == 0;
            }
            return true;
        }
    }

    public class CPU {
        public int CicloClockAtual { get; set; } = 0;

        public void ExecutarThreads(List<MinhasThreads> threadsParaExecutar, int ciclosParaRodar, bool usoDeQuantum = false) {
            int quantumRestante = ciclosParaRodar;

            foreach (var threadExecutando in threadsParaExecutar.Where(t => t.EstadoThread != EstadosProcesso_Threads.Finalizado)) {
                Logger.Debug($"\n[THREAD] Executando {threadExecutando.IdentificadorThread} | Ciclos restantes: {threadExecutando.CiclosRestantes}");
                threadExecutando.EstadoThread = EstadosProcesso_Threads.Executando;

                while (quantumRestante > 0 && threadExecutando.EstadoThread != EstadosProcesso_Threads.Finalizado) {
                    CicloClockAtual++;
                    quantumRestante--;

                    bool terminou = threadExecutando.ExecutarMinhaThread();
                    if (terminou) {
                        threadExecutando.EstadoThread = EstadosProcesso_Threads.Finalizado;
                        Logger.Debug($"[THREAD] {threadExecutando.IdentificadorThread} finalizou.");
                    }
                }

                if (quantumRestante == 0) {
                    Logger.Debug($"[PROCESSO] Quantum esgotado. Processo pausado.");
                    break;
                }

                if (threadExecutando.EstadoThread != EstadosProcesso_Threads.Finalizado) {
                    threadExecutando.EstadoThread = EstadosProcesso_Threads.Pronto;
                    Logger.Debug($"[THREAD] {threadExecutando.IdentificadorThread} pausada. Ciclos restantes: {threadExecutando.CiclosRestantes}");
                }
            }
        }
    }

    public class Escalonador {
        public Processo EscolherProximoProcesso(List<Processo> listaDeProntos, string algoritimoEscolhido = "RR") {
            if (listaDeProntos == null || listaDeProntos.Count == 0) return null;

            if (algoritimoEscolhido == "FCFS") {
                var processo = listaDeProntos.First();
                Logger.Info($"\n[ESCALONADOR - FCFS] Processo {processo.IdentificadorProcesso} selecionado.");
                return processo;
            }
            else if (algoritimoEscolhido == "SJF") {
                var processo = listaDeProntos.Aggregate((menor, atual) => atual.MemoriaTotalDoProcesso < menor.MemoriaTotalDoProcesso ? atual : menor);
                Logger.Info($"\n[ESCALONADOR - SJF] Processo {processo.IdentificadorProcesso} selecionado.");
                return processo;
            }
            else if (algoritimoEscolhido == "RR") {
                var processo = listaDeProntos.First();
                Logger.Info($"\n[ESCALONADOR - RR] Processo {processo.IdentificadorProcesso} selecionado.");
                return processo;
            }
            else {
                Logger.Error($"[ERRO] Algoritmo '{algoritimoEscolhido}' não implementado.");
                return null;
            }
        }
    }
}
