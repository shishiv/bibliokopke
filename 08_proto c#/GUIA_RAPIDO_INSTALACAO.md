# 🚀 Guia Rápido de Instalação - BibliotecaJK v3.0

> **Para usuários finais**: Como instalar e usar o BibliotecaJK

---

## ⏱️ Instalação em 5 Minutos

### Passo 1: Download
- Baixe: `BibliotecaJK-Setup-v3.0.exe`
- Tamanho: ~100 MB
- Tempo de download: depende da conexão

### Passo 2: Executar Instalador
1. Duplo clique em `BibliotecaJK-Setup-v3.0.exe`
2. Se aparecer aviso do Windows Defender:
   - Clique em "Mais informações"
   - Clique em "Executar assim mesmo"
3. Clique em "Avançar"
4. Aceite o local de instalação (padrão: `C:\Program Files\BibliotecaJK`)
5. Escolha opções:
   - ☑ Atalho na Área de Trabalho (recomendado)
   - ☑ Atalho na Barra de Tarefas
6. Clique em "Instalar"
7. Aguarde 1-2 minutos

### Passo 3: Configurar MySQL
⚠️ **IMPORTANTE**: Você precisa ter MySQL instalado!

#### Se você JÁ TEM MySQL instalado:
1. Abra MySQL Workbench ou linha de comando
2. Execute o arquivo `C:\Program Files\BibliotecaJK\Database\schema.sql`
3. Isso criará o banco `bibliokopke` e as tabelas

#### Se você NÃO TEM MySQL:
1. Baixe MySQL 8.0:
   - https://dev.mysql.com/downloads/installer/
   - Escolha: "MySQL Installer for Windows"
   - Versão: 8.0.x (Community)

2. Instale MySQL:
   - Execute o instalador
   - Escolha: "Developer Default" ou "Server only"
   - Configure senha do root (anote!)
   - Finalize instalação

3. Criar banco de dados:
   - Abra MySQL Workbench (instalado com MySQL)
   - Conecte como root
   - Arquivo → Abrir SQL Script
   - Selecione: `C:\Program Files\BibliotecaJK\Database\schema.sql`
   - Clique no raio (⚡) para executar

### Passo 4: Primeiro Acesso
1. Abra BibliotecaJK (ícone no Desktop ou Menu Iniciar)
2. Primeiro acesso:
   - **Usuário**: `admin`
   - **Senha**: `admin123`
3. Clique em "Entrar"

### Passo 5: Alterar Senha (RECOMENDADO!)
1. Menu → Cadastros → Funcionários
2. Encontre "Administrador"
3. Clique em "Editar"
4. Altere a senha
5. Salve

---

## ✅ Requisitos do Sistema

### Mínimos
- ✅ Windows 10 (64-bit) ou superior
- ✅ 2 GB de RAM
- ✅ 200 MB de espaço em disco
- ✅ MySQL 8.0 ou superior

### Recomendados
- 🌟 Windows 11 (64-bit)
- 🌟 4 GB de RAM
- 🌟 500 MB de espaço em disco
- 🌟 SSD para melhor performance

---

## 🎯 Primeiros Passos

### 1. Cadastrar Alunos
1. Menu → Cadastros → Alunos
2. Clique em "Novo"
3. Preencha os dados:
   - Nome completo
   - Matrícula (único)
   - CPF (formato: 000.000.000-00)
   - Endereço e contatos
4. Clique em "Salvar"

### 2. Cadastrar Livros
1. Menu → Cadastros → Livros
2. Clique em "Novo"
3. Preencha:
   - ISBN (único)
   - Título
   - Autor
   - Editora
   - Quantidade disponível
   - Categoria
4. Clique em "Salvar"

### 3. Registrar Empréstimo
1. Menu → Empréstimos → Novo Empréstimo
2. Selecione o aluno
3. Selecione o livro
4. Data de devolução: automática (14 dias)
5. Clique em "Emprestar"

### 4. Registrar Devolução
1. Menu → Empréstimos → Devoluções
2. Busque o empréstimo ativo
3. Clique em "Devolver"
4. Se houver multa, será calculada automaticamente
5. Confirme a devolução

### 5. Configurar Backup (IMPORTANTE!)
1. Menu → Ferramentas → Backup e Restauração
2. Configure:
   - Host: `localhost` (se MySQL local)
   - Porta: `3306` (padrão)
   - Usuário: `root` (ou seu usuário MySQL)
   - Senha: sua senha MySQL
   - Banco: `bibliokopke`
