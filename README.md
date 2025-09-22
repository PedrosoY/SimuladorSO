# Visão geral

Este projeto foi implementado em C# 9 e tem como objetivo simular, de forma controlada e educativa, o comportamento de um sistema operacional: criação e finalização de processos, criação de threads internas aos processos, alocação de memória, escalonamento, alocação de recursos (I/O), timeouts para bloqueios e cálculo de métricas didáticas.

Importante sobre distribuição
- O repositório contém apenas os arquivos-fonte `.cs` crus
  - Se você quiser compilar a partir do código abra o projeto pelo arquivo `MiniSOVisual.sln` ou pegue os arquivos `.cs`, crie um projeto Windows Forms no Visual Studio, adicione os arquivos ao projeto e ajuste o `namespace`/o `designer` onde necessário (o `FormPrincipal` precisa estar corretamente conectado ao arquivo `.Designer.cs`). Em outras palavras: é necessário montar o formulário no Visual Studio para rodar a partir do código-fonte.
  - Se você só quer ver o resultado final, baixe a release — mais especificamente o arquivo `MiniSOVisual.exe` (publicado nas Releases). Basta baixar e executar o `MiniSOVisual.exe` com dois cliques no Windows para abrir o simulador.
- Observação sobre alertas de segurança: em alguns sistemas o Windows pode exibir um aviso (SmartScreen/Defender) ao executar um `.exe` baixado da internet. Isso pode ocorrer como falso positivo; o executável aqui é o meu build do projeto. Caso apareça esse aviso, você pode prosseguir e continuar a execução.

A seguir explico detalhadamente como o simulador funciona, descrevendo cada parte / classe e o fluxo de execução de como foi projetado.

1) Visão geral do modelo de simulação
- Não é criado processos ou threads reais do sistema operacional. Em vez disso é simulado processos e threads internamente usando classes próprias (Processo, MinhasThreads, CPU, etc.). A ideia é reproduzir comportamentos observáveis de um SO (escalonamento, consumo de ciclos, bloqueio por I/O, liberação e realocação de memória) sem usar Task.Run, Thread real ou spawn de processos do SO.
- A simulação é discreta e baseada em "ciclos" de CPU; cada execução de thread decrementa um contador de ciclos restantes até que a thread finalize.

2) Enum de estados
- `EstadosProcesso_Threads`:
  - Valores: `Pronto`, `Executando`, `Bloqueado`, `Finalizado`.
  - Esse enum é usado tanto para processos quanto para as threads internas para representar estados principais.

3) Utilitários e Logger
- `Utilidades`:
  - `GlobalRandom`: única fonte de aleatoriedade para gerar memórias requeridas e ciclos de threads. Uso um `Random` compartilhado para resultados reprodutíveis em execução única.
- `Logger`:
  - Implemento métodos `Info`, `InfoIndented`, `WarningInfo`, `Warning`, `Error`, `Debug`.
  - `Logger` escreve mensagens com timestamp e nível. Essas mensagens são roteadas para a saída padrão, e no front-end é feito um `Console.SetOut` para redirecionar para o componente de logs da interface.
  - Internamente o codigo trata mensagens multiline e controle de indentação para facilitar leitura.

4) Modelo de recursos
- `Recursos`:
  - Cada recurso tem `Id`, `nomeRecurso`, `estaAlocado` e `processoAlocador`.
  - Recursos simulam dispositivos únicos (Teclado, Mouse, Impressora, etc.). Apenas um processo pode alocar um recurso por vez.
  - O sistema tem uma lista fixa `ListaDeRecursosSistema` com IDs e nomes predefinidos.

