using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppAgendaDeTarefas;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string descricao;
    private int i;
    private List<String> listaTarefas = new List<String>();

    // Lista para armazenar as datas de criação das tarefas
    private List<DateTime> datasCriacao = new List<DateTime>();

    // Lista de categorias para preencher o ComboBox (necessita de um ComboBox no XAML)
    public List<string> ListaDeCategorias { get; set; }

    // Propriedade para capturar a seleção do ComboBox via Binding (necessita de Binding no XAML)
    public string CategoriaAtual { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        ListaDeCategorias = new List<string>
        {
            "Manutenção Pessoal",
            "Higiene",
            "Manutenção do Ambiente",
            "Compras & Suprimentos",
            "Administrativo & Organização",
            "Comunicação & Foco"
        };

        // 2. Define um valor padrão para CategoriaAtual
        CategoriaAtual = ListaDeCategorias.FirstOrDefault() ?? "Sem Categoria";

        // 3. Define o contexto de dados para os bindings (Necessário para o ComboBox e CategoriaAtual)
        DataContext = this;
    }

    /// <summary>
    /// funcionalidade inicial 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Adicionar_Click(object sender, RoutedEventArgs e)
    {
        // 1. Pega o texto da tarefa e a categoria selecionada
        string novaTarefa = TxtTarefa.Text.Trim();
        string categoriaSelecionada = CategoriaAtual ?? "Sem Categoria";

        if (!string.IsNullOrWhiteSpace(novaTarefa))
        {
            // MODIFICAÇÃO: Adiciona data/hora à tarefa
            DateTime dataCriacao = DateTime.Now;
            string itemFormatado =
                $" {novaTarefa} [{categoriaSelecionada}] - Criada em: {dataCriacao:dd/MM/yyyy HH:mm}";

            ListaTarefas.Items.Add(itemFormatado);
            listaTarefas.Add(itemFormatado);
            datasCriacao.Add(dataCriacao); // ← Armazena a data de criação

            TxtTarefa.Clear();
            TxtTarefa.Focus();
        }
        else
        {
            MessageBox.Show("Digite uma tarefa válida!");
        }

        ContadorTarefa();
    }

    private void Limpar_lista_Click(object sender, RoutedEventArgs e)
    {
        if (ListaTarefas.Items.Count > 0)
        {
            ListaTarefas.Items.Clear();

            MessageBox.Show("A lista foi limpa com sucesso!", "Limpeza");
        }

        TbxContadorTarefas.Text = String.Empty;
    }



    private void Editartarefa_OnClick(object sender, RoutedEventArgs e)
    {
        // Verifica se existe uma tarefa selecionada
        if (ListaTarefas.SelectedItem != null)
        {
            // VALIDAÇÃO ADICIONADA: Verifica se o campo de texto não está vazio
            if (string.IsNullOrWhiteSpace(TxtTarefa.Text))
            {
                MessageBox.Show("Digite um texto válido para editar a tarefa!", "Campo Vazio");
                return; // Sai do método se o campo estiver vazio
            }

            // Pega o índice do item selecionado
            int index = ListaTarefas.SelectedIndex;

            // Edita a tarefa no ListBox usando o texto do TextBox de edição
            ListaTarefas.Items[index] = TxtTarefa.Text;

            MessageBox.Show("Tarefa editada com sucesso!");
        }
        else
        {
            MessageBox.Show("Selecione uma tarefa para editar!");
        }
    }

    //Botão de duplicar tarefa selecionada
    private void BtnDuplicar(object sender, RoutedEventArgs e)
    {
        if (ListaTarefas.SelectedItem != null)
        {
            string duplicata = ListaTarefas.SelectedItem.ToString();
            ListaTarefas.Items.Add(duplicata);

            MessageBox.Show("Tarefa Duplicada com sucesso!", "Duplicar", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Selecione uma tarefa para duplicar!", "Duplicar", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        ContadorTarefa();
    }

    private void ContadorTarefa()
    {
        for (i = 1; i <= ListaTarefas.Items.Count; i++)
        {
            TbxContadorTarefas.Text = i.ToString();
        }
    }

    private void BtnMoverTarefaUp_Click(object sender, RoutedEventArgs e)
    {
        // Obtem o índice da tarefa selecionada.
        int selectedIndex = ListaTarefas.SelectedIndex;

        //Verifica se não existe nenhuma tarefa selecionada.
        if (selectedIndex == -1)
        {
            MessageBox.Show("Selecione uma tarefa para movimentá-la!",
                "Mover", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        //Verifica se a tarefa já está no topo
        if (selectedIndex == 0)
        {
            MessageBox.Show("A tarefa já está posicionada no topo da lista!",
                "Mover", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string tarefaParaMover = ListaTarefas.SelectedItem as string;

        // Remove a tarefa da posição atual
        ListaTarefas.Items.RemoveAt(selectedIndex);

        // Insere a tarefa na posição anterior
        ListaTarefas.Items.Insert(selectedIndex - 1, tarefaParaMover);

        // Atualiza a seleção para o novo índice
        ListaTarefas.SelectedIndex = selectedIndex - 1;

        MessageBox.Show("Tarefa movida para cima com sucesso!",
            "Mover", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void BtnMoverTarefaDown_Click(object sender, RoutedEventArgs e)
    {
        // Obtem o índice da tarefa selecionada.
        int selectedIndex = ListaTarefas.SelectedIndex;

        //Verifica se não existe nenhuma tarefa selecionada.
        if (selectedIndex == -1)
        {
            MessageBox.Show("Selecione uma tarefa para movimentá-la!",
                "Mover", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        //Verifica se a tarefa já está no final da lista
        if (selectedIndex == ListaTarefas.Items.Count - 1)
        {
            MessageBox.Show("A tarefa já está posicionada no final da lista!",
                "Mover", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }


        string tarefaParaMover = ListaTarefas.SelectedItem as string;

        // Remove a tarefa da posição atual
        ListaTarefas.Items.RemoveAt(selectedIndex);

        // Insere a tarefa na posição anterior
        ListaTarefas.Items.Insert(selectedIndex + 1, tarefaParaMover);

        // Atualiza a seleção para o novo índice
        ListaTarefas.SelectedIndex = selectedIndex + 1;

        MessageBox.Show("Tarefa movida para baixo com sucesso!",
            "Mover", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void BtnConcluida_OnClick(object sender, RoutedEventArgs e)
    {
        if (ListaTarefas.SelectedItem != null)
        {
            int index = ListaTarefas.SelectedIndex;
            string selecao = ListaTarefas.SelectedItem.ToString();

            if (!selecao.EndsWith("✅"))
            {
                selecao += "✅";
                MessageBox.Show($"Status da tarefa: Concluida!",
                    "Gerenciador de Tarefas", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ListaTarefas.Items[index] = selecao;
        }
    }


    private void BtnExportarTxt_Click(object sender, RoutedEventArgs e)
    {
        if (ListaTarefas.Items.Count == 0)
        {
            MessageBox.Show("Não há tarefas para exportar.", "Exportar TXT",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = "tarefas.txt",
            Filter = "Arquivo de texto (*.txt)|*.txt",
            Title = "Exportar lista de tarefas"
        };

        if (dialog.ShowDialog() == true)
        {
            List<string> linhas = new List<string>();

            foreach (var item in ListaTarefas.Items)
                linhas.Add(item.ToString());

            File.WriteAllLines(dialog.FileName, linhas, Encoding.UTF8);

            MessageBox.Show("Arquivo salvo com sucesso!", "Exportar TXT",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }


    private void BtnImportarTxt_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Arquivo de texto (*.txt)|*.txt",
            Title = "Importar lista de tarefas"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                string[] linhas = File.ReadAllLines(dialog.FileName, Encoding.UTF8);

                foreach (string tarefa in linhas)
                {
                    if (!string.IsNullOrWhiteSpace(tarefa))
                    {
                        ListaTarefas.Items.Add(tarefa);
                        listaTarefas.Add(tarefa);
                    }
                }

                MessageBox.Show("Tarefas importadas com sucesso!", "Importar TXT",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao importar arquivo: {ex.Message}", "Erro",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void BtnBuscar_OnClick(object sender, RoutedEventArgs e)
    {
        string texto = TxtBusca.Text?.ToLower() ?? "";

        // Se caixa vazia → volta a lista completa
        if (string.IsNullOrWhiteSpace(texto))
        {
            AtualizarLista();
            return;
        }

        // Filtramos pela lista original
        var filtradas = listaTarefas
            .Where(t => t.ToLower().Contains(texto))
            .ToList();

        // Limpa visual da ListBox
        ListaTarefas.Items.Clear();

        // Repreenche com resultados filtrados
        foreach (var t in filtradas)
            ListaTarefas.Items.Add(t);
    }

    private void AtualizarLista()
    {
        ListaTarefas.Items.Clear();

        foreach (var t in listaTarefas)
            ListaTarefas.Items.Add(t);
    }



    private void btnTema_Click(object sender, RoutedEventArgs e)
    {

        string textoDoBotao = btnTema.Content.ToString();

        if (textoDoBotao == "Modo Escuro")
        {

            this.Background = Brushes.DimGray; // Fundo da Janela

            TxtTarefa.Background = Brushes.DimGray;
            TxtTarefa.Foreground = Brushes.White;

            TxtTarefa.Background = Brushes.DimGray;
            TxtTarefa.Foreground = Brushes.White;

            btnTema.Content = "Modo Claro";
            btnTema.Background = Brushes.LightGray;
        }
        else
        {
            this.Background = Brushes.White; // Volta o fundo branco

            TxtTarefa.Background = Brushes.White;
            TxtTarefa.Foreground = Brushes.Black;

            TxtTarefa.Background = Brushes.White;
            TxtTarefa.Foreground = Brushes.Black;

            btnTema.Content = "Modo Escuro";
        }
    }


    private void OrdenarZaA_OnClick(object sender, RoutedEventArgs e)
    {
        listaTarefas = listaTarefas
            .OrderByDescending(x => x)
            .ToList();

        // Atualiza a ListBox
        ListaTarefas.Items.Clear();
        foreach (var tarefa in listaTarefas)
            ListaTarefas.Items.Add(tarefa);
    }

    private void OrdenarAaZ_OnClick(object sender, RoutedEventArgs e)
    {
        listaTarefas = listaTarefas
            .OrderBy(x => x) // Ordem crescente (A → Z)
            .ToList();

        // Atualiza a ListBox
        ListaTarefas.Items.Clear();
        foreach (var tarefa in listaTarefas)
            ListaTarefas.Items.Add(tarefa);
    }


    private void Remover_Click(object sender, RoutedEventArgs e)
    {

        if (ListaTarefas.SelectedItem != null)
        {

            MessageBoxResult resultado = System.Windows.MessageBox.Show(
                "Tem certeza que deseja excluir a tarefa selecionada?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );


            if (resultado == MessageBoxResult.Yes)
            {

                ListaTarefas.Items.Remove(ListaTarefas.SelectedItem);
            }

        }
        else
        {

            MessageBox.Show("Selecione uma tarefa para excluir.");
        }


    }
}