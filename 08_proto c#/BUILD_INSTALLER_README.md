# 📦 Guia de Criação do Instalador - BibliotecaJK v3.0

Este guia explica como criar o instalador executável do BibliotecaJK para distribuição.

## 📋 Pré-requisitos

### 1. .NET SDK 8.0 ou superior
- **Download**: https://dotnet.microsoft.com/download/dotnet/8.0
- **Verificar instalação**: Abra PowerShell e execute `dotnet --version`
- Deve retornar algo como: `8.0.x`

### 2. Inno Setup 6.x (Compilador de Instaladores)
- **Download**: https://jrsoftware.org/isdl.php
- **Versão recomendada**: Inno Setup 6.2.2 ou superior
- **Instalação padrão**: `C:\Program Files (x86)\Inno Setup 6\`
- É **GRATUITO** e open-source

### 3. Sistema Operacional
- Windows 10 ou superior (64-bit)
- PowerShell 5.1 ou superior

---

## 🚀 Processo de Build (2 Passos Simples)

### Passo 1: Publicar a Aplicação

Abra PowerShell **como Administrador** na pasta do projeto e execute:

```powershell
.\build-release.ps1
```

**O que este script faz:**
- ✅ Compila a aplicação em modo Release
- ✅ Publica como self-contained (inclui runtime .NET)
- ✅ Otimiza com ReadyToRun para performance
- ✅ Cria pasta `publish/BibliotecaJK/` com todos os arquivos
- ✅ Copia schema.sql e documentação
- ✅ Cria arquivo VERSION.txt

**Tempo estimado:** 2-5 minutos

**Saída esperada:**
```
✓ .NET SDK encontrado: 8.0.x
✓ Pasta publish criada
✓ Aplicação publicada com sucesso!
✓ schema.sql copiado
✓ README.txt copiado
...
Build concluído com sucesso!
```

### Passo 2: Criar o Instalador

Execute o script batch:

```cmd
build-installer.bat
```

**O que este script faz:**
- ✅ Verifica instalação do Inno Setup
- ✅ Compila o script `BibliotecaJK-Setup.iss`
- ✅ Cria instalador executável em `publish/Installer/`
- ✅ Comprime e otimiza o instalador (LZMA2 ultra)

**Tempo estimado:** 1-3 minutos

**Saída esperada:**
```
[OK] Inno Setup encontrado
[OK] Aplicacao publicada encontrada
Compilando instalador com Inno Setup...
========================================
  Instalador criado com sucesso!
