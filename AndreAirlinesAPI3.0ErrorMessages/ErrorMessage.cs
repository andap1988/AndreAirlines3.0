﻿using Microsoft.AspNetCore.Mvc;
using System;

namespace AndreAirlinesAPI3._0ErrorMessages
{
    public class ErrorMessage : ControllerBase
    {
        public static ContentResult Awnser(string content)
        {
            var msg = "";
            if (content == "offline")
            {
                msg = "O serviço de busca de endereço está offline.";
            }
            else if (content == "noCEP")
            {
                msg = "O CEP é obrigatório.";
            }
            else if (content == "CEP")
            {
                msg = "O CEP informado está inválido.";
            }
            else if (content == "aeroporto")
            {
                msg = "O aeroporto já está cadastrado";
            }
            return Awnser(msg);
        }

        public static string ReturnMessage(string errorCode)
        {
            var msg = "";

            if (errorCode == "No connection could be made because the target machine actively refused it.")
                msg = "A API de consulta está fora do ar. Tente novamente em instantes.";
            else if (errorCode.Substring(0, 1) == "4")
                msg = "Houve um erro de rede. Favor atualizar a página e tentar novamente.";
            else if (errorCode.Substring(0, 1) == "5")
                msg = "Houve um erro no servidor. Favor atualizar a página e tentar novamente.";
            else if (errorCode == "NotFound")
                msg = "Não existe em nosso sistema. Favor tentar novamente.";
            else if (errorCode == "BadRequest")
                msg = "Não existe no sistema. Favor tentar novamente.";
            else if (errorCode == "noCpf")
                msg = "CPF informado é inválido. Favor tentar novamente.";
            else if (errorCode == "yesPassenger")
                msg = "O passageiro informado já está cadastrado no sistema. Favor tentar novamente.";
            else if (errorCode == "yesAirport")
                msg = "O aeroporto informado já está cadastrado no sistema. Favor tentar novamente.";
            else if (errorCode.Contains("timeout"))
                msg = "A API de consulta está fora do ar. Tente novamente em instantes.";
            else if (errorCode.Contains("noLength"))
                msg = "Ao digitar o ID, é necessário conter 24 caracteres.";
            else if (errorCode == "yesUser")
                msg = "O usuário informado já está cadastrado no sistema. Favor tentar novamente.";
            else if (errorCode == "noPermited")
                msg = "O usuário informado não tem permissão para realizar essa tarefa. Favor tentar novamente.";

            return msg;
        }
    }
}
