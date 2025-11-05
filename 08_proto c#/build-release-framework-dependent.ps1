# Script PowerShell para publicar BibliotecaJK (Framework-Dependent)
# Esta versão é MENOR mas requer .NET Runtime 8.0 instalado no PC destino
# Execute este script no Windows com PowerShell

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  BibliotecaJK - Build Release v3.0" -ForegroundColor Cyan
Write-Host "  (Framework-Dependent)" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "AVISO: Esta versão requer .NET Runtime 8.0" -ForegroundColor Yellow
Write-Host "instalado no PC de destino!" -ForegroundColor Yellow
Write-Host ""

# Verificar se dotnet está instalado
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO: .NET SDK não encontrado!" -ForegroundColor Red
    Write-Host "Instale o .NET 8.0 SDK de: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ .NET SDK encontrado: $dotnetVersion" -ForegroundColor Green
Write-Host ""

# Limpar publicações anteriores
Write-Host "Limpando publicações anteriores..." -ForegroundColor Yellow
if (Test-Path "./publish-fd") {
    Remove-Item -Path "./publish-fd" -Recurse -Force
    Write-Host "✓ Pasta publish-fd removida" -ForegroundColor Green
}

# Criar pasta publish
New-Item -ItemType Directory -Path "./publish-fd" -Force | Out-Null
Write-Host "✓ Pasta publish-fd criada" -ForegroundColor Green
Write-Host ""

# Publicar versão framework-dependent para Windows x64
Write-Host "Publicando aplicação (framework-dependent, win-x64)..." -ForegroundColor Yellow
Write-Host "Esta versão será MUITO menor (~10-20 MB)" -ForegroundColor Gray
Write-Host ""

dotnet publish BibliotecaJK.csproj `
    -c Release `
    -r win-x64 `
    --self-contained false `
    -p:PublishSingleFile=false `
    -p:PublishReadyToRun=true `
    -o "./publish-fd/BibliotecaJK"

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "ERRO: Falha ao publicar a aplicação!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "✓ Aplicação publicada com sucesso!" -ForegroundColor Green
Write-Host ""

# Copiar arquivos adicionais para o instalador
Write-Host "Copiando arquivos adicionais..." -ForegroundColor Yellow

# Criar pasta Install dentro de publish-fd
New-Item -ItemType Directory -Path "./publish-fd/Install" -Force | Out-Null

# Copiar schema.sql
Copy-Item -Path "./schema.sql" -Destination "./publish-fd/Install/schema.sql" -Force
Write-Host "✓ schema.sql copiado" -ForegroundColor Green

# Copiar documentação
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
        Write-Host "✓ $doc copiado" -ForegroundColor Green
    }
}

# Criar arquivo de versão com aviso sobre .NET Runtime
$versionInfo = @"
BibliotecaJK v3.0 FINAL (Framework-Dependent)
Build: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
Runtime: win-x64 (requer .NET Runtime 8.0)
.NET: 8.0

IMPORTANTE:
Esta versão REQUER .NET Runtime 8.0 instalado no Windows.
Download: https://dotnet.microsoft.com/download/dotnet/8.0

Requisitos:
- Windows 10 ou superior
- .NET Runtime 8.0 ou superior (OBRIGATÓRIO)
- MySQL 8.0 ou superior
- 50 MB de espaço em disco

Vantagens desta versão:
- ✅ Instalador muito menor (~10-20 MB vs ~100 MB)
- ✅ Atualização automática do .NET via Windows Update
- ✅ Melhor integração com o sistema

Desvantagens:
- ⚠️ Requer instalação do .NET Runtime 8.0
- ⚠️ Mais uma dependência para gerenciar

Desenvolvido por: BibliotecaJK Team
"@

$versionInfo | Out-File -FilePath "./publish-fd/Install/VERSION.txt" -Encoding UTF8
Write-Host "✓ VERSION.txt criado" -ForegroundColor Green

# Criar script de verificação de .NET para o instalador
$dotnetCheck = @'
@echo off
REM Verificar se .NET Runtime 8.0 está instalado

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
Write-Host "✓ verificar-dotnet.bat criado" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Build concluído com sucesso!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "IMPORTANTE:" -ForegroundColor Yellow
Write-Host "  Esta versão requer .NET Runtime 8.0" -ForegroundColor White
Write-Host "  instalado no PC de destino." -ForegroundColor White
Write-Host ""
Write-Host "Tamanho estimado do instalador:" -ForegroundColor Cyan
Write-Host "  Self-contained:       ~100 MB (inclui .NET)" -ForegroundColor Gray
Write-Host "  Framework-dependent:  ~10-20 MB (requer .NET)" -ForegroundColor Yellow
Write-Host ""
Write-Host "Próximo passo:" -ForegroundColor Yellow
Write-Host "  1. Edite BibliotecaJK-Setup.iss" -ForegroundColor White
Write-Host "  2. Mude 'publish\BibliotecaJK' para 'publish-fd\BibliotecaJK'" -ForegroundColor White
Write-Host "  3. Execute: .\build-installer.bat" -ForegroundColor White
Write-Host ""
Write-Host "Arquivos em: ./publish-fd/" -ForegroundColor Cyan
Write-Host ""