========================================
Arquivo: publish\Installer\BibliotecaJK-Setup-v3.0.exe
```

---

## 📂 Estrutura de Arquivos Gerada

Após executar os dois scripts, você terá:

```
08_proto c#/
├── publish/
│   ├── BibliotecaJK/              # Aplicação publicada
│   │   ├── BibliotecaJK.exe       # Executável principal
│   │   ├── BibliotecaJK.dll       # Assembly principal
│   │   ├── MySql.Data.dll         # Dependências
│   │   ├── BCrypt.Net-Next.dll
│   │   └── ... (runtime .NET)
│   │
│   ├── Install/                    # Arquivos para instalador
│   │   ├── schema.sql              # Script do banco
│   │   ├── VERSION.txt             # Info de versão
│   │   └── Documentacao/           # Manuais
│   │       ├── README.txt
│   │       ├── MANUAL_USUARIO.md
│   │       ├── INSTALACAO.md
│   │       ├── ARQUITETURA.md
│   │       └── TESTES.md
│   │
│   └── Installer/
│       └── BibliotecaJK-Setup-v3.0.exe  ⭐ INSTALADOR FINAL
│
├── build-release.ps1               # Script de publicação
├── build-installer.bat             # Script do instalador
├── BibliotecaJK-Setup.iss          # Configuração Inno Setup
└── BUILD_INSTALLER_README.md       # Este arquivo
```

---

## 🎯 O Instalador Final

### Arquivo Gerado
- **Nome**: `BibliotecaJK-Setup-v3.0.exe`
- **Localização**: `publish/Installer/`
- **Tamanho estimado**: ~80-120 MB (inclui runtime .NET)
- **Compressão**: LZMA2 Ultra64 (melhor compressão)

### O que o instalador faz:

1. **Instalação no sistema:**
   - Pasta padrão: `C:\Program Files\BibliotecaJK\`
   - Pode ser alterada pelo usuário

2. **Atalhos criados:**
   - ✅ Menu Iniciar → BibliotecaJK
   - ✅ Menu Iniciar → Manual do Usuário
   - ✅ Menu Iniciar → Guia de Instalação
   - ✅ Menu Iniciar → Documentação Técnica
   - ✅ Menu Iniciar → Desinstalar BibliotecaJK
   - ⭕ Área de Trabalho (opcional)
   - ⭕ Barra de Tarefas (opcional)

3. **Arquivos incluídos:**
   - Executável e todas as DLLs
   - schema.sql para criar o banco
   - Toda a documentação
   - Arquivo VERSION.txt

4. **Registro no Windows:**
   - Adiciona entrada em "Adicionar ou Remover Programas"
   - Registra caminho de instalação no Registry
   - Cria desinstalador automático

5. **Pós-instalação:**
   - Oferece abrir o Guia de Instalação
   - Oferece executar o programa imediatamente
   - Cria pasta para backups em Documentos

---

## 🔧 Customização do Instalador

### Alterar Ícone do Instalador

Edite `BibliotecaJK-Setup.iss` linha 21:

```ini
SetupIconFile=caminho\para\seu\icone.ico
```

Requisitos do ícone:
- Formato: `.ico`
- Tamanhos recomendados: 16x16, 32x32, 48x48, 256x256

### Alterar Versão

Edite `BibliotecaJK-Setup.iss` linha 6:

```ini
#define MyAppVersion "3.0"  ; Altere para "3.1", "4.0", etc.
```

### Alterar Nome do Arquivo Final

Edite `BibliotecaJK-Setup.iss` linha 24:

```ini
OutputBaseFilename=BibliotecaJK-Setup-v{#MyAppVersion}
```

### Adicionar/Remover Arquivos

Edite a seção `[Files]` em `BibliotecaJK-Setup.iss`:

```ini
[Files]
; Adicionar novo arquivo
Source: "caminho\arquivo.txt"; DestDir: "{app}"; Flags: ignoreversion
```

---

## 🐛 Solução de Problemas

### Erro: "dotnet: command not found"
- **Causa**: .NET SDK não está instalado ou não está no PATH
- **Solução**:
  1. Instale o .NET SDK 8.0
  2. Reinicie o PowerShell
  3. Execute `dotnet --version` para verificar

### Erro: "Inno Setup não encontrado"
- **Causa**: Inno Setup não está instalado ou em caminho diferente
- **Solução**:
  1. Instale o Inno Setup 6.x
  2. Se instalou em local diferente, edite `build-installer.bat` linha 7:
     ```bat
     set "INNO_SETUP_PATH=C:\Seu\Caminho\ISCC.exe"
     ```

### Erro: "Aplicação não foi publicada"
- **Causa**: Passo 1 não foi executado ou falhou
- **Solução**: Execute `.\build-release.ps1` primeiro

### Erro: "Access Denied" ao executar PowerShell
- **Causa**: Política de execução de scripts
- **Solução**: Execute PowerShell como Administrador e execute:
  ```powershell
  Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
  ```

### Instalador muito grande (>200 MB)
- **Causa**: Self-contained inclui todo o runtime .NET
- **Solução alternativa**: Publicar como framework-dependent
  - Edite `build-release.ps1` linha 28:
    ```powershell
    --self-contained false
    ```
  - Usuários precisarão instalar .NET Runtime 8.0
  - Tamanho reduz para ~10-20 MB

---

## 📊 Checklist de Build

Antes de distribuir o instalador, verifique:

- [ ] Versão correta no `BibliotecaJK-Setup.iss`
- [ ] Documentação atualizada (README, MANUAL, etc.)
- [ ] schema.sql testado e funcional
- [ ] Aplicação compila sem erros
- [ ] Instalador foi testado em máquina limpa
- [ ] Desinstalador funciona corretamente
- [ ] Atalhos foram criados corretamente
- [ ] Programa executa após instalação
- [ ] Conexão MySQL funciona
- [ ] Backup automático funciona

---

## 🎓 Teste do Instalador

### Teste Básico (Mesma Máquina)

1. Execute o instalador: `publish\Installer\BibliotecaJK-Setup-v3.0.exe`
2. Siga o assistente de instalação
3. Verifique se os atalhos foram criados
4. Execute o programa pelo Menu Iniciar
5. Teste o login (admin / admin123)
6. Verifique se conecta ao MySQL
7. Desinstale pelo "Adicionar ou Remover Programas"
8. Verifique se a desinstalação foi completa

### Teste Completo (Máquina Limpa)

Recomendado testar em:
- ✅ Windows 10 (64-bit)
- ✅ Windows 11 (64-bit)
- ✅ Máquina virtual limpa
- ✅ Sem .NET instalado (se self-contained)
- ✅ Com MySQL já instalado
- ✅ Sem MySQL (deve mostrar erro claro)

---

## 📮 Distribuição

### Onde hospedar o instalador:

1. **GitHub Releases** (Recomendado)
   - Gratuito até 2 GB por release
   - Versionamento automático
   - Download público

2. **Google Drive / OneDrive**
   - Fácil compartilhamento
   - Sem versionamento

3. **Servidor próprio**
   - Controle total
   - Estatísticas de download

### Informações para distribuir:

```markdown
## BibliotecaJK v3.0 - Sistema de Gerenciamento de Biblioteca

### Download
- **Instalador Windows (64-bit)**: BibliotecaJK-Setup-v3.0.exe (XX MB)

### Requisitos
- Windows 10 ou superior (64-bit)
- MySQL 8.0 ou superior
- 200 MB de espaço em disco

### Instalação
1. Execute o instalador
2. Siga o assistente de instalação
3. Configure o MySQL (veja Guia de Instalação)
4. Execute pelo Menu Iniciar

### Primeiro Acesso
- **Usuário**: admin
- **Senha**: admin123

### Documentação
Incluída no instalador:
- Manual do Usuário
- Guia de Instalação
- Documentação Técnica
```

---

## 🔄 Atualização de Versão

Para criar uma nova versão (ex: v3.1):

1. Atualize o código-fonte
2. Altere versão em `BibliotecaJK-Setup.iss`:
   ```ini
   #define MyAppVersion "3.1"
   ```
3. Atualize `README.txt` e documentação
4. Execute `.\build-release.ps1`
5. Execute `.\build-installer.bat`
6. Teste o novo instalador
7. Distribua com release notes

**Dica**: O instalador Inno Setup detecta versões anteriores e oferece desinstalação automática.

---

## 📝 Notas Importantes

1. **Self-Contained vs Framework-Dependent:**
   - ✅ **Self-contained** (atual): Maior, mas não requer .NET instalado
   - ⚠️ **Framework-dependent**: Menor, mas requer .NET Runtime 8.0

2. **Assinatura Digital:**
   - Para produção profissional, considere assinar o instalador com certificado digital
   - Evita avisos do Windows SmartScreen
   - Aumenta confiança do usuário

3. **Antivírus:**
   - Alguns antivírus podem bloquear instaladores não assinados
   - Teste com Windows Defender ativado
   - Considere enviar para análise do VirusTotal

4. **Tamanho do Instalador:**
   - Self-contained .NET 8.0: ~80-120 MB
   - Compressão LZMA2 já está no máximo
   - Normal para aplicações .NET modernas

---

## 🆘 Suporte

### Problemas com o Build
- Verifique logs em: `publish/BibliotecaJK/`
- Logs do Inno Setup: `publish/Installer/`

### Problemas com Instalação
- Verifique logs do Windows: Event Viewer
- Logs da aplicação: `%LOCALAPPDATA%\BibliotecaJK\`

### Contato
- GitHub Issues: https://github.com/shishiv/bibliokopke/issues
- Email: [seu-email]

---

## ✅ Resultado Final

Após seguir este guia, você terá:

✅ Um instalador profissional Windows (`.exe`)
✅ Instalação automatizada com assistente
✅ Atalhos no Menu Iniciar e Área de Trabalho
✅ Desinstalador integrado
✅ Documentação incluída
✅ Pronto para distribuição

**Arquivo final**: `publish/Installer/BibliotecaJK-Setup-v3.0.exe`

---

**Desenvolvido com ❤️ pela BibliotecaJK Team**

*Última atualização: 2025-11-05*
