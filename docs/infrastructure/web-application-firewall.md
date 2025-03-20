# Web Application Firewall (WAF)

The WAF provides OWASP 3.2 rules applied to gateways to protect against malicious requests. It can generate false positives (e.g., double hyphens in headers), blocking genuine requests in prevent mode.

## WAF Modes
- **Detect**: Logs OWASP rule violations but allows requests. Suitable for fine-tuning exclusions, not recommended as normal mode.
- **Prevent**: Blocks requests violating rules (with exclusions). Recommended normal mode.

## Switching Modes
Normally managed via Terraform during provisioning. For manual switching (e.g., testing):
1. Start in detect mode.
2. In resource group, filter to Application Gateway WAF Policy:  
   ![WAF Filter](waf-filter-types.png)
3. Select option, click Apply:  
   ![WAF Policy](waf-filter-to-waf-policy.png)
4. Choose WAF policy, click "Switch to prevention mode" (enable PIM):  
   ![Switch Mode](waf-switch-to-prevent.png)
5. Policy Mode changes to Prevention after a few seconds.

## Viewing Logs
1. Filter to Application Gateway type, select gateway tied to WAF:  
   ![AG Logs](ag-logs.png)
2. Click Logs from left menu:  
   ![Logs Screen](ag-logs-init-screen.png)
3. Paste query (below), adjust Time range (default: 24h, e.g., switch to 1h/30m):  
   ![Logs Query](ag-logs-query.png)
4. Logs lag 1-2 mins, times in UTC (adjust for BST).

### Query Example