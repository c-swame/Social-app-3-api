# Social-app-3-api


Social-app-3-api é uma API Web que permite o cadastro e login e outras operações CRUD de pessoas usuárias, bem como permite no mínimo o CRUD basico com posts.
Ou seja, é a base de uma api para a produção de uma rede social onde os usuários podem fazer posts.


## Funções

- [x] CRUD de usuários; /

- [x] CRUD de postss;/

#### extras:
- [ ] Buscar o último post/

## Configurações Iniciais e uso
- Após clonar o repositório, o primeiro passo é configurar a string de conexão com o banco de dados no arquivo appsettings -> ConnectionStrings.DataBase.
Lá há um exemplo de string de conexão que deve ser utilizado para conectar ao sqlserver.

- Em seguida basta rodar o comando
```
dotnet restore
```
ou equivalente para instalar as dependências (no visual studio basta selecionar a solução "Social-app-3-api.sln").

- Gerar as migrations e o banco de dados que será utilizado pelo aplicativo, com os comandos:
```
para o Visual Studio:
add-migration NomeDaMigration

para o terminal:
dotnet ef migrations add NomeDaMigration
```
Seguido do comando:
```
para o Visual Studio:
Update-Database

para o terminal:
dotnet ef database update
```

- Agora basta rodar o comando
```
dotnet run
```
ou executar o programa manualmente e o ver rodando no seu localhost

Para mais detalhes ou testar a pi, você pode ir na rota '/swagger/index.html', onde está a documentação da api através do Swagger


## Future implementations

1. Finalizar testes e Adicionar testes de carga;
2. Adicionar secção de comentários;
3. Finalizar crud;
4. Adicionar chat;

### Contatos!
- email: caio_swame@hotmail.com
- linkedin: [caio-s-s-paulino](https://www.linkedin.com/in/caio-s-s-paulino/)

\
\
Desejo tudo de bom!