3. Escolha pasta para backups:
   - Recomendado: `C:\Backups\BibliotecaJK\`
   - Ou: OneDrive, Google Drive, etc.
4. Clique em "Testar Conexão"
5. Se OK:
   - ☑ Marque "Agendar backup diário"
   - Escolha horário (ex: 23:00)
   - Dias de retenção: 30 (padrão)
6. Clique em "Salvar"

---

## 📚 Funcionalidades Principais

### Gestão de Alunos
- ✅ Cadastro completo
- ✅ Busca rápida
- ✅ Edição e exclusão
- ✅ Validação de CPF
- ✅ Histórico de empréstimos

### Gestão de Livros
- ✅ Cadastro com ISBN
- ✅ Controle de quantidade
- ✅ Categorias
- ✅ Busca avançada
- ✅ Disponibilidade em tempo real

### Empréstimos
- ✅ Registro rápido
- ✅ Prazo automático (14 dias)
- ✅ Renovação
- ✅ Cálculo de multas
- ✅ Histórico completo

### Devoluções
- ✅ Busca por aluno ou livro
- ✅ Multa automática (R$ 2,00/dia)
- ✅ Registro de observações
- ✅ Liberação de exemplar

### Reservas
- ✅ Reservar livros indisponíveis
- ✅ Fila de espera
- ✅ Notificação (quando disponível)
- ✅ Cancelamento

### Relatórios
- ✅ Empréstimos por período
- ✅ Empréstimos ativos
- ✅ Empréstimos em atraso
- ✅ Histórico por aluno
- ✅ Estatísticas de livros
- ✅ Reservas pendentes
- ✅ Ranking de livros

### Backup
- ✅ Backup manual
- ✅ Backup automático diário
- ✅ Retenção configurável
- ✅ Limpeza automática
- ✅ Credenciais criptografadas

---

## ⚙️ Configurações Importantes

### Conexão com MySQL
O sistema conecta em:
- **Host**: localhost
- **Porta**: 3306
- **Banco**: bibliokopke
- **Usuário**: Configurado no código (padrão: root)

Para alterar, edite:
- Arquivo: `C:\Program Files\BibliotecaJK\BibliotecaJK.dll.config`

### Dados do Sistema
- **Configurações**: `%LOCALAPPDATA%\BibliotecaJK\`
- **Backups**: Pasta escolhida na configuração
- **Logs**: (se habilitado)

### Perfis de Usuário

#### ADMIN (Administrador)
- ✅ Todas as funcionalidades
- ✅ Cadastrar funcionários
- ✅ Editar/excluir registros
- ✅ Acessar relatórios
- ✅ Configurar backup

#### BIBLIOTECARIO (Bibliotecário)
- ✅ Cadastrar alunos e livros
- ✅ Registrar empréstimos e devoluções
- ✅ Consultar relatórios
- ❌ Não pode cadastrar funcionários

---

## 🆘 Solução de Problemas

### ❌ "Erro ao conectar ao banco de dados"
**Causa**: MySQL não está rodando ou credenciais erradas

**Solução**:
1. Abra "Serviços" do Windows (Win + R → `services.msc`)
2. Procure "MySQL80" ou "MySQL"
3. Clique com botão direito → Iniciar
4. Se não existir, reinstale o MySQL

### ❌ "Login ou senha incorretos"
**Causa**: Credenciais erradas

**Solução**:
1. Verifique CAPS LOCK
2. Senha padrão: `admin123`
3. Se esqueceu: Execute SQL no MySQL:
   ```sql
   UPDATE Funcionario
   SET senha_hash = '$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy'
   WHERE login = 'admin';
   ```
   Isso reseta para `admin123`

### ❌ "Livro não disponível"
**Causa**: Todos os exemplares estão emprestados

**Solução**:
1. Verifique devoluções pendentes
2. Ou faça uma reserva (Menu → Reservas)

### ❌ "CPF inválido"
**Causa**: CPF em formato incorreto ou inválido

**Solução**:
1. Use formato: `000.000.000-00`
2. Verifique se o CPF é válido (algoritmo de verificação)

### ❌ "Backup falhou"
**Causa**: mysqldump não encontrado ou credenciais erradas

**Solução**:
1. Verifique se MySQL está instalado
2. Teste a conexão primeiro
3. Verifique credenciais do backup
4. Adicione MySQL ao PATH do Windows

### ❌ Windows Defender bloqueia
**Causa**: Instalador não tem assinatura digital

**Solução**:
1. Clique em "Mais informações"
2. Clique em "Executar assim mesmo"
3. É seguro! Código-fonte disponível

---

## 📞 Suporte e Documentação

### Documentação Completa
Instalada em: `C:\Program Files\BibliotecaJK\Documentacao\`

- **Manual do Usuário**: Guia completo (~2.800 linhas)
- **Guia de Instalação**: Detalhes técnicos
- **Arquitetura**: Documentação técnica
- **Testes**: Casos de uso e validações

### Atalhos no Menu Iniciar
- Menu Iniciar → BibliotecaJK → Manual do Usuário
- Menu Iniciar → BibliotecaJK → Guia de Instalação

### Contato
- **GitHub**: https://github.com/shishiv/bibliokopke/issues
- **Email**: [configurar]

---

## 🔄 Atualização de Versão

Quando lançar nova versão:
1. Faça backup do banco MySQL
2. Baixe novo instalador
3. Execute o novo instalador
4. Ele detectará versão anterior
5. Oferecerá desinstalar e instalar nova
6. Seus dados no MySQL são mantidos

---

## 🗑️ Desinstalação

Para remover o BibliotecaJK:

1. **Via Instalador**:
   - Menu Iniciar → BibliotecaJK → Desinstalar BibliotecaJK

2. **Via Painel de Controle**:
   - Configurações → Aplicativos
   - Procure "BibliotecaJK"
   - Clique em "Desinstalar"

3. **O que é removido**:
   - ✅ Programa e arquivos
   - ✅ Atalhos
   - ✅ Entrada no registro
   - ❌ Banco de dados MySQL (preservado)
   - ⚠️ Configurações locais (pergunta)

4. **Remover banco de dados** (se desejar):
   ```sql
   DROP DATABASE bibliokopke;
   ```

---

## ✅ Checklist de Instalação

Use esta lista para verificar se instalou corretamente:

- [ ] MySQL 8.0 instalado e rodando
- [ ] BibliotecaJK instalado
- [ ] Banco `bibliokopke` criado (schema.sql executado)
- [ ] Login funcionando (admin / admin123)
- [ ] Senha do admin alterada
- [ ] Backup configurado e testado
- [ ] Primeiro aluno cadastrado
- [ ] Primeiro livro cadastrado
- [ ] Primeiro empréstimo testado
- [ ] Documentação consultada

---

## 🎓 Dicas de Uso

### Performance
- ✅ Use SSD para melhor performance
- ✅ Mantenha MySQL otimizado
- ✅ Faça backup regularmente

### Segurança
- ✅ Altere senha padrão
- ✅ Crie usuários específicos (não use sempre admin)
- ✅ Faça backup em local seguro (nuvem)
- ✅ Não compartilhe senhas

### Organização
- ✅ Use categorias nos livros
- ✅ Mantenha matrículas padronizadas
- ✅ Registre observações importantes
- ✅ Consulte relatórios regularmente

### Backup
- ✅ Configure backup automático
- ✅ Teste restauração periodicamente
- ✅ Mantenha backups em múltiplos locais
- ✅ Verifique logs de backup

---

## 📊 Fluxo de Trabalho Típico

### Manhã
1. Abrir o sistema
2. Verificar devoluções do dia
3. Processar devoluções atrasadas (calcular multas)

### Durante o Dia
1. Cadastrar novos alunos
2. Registrar empréstimos
3. Processar devoluções
4. Atender reservas

### Fim do Dia
1. Gerar relatório de empréstimos do dia
2. Verificar empréstimos em atraso
3. Verificar se backup automático está configurado

### Semanal
1. Relatório de empréstimos da semana
2. Ranking de livros mais emprestados
3. Verificar reservas pendentes

### Mensal
1. Relatório completo mensal
2. Estatísticas de uso
3. Verificar integridade dos backups
4. Limpar dados obsoletos (se necessário)

---

**BibliotecaJK v3.0 - Sistema de Gerenciamento de Biblioteca**

*Desenvolvido com ❤️ pela BibliotecaJK Team*

**Última atualização**: 2025-11-05

---

## 📥 Download

**Versão Atual**: 3.0 FINAL
**Tamanho**: ~100 MB
**Plataforma**: Windows 10/11 (64-bit)

[🔗 Baixar BibliotecaJK-Setup-v3.0.exe](#)

---

**Boa sorte com sua biblioteca! 📚**
