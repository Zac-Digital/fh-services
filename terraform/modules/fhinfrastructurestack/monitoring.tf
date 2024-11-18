resource "azurerm_log_analytics_workspace" "app_services" {
  name = "${var.prefix}-la-as-familyhubs"
  resource_group_name = local.resource_group_name
  location = var.location
  tags = local.tags
}

resource "azurerm_application_insights" "app_insights" {
  name = "${var.prefix}-ai-as-familyhubs"
  resource_group_name = local.resource_group_name
  location = var.location
  application_type = "web"
  sampling_percentage = 0
  workspace_id = azurerm_log_analytics_workspace.app_services.id
  tags = local.tags
}