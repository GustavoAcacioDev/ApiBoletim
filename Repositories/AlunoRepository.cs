using APIBoletim.Context;
using APIBoletim.Domains;
using APIBoletim.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIBoletim.Repositories
{
    public class AlunoRepository : IAluno
    {
        //Chamamos a classe de conexão com o banco
        BoletimContext conexao = new BoletimContext();

        //Chamamos o objeto que poderá receber e executar os comandos do banco
        SqlCommand cmd = new SqlCommand();


        public Aluno Alterar(int id, Aluno a)
        {
            cmd.Connection = conexao.Conectar();

            cmd.CommandText = "UPDATE Aluno SET " +
                "Nome = @nome, " +
                "Ra = @ra, " +
                "Idade = @idade WHERE IdAluno = @id" ;
                
            cmd.Parameters.AddWithValue("@nome", a.Nome);
            cmd.Parameters.AddWithValue("@ra", a.RA);
            cmd.Parameters.AddWithValue("@idade", a.Idade);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            conexao.Desconectar();
            return a;
        }

        public Aluno BuscarPorId(int id)
        {
            cmd.Connection = conexao.Conectar();

            //Comando do banco de dados
            cmd.CommandText = "SELECT * FROM Aluno WHERE IdAluno = @id ";

            //Atribuição da variável do comando '@id' com a que vem do argumento
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader dados = cmd.ExecuteReader();

            Aluno a = new Aluno();

            while (dados.Read())
            {
                a.IdAluno   = Convert.ToInt32(dados.GetValue(0));
                a.Nome      = dados.GetValue(1).ToString();
                a.RA        = dados.GetValue(2).ToString();
                a.Idade  = Convert.ToInt32(dados.GetValue(3));
            }

            cmd.Connection = conexao.Desconectar();

            return a;
        }

        public Aluno Cadastrar(Aluno a)
        {
            cmd.Connection = conexao.Conectar();

            cmd.CommandText =
                "INSERT INTO Aluno (Nome, RA, Idade) " +
                "VALUES" +
                "(@nome, @ra, @idade)";
            cmd.Parameters.AddWithValue("@nome", a.Nome);
            cmd.Parameters.AddWithValue("@ra", a.RA);
            cmd.Parameters.AddWithValue("@idade", a.Idade);

            // Comando para injetar os dados no banco / DML
            cmd.ExecuteNonQuery();

            cmd.Connection = conexao.Desconectar();
            return a;
        }

        public void Excluir(int id)
        {
            cmd.Connection = conexao.Conectar();

            cmd.CommandText = "DELETE FROM Aluno WHERE IdAluno = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            conexao.Desconectar();
        }

        public List<Aluno> LerTodos()
        {
            //Abrir conexão
            cmd.Connection = conexao.Conectar();

            //Preparar a query
            cmd.CommandText = "SELECT * FROM Aluno";

            SqlDataReader dados = cmd.ExecuteReader();

            //Criamos a lista pra guardar os alunos
            List<Aluno> alunos = new List<Aluno>();

            while (dados.Read())
            {
                alunos.Add(
                    new Aluno()
                    {
                        IdAluno     = Convert.ToInt32(dados.GetValue(0)),
                        Nome        = dados.GetValue(1).ToString(),
                        RA          = dados.GetValue(2).ToString(),
                        Idade       = Convert.ToInt32(dados.GetValue(3))
                    }
                );
            }

            //Fechar conexão
            conexao.Conectar();

            return alunos;
        }
    }
}
