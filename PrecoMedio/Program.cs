Console.WriteLine("=== INICIANDO PREÇO MÉDIO ===");

var papel = new Papel();
papel.Id = 1;
papel.Nome = "LVS11";

var anotacaoUm = new Anotacao(1, null, Operacao.COMPRA, papel, 10M, 100M, 1.5M, 0M);

Console.WriteLine("=== RESULTADO PRIMEIRO PREÇO MÉDIO ===");

anotacaoUm.ImprimirAnotacao();


var anotacaoDois = new Anotacao(2, anotacaoUm, Operacao.COMPRA, papel, 5M, 102M, 1.5M, 0M);

Console.WriteLine("=== RESULTADO SEGUNDO PREÇO MÉDIO ===");

anotacaoDois.ImprimirAnotacao();

var anotacaoTres = new Anotacao(3, anotacaoDois, Operacao.COMPRA, papel, 20M, 90M, 1.5M, 0M);

Console.WriteLine("=== RESULTADO TERCEIRO PREÇO MÉDIO ===");

anotacaoTres.ImprimirAnotacao();

var anotacaoQuatro = new Anotacao(4, anotacaoTres, Operacao.VENDA, papel, 15M, 100M, 3M, 2M);

Console.WriteLine("=== RESULTADO QUARTO PREÇO MÉDIO ===");

anotacaoQuatro.ImprimirAnotacao();

//var anotacaoCinco = new Anotacao(5, anotacaoQuatro, Operacao.VENDA, papel, 25M, 100M, 5M, 3M);

//Console.WriteLine("=== ENCERRANDO POSIÇÃO ===");

//anotacaoCinco.ImprimirAnotacao();

var anotacaoCinco = new Anotacao(5, anotacaoQuatro, Operacao.COMPRA, papel, 20M, 94.7M, 0M, 0M);

Console.WriteLine("=== VERIFICANDO PREÇO MÉDIO ===");

anotacaoCinco.ImprimirAnotacao();


Console.WriteLine("=== TERMINOU PREÇO MÉDIO ===");

Console.ReadKey();



public class Anotacao
{
    //public Anotacao() { }

    public Anotacao(
            long operacaoId,
            Anotacao operacaoAnterior,
            Operacao operacao,
            Papel papel,
            decimal quantidade,
            decimal preco,
            decimal taxa,
            decimal dedoDuro
        )
    {
        if(quantidade < 0)
        {
            throw new Exception(@$"
                                        
                    ===================================
                    Quantidade deve ser maior que Zero.
                    ===================================
                                        ");
        }

        Id = operacaoId;
        Data = DateTime.Now;
        Operacao = operacao;
        Papel = papel;
        DedoDuro = dedoDuro;
        Quantidade = quantidade;
        Preco = preco;
        Taxa = taxa;

        if (operacao == Operacao.COMPRA)
        {
            TotalOperacao = quantidade * preco + taxa;

            DedoDuro = 0M;
        }
        else if (operacao == Operacao.VENDA)
        {
            TotalOperacao = quantidade * preco - taxa;

            DedoDuro = dedoDuro;
        }

        if (operacaoAnterior != null)
        {
            if (operacao == Operacao.COMPRA)
            {
                SaldoQuantidade = operacaoAnterior.SaldoQuantidade + quantidade;
                TotalInvestido = operacaoAnterior.TotalInvestido + TotalOperacao;
                PrecoMedio = TotalInvestido / SaldoQuantidade;
            }
            else if (operacao == Operacao.VENDA)
            {
                SaldoQuantidade = operacaoAnterior.SaldoQuantidade - quantidade;

                if (SaldoQuantidade < 0)
                {
                    throw new Exception(@$"
                                        
                    ===================
                    Quantidade inválida:
                    Você está tentando 
                    vender ${quantidade} cota(s)
                    mas possui {operacaoAnterior.SaldoQuantidade} cota(s)!
                    ===================
                                        ");
                }

                PrecoMedio = SaldoQuantidade == 0 ?  0 : operacaoAnterior.PrecoMedio;
                TotalInvestido = PrecoMedio * SaldoQuantidade;
            }
        }
        else if (operacaoAnterior == null)
        {
            SaldoQuantidade = quantidade;
            TotalInvestido = TotalOperacao;
            PrecoMedio = TotalOperacao / SaldoQuantidade;
        }
    }

    public long Id { get; set; }

    public DateTime Data { get; set; }

    public Papel Papel { get; set; }

    public Operacao Operacao { get; set; }

    public decimal Quantidade { get; set; }

    public decimal Preco { get; set; }

    public decimal Taxa { get; set; }

    public decimal TotalOperacao { get; set; }

    public decimal DedoDuro { get; set; }

    public decimal SaldoQuantidade { get; set; }

    public decimal PrecoMedio { get; set; }

    public decimal TotalInvestido { get; set; }


    public void ImprimirAnotacao()
    {
        Console.WriteLine(String.Format($@"
                Código: {this.Id}
                Data: {this.Data.ToString("dd/MM/yyyy")}
                Operação: {this.Operacao}
                Papel: {this.Papel.Nome}
                Quantidade: {this.Quantidade}
                Preço: {this.Preco.ToString("C")}
                Taxa: {this.Taxa.ToString("C")}
                Total Operação: {this.TotalOperacao.ToString("C")}
                Dedo Duro: {this.DedoDuro.ToString("C")}
                Saldo de Cotas: {this.SaldoQuantidade}
                Preço Médio: {this.PrecoMedio.ToString("C")}
                Total Investido: {this.TotalInvestido.ToString("C")}
            ")); 
    }
}

public class Papel
{
    public long Id { get; set; }

    public string Nome { get; set; }
}

public enum Operacao : short
{
    COMPRA = 1,
    VENDA = 2,
    SUBSCRICAO = 3,
    AMORTIZACAO = 4,
    SPLIT = 5,
    AGRUPAMENTO = 6
}



