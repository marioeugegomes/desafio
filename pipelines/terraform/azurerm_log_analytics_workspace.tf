resource "azurerm_log_analytics_workspace" "loganalytics" {
  name                = "${local.aplicacao}-${local.ambiente}${local.aliasAssinatura}-law"
  location            = local.localizacao
  resource_group_name = local.ambiente == "prod" ? data.azurerm_resource_group.rg[0].name : azurerm_resource_group.rg[0].name
  tags                = local.tags
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "appinsights" {
  name                = "${local.aplicacao}-${local.ambiente}${local.aliasAssinatura}-ai"
  location            = local.localizacao
  resource_group_name = local.ambiente == "prod" ? data.azurerm_resource_group.rg[0].name : azurerm_resource_group.rg[0].name
  workspace_id        = azurerm_log_analytics_workspace.loganalytics.id
  application_type    = "web"
  retention_in_days   = 30
  tags                = local.tags
}