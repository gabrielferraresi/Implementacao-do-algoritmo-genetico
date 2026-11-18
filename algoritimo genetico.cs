using System;
using System.Linq;

class Program
{
    const int QuantidadeBlocos = 4;

    static void Main()
    {
        int total = LerInteiro(
            "Digite quantos individuos voce quer?",
            n => n >= 2 && n % 2 == 0,
            "Digite uma quantidade par de individuos, maior ou igual a 2:");

        int tamanhoBloco = LerInteiro(
            "Quantos genes cada bloco irá ter?",
            n => n >= 2,
            "Digite pelo menos 2 genes por bloco:");

        int tamanhoTotal = tamanhoBloco * QuantidadeBlocos;

        int[][] pais = GerarPopulacao(total, tamanhoTotal);

        for (int i = 0; i < pais.Length; i++)
            Imprimir($"Indivíduo {i + 1}", pais[i], tamanhoBloco);

        int corte = LerInteiro(
            $"A partir de que ponto dos genes voce quer misturar os filhos?\nEntre 1 e {tamanhoBloco - 1} (dentro de cada bloco)",
            n => n >= 1 && n < tamanhoBloco,
            "digite um ponto de mistura valido!");

        int[][] filhos = Cruzar(pais, tamanhoBloco, corte);

        for (int i = 0; i < filhos.Length; i++)
            Imprimir($"Filho {i + 1}", filhos[i], tamanhoBloco);
    }

    // Lê um inteiro e repete a pergunta até a condição ser atendida
    static int LerInteiro(string pergunta, Func<int, bool> valido, string mensagemErro)
    {
        Console.WriteLine(pergunta);

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int valor) && valido(valor))
                return valor;

            Console.WriteLine(mensagemErro);
        }
    }

    // Cria a população com genes aleatórios (0 ou 1)
    static int[][] GerarPopulacao(int total, int tamanho)
    {
        return Enumerable.Range(0, total)
            .Select(_ => Enumerable.Range(0, tamanho)
                .Select(__ => Random.Shared.Next(2))
                .ToArray())
            .ToArray();
    }

    // Cruza os indivíduos de dois em dois
    static int[][] Cruzar(int[][] pais, int tamanhoBloco, int corte)
    {
        var filhos = new int[pais.Length][];

        for (int par = 0; par < pais.Length; par += 2)
        {
            int[] a = pais[par];
            int[] b = pais[par + 1];

            filhos[par] = new int[a.Length];
            filhos[par + 1] = new int[a.Length];

            for (int g = 0; g < a.Length; g++)
            {
                bool antesDoCorte = g % tamanhoBloco < corte;

                filhos[par][g] = antesDoCorte ? a[g] : b[g];
                filhos[par + 1][g] = antesDoCorte ? b[g] : a[g];
            }
        }

        return filhos;
    }

    // Mostra os genes separados por blocos
    static void Imprimir(string titulo, int[] genes, int tamanhoBloco)
    {
        var blocos = genes
            .Chunk(tamanhoBloco)
            .Select(b => string.Join(" ", b));

        Console.WriteLine($"{titulo}: {string.Join(" | ", blocos)}");
    }
}