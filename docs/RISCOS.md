# Registro de Riscos — EduSecure 360

**Fonte dos dados:** varreduras automatizadas com VulnScanner (`scan_edusecure.py`) contra a API do EduSecure 360, em 4 rodadas (11/09 a 14/09/2026).
**Escala de probabilidade/impacto:** Baixo (1) / Médio (2) / Alto (3). Severidade = Probabilidade × Impacto (1-3 Baixo, 4-6 Médio, 7-9 Alto).

## Riscos tratados (evidência do ciclo detectar → corrigir → validar)

| ID | Risco | Categoria (OWASP/NIST/PCI) | Prob. | Impacto | Severidade | Data detecção | Data correção | Responsável | Evidência de validação |
|---|---|---|---|---|---|---|---|---|---|
| R-01 | Ausência de header CSP (exposição a XSS/injeção de conteúdo) | OWASP A03 / NIST SI-10 | 2 | 3 | Alto (6) | 11/09/2026 | 14/09/2026 | *(preencher)* | Scan v2: header presente com `default-src ''self''; frame-ancestors ''none''` |
| R-02 | Site vulnerável a Clickjacking (sem X-Frame-Options) | OWASP A05 | 2 | 2 | Médio (4) | 11/09/2026 | 14/09/2026 | *(preencher)* | Scan v2: `X-Frame-Options: DENY` presente |
| R-03 | Ausência de X-Content-Type-Options (MIME sniffing) | OWASP A05 | 1 | 2 | Baixo (2) | 11/09/2026 | 14/09/2026 | *(preencher)* | Scan v2: `X-Content-Type-Options: nosniff` presente |
| R-04 | Ausência de Referrer-Policy (vazamento de URLs sensíveis) | OWASP A05 | 1 | 1 | Baixo (1) | 11/09/2026 | 14/09/2026 | *(preencher)* | Scan v2: `Referrer-Policy: strict-origin-when-cross-origin` presente |
| R-05 | Ausência de Permissions-Policy (câmera/microfone irrestritos) | OWASP A05 | 1 | 1 | Baixo (1) | 11/09/2026 | 14/09/2026 | *(preencher)* | Scan v2: header presente restringindo câmera/microfone/geolocalização |
| R-06 | Sem redirecionamento automático HTTP → HTTPS | OWASP A02 / PCI Req 4.2 / NIST SC-8 | 2 | 3 | Alto (6) | 11/09/2026 | 14/09/2026 | *(preencher)* | Scan v4: `http://localhost:5000 → https://localhost:5001` confirmado via `AddHttpsRedirection(HttpsPort=5001)` |
| R-07 | Falso positivo de Open Redirect no próprio scanner (17 ocorrências) | N/A — defeito na ferramenta, não no EduSecure | — | — | — | 14/09/2026 | 14/09/2026 | *(preencher)* | Bug identificado em `redirect_checker.py` (substring match em vez de validar host); corrigido e revalidado — 0 CRITICAL no scan final |

## Riscos em aberto (residuais)

| ID | Risco | Categoria | Prob. | Impacto | Severidade | Status | Justificativa / plano |
|---|---|---|---|---|---|---|---|
| R-08 | HSTS ausente | OWASP A02 / PCI Req 4.2 / NIST SC-8 | 1 | 2 | Baixo (2) | Aceito (residual) | `app.UseHsts()` implementado e ativo no código; o ASP.NET Core **intencionalmente** omite o header quando o host é `localhost`, para não bloquear ambientes de desenvolvimento em HTTPS permanentemente. Validado que o middleware está correto — comportamento não observável apenas por ser ambiente de laboratório local. Mitigação plena ocorre automaticamente com domínio real em produção. |
| R-09 | Header `Server: Kestrel` expõe tecnologia do servidor | OWASP A05 | 2 | 1 | Baixo (2) | Aberto | Recomendação: remover/mascarar o header `Server` via middleware (`context.Response.Headers.Remove("Server")`) ou configuração do Kestrel. Baixo esforço, pode ser feito a qualquer momento. |
| R-10 | TLS 1.3 não suportado (apenas TLS 1.2) | NIST SC-8 | 1 | 1 | Baixo (1) | Aceito (residual) | TLS 1.2 é considerado aceitável pelos frameworks de referência. Migrar para TLS 1.3 depende de suporte do ambiente de hospedagem final; não é bloqueante para o ambiente de laboratório. |

## Resumo executivo

- **Score de segurança (VulnScanner):** evoluiu de **51/100 → 85/100** ao longo de 4 rodadas de teste.
- **Compliance geral:** evoluiu de **44% → 56%** (OWASP Top 10: 40%→60%, NIST SP 800-53: 40%→60%, PCI DSS: 50% mantido).
- **Findings críticos:** 0 em todas as rodadas válidas (os 17 CRITICAL da rodada 3 foram um falso positivo do scanner, identificado e corrigido pela equipe).
- **Riscos altos tratados:** 2 de 3 (CSP e redirecionamento HTTPS); o terceiro (HSTS) é residual aceito por comportamento de framework, não por ausência de controle.

---
*Registro construído a partir de dados reais gerados pelo laboratório do grupo (varreduras VulnScanner contra a API EduSecure 360), conforme exigido pelo manual do Projeto Integrador IV. Campos "Responsável" a preencher pelo grupo conforme divisão de tarefas.*
