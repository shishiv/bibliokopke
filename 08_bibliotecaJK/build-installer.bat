@echo off
REM Script para compilar o instalador BibliotecaJK usando Inno Setup
REM Execute este script DEPOIS de executar build-release.ps1

echo ========================================
echo   BibliotecaJK - Build Installer v3.0
echo ========================================
echo.

REM Verificar se Inno Setup está instalado
set "INNO_SETUP_PATH=C:\Program Files (x86)\Inno Setup 6\ISCC.exe"

if not exist "%INNO_SETUP_PATH%" (
    echo [ERRO] Inno Setup nao encontrado!
    echo.
    echo Instale o Inno Setup 6 de:
    echo https://jrsoftware.org/isdl.php
    echo.
    echo Procurando em: %INNO_SETUP_PATH%
    echo.
    pause
    exit /b 1
)

echo [OK] Inno Setup encontrado
echo.

REM Verificar se a aplicação foi publicada
if not exist "publish\BibliotecaJK\BibliotecaJK.exe" (
    echo [ERRO] Aplicacao nao foi publicada!
    echo.
    echo Execute primeiro: .\build-release.ps1
    echo.
    pause
    exit /b 1
)

echo [OK] Aplicacao publicada encontrada
echo.

REM Criar pasta de saída do instalador
if not exist "publish\Installer" (
    mkdir "publish\Installer"
    echo [OK] Pasta publish\Installer criada
    echo.
)

REM Compilar o instalador com Inno Setup
echo Compilando instalador com Inno Setup...
echo Isso pode levar alguns minutos...
echo.

"%INNO_SETUP_PATH%" "BibliotecaJK-Setup.iss"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERRO] Falha ao compilar o instalador!
    echo.
    pause
    exit /b 1
)

echo.
echo ========================================
echo   Instalador criado com sucesso!
echo ========================================
echo.
echo Arquivo: publish\Installer\BibliotecaJK-Setup-v3.0.exe
echo.
echo Voce pode distribuir este arquivo para instalar
echo o BibliotecaJK em qualquer computador Windows.
echo.
pause
