# ProjectRabbitMQ

Sistema de processamento assíncrono utilizando **RabbitMQ**, onde
atualizações de clientes são realizadas via eventos e persistidas em
arquivo JSON, sem uso de banco de dados.

------------------------------------------------------------------------

## 🧠 Visão Geral

Este projeto demonstra a implementação de uma arquitetura orientada a
eventos utilizando RabbitMQ para comunicação entre serviços
desacoplados.

O objetivo principal é simular um cenário de atualização de status de
clientes utilizando:

-   Producer (API)
-   Consumer (Worker)
-   Persistência em arquivo JSON
-   Comunicação assíncrona via fila

------------------------------------------------------------------------

## 🏗 Arquitetura

    ProjectRabbitMQ
    │
    ├── Producer (ASP.NET Core API)
    │     ├── Publica mensagens no RabbitMQ
    │     └── Disponibiliza endpoint GET para consulta de clientes
    │
    ├── Consumer (Worker Service)
    │     └── Consome mensagens e atualiza customer.json
    │
    └── SharedData
          └── customer.json

### 🔁 Fluxo de funcionamento

1.  O Producer recebe uma requisição para atualizar o status de um
    cliente.
2.  A API publica um evento `UpdateCustomerStatus` no RabbitMQ.
3.  O Consumer escuta a fila.
4.  Ao receber a mensagem:
    -   Lê o arquivo `customer.json`
    -   Atualiza o status do cliente
    -   Persiste novamente no arquivo
5.  O endpoint GET do Producer retorna o cliente já atualizado.

------------------------------------------------------------------------

## 🚀 Tecnologias Utilizadas

-   .NET 8
-   ASP.NET Core
-   Worker Service
-   RabbitMQ

------------------------------------------------------------------------

## 🐰 RabbitMQ

O projeto utiliza RabbitMQ como broker de mensagens para:

-   Comunicação assíncrona
-   Desacoplamento entre serviços
-   Processamento orientado a eventos

------------------------------------------------------------------------

## ⚙️ Como Executar

### Executar os Projetos

-   Inicie primeiro o Consumer
-   Depois inicie o Producer
-   Realize requisições via Swagger ou Postman

------------------------------------------------------------------------

## 🎯 Objetivo do Projeto

Projeto educacional focado em:

-   Entendimento prático de RabbitMQ
-   Simulação de microsserviços
-   Processamento assíncrono
-   Comunicação entre aplicações
-   Arquitetura limpa

------------------------------------------------------------------------

Desenvolvido por Kauê Correia
