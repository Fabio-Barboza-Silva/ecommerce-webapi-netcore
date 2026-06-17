# 🎉 Ecommerce Web API - .NET Core

Este é um projeto de uma **Web API robusta** para gerenciamento de um E-commerce, 
desenvolvida em C# utilizando as melhores práticas de mercado e arquitetura em camadas. 
O sistema realiza consultas, cadastros e processa vendas aplicando regras de negócio complexas 
com integração a banco de dados relacional.

---

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C# (.NET 8)
* **Paradigma:** Programação Orientada a Objetos (POO)
* **Persistência:** Entity Framework Core (ORM)
* **Banco de Dados:** SQL Server
* **Arquitetura:** Web API RESTful organizada em camadas (`Models`, `Data`)
* **Testes Automatizados:** Arquivos nativos `.http` para validação de endpoints

---

## 🛡️ Regras de Negócio Implementadas

* **Validação de Estoque Dinâmica:** O sistema intercepta requisições de venda e valida a quantidade
* disponível no banco de dados. Caso o estoque seja insuficiente, a API barra a transação utilizando
*  tratamento de exceções adequado e retorna um HTTP Status `400 Bad Request` com uma mensagem amigável
*  para o cliente, impedindo inconsistências no SQL Server.

---

## 🚀 Como Executar e Testar o Projeto

1. Clone o repositório para sua máquina local.
2. Certifique-se de ter o **SQL Server** configurado e ajuste a string de conexão no `EcommerceContext`.
3. Execute o comando `dotnet run` ou utilize o Visual Studio para iniciar o servidor local.
4. Utilize o arquivo incluso `teste.http` dentro do próprio Visual Studio para disparar requisições `GET` e `POST` diretamente para a API.