5) A classe central: Sistema
- Responsabilidades:
  - Gerenciar memória simulada (`QuantidadeMemoriaRAM_Sistema` e `QuantidadeMemoriaOriginalSistema`).
  - Manter as listas de processos: `ListaDeProcessosProntos`, `ListaDeProcessosBloqueados`, `ListaDeProcessosFinalizados`.
  - Controlar o escalonador (`escalonadorSistema`) e a CPU simulada (`cpuSistema`).
  - Contabilizar métricas: `ContextSwitches`, `TotalProcessesCreated`, `TotalProcessesFinished`, `SimulationStart`.
  - Gerenciar alocação e liberação de recursos e memória.
- Principais métodos e fluxos:
  - `CriarProcesso(identificadorProcesso = null, quantidadeMemoriaRAMUsada = null)`:
    - Crio um `Processo` novo. Se não informo memória, calculo aleatoriamente entre 10% e 25% da memória original do sistema (isso produz cenários variados de alocação).
    - Se o processo pedir mais memória do que a disponível, deixo o processo com estado `Bloqueado` e o coloco em `ListaDeProcessosBloqueados`. Caso contrário, aloco memória (subtraio do total) e adiciono o processo em `ListaDeProcessosProntos`, e chamo `CriarThreadsDinamicamente()` no processo.
    - Incremento `TotalProcessesCreated`.
  - `FinalizarProcesso(Processo processo)`:
    - Marco o processo como `Finalizado`, restauro a memória que estava alocada para ele e removo-o da lista de prontos se necessário. Atualizo `FinishedAt` e `TotalProcessesFinished`, e adiciono à lista de finalizados.
    - Após liberar memória, verifico processos bloqueados por falta de memória e tento desbloqueá-los (se couber a memória liberada).
  - `TentarDesbloquearProcessos(Processo p)`:
    - Retorna true se `p.MemoriaTotalDoProcesso <= QuantidadeMemoriaRAM_Sistema`.
  - `EscolherProximoProcesso(string algoritmo)`:
    - Atualizo o indicador de algoritmo e delego ao `escalonadorSistema` a escolha do próximo processo.
  - `SolicitarRecursos(Processo solicitante, Recursos recurso)`:
    - Verifico se o processo que solicita já possui recursos com IDs maiores — se ele pedir de forma fora de ordem, executo um "backoff" (libero recursos que possui) para diminuir chance de deadlock por ordem incorreta.
    - Se o recurso estiver livre, aloco ao processo; se estiver ocupado, marco o processo como `Bloqueado`, registro `BlockedSince` e o adiciono em `ListaDeProcessosBloqueados`.
  - `LiberarRecursos(Processo processoLiberador, Recursos? recursoFinalizado = null)`:
    - Se `recursoFinalizado` for null, libero todos os recursos que o processo possui; caso contrário libero apenas o recurso indicado (desde que o processo realmente o possua).
    - Ao liberar, verifico a lista de processos bloqueados que aguardavam esse recurso. Para o primeiro processo esperando, aloco este recurso e o movo para a lista de prontos. Isso reflete políticas simples de waking up.
  - `ExecutarProcesso(Processo processo)`:
    - Com 50% de chance, tento alocar um recurso aleatório antes de rodar (simulando um pedido de I/O). Se não conseguir, o processo fica bloqueado e não consome CPU neste passo.
    - Decido os ciclos permitidos: se o algoritmo é `RR` uso o `Quantum` configurado; caso contrário uso `int.MaxValue` como "rodar até terminar" no modelo atual.
    - Registro `StartedAt` se necessário e incremento `ContextSwitches`.
    - Chamo `cpuSistema.ExecutarThreads(process.ListaDeMinhasThreads, ciclosRestantes)`.
    - Atualizo `processo.TotalCiclosExecutados` com a diferença do clock antes/depois.
    - Chamo `processo.CalcularEstadoProcesso()`; se o processo finalizou, libero seus recursos e chamo `FinalizarProcesso`. Caso contrário, reinsiro o processo na fila de prontos (append se RR, insert(0) para outros algoritmos, preservando comportamento demonstrativo).
  - `VerificarTimeoutsBloqueados()`:
    - Percorro cópia da lista de bloqueados e, se um processo ficar bloqueado por mais de 2000 ms, aplico backoff: limpo sua lista de recursos esperados, libero recursos que possui, tento alocar memória se ele estava bloqueado por memória e, se possível, movo-o para prontos. Isso evita bloqueios infinitos por espera de recursos ou falta de memória.

