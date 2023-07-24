locals {
  dominio = "PocCliente.com.br"

  aliasAssinatura = var.aliasAssinatura != "PocCliente" ? "-${lower(var.aliasAssinatura)}" : ""
  localizacao = var.ambiente == "prod" ? "Brazil South" : "East US"
  aplicacao   = var.aplicacao
  ambiente    = var.ambiente
}
