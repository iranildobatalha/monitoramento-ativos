# monitoramento-ativos
Uma aplicação de console em C# para aconselhar sobre a venda ou compra de ativos da B3 baseado em um limiar predefinido.

# A aplicação
O objetivo do sistema é avisar, via e-mail, caso a cotação de um ativo da B3 caia mais do que certo nível, ou suba acima de outro.

O programa é uma aplicação de console (não possui interface gráfica). É chamado via linha de comando com 3 parâmetros:
- O ativo a ser monitorado
- O preço de referência para venda
- O preço de referência para compra

Para a funcionalidade de enviar o email, a aplicação precisa de um arquivo de configuração json com:

- O e-mail de destino dos alertas
- As configurações de acesso ao servidor de SMTP que irá enviar o e-mail

O programa fica continuamente monitorando a cotação do ativo enquanto estiver rodando e só para quando o processo é interrompido.

# Funcionamento

Por exemplo, tomemos o ativo PETR4. Toda vez que o preço do ativo for maior que o segundo parâmetro passado na linha de comando, um e-mail é enviado aconselhando a venda. Toda vez que o preço for menor que o terceiro parâmetro da linha de comando, um e-mail é enviado aconselhando a compra.

Ex.

> stock-quote-alert.exe PETR4 22.67 22.59

# Conclusão

Esta aplicação é apenas um projeto rotineiro e ainda cabem melhorias nele, que são bem vindas!
