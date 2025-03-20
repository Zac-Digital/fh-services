# Web Application Firewall

The WAF provides OWASP 3.2 rules that are applied to our gateways to protect against malicious requests. This also can hinder our applications by generating false-positives such as double hyphens being included in header values, which are genuine but if the WAF is in prevent mode, would stop the request.

WAFs have two modes: detect and prevent. Detect will log any request that violates one of the OWASP rules but will still allow it through. This is fine whilst fine tuning is being done to exclude conditionally on rules that are flagging as false positives but isn’t recommended as the normal mode.

Prevent will run the same rules (with any exclusions) and block those requests which violate its policies. This is the recommended normal mode to use.

**Switching Modes**

Normally this is done via terraform when the environment is being provisioned but in some circumstances the mode might need to be switched manually, say for testing purposes. Assuming we are starting in detect mode, to switch to prevent mode, go to your resource group and filter to the Application Gateway WAF Policy:

![](../img/WAF%20Filter%20Types.png)

Choose the option and then click Apply:

![](../img/WAF%20Filter%20to%20WAF%20Policy.png)

Select the WAF policy resource you require and click the Switch to prevention mode button **note** you need to enablePIM:

![](../img/WAF%20Switch%20to%20Prevent.png)

This will take a few seconds and you will see that the Policy Mode on the right changes to Prevention.

If you want to see the logs, in the same way as above, filter to the Application gateway type and then choose the application gateway associated to the WAF that was switched to prevent mode:

![](../img/Web%20Application%20Firewall%20Logs.png)

The Logs are found on the left-hand menu. Click this and you are presented with the following screen:

![](../img/Web%20Application%20Firewall%20Init%20Screen.png)

Ignore the Queries hub as we are going to use our own query. You paste the query provided below into the window. You will want to change the dropdown for the Time range as this defaults to the last 24 hours and we will want the last hour/30mins - whatever makes sense. You can add custom time ranges into the query. If you do, consult the docs. Starting point can be found:

[Get started with log queries in Azure Monitor - Azure Monitor | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/get-started-queries?toc=%2Fazure%2Fazure-monitor%2Ftoc.json)

You can run the query. Just be aware that there is usually a minute or two of lag and that the date times are UTC so depending on if the UK is in BST, these might be an hour behind.

![](../img/Web%20Application%20Firewall%20ag%20logs%20query.png)

This is the default sort of query which will you enough info. For example:

* action\_s - the WAF action it took on the request e.g. Matched
* ruleId\_s - the WAF OWASP rule Id matched by the request, for example 920350
* ruleGroup\_s - the group that the rule belongs to

AzureDiagnostics

| where OperationName == "ApplicationGatewayFirewall"

| project TimeGenerated, action\_s, Message, ruleId\_s, ruleGroup\_s, details\_message\_s, details\_data\_s, details\_file\_s

| take 100

You can also choose to only take a select number. If you want all for the period, just remove the line:

| take 100

If you want every property, run:

AzureDiagnostics

This will give you lots of other properties but the above query is probably the useful ones.

**Other Information**

Useful links:

* [Troubleshoot - Azure Web Application Firewall | Microsoft Learn](https://learn.microsoft.com/en-us/azure/web-application-firewall/ag/web-application-firewall-troubleshoot)