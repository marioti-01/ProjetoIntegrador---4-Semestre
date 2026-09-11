# Diretrizes iniciais da PSI - EduSecure 360

- Usuários devem acessar somente recursos compatíveis com sua função.
- Senhas não podem ser armazenadas em texto simples; o laboratório usa BCrypt.
- Tokens, senhas e segredos não devem ser escritos em logs.
- Eventos relevantes de autenticação e administração devem ser registrados.
- Contas com comportamento anormal podem sofrer bloqueio temporário.
- Dados educacionais fictícios devem permanecer restritos aos perfis autorizados.
- Incidentes devem seguir processo de detecção, análise, contenção, recuperação e revisão.
- Permissões administrativas devem ser periodicamente revisadas.
- Em produção, segredos devem sair do código/Compose e usar mecanismo seguro de secrets.
