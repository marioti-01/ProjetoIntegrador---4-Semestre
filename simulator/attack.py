import os,time,requests
base=os.getenv("TARGET","http://portal")
email=os.getenv("EMAIL","admin@edusecure.local")
print(f"[EduSecure simulator] Alvo interno: {base} | conta fictícia: {email}")
for i in range(1,9):
    try:
        r=requests.post(f"{base}/api/auth/login",json={"email":email,"password":f"senha-errada-{i}"},timeout=5)
        print(f"Tentativa {i}: HTTP {r.status_code} - {r.text[:120]}")
    except Exception as e: print(f"Tentativa {i}: erro {e}")
    time.sleep(1)
print("Fim. Abra o Grafana e o Alertmanager para observar a detecção.")
