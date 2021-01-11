using Common;
using Common.Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Communication
{
    class PreCommunicationCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            TargetWorker targetWorker = new TargetWorker(serviceProvider);
            ServiceWorker serviceWorker = new ServiceWorker(serviceProvider);
            Handler handler = new Handler(serviceWorker, targetWorker);

            if (!(targetWorker.Contains(ptest_communication.Fields.ptest_type) || targetWorker.Contains(ptest_communication.Fields.ptest_main))) return;

            if (targetWorker.GetOptionSetValue(ptest_communication.Fields.ptest_type) == (int)ptest_communication_ptest_type.Telefon
                && targetWorker.GetAttribute<bool>(ptest_communication.Fields.ptest_main) && handler.HasAnotherCommunication(false))
                throw new InvalidPluginExecutionException("У контакта уже есть средство связи с основным телефоном");

            if (targetWorker.GetOptionSetValue(ptest_communication.Fields.ptest_type) == (int)ptest_communication_ptest_type.Telefon
                && !targetWorker.GetAttribute<bool>(ptest_communication.Fields.ptest_main) && handler.HasAnotherCommunication(true))
                throw new InvalidPluginExecutionException("У контакта уже есть средство связи с основным телефоном");

            if (targetWorker.GetOptionSetValue(ptest_communication.Fields.ptest_type) == (int)ptest_communication_ptest_type.E_mail
                && handler.HasAnotherCommunication(true))
                throw new InvalidPluginExecutionException("У контакта уже есть средство связи с основным телефоном");

            // 1-ая Основной телефон: найти другие средства связи данного контакта и проверить, что их нет
            // 2-ая Не основной телефон: найти другие средства связи с основным телефон и проверить, что их нет
            // 3-ая Основная почта => 2
            // 4-ая неосновная почта => 2
        }
    }
}
