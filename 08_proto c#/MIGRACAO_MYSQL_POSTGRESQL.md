# Script para atualizar referencias MySQL para PostgreSQL/Supabase
# NOTA: Este arquivo e apenas documentacao das mudancas necessarias
# As mudancas foram aplicadas manualmente nos arquivos

ARQUIVOS AFETADOS:
1. Forms/FormBackup.cs
   - Linha 48: "Panel Conexão MySQL" → "Panel Conexão PostgreSQL"
   - Linha 59: "CREDENCIAIS DO MYSQL" → "CREDENCIAIS DO POSTGRESQL/SUPABASE"
   - Linha 363: "Informe o host do MySQL" → "Informe o host do PostgreSQL/Supabase"
   - Linha 79: porta padrão 3306 (MySQL) → 5432 (PostgreSQL)

2. BLL/BackupConfig.cs
   - Linha 148: comentário "string de conexão MySQL" → "PostgreSQL"

JUSTIFICATIVA:
Com a migração para Supabase/PostgreSQL, todos os textos da interface
devem refletir a nova tecnologia para evitar confusão do usuário.

STATUS:
✅ BackupService já usa Npgsql 8.0.8
✅ DAL files já usam Npgsql 8.0.8
✅ FormBackup atualizado com textos "PostgreSQL/Supabase"
✅ BackupConfig atualizado com comentário "PostgreSQL"
✅ Porta padrão alterada de 3306 (MySQL) para 5432 (PostgreSQL)

CONCLUÍDO:
Todos os textos da UI foram atualizados para PostgreSQL/Supabase.
Npgsql atualizado para versão 8.0.8 (versão estável mais recente compatível com .NET 8.0).
