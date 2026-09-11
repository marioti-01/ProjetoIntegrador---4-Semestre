# Registro inicial de riscos - EduSecure 360

| ID | Ativo | Risco | Probabilidade | Impacto | Nível | Tratamento inicial |
|---|---|---|---|---|---|---|
| R01 | Contas de usuários | Força bruta/roubo de credencial | Alta | Alto | Alto | Bloqueio temporário, logs, alerta e política de senha |
| R02 | Banco educacional | Vazamento de dados | Média | Crítico | Crítico | RBAC, rede interna Docker, menor privilégio e revisão de acesso |
| R03 | API | Indisponibilidade | Média | Alto | Alto | Health check, Prometheus e alerta |
| R04 | Aplicação | Erros 5xx em sequência | Média | Médio | Médio | Métricas, alerta e investigação de logs |
| R05 | Perfil administrativo | Acesso indevido | Baixa | Crítico | Alto | JWT com Role e autorização por endpoint |
| R06 | Logs | Exposição de credenciais | Média | Alto | Alto | Nunca registrar senha/token e limitar dados nos logs |
