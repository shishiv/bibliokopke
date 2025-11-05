# Script PowerShell para publicar BibliotecaJK
# Execute este script no Windows com PowerShell

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  BibliotecaJK - Build Release v3.0" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
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
if (Test-Path "./publish") {
    Remove-Item -Path "./publish" -Recurse -Force
    Write-Host "✓ Pasta publish removida" -ForegroundColor Green
}

# Criar pasta publish
New-Item -ItemType Directory -Path "./publish" -Force | Out-Null
Write-Host "✓ Pasta publish criada" -ForegroundColor Green
Write-Host ""

# Publicar versão self-contained para Windows x64
Write-Host "Publicando aplicação (self-contained, win-x64)..." -ForegroundColor Yellow
Write-Host "Isso pode levar alguns minutos..." -ForegroundColor Gray
Write-Host ""

dotnet publish BibliotecaJK.csproj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -p:PublishReadyToRun=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -o "./publish/BibliotecaJK"

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

# Criar pasta Install dentro de publish
New-Item -ItemType Directory -Path "./publish/Install" -Force | Out-Null

# Copiar schema.sql
Copy-Item -Path "./schema.sql" -Destination "./publish/Install/schema.sql" -Force
Write-Host "✓ schema.sql copiado" -ForegroundColor Green

# Copiar documentação
$docs = @(
    "README.txt",
    "MANUAL_USUARIO.md",
    "INSTALACAO.md",
    "ARQUITETURA.md",
    "TESTES.md"
)

New-Item -ItemType Directory -Path "./publish/Install/Documentacao" -Force | Out-Null

foreach ($doc in $docs) {
    if (Test-Path "./$doc") {
        Copy-Item -Path "./$doc" -Destination "./publish/Install/Documentacao/$doc" -Force
        Write-Host "✓ $doc copiado" -ForegroundColor Green
    }
}

# Criar arquivo de versão
$versionInfo = @"
BibliotecaJK v3.0 FINAL
Build: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
Runtime: win-x64 (self-contained)
.NET: 8.0

Requisitos:
- Windows 10 ou superior
- MySQL 8.0 ou superior
- 200 MB de espaço em disco

Desenvolvido por: BibliotecaJK Team
"@

$versionInfo | Out-File -FilePath "./publish/Install/VERSION.txt" -Encoding UTF8
Write-Host "✓ VERSION.txt criado" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Build concluído com sucesso!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Próximo passo:" -ForegroundColor Yellow
Write-Host "  Execute: .\build-installer.bat" -ForegroundColor White
Write-Host "  para criar o instalador com Inno Setup" -ForegroundColor Gray
Write-Host ""
Write-Host "Arquivos em: ./publish/" -ForegroundColor Cyan
Write-Host ""
