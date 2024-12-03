
# SQL Server Instance
resource "azurerm_mssql_server" "reporting_sql_server" {
  name = "${var.prefix}-as-fh-sql-server-reporting"
  resource_group_name = local.resource_group_name
  location = var.location
  version = "12.0"

  administrator_login = var.sql_server_reporting_user
  administrator_login_password = var.sql_server_reporting_pwd

  tags = local.tags
}

resource "azurerm_mssql_server_extended_auditing_policy" "reporting_sql_server_auditing_policy" {
  server_id = azurerm_mssql_server.reporting_sql_server.id
  log_monitoring_enabled = true
  audit_actions_and_groups = local.sal_audit_actions_and_groups
}

resource "azurerm_monitor_diagnostic_setting" "reporting_sql_server_diagnostics" {
  name = "reporting-sql-server-diagnostics"
  target_resource_id = "${azurerm_mssql_server.reporting_sql_server.id}/databases/master"
  log_analytics_workspace_id = azurerm_log_analytics_workspace.app_services.id
  enabled_log {
    category_group = "AllLogs"
  }
  metric {
    category = "Basic"
    enabled  = false
  }
  metric {
    category = "InstanceAndAppAdvanced"
    enabled  = false
  }
  metric {
    category = "WorkloadManagement"
    enabled  = false
  }
}

# SQL Server Database for Report Staging API
resource "azurerm_mssql_database" "report_staging_db" {
  name = "${var.prefix}-fh-report-staging-db"
  server_id = azurerm_mssql_server.reporting_sql_server.id
  collation = "SQL_Latin1_General_CP1_CI_AS"

  max_size_gb = 250
  sku_name = var.environment == "Dev" ? "S2" : "S3"
  zone_redundant = false
  storage_account_type = "Local"

  short_term_retention_policy {
      retention_days = "7"
      backup_interval_in_hours = "24"
  }

  long_term_retention_policy {
      monthly_retention = "PT0S"
  }

  threat_detection_policy {
      disabled_alerts = []
      email_account_admins = "Disabled"
      email_addresses = []
      retention_days = 0
      state = "Disabled"
  }

  tags = local.tags
}

# Private DNS Zone - SQL Server A Records
resource "azurerm_private_dns_a_record" "sql_report_stg_api" {
  name                = "${var.prefix}-as-fh-report-stg-api"
  zone_name           = azurerm_private_dns_zone.sqlserver.name
  resource_group_name = local.resource_group_name
  ttl                 = "10"
  records             = [var.pvt_endp_report_stg_api_ip_address]
  tags = local.tags
  lifecycle {
    ignore_changes = [tags]
  }
}

resource "azurerm_private_dns_a_record" "sql_report_stg_api_scm" {
  name                = "${var.prefix}-as-fh-report-stg-api.scm"
  zone_name           = azurerm_private_dns_zone.sqlserver.name
  resource_group_name = local.resource_group_name
  ttl                 = "10"
  records             = [var.pvt_endp_report_stg_api_ip_address]
  tags = local.tags
  lifecycle {
    ignore_changes = [tags]
  }
}
