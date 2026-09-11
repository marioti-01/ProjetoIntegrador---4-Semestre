# EduSecure 360

Projeto integrador de cibersegurança: ambiente educacional fictício com governança, gestão de riscos, monitoramento contínuo e resposta a incidentes.

## Arquitetura

Portal Nginx -> ASP.NET Core API -> PostgreSQL

API -> métricas -> Prometheus -> Alertmanager -> n8n -> API de incidentes

Docker/cAdvisor -> Prometheus -> Grafana

## O que já está implementado

- Portal educacional simples com perfis Aluno, Professor e Administrador.
- API ASP.NET Core .NET 10.
- JWT com Role e autorização por perfil.
- Senhas com BCrypt.
- PostgreSQL com dados fictícios criados automaticamente.
- Logs de segurança persistidos no banco.
- Bloqueio temporário após várias falhas de login.
- Endpoint `/metrics` para Prometheus.
- Dashboard Grafana provisionado automaticamente.
- Alertas de possível força bruta, API offline e erros 5xx.
- Alertmanager configurado para encaminhar alertas ao n8n.
- Workflow n8n pronto para importação.
- Simulador de força bruta controlado e restrito ao laboratório Docker.
- Registro inicial de riscos, PSI resumida e plano de resposta.

## Pré-requisitos

- Docker Desktop com Docker Compose.
- Windows 10/11, Linux ou macOS com Docker disponível.

Você não precisa instalar PostgreSQL, .NET, Prometheus ou Grafana localmente para rodar pelo Docker.

## Subir o ambiente

```powershell
docker compose up -d --build
```

ou no PowerShell:

```powershell
./scripts/start.ps1
```

## URLs

- Portal: http://localhost:8080
- Swagger/API: http://localhost:5000/swagger
- API health: http://localhost:5000/health
- Métricas: http://localhost:5000/metrics
- Grafana: http://localhost:3000 — `admin / admin`
- Prometheus: http://localhost:9090
- Alertmanager: http://localhost:9093
- n8n: http://localhost:5678
- cAdvisor: http://localhost:8081

## Contas fictícias

| Perfil | Login | Senha |
|---|---|---|
| Aluno | aluno@edusecure.local | Aluno@123 |
| Professor | professor@edusecure.local | Professor@123 |
| Administrador | admin@edusecure.local | Admin@123 |

## Primeira configuração do n8n

1. Abra `http://localhost:5678`.
2. Crie o usuário local solicitado pelo n8n.
3. Importe `/files/edusecure-alert-workflow.json` pela interface (o mesmo arquivo está na pasta `n8n`).
4. Ative o workflow.
5. O Alertmanager já envia para `http://n8n:5678/webhook/prometheus-alert`.

## Demonstração de força bruta

Depois que o ambiente estiver saudável e o workflow n8n estiver ativo:

```powershell
docker compose --profile demo run --rm attack-simulator
```

ou:

```powershell
./scripts/demo-attack.ps1
```

O simulador tenta senhas erradas apenas contra `admin@edusecure.local` dentro da rede Docker. A API registra as falhas, bloqueia temporariamente a conta e o Prometheus dispara o alerta quando o limiar é atingido.

## Roteiro rápido para a banca

1. Entre no portal como Aluno e mostre as notas.
2. Entre como Professor e mostre a turma.
3. Entre como Administrador e mostre usuários/logs.
4. Abra o Grafana e confirme a API online.
5. Execute o simulador.
6. Observe as falhas de login no Grafana.
7. Confira o alerta no Alertmanager.
8. Com n8n ativo, veja o incidente aparecer no painel do Administrador.
9. Explique o risco R01 e a contenção por bloqueio temporário.

## Reset total do laboratório

```powershell
docker compose down -v
```

Isso remove também os dados persistidos e, na próxima subida, recria os usuários fictícios.

## Observações técnicas

Para facilitar a apresentação, esta versão usa `EnsureCreated` do EF Core em vez de migrations. Depois que o modelo estabilizar, a equipe pode migrar para migrations formais. Segredos presentes no Compose são exclusivamente de laboratório; em produção devem ser gerenciados externamente.

## Origem da base

A organização inicial de API, autenticação JWT e uso de BCrypt foram reaproveitados conceitualmente do projeto de treino EventPlus fornecido pela equipe. O domínio, banco, perfis, monitoramento e segurança foram reconstruídos para o cenário EduSecure 360.
