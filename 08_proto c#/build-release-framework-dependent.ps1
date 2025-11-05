# Script PowerShell para publicar BibliotecaJK (Framework-Dependent)
# Esta versao e MENOR mas requer .NET Runtime 8.0 instalado no PC destino
# Execute este script no Windows com PowerShell

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  BibliotecaJK - Build Release v3.0" -ForegroundColor Cyan
Write-Host "  (Framework-Dependent)" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "AVISO: Esta versao requer .NET Runtime 8.0" -ForegroundColor Yellow
Write-Host "instalado no PC de destino!" -ForegroundColor Yellow
Write-Host ""

# Verificar se dotnet esta instalado
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO: .NET SDK nao encontrado!" -ForegroundColor Red
    Write-Host "Instale o .NET 8.0 SDK de: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

Write-Host "[OK] .NET SDK encontrado: $dotnetVersion" -ForegroundColor Green
Write-Host ""

# Limpar publicacoes anteriores
Write-Host "Limpando publicacoes anteriores..." -ForegroundColor Yellow
if (Test-Path "./publish-fd") {
    Remove-Item -Path "./publish-fd" -Recurse -Force
    Write-Host "[OK] Pasta publish-fd removida" -ForegroundColor Green
}

# Criar pasta publish
New-Item -ItemType Directory -Path "./publish-fd" -Force | Out-Null
Write-Host "[OK] Pasta publish-fd criada" -ForegroundColor Green
Write-Host ""

# Publicar versao framework-dependent para Windows x64
Write-Host "Publicando aplicacao (framework-dependent, win-x64)..." -ForegroundColor Yellow
Write-Host "Esta versao sera MUITO menor (~10-20 MB)" -ForegroundColor Gray
Write-Host ""

dotnet publish BibliotecaJK.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=false -p:PublishReadyToRun=true -o "./publish-fd/BibliotecaJK"

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "ERRO: Falha ao publicar a aplicacao!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "[OK] Aplicacao publicada com sucesso!" -ForegroundColor Green
Write-Host ""

# Copiar arquivos adicionais para o instalador
Write-Host "Copiando arquivos adicionais..." -ForegroundColor Yellow

# Criar pasta Install dentro de publish-fd
New-Item -ItemType Directory -Path "./publish-fd/Install" -Force | Out-Null

# Copiar schema.sql
Copy-Item -Path "./schema.sql" -Destination "./publish-fd/Install/schema.sql" -Force
Write-Host "[OK] schema.sql copiado" -ForegroundColor Green

# Copiar documentacao
$docs = @(
    "README.txt",
    "MANUAL_USUARIO.md",
    "INSTALACAO.md",
    "ARQUITETURA.md",
    "TESTES.md"
)

New-Item -ItemType Directory -Path "./publish-fd/Install/Documentacao" -Force | Out-Null

foreach ($doc in $docs) {
    if (Test-Path "./$doc") {
        Copy-Item -Path "./$doc" -Destination "./publish-fd/Install/Documentacao/$doc" -Force
        Write-Host "[OK] $doc copiado" -ForegroundColor Green
    }
}

# Criar arquivo de versao com aviso sobre .NET Runtime
$versionInfo = @"
BibliotecaJK v3.0 FINAL (Framework-Dependent)
Build: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
Runtime: win-x64 (requer .NET Runtime 8.0)
.NET: 8.0

IMPORTANTE:
Esta versao REQUER .NET Runtime 8.0 instalado no Windows.
Download: https://dotnet.microsoft.com/download/dotnet/8.0

Requisitos:
- Windows 10 ou superior
- .NET Runtime 8.0 ou superior (OBRIGATORIO)
- MySQL 8.0 ou superior
- 50 MB de espaco em disco

Vantagens desta versao:
- Instalador muito menor (~10-20 MB vs ~100 MB)
- Atualizacao automatica do .NET via Windows Update
- Melhor integracao com o sistema

Desvantagens:
- Requer instalacao do .NET Runtime 8.0
- Mais uma dependencia para gerenciar

Desenvolvido por: BibliotecaJK Team
"@

$versionInfo | Out-File -FilePath "./publish-fd/Install/VERSION.txt" -Encoding UTF8
Write-Host "[OK] VERSION.txt criado" -ForegroundColor Green

# Criar script de verificacao de .NET para o instalador
$dotnetCheck = @'
@echo off
REM Verificar se .NET Runtime 8.0 esta instalado

echo Verificando .NET Runtime 8.0...
echo.

dotnet --list-runtimes | find "Microsoft.WindowsDesktop.App 8.0" >nul
if %ERRORLEVEL% EQU 0 (
    echo [OK] .NET Runtime 8.0 encontrado!
    echo.
    echo Voce pode executar BibliotecaJK.exe
    pause
    exit /b 0
) else (
    echo [ERRO] .NET Runtime 8.0 NAO encontrado!
    echo.
    echo Por favor, instale o .NET Runtime 8.0 de:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    echo Procure por: ".NET Desktop Runtime 8.0"
    echo.
    pause
    exit /b 1
)
'@

$dotnetCheck | Out-File -FilePath "./publish-fd/BibliotecaJK/verificar-dotnet.bat" -Encoding ASCII
Write-Host "[OK] verificar-dotnet.bat criado" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Build concluido com sucesso!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "IMPORTANTE:" -ForegroundColor Yellow
Write-Host "  Esta versao requer .NET Runtime 8.0" -ForegroundColor White
Write-Host "  instalado no PC de destino." -ForegroundColor White
Write-Host ""
Write-Host "Tamanho estimado do instalador:" -ForegroundColor Cyan
Write-Host "  Self-contained:       ~100 MB (inclui .NET)" -ForegroundColor Gray
Write-Host "  Framework-dependent:  ~10-20 MB (requer .NET)" -ForegroundColor Yellow
Write-Host ""
Write-Host "Proximo passo:" -ForegroundColor Yellow
Write-Host "  1. Edite BibliotecaJK-Setup.iss" -ForegroundColor White
Write-Host "  2. Mude 'publish\BibliotecaJK' para 'publish-fd\BibliotecaJK'" -ForegroundColor White
Write-Host "  3. Execute: .\build-installer.bat" -ForegroundColor White
Write-Host ""
Write-Host "Arquivos em: ./publish-fd/" -ForegroundColor Cyan
Write-Host ""