6) Processo e threads internas
- `Processo`:
  - Campos que guardo: `CreatedAt`, `StartedAt`, `FinishedAt`, `TotalCiclosExecutados`, `IdentificadorProcesso` (GUID curto), `EstadosProcessoAtual`, `MemoriaTotalDoProcesso`, `MemoriaFoiAlocada`, `ListaDeMinhasThreads`, `ListaDeRecursosAlocados`, `ListaDeRecursosEsperados`, `BlockedSince`.
  - `CriarThreadsDinamicamente()`:
    - Divido a `MemoriaTotalDoProcesso` entre várias threads. Cada thread recebe uma fatia entre 10% e 25% da memória restante até que a memória do processo seja totalmente distribuída. Essa divisão cria múltiplas `MinhasThreads` demonstrando execução interna concorrente do processo.
    - Para cada thread criada, registro no log sua identificação, memória consumida e ciclos iniciais.
  - `CalcularEstadoProcesso()`:
    - Se todas as `MinhasThreads` estiverem `Finalizado`, então o processo passa a `Finalizado`. Caso contrário, eu defino o processo como `Pronto`.
- `MinhasThreads`:
  - Cada thread simulada tem `IdentificadorThread` (GUID curto), `MemoriaConsumida`, `EstadoThread` e `CiclosRestantes`.
  - No construtor, seto `CiclosRestantes` como um número aleatório entre 1 e 99 para variar o tempo de vida das threads.
  - `ExecutarMinhaThread()` decrementa `CiclosRestantes` e retorna `true` quando chega a zero (indicando finalização da thread).
  - Essas threads não são threads de sistema; são apenas objetos com estado e contadores que o `CPU` consome.

7) CPU simulada
- `CPU`:
  - Mantenho um contador `CicloClockAtual` global.
  - `ExecutarThreads(List<MinhasThreads> threadsParaExecutar, int ciclosParaRodar, bool usoDeQuantum = false)`:
    - Para cada thread não finalizada, marco `Executando` e então entro em um loop interno consumindo `quantumRestante` (inicialmente igual a `ciclosParaRodar`).
    - Em cada iteração, incremento `CicloClockAtual` e decremento `quantumRestante`, chamo `ExecutarMinhaThread()`.
    - Se `quantumRestante` chega a zero, faço debug e paro (essa é a preempção do quantum no RR).
    - Se a thread terminar, marco como `Finalizado` e sigo para próxima. Se parar por quantum, marco a thread atual como `Pronto` (pausada).
    - O comportamento garante que um processo, que contém N threads, terá as suas threads executadas em sequência até o quantum acabar (modelo simples, adequado para visualização).

8) Escalonador
- `Escalonador.EscolherProximoProcesso(List<Processo> listaDeProntos, string algoritimoEscolhido)`:
  - `FCFS`: retorno o primeiro processo da lista (FIFO).
  - `SJF`: retorno o processo com menor `MemoriaTotalDoProcesso` (no modelo uso o tamanho de memória como proxy para "tempo estimado" — é uma simplificação).
  - `RR`: retorno o primeiro da fila; a lógica de reinserção no fim é tratada em `ExecutarProcesso`.
  - Se o sistema receber um algoritmo desconhecido, ele logga erro e retorno null.

