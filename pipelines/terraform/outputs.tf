output "web-app-name" {
  value = azurerm_linux_web_app.webapp_a.name
  sensitive = true
}
