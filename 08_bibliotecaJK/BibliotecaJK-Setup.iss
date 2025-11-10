; Script Inno Setup para BibliotecaJK v3.0
; Requer Inno Setup 6.x - Download: https://jrsoftware.org/isdl.php

#define MyAppName "BibliotecaJK"
#define MyAppVersion "3.0"
#define MyAppPublisher "BibliotecaJK Team"
#define MyAppURL "https://github.com/shishiv/bibliokopke"
#define MyAppExeName "BibliotecaJK.exe"

[Setup]
; Informações básicas
AppId={{8F9A3B2C-1D4E-5F6A-7B8C-9D0E1F2A3B4C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

; Diretórios
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes

; Saída
OutputDir=publish\Installer
OutputBaseFilename=BibliotecaJK-Setup-v{#MyAppVersion}
SetupIconFile=..\..\..\icon.ico
UninstallDisplayIcon={app}\{#MyAppExeName}

; Compressão
Compression=lzma2/ultra64
SolidCompression=yes
LZMAUseSeparateProcess=yes
LZMADictionarySize=1048576
LZMANumFastBytes=273

; Privilégios e compatibilidade
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

; Visual
WizardStyle=modern
WizardImageFile=compiler:WizModernImage-IS.bmp
WizardSmallImageFile=compiler:WizModernSmallImage-IS.bmp

; Licença e info
LicenseFile=publish\Install\Documentacao\README.txt
InfoBeforeFile=publish\Install\Documentacao\INSTALACAO.md

; Desinstalação
UninstallDisplayName={#MyAppName} v{#MyAppVersion}
UninstallFilesDir={app}\uninst

; Versionamento
VersionInfoVersion={#MyAppVersion}.0.0
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription=Sistema de Gerenciamento de Biblioteca
VersionInfoCopyright=Copyright (C) 2025 {#MyAppPublisher}
VersionInfoProductName={#MyAppName}
VersionInfoProductVersion={#MyAppVersion}

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na Área de Trabalho"; GroupDescription: "Atalhos adicionais:"; Flags: unchecked
Name: "quicklaunchicon"; Description: "Criar atalho na Barra de Tarefas"; GroupDescription: "Atalhos adicionais:"; Flags: unchecked

[Files]
; Executável principal e DLLs
Source: "publish\BibliotecaJK\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

; Banco de dados (schema SQL)
Source: "publish\Install\schema.sql"; DestDir: "{app}\Database"; Flags: ignoreversion

; Documentação
Source: "publish\Install\Documentacao\*"; DestDir: "{app}\Documentacao"; Flags: ignoreversion recursesubdirs

; Arquivo de versão
Source: "publish\Install\VERSION.txt"; DestDir: "{app}"; Flags: ignoreversion

; README na raiz
Source: "publish\Install\Documentacao\README.txt"; DestDir: "{app}"; DestName: "LEIA-ME.txt"; Flags: ignoreversion isreadme

[Icons]
; Menu Iniciar
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Comment: "Sistema de Gerenciamento de Biblioteca"
Name: "{group}\Manual do Usuário"; Filename: "{app}\Documentacao\MANUAL_USUARIO.md"; Comment: "Manual completo do sistema"
Name: "{group}\Guia de Instalação"; Filename: "{app}\Documentacao\INSTALACAO.md"; Comment: "Guia de instalação e configuração"
Name: "{group}\Documentação Técnica"; Filename: "{app}\Documentacao\ARQUITETURA.md"; Comment: "Documentação da arquitetura do sistema"
Name: "{group}\Pasta de Instalação"; Filename: "{app}"; Comment: "Abrir pasta de instalação"
Name: "{group}\Desinstalar {#MyAppName}"; Filename: "{uninstallexe}"; Comment: "Remover {#MyAppName}"

; Área de Trabalho
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; Comment: "Sistema de Gerenciamento de Biblioteca"

; Barra de Tarefas
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon; Comment: "Sistema de Gerenciamento de Biblioteca"

[Registry]
; Adicionar ao PATH do sistema (opcional)
Root: HKLM; Subkey: "Software\{#MyAppPublisher}\{#MyAppName}"; Flags: uninsdeletekeyifempty
Root: HKLM; Subkey: "Software\{#MyAppPublisher}\{#MyAppName}"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"; Flags: uninsdeletevalue
Root: HKLM; Subkey: "Software\{#MyAppPublisher}\{#MyAppName}"; ValueType: string; ValueName: "Version"; ValueData: "{#MyAppVersion}"; Flags: uninsdeletevalue

[Run]
; Executar após instalação
Filename: "{app}\Documentacao\INSTALACAO.md"; Description: "Abrir Guia de Instalação (Configurar MySQL)"; Flags: postinstall shellexec skipifsilent nowait
Filename: "{app}\{#MyAppExeName}"; Description: "Executar {#MyAppName}"; Flags: postinstall skipifsilent nowait

[UninstallRun]
; Limpeza ao desinstalar (opcional)
Filename: "{cmd}"; Parameters: "/c rd /s /q ""{localappdata}\BibliotecaJK"""; Flags: runhidden; RunOnceId: "CleanupAppData"

[Code]
// Código Pascal Script para verificações customizadas

function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
  MySQLVersion: String;
begin
  Result := True;

  // Verificar .NET Runtime (caso não seja self-contained)
  // Esta verificação é opcional se usarmos self-contained

  // Mensagem de boas-vindas
  if MsgBox('Bem-vindo ao instalador do BibliotecaJK v' + '{#MyAppVersion}' + '!' + #13#10#13#10 +
            'Este assistente instalará o Sistema de Gerenciamento de Biblioteca.' + #13#10#13#10 +
            'REQUISITOS:' + #13#10 +
            '• Windows 10 ou superior (64-bit)' + #13#10 +
            '• MySQL 8.0 ou superior (OBRIGATÓRIO)' + #13#10 +
            '• 200 MB de espaço em disco' + #13#10#13#10 +
            'Deseja continuar?',
            mbInformation, MB_YESNO) = IDNO then
  begin
    Result := False;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // Criar diretório para backups (padrão)
    CreateDir(ExpandConstant('{userdocs}\BibliotecaJK\Backups'));

    // Log de instalação
    SaveStringToFile(ExpandConstant('{app}\install.log'),
                     'Instalado em: ' + GetDateTimeString('yyyy-mm-dd hh:nn:ss', #0, #0) + #13#10,
                     False);
  end;
end;

function InitializeUninstall(): Boolean;
begin
  Result := True;

  if MsgBox('Deseja realmente remover o BibliotecaJK?' + #13#10#13#10 +
            'ATENÇÃO: Os dados do banco MySQL NÃO serão removidos.' + #13#10 +
            'Você precisará remover o banco manualmente se desejar.',
            mbConfirmation, MB_YESNO) = IDNO then
  begin
    Result := False;
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  DataPath: String;
begin
  if CurUninstallStep = usPostUninstall then
  begin
    DataPath := ExpandConstant('{localappdata}\BibliotecaJK');

    if DirExists(DataPath) then
    begin
      if MsgBox('Deseja remover também as configurações e backups locais?' + #13#10 +
                '(Localização: ' + DataPath + ')',
                mbConfirmation, MB_YESNO) = IDYES then
      begin
        DelTree(DataPath, True, True, True);
      end;
    end;
  end;
end;