9) Integração com a interface e controle da simulação
- A interface principal controla o ciclo da simulação, exibe tabelas com `ProcessoViewModel` para as listas de prontos/bloqueados/finalizados, exibe `txtLog` com as mensagens do `Logger` e exibe as métricas calculadas.
- O `BindingList<ProcessoViewModel>` e usado para popular as grids, e uma classe `ProcessoViewModel.FromProcesso()` para transformar os objetos `Processo` em linhas amigáveis.
- Para execução contínua tem um timer com pequeno intervalo (no código o intervalo é 20 ms). Esse timer faz a chamada periódica ao procedimento que escolhe o próximo processo e chama `ExecutarProcesso`.
- também existe um parser simples de linha de comando interno (`ProcessCommand(string cmd)`) que aceita comandos como:
  - `criar <n>` / `create <n>` — cria N processos;
  - `rodar` / `run` — alterna execução contínua (start/stop);
  - `passo` / `step` — executa um passo único;
  - `liberar <nomeOuId>` / `release` — libera recurso específico;
  - `status` — força atualização da interface com dados atuais.
- Para enviar logs do sistema para a área de logs, temos a implementação de um `ControlWriter` (subclasse de `TextWriter`) e faço `Console.SetOut(new ControlWriter(AppendLog))` para que todas as chamadas `Console.Write`/`Logger` apareçam na área textual.

10) Métricas que são exibidas e seus calculos
- Apos o calculo, mostro as métricas na interface com os seguintes critérios:
  - `TotalProcessesCreated` — contador simples incrementado em `CriarProcesso`.
  - `TotalProcessesFinished` — incrementado em `FinalizarProcesso`.
  - `ContextSwitches` — incremento cada vez que um processo começa sua fatia de execução (uso para mostrar trocas de contexto).
  - `Throughput (proc/s)` — calculado como `TotalProcessesFinished / (DateTime.Now - SimulationStart).TotalSeconds`.
  - `Tempo médio turnaround` — média de `(FinishedAt - CreatedAt)` em milissegundos para processos finalizados.
  - `Tempo médio de espera (approx)` — aproximado como `turnaround_ms - cpu_cycles_ms` (no modelo cycles são contadores não necessariamente tempo real, então isto é uma aproximação).
  - `Média ciclos CPU` — média de `TotalCiclosExecutados` dos processos finalizados.

11) Comportamentos principais que o usuário verá
- Criação de processos com memória variável, que podem entrar em `Pronto` ou `Bloqueado` de imediato (se a memória requerida exceder o disponível).
- Cada processo cria internamente um conjunto de threads simuladas. Essas threads têm ciclos independentes formando o trabalho do processo.
- Durante a execução, processos podem requisitar recursos (50% de chance antes de executar em cada fatia), sendo bloqueados se o recurso estiver ocupado.
- Quando um processo finaliza, memória e recursos são liberados e a liberação pode permitir que processos bloqueados por memória voltem para prontos.
- Processos bloqueados por recursos são acordados quando o recurso é liberado; a política é acordar o primeiro processo que aguardava aquele recurso.
- Timeouts em bloqueio (2s) forçam backoff e tentativas de re-alocação, evitando bloqueios indefinidos no cenário didático.

12) Comandos de execução e interatividade
- Foram implementados um pequeno conjunto de comandos textuais para facilitar demonstração e testes:
  - `criar 3` — cria 3 processos.
  - `passo` / `step` — executa a próxima fatia (um step).
  - `rodar` / `run` — inicia/parar execução contínua.
  - `liberar Impressora` — libera recurso especificado (por nome ou id).
  - `status` — atualiza vistas com o estado atual das filas e métricas.

13) Informações finais em primeira pessoa
- Este simulador é propositalmente didático: todas as decisões de modelagem (por exemplo usar memória como heurística para SJF, dividir memória entre threads, usar backoff por ordem de ID) foram feitas para criar cenários interessantes para aulas e experimentos sem depender de threads reais do sistema.
- Os logs detalhados via são dirigidos via `Logger` para que qualquer execução possa ser acompanhada passo a passo no txt de logs.

## Contato / Créditos
Nome: Pedro Paulo Santos de Jesus Junior
RA: 113538

Nome: Gabriel Marques Paulon
RA: 113142

--- 
