# Documentação Técnica do Banco de Dados
## Sistema: App Biblioteca Fronteira

---

## 1. Visão Geral

O banco de dados foi projetado para atender às necessidades de gestão da biblioteca escolar, com integração ao sistema SIMADE. O modelo contempla usuários (alunos, professores, bibliotecários), acervo de livros, empréstimos, reservas, histórico, logs de auditoria e relatórios.

---

## 2. Estrutura das Tabelas

### 2.1. usuario
- **Finalidade:** Armazena dados dos usuários integrados ao SIMADE.
- **Campos principais:** 
  - `codigo_simade` (PK): Identificador único do SIMADE.
  - `codigo_inep`: Código da escola.
  - `nome_completo`, `data_nascimento`, `cpf`, `email`, `telefone`, `endereco`.
  - `tipo_usuario`: ALUNO, PROFESSOR, BIBLIOTECARIO.
  - `cor_raca`, `sexo`, `estado_civil`, `nacionalidade`, `uf_nascimento`, `municipio_nascimento`.
  - `ativo`: Status do usuário.
  - `data_cadastro`, `data_atualizacao`.

### 2.2. livro
- **Finalidade:** Cadastro do acervo de livros.
- **Campos principais:** 
  - `id_livro` (PK), `isbn`, `titulo`, `autor`, `editora`, `ano_publicacao`, `categoria`, `numero_paginas`, `idioma`, `sinopse`, `localizacao`, `status`, `quantidade_total`, `quantidade_disponivel`, `data_cadastro`, `data_atualizacao`.

### 2.3. emprestimo
- **Finalidade:** Controle de empréstimos de livros.
- **Campos principais:** 
  - `id_emprestimo` (PK), `codigo_simade` (FK), `id_livro` (FK), `data_emprestimo`, `data_devolucao_prevista`, `data_devolucao_real`, `status`, `observacoes`, `renovacoes`, `data_cadastro`.

### 2.4. reserva
- **Finalidade:** Gerenciamento de reservas de livros.
- **Campos principais:** 
  - `id_reserva` (PK), `codigo_simade` (FK), `id_livro` (FK), `data_reserva`, `data_validade`, `status`, `motivo_cancelamento`, `data_cadastro`.

### 2.5. historico_emprestimo
- **Finalidade:** Registro histórico de todos os empréstimos realizados.
- **Campos principais:** 
  - `id_historico` (PK), `id_emprestimo` (FK), `codigo_simade` (FK), `id_livro` (FK), `data_emprestimo`, `data_devolucao`, `dias_atraso`, `multa`, `observacoes`, `data_registro`.

### 2.6. log_sistema
- **Finalidade:** Auditoria de ações no sistema.
- **Campos principais:** 
  - `id_log` (PK), `codigo_simade` (FK), `tabela_afetada`, `acao`, `dados_anteriores`, `dados_novos`, `ip_usuario`, `data_acao`.

### 2.7. relatorio
- **Finalidade:** Armazenamento de relatórios gerados pelo sistema.
- **Campos principais:** 
  - `id_relatorio` (PK), `tipo_relatorio`, `periodo`, `dados_relatorio`, `codigo_simade` (FK), `data_geracao`, `arquivo_gerado`.

---

## 3. Relacionamentos

- **usuario** 1:N **emprestimo**, **reserva**, **historico_emprestimo**, **log_sistema**, **relatorio**
- **livro** 1:N **emprestimo**, **reserva**, **historico_emprestimo**
- **emprestimo** 1:1 **historico_emprestimo** (por empréstimo devolvido)
- Integridade referencial garantida por chaves estrangeiras e ON DELETE CASCADE/SET NULL.

---

## 4. Triggers e Regras de Negócio

- **Triggers de auditoria:** Logam inserções e atualizações de usuários.
- **Trigger de empréstimo:** Atualiza quantidade disponível e status do livro ao emprestar/devolver.
- **Trigger de devolução:** Insere registro no histórico ao devolver livro.

---

## 5. Views Úteis

- **vw_emprestimos_ativos:** Lista todos os empréstimos ativos, atrasados ou renovados.
- **vw_livros_populares:** Lista livros mais emprestados.
- (Outras views podem ser criadas conforme necessidade.)

---

## 6. Dados Iniciais

- Usuário bibliotecário padrão cadastrado.
- Exemplos de livros inseridos para testes.

---

## 7. Observações

- O modelo está preparado para integração com o SIMADE, utilizando o código_simade como chave primária dos usuários.
- O banco utiliza padrões de normalização e índices para otimizar buscas e integridade.
- O sistema de logs e histórico garante rastreabilidade e auditoria das operações.

---

## 8. Próximos Passos

- Implementar consultas SQL (DML) para operações básicas.
- Documentar fluxos de uso e integrações.
- Validar modelo com a equipe e stakeholders.

---

## 9. Referências de Arquivos do Projeto

- Script de criação do banco: `02_modelagem_banco/banco_de_dados.sql`
- Exemplos de consultas SQL: `02_modelagem_banco/exemplos_consultas.sql`
- Diagramas UML: `04_diagramas/`
- Cronograma e aceite: `01_planejamento/`
- Relatórios e atualizações: `05_relatorios/`
