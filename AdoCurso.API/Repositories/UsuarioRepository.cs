﻿using AdoCurso.API.Models.Entities;
using AdoCurso.API.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AdoCurso.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _connectionDB;
        public UsuarioRepository()
        {
            _connectionDB = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AdoCurso;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        }

        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Usuarios",
                                                    (SqlConnection)_connectionDB);

                _connectionDB.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = reader.GetInt32("Id");
                    usuario.Nome = reader.GetString("Nome");
                    usuario.Email = reader.GetString("Email");
                    usuario.Sexo = reader.GetString("Sexo");
                    usuario.RG = reader.GetString("RG");
                    usuario.CPF = reader.GetString("CPF");
                    usuario.NomeMae = reader.GetString("NomeMae");
                    usuario.SituacaoCadastro = reader.GetString("SituacaoCadastro");
                    usuario.DataCadastro = reader.GetDateTimeOffset(8);

                    usuarios.Add(usuario);
                }
            }
            finally
            {
                _connectionDB.Close();
            }

            return usuarios;
        }

        public Usuario GetById(int id)
        {
            Usuario usuario = new Usuario();
            Contato contato = new Contato();

            try
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Usuarios u LEFT JOIN Contatos c " +
                                                    $"ON c.UsuarioId = u.Id WHERE u.Id = @Id;",
                                                    (SqlConnection)_connectionDB);
                // Segurança contra SQL Injection
                command.Parameters.AddWithValue("@Id", id);

                _connectionDB.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuario.Id = reader.GetInt32("Id");
                    usuario.Nome = reader.GetString("Nome");
                    usuario.Email = reader.GetString("Email");
                    usuario.Sexo = reader.GetString("Sexo");
                    usuario.RG = reader.GetString("RG");
                    usuario.CPF = reader.GetString("CPF");
                    usuario.NomeMae = reader.GetString("NomeMae");
                    usuario.SituacaoCadastro = reader.GetString("SituacaoCadastro");
                    usuario.DataCadastro = reader.GetDateTimeOffset(8);

                    contato.Id = reader.GetInt32(9); // Irá pegar a nona coluna da consulta que é o ID do contato.
                    contato.Celular = reader.GetString("Celular");
                    contato.Telefone = reader.GetString("Telefone");
                    contato.UsuarioId = usuario.Id;

                    usuario.Contato = contato; // Atribui o contato buscado no banco e atribui a propriedade Contato do objeto Usuarios.

                    return usuario;
                }
            }
            finally
            {
                _connectionDB.Close();
            }

            return null;
        }

        public void Insert(Usuario usuario)
        {
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) " +
                                                    $"VALUES(@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro);" +
                                                    $"SELECT CAST(scope_identity() AS int);", // Irá pegar o último id do escopo da tabela Usuarios.
                                                    (SqlConnection)_connectionDB);

                // Segurança contra SQL Injection
                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                command.Parameters.AddWithValue("@RG", usuario.RG);
                command.Parameters.AddWithValue("@CPF", usuario.CPF);
                command.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                command.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);


                _connectionDB.Open();
                usuario.Id = (int)command.ExecuteScalar(); // irá atribuir o último id da tabela ao id do usuario recem criado.
            }
            finally
            {

                _connectionDB.Close();

            }
        }

        public void Update(Usuario usuario)
        {
            try
            {
                SqlCommand command = new SqlCommand($"UPDATE Usuarios SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @RG, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @DataCadastro " +
                                                    $"WHERE Usuarios.Id = @Id;",
                                                    (SqlConnection)_connectionDB);

                // Segurança contra SQL Injection
                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                command.Parameters.AddWithValue("@RG", usuario.RG);
                command.Parameters.AddWithValue("@CPF", usuario.CPF);
                command.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                command.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);

                command.Parameters.AddWithValue("@Id", usuario.Id);

                _connectionDB.Open();
                command.ExecuteNonQuery();

            }
            finally
            {

                _connectionDB.Close();

            }
        }

        public void Delete(int id)
        {
            try 
            {
                SqlCommand command = new SqlCommand($"DELETE Usuarios WHERE Usuarios.Id = @Id;",
                                                    (SqlConnection)_connectionDB);

                command.Parameters.AddWithValue("@Id", id);

                _connectionDB.Open();
                command.ExecuteNonQuery();
            } 
            finally 
            {
                _connectionDB.Close();
            }
        }
    }
}
