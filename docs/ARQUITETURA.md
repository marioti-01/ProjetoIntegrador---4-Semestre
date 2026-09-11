# Arquitetura - EduSecure 360

```text
Usuário
  |
  v
Nginx / Portal :8080
  |
  v
ASP.NET Core API :5000 ----> PostgreSQL
  |
  +---- /metrics ----> Prometheus :9090 ----> Grafana :3000
                              |
                              v
                        Alertmanager :9093
                              |
                              v
                           n8n :5678
                              |
                              v
                    API /api/incidentes/webhook

Docker ----> cAdvisor :8081 ----> Prometheus
```

## Perfis
- **Aluno:** consulta perfil e notas.
- **Professor:** consulta turmas/alunos e pode atualizar notas de suas próprias turmas.
- **Administrador:** consulta usuários, logs, incidentes e resumo de segurança.

## Controles implementados
- BCrypt para senha.
- JWT com `Role`.
- RBAC via `[Authorize(Roles = ...)]`.
- Bloqueio temporário após falhas repetidas.
- Logs sem senha/token.
- Cabeçalhos de segurança no Nginx.
- Banco e serviços em rede Docker própria.
