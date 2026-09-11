# Plano de testes

## Funcionais
1. Login de Aluno e leitura de notas.
2. Login de Professor e leitura de turmas.
3. Login de Administrador e acesso a logs/incidentes.
4. Tentativa de Aluno em rota administrativa deve resultar em `403`.

## Segurança
1. Executar o `attack-simulator`.
2. Confirmar pelo menos 5 `LOGIN_FAILED`.
3. Confirmar `ACCOUNT_LOCKED`.
4. Confirmar alerta `EduSecurePossibleBruteForce`.
5. Com n8n ativo, confirmar criação de incidente.

## Disponibilidade
1. Confirmar `up{job="edusecure-api"} = 1`.
2. Parar temporariamente o container `edusecure-api`.
3. Confirmar alerta `EduSecureApiDown`.
4. Subir novamente a API.

## Evidências sugeridas
Tirar prints do `docker compose ps`, portal, Swagger, Grafana, Alertmanager, execução do simulador, logs administrativos e incidente criado.
