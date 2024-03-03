# Especificações do Projeto

<span style="color:red">Pré-requisitos: <a href="1-Documentação de Contexto.md"> Documentação de Contexto</a></span>

Definição do problema e ideia de solução a partir da perspectiva do usuário. É composta pela definição do  diagrama de personas, histórias de usuários, requisitos funcionais e não funcionais além das restrições do projeto.

Apresente uma visão geral do que será abordado nesta parte do documento, enumerando as técnicas e/ou ferramentas utilizadas para realizar a especificações do projeto

## Arquitetura e Tecnologias

A arquitetura de software utilizada em nosso projeto é o MVC, e usaremos as seguintes tecnologias:

|Função   | Tecnologia utilizada  |
|---------|-----------------------|
|IDE      |Visual Studio  |
|Linguagens utilizadas no front-end | HTML, CSS, Javascript|
|Linguagens utilizadas no back-end  | C#|
|Frameworks |Bootstrap, Asp.Net.,  Entity Framework|
|Banco de dados| Microsoft SQL Server|

<img src ="img\Diagrama_Arquitetura.jpg">

## Project Model Canvas

Colocar a imagem do modelo construído apresentando a proposta de solução.

> **Links Úteis**:
> Disponíveis em material de apoio do projeto

## Requisitos

As tabelas que se seguem apresentam os requisitos funcionais e não funcionais que detalham o escopo do projeto. Para determinar a prioridade de requisitos, aplicar uma técnica de priorização de requisitos e detalhar como a técnica foi aplicada.

### Requisitos Funcionais

|ID    | Descrição do Requisito  | Prioridade |
|------|-----------------------------------------|----|
|RF-001| O sistema deve permitir ao usuário realizar o login   | ALTA | 
|RF-002|O sistema deve permitir ao gestor cadastrar o usuário | ALTA | 
|RF-003|O sistema deve permitir ao usuário cadastrar o importador | ALTA | 
|RF-004|O sistema deve permitir ao usuário cadastrar o exportador| ALTA | 
|RF-005|O sistema deve permitir ao usuário cadastrar o vendedor | ALTA | 
|RF-006|O sistema deve permitir ao usuário cadastrar o Agente de Carga   | ALTA | 
|RF-007|O sistema deve permitir ao usuário cadastrar o País de Destino     | ALTA |
|RF-008|O sistema deve permitir ao usuário cadastrar a Fronteira | ALTA |
|RF-009|O sistema deve permitir ao usuário cadastrar as despesas | ALTA |
|RF-010|O sistema deve permitir o usuário cadastrar o fornecedor de serviço  | ALTA |
|RF-011|O sistema deve permitir o usuário cadastrar os documentos de exportação  | ALTA |
|RF-012|O sistema deve permitir o usuário cadastrar os valores da Exportação | ALTA |


### Requisitos não Funcionais

|ID     | Descrição do Requisito  |Prioridade |
|-------|-------------------------|----|
|RNF-001| O sistema deve ser responsivo para rodar em um dispositivos móvel | MÉDIA | 
|RNF-002| Deve processar requisições do usuário em no máximo 3s |  BAIXA | 

## Restrições

O projeto está restrito pelos itens apresentados na tabela a seguir.

|ID| Restrição                                             |
|--|-------------------------------------------------------|
|01| O projeto deverá ser entregue até o final do semestre |
|02| Não pode ser desenvolvido um módulo de backend        |
|03| o aplicativo deve se restringir as tecnologias descritas neste repositório |


## Diagrama de Casos de Uso

O diagrama de casos de uso é o próximo passo após a elicitação de requisitos, que utiliza um modelo gráfico e uma tabela com as descrições sucintas dos casos de uso e dos atores. Ele contempla a fronteira do sistema e o detalhamento dos requisitos funcionais com a indicação dos atores, casos de uso e seus relacionamentos. 

As referências abaixo irão auxiliá-lo na geração do artefato “Diagrama de Casos de Uso”.

> **Links Úteis**:
> - [Criando Casos de Uso](https://www.ibm.com/docs/pt-br/elm/6.0?topic=requirements-creating-use-cases)
> - [Como Criar Diagrama de Caso de Uso: Tutorial Passo a Passo](https://gitmind.com/pt/fazer-diagrama-de-caso-uso.html/)
> - [Lucidchart](https://www.lucidchart.com/)
> - [Astah](https://astah.net/)
> - [Diagrams](https://app.diagrams.net/)

## Modelo ER (Projeto Conceitual)

O Modelo ER representa através de um diagrama como as entidades (coisas, objetos) se relacionam entre si na aplicação interativa.

![Diagrama_ER](img/Diagrama_ER.png)

## Projeto da Base de Dados

O projeto da base de dados corresponde à representação das entidades e relacionamentos identificadas no Modelo ER, no formato de tabelas, com colunas e chaves primárias/estrangeiras necessárias para representar corretamente as restrições de integridade.
