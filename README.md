# Loja Virtual
# 🛒 UVVirtual — Sistema de Loja Virtual

A **UVVirtual** é uma aplicação desktop de loja virtual desenvolvida como projeto acadêmico da disciplina de Programação Orientada a Objetos II, do curso de Ciência da Computação.

O projeto tem como objetivo simular o funcionamento de uma plataforma de comércio eletrônico, permitindo que clientes realizem compras e acompanhem seus pedidos, enquanto administradores gerenciam produtos, cupons e o andamento das vendas.

## 🚀 Funcionalidades

### 👤 Área do Cliente

- Cadastro de clientes Pessoa Física (CPF) e Pessoa Jurídica (CNPJ).
- Autenticação de usuários.
- Visualização de produtos com imagens, preços e descrições.
- Carrinho de compras com controle de quantidade.
- Aplicação de cupons de desconto.
- Pagamentos simulados por cartão e boleto.
- Consulta ao histórico de pagamentos.
- Acompanhamento do status dos pedidos.
- Visualização das informações do perfil.
- Ouvidoria e interface de suporte ao cliente.

### 🔐 Área Administrativa

- Autenticação com controle de acesso.
- Painel administrativo com indicadores gerais.
- Cadastro de produtos.
- Gerenciamento de cupons de desconto.
- Consulta aos pedidos realizados.
- Atualização das etapas dos pedidos:
  - Pedido feito
  - Produzindo
  - Enviado
  - Entregue

## 🛠️ Tecnologias Utilizadas

- **C#** — Linguagem de programação.
- **.NET 10** — Plataforma de desenvolvimento.
- **WPF (Windows Presentation Foundation)** — Interface gráfica.
- **XAML** — Estruturação das interfaces.
- **Material Design in XAML** — Estilização e ícones.
- **SQL Server LocalDB** — Banco de dados relacional.
- **Entity Framework Core** — Mapeamento objeto-relacional (ORM).
- **ADO.NET** — Manipulação direta de dados utilizando SQL.
- **Visual Studio** — Ambiente de desenvolvimento.

## 🏗️ Arquitetura do Sistema

O sistema foi desenvolvido aplicando conceitos de Programação Orientada a Objetos, organização em camadas e separação de responsabilidades.

O projeto está organizado nas seguintes pastas:

- `model` — Classes que representam as entidades do sistema.
- `view` — Interfaces gráficas desenvolvidas em WPF.
- `controll` — Classes responsáveis pela lógica das operações.
- `data` — Configuração e acesso ao banco de dados.
- `services` — Serviços auxiliares utilizados pela aplicação.
- `components` — Componentes reutilizáveis da interface.

## 🗄️ Banco de Dados

O projeto utiliza o **SQL Server LocalDB**, com persistência de dados realizada principalmente pelo Entity Framework Core.

Foi utilizada a abordagem **Code First**, permitindo a criação e atualização da estrutura do banco por meio de migrations.

Entre as principais entidades estão:

- Cliente
- Pessoa Física
- Pessoa Jurídica
- Produto
- Fornecedor
- Pedido
- ItemPedido
- Pagamento
- Cupom

## 📚 Objetivo Acadêmico

O desenvolvimento da UVVirtual teve como objetivo aplicar, na prática, conhecimentos adquiridos durante a disciplina de Programação Orientada a Objetos II, incluindo:

- Encapsulamento e herança.
- Polimorfismo e abstração.
- Princípios SOLID e Clean Code.
- Organização em camadas.
- Persistência em banco de dados relacional.
- Entity Framework Core e ADO.NET.
- Relacionamentos entre entidades.
- Autenticação e controle de acesso.
- Desenvolvimento de interfaces gráficas com WPF.

## ⚠️ Observações

Este projeto foi desenvolvido exclusivamente para **fins acadêmicos e educacionais**.

Os pagamentos realizados no sistema são simulações, não envolvendo transações financeiras reais.

Algumas funcionalidades, como o chatbot de suporte, ainda estão em desenvolvimento.

---

**Projeto acadêmico desenvolvido para a disciplina de Programação Orientada a Objetos II — Ciência da Computação.**
